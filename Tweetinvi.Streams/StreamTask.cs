using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.Models;
using HttpMethod = System.Net.Http.HttpMethod;

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
        private readonly Func<ITwitterQuery> _generateTwitterQuery;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ITweetinviEvents _tweetinviEvents;
        private readonly IFactory<ITwitterTimeoutException> _twitterTimeoutExceptionFactory;
        private readonly IHttpClientWebHelper _httpClientWebHelper;

        private bool _isNew;
        private Exception _lastException;

        private ITwitterQuery _twitterQuery;
        private StreamReader _currentStreamReader;
        private HttpClient _currentHttpClient;

        public StreamTask(
            Func<string, bool> processObject,
            Func<ITwitterQuery> generateTwitterQuery,
            IExceptionHandler exceptionHandler,
            ITweetinviEvents tweetinviEvents,
            IFactory<ITwitterTimeoutException> twitterTimeoutExceptionFactory,
            IHttpClientWebHelper httpClientWebHelper)
        {
            _processObject = processObject;
            _generateTwitterQuery = generateTwitterQuery;
            _exceptionHandler = exceptionHandler;
            _tweetinviEvents = tweetinviEvents;
            _twitterTimeoutExceptionFactory = twitterTimeoutExceptionFactory;
            _httpClientWebHelper = httpClientWebHelper;
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
            SetStreamState(StreamState.Running);

            _twitterQuery = _generateTwitterQuery();

            if (_twitterQuery.TwitterCredentials == null)
            {
                throw new TwitterNullCredentialsException();
            }

            if (!_twitterQuery.TwitterCredentials.AreSetupForUserAuthentication())
            {
                throw new TwitterInvalidCredentialsException(_twitterQuery.TwitterCredentials);
            }

            _currentHttpClient = GetHttpClient(_twitterQuery);
            _currentStreamReader = GetStreamReader(_currentHttpClient, _twitterQuery).Result;

            int numberOfRepeatedFailures = 0;

            while (StreamState != StreamState.Stop)
            {
                if (StreamState == StreamState.Pause)
                {
                    using (EventWaitHandle tmpEvent = new ManualResetEvent(false))
                    {
                        tmpEvent.WaitOne(TimeSpan.FromMilliseconds(STREAM_RESUME_DELAY));
                    }

                    continue;
                }

                try
                {
                    var json = GetJsonResponseFromReader(_currentStreamReader, _twitterQuery);

                    var isJsonResponseValid = json.IsMatchingJsonFormat();
                    if (!isJsonResponseValid)
                    {
                        if (json == string.Empty)
                        {
                            continue;
                        }

                        if (json != null)
                        {
                            throw new WebException(json);
                        }

                        if (TryHandleInvalidResponse(numberOfRepeatedFailures))
                        {
                            ++numberOfRepeatedFailures;
                            continue;
                        }

                        throw new WebException("Stream cannot be read.");
                    }

                    numberOfRepeatedFailures = 0;

                    if (StreamState == StreamState.Running && !_processObject(json))
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

            if (_currentStreamReader != null)
            {
                _currentStreamReader.Dispose();
            }

            if (_currentHttpClient != null)
            {
                _currentHttpClient.Dispose();
            }
        }

        private HttpClient GetHttpClient(ITwitterQuery twitterQuery)
        {
            if (twitterQuery == null)
            {
                SetStreamState(StreamState.Stop);
                return null;
            }

            twitterQuery.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

            var queryBeforeExecuteEventArgs = new QueryBeforeExecuteEventArgs(twitterQuery);
            _tweetinviEvents.RaiseBeforeQueryExecute(queryBeforeExecuteEventArgs);

            if (queryBeforeExecuteEventArgs.Cancel)
            {
                SetStreamState(StreamState.Stop);
                return null;
            }

            return _httpClientWebHelper.GetHttpClient(twitterQuery);
        }

        private async Task<StreamReader> GetStreamReader(HttpClient client, ITwitterQuery twitterQuery)
        {
            try
            {
                var uri = new Uri(twitterQuery.QueryURL);
                var endpoint = uri.GetEndpointURL();
                var queryParameters = uri.Query.Remove(0, 1);
                var httpMethod = new HttpMethod(twitterQuery.HttpMethod.ToString());

                HttpRequestMessage request;

                if (httpMethod == HttpMethod.Post)
                {
                    request = new HttpRequestMessage(httpMethod, endpoint)
                    {
                        Content = new StringContent(queryParameters, Encoding.UTF8, "application/x-www-form-urlencoded")
                    };
                }
                else
                {
                    request = new HttpRequestMessage(httpMethod, twitterQuery.QueryURL);
                }


                var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                var body = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return new StreamReader(body, Encoding.GetEncoding("utf-8"));
            }
            catch (WebException wex)
            {
                client.Dispose();
                HandleWebException(wex);
            }
            catch (Exception ex)
            {
                client.Dispose();

                if (ex is AggregateException && ex.InnerException is WebException)
                {
                    HandleWebException(ex.InnerException as WebException);
                }

                _lastException = ex;
                SetStreamState(StreamState.Stop);
            }

            return null;
        }

        private string GetJsonResponseFromReader(StreamReader reader, ITwitterQuery twitterQuery)
        {
            var requestTask = reader.ReadLineAsync();
            var resultingTask = TaskEx.WhenAny(requestTask, TaskEx.Delay(STREAM_DISCONNECTED_DELAY)).Result;

            if (resultingTask != requestTask)
            {
                var twitterQueryParameter = _twitterTimeoutExceptionFactory.GenerateParameterOverrideWrapper("twitterQuery", twitterQuery);
                var twitterTimeoutException = _twitterTimeoutExceptionFactory.Create(twitterQueryParameter);
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
                _currentStreamReader.Dispose();
                _currentStreamReader = GetStreamReader(_currentHttpClient, _twitterQuery).Result;
                return true;
            }

            if (numberOfRepeatedFailures == 2)
            {
                _currentStreamReader.Dispose();
                _currentHttpClient.Dispose();

                _currentHttpClient = GetHttpClient(_twitterQuery);
                _currentStreamReader = GetStreamReader(_currentHttpClient, _twitterQuery).Result;
                return true;
            }

            return false;
        }

        private bool ShouldContinueAfterHandlingException(Exception ex)
        {
            // NOTE : I am aware that all the paths return false.
            // But having such a method to control whether the stream needs to
            // continue running in case of Exception is great.

            var aex = ex as AggregateException;
            if (aex != null)
            {
                ex = aex.InnerException;
            }

            _lastException = ex;

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

                // Even though the user has asked to specifically handle WebException,
                // We need to inform him that something has gone wrong. 
                // Therefore the connection needs to be stopped with the appropriate information.
                return false;
            }

            var isExceptionThrownByStreamBeingStoppedByUser = _lastException is IOException && StreamState == StreamState.Stop;
            if (isExceptionThrownByStreamBeingStoppedByUser)
            {
                _lastException = null;
            }

            return false;
        }

        private void HandleWebException(WebException wex)
        {
            _lastException = _exceptionHandler.GenerateTwitterException(wex, _twitterQuery.QueryURL);

            if (!_exceptionHandler.SwallowWebExceptions)
            {
                SetStreamState(StreamState.Stop);
                throw _lastException;
            }
        }

        public void Resume()
        {
            SetStreamState(StreamState.Running);
        }

        public void Pause()
        {
            SetStreamState(StreamState.Pause);
        }

        public void Stop()
        {
            if (_currentStreamReader != null)
            {
                _currentStreamReader.Dispose();
            }

            if (_currentHttpClient != null)
            {
                _currentHttpClient.Dispose();
            }

            SetStreamState(StreamState.Stop);
        }

        private void SetStreamState(StreamState value)
        {
            if (StreamState == value)
            {
                return;
            }

            if (_isNew && value == StreamState.Running)
            {
                _isNew = false;
            }

            StreamState = value;
            this.Raise(StreamStateChanged, new GenericEventArgs<StreamState>(value));
        }
    }
}