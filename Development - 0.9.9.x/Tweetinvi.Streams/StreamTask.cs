using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Streams
{
    public interface IStreamTask
    {
        event EventHandler StreamStarted;
        event EventHandler<GenericEventArgs<StreamState>> StreamStateChanged;
        
        StreamState StreamState { get; }
        Exception LastException { get; }

        void Start();
        void Resume();
        void Pause();
        void Stop();
    }

    public class StreamTask : IStreamTask
    {
        public event EventHandler StreamStarted;
        public event EventHandler<GenericEventArgs<StreamState>> StreamStateChanged;

        // https://dev.twitter.com/streaming/overview/connecting#stalls
        private const int STREAM_DISCONNECTED_DELAY = 90000;
        private const int STREAM_RESUME_DELAY = 1000;

        private readonly Func<string, bool> _processObject;
        private readonly Func<HttpWebRequest> _generateWebRequest;
        private readonly ITwitterQueryFactory _twitterQueryFactory;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ITweetinviEvents _tweetinviEvents;
        private readonly IWebHelper _webHelper;
        private readonly IFactory<ITwitterTimeoutException> _twitterTimeoutExceptionFactory;
        
        private bool _isNew;

        private Exception _lastException;
        private WebRequest _currentWebRequest;
        private StreamReader _currentStreamReader;

        public StreamTask(
            Func<string, bool> processObject,
            Func<HttpWebRequest> generateWebRequest,
            ITwitterQueryFactory twitterQueryFactory,
            IExceptionHandler exceptionHandler,
            ITweetinviEvents tweetinviEvents,
            IWebHelper webHelper,
            IFactory<ITwitterTimeoutException> twitterTimeoutExceptionFactory)
        {
            _processObject = processObject;
            _generateWebRequest = generateWebRequest;
            _twitterQueryFactory = twitterQueryFactory;
            _exceptionHandler = exceptionHandler;
            _tweetinviEvents = tweetinviEvents;
            _webHelper = webHelper;
            _twitterTimeoutExceptionFactory = twitterTimeoutExceptionFactory;
            _isNew = true;
        }

        public StreamState StreamState { get; private set; }
        public Exception LastException { get { return _lastException; } }

        public void Start()
        {
            if (StreamState == StreamState.Stop && !_isNew)
            {
                return;
            }

            this.Raise(StreamStarted);
            SetStreamState(StreamState.Resume);

            _currentWebRequest = _generateWebRequest();
            _currentStreamReader = CreateStreamReaderFromWebRequest(_currentWebRequest);

            int numberOfRepeatedFailures = 0;

            while (StreamState != StreamState.Stop)
            {
                if (StreamState == StreamState.Pause)
                {
                    using (EventWaitHandle tmpEvent = new ManualResetEvent(false))
                    {
                        tmpEvent.WaitOne(TimeSpan.FromSeconds(STREAM_RESUME_DELAY));
                    }

                    continue;
                }

                try
                {
                    var jsonResponse = GetJsonResponseFromReader();
                    
                    var isJsonResponseValid = jsonResponse != null;
                    if (!isJsonResponseValid)
                    {
                        if (TryHandleInvalidResponse(numberOfRepeatedFailures))
                        {
                            ++numberOfRepeatedFailures;
                            continue;
                        }

                        break;
                    }

                    numberOfRepeatedFailures = 0;

                    if (jsonResponse == string.Empty)
                    {
                        continue;
                    }

                    if (StreamState == StreamState.Resume && !_processObject(jsonResponse))
                    {
                        SetStreamState(StreamState.Stop);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (!ShouldContinueAfterHandlingException(ex))
                    {
                        SetStreamState(StreamState.Stop);
                        break;
                    }
                }
            }

            if (_currentWebRequest != null)
            {
                _currentWebRequest.Abort();
            }

            if (_currentStreamReader != null)
            {
                _currentStreamReader.Dispose();
            }
        }

        private StreamReader CreateStreamReaderFromWebRequest(WebRequest webRequest)
        {
            if (webRequest == null)
            {
                SetStreamState(StreamState.Stop);
                return null;
            }

            StreamReader reader = null;

            try
            {
                var twitterQuery = _twitterQueryFactory.Create(webRequest.RequestUri.AbsoluteUri);
                var queryBeforeExecuteEventArgs = new QueryBeforeExecuteEventArgs(twitterQuery);
                _tweetinviEvents.RaiseBeforeQueryExecute(queryBeforeExecuteEventArgs);

                if (queryBeforeExecuteEventArgs.Cancel)
                {
                    SetStreamState(StreamState.Stop);
                    return null;
                }

                // TODO : LINVI - THIS CODE HAS CHANGED AND NEEDS TO BE CHECKED WITH ASP.NET
                var responseStream = _webHelper.GetResponseStreamAsync(webRequest).Result;
                if (responseStream != null)
                {
                    reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                }
            }
            catch (WebException wex)
            {
                HandleWebException(wex);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    if (ex.Message == "Stream was not readable.")
                    {
                        webRequest.Abort();
                    }
                }

                _lastException = ex;
                SetStreamState(StreamState.Stop);
            }

            return reader;
        }

        private string GetJsonResponseFromReader()
        {
            var requestTask = _currentStreamReader.ReadLineAsync();
            var resultingTask = TaskEx.WhenAny(requestTask, TaskEx.Delay(STREAM_DISCONNECTED_DELAY)).Result;

            if (resultingTask != requestTask)
            {
                var urlParameter = new ConstructorNamedParameter("url", _currentWebRequest.RequestUri.AbsoluteUri);
                var twitterTimeoutException = _twitterTimeoutExceptionFactory.Create(urlParameter);
                throw (Exception)twitterTimeoutException;
            }

            var jsonResponse = requestTask.Result;
            return jsonResponse;
        }

        private bool TryHandleInvalidResponse(int numberOfRepeatedFailures)
        {
            if (numberOfRepeatedFailures == 0)
            {
                return true;
            }

            if (numberOfRepeatedFailures == 1)
            {
                _currentWebRequest.Abort();
                _currentStreamReader = CreateStreamReaderFromWebRequest(_currentWebRequest);
                return true;
            }

            if (numberOfRepeatedFailures == 2)
            {
                _currentWebRequest.Abort();
                _currentWebRequest = _generateWebRequest();
                _currentStreamReader = CreateStreamReaderFromWebRequest(_currentWebRequest);
                return true;
            }

            return false;
        }

        private bool ShouldContinueAfterHandlingException(Exception ex)
        {
            var timeoutException = ex as TwitterTimeoutException;
            if (timeoutException != null)
            {
                _lastException = timeoutException;
                return false;
            }

            var webException = ex as WebException;
            if (webException != null)
            {
                HandleWebException(webException);
                return true;
            }

            var aex = ex as AggregateException;
            if (aex != null)
            {
                _lastException = aex.InnerException;
            }
            else
            {
                _lastException = ex;
            }

            if (_lastException is IOException && StreamState == StreamState.Stop)
            {
                _lastException = null;
            }

            return false;
        }

        private void HandleWebException(WebException wex)
        {
            _lastException = _exceptionHandler.GenerateTwitterException(wex, _currentWebRequest.RequestUri.AbsoluteUri);

            if (!_exceptionHandler.SwallowWebExceptions)
            {
                SetStreamState(StreamState.Stop);
                throw _lastException;
            }
        }

        public void Resume()
        {
            SetStreamState(StreamState.Resume);
        }

        public void Pause()
        {
            SetStreamState(StreamState.Pause);
        }

        public void Stop()
        {
            if (_currentWebRequest != null)
            {
                _currentWebRequest.Abort();
            }

            SetStreamState(StreamState.Stop);
        }

        private void SetStreamState(StreamState value)
        {
            if (StreamState == value)
            {
                return;
            }

            if (_isNew && value == StreamState.Resume)
            {
                _isNew = false;
            }

            StreamState = value;
            this.Raise(StreamStateChanged, new GenericEventArgs<StreamState>(value));
        }
    }
}
