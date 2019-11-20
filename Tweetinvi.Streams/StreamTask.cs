using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Streaming;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Tweetinvi.Streams
{
    public interface IStreamTask
    {
        event EventHandler StreamStarted;
        event EventHandler<GenericEventArgs<StreamState>> StreamStateChanged;
        event EventHandler KeepAliveReceived;

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
        public event EventHandler KeepAliveReceived;

        // https://dev.twitter.com/streaming/overview/connecting#stalls
        private const int STREAM_DISCONNECTED_DELAY = 90000;
        private const int STREAM_RESUME_DELAY = 1000;

        private readonly Func<string, bool> _processObject;
        private readonly Func<ITwitterRequest> _generateTwitterRequest;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ITweetinviEvents _tweetinviEvents;
        private readonly IHttpClientWebHelper _httpClientWebHelper;

        private bool _isNew;

        private ITwitterRequest _twitterRequest;
        private StreamReader _currentStreamReader;
        private HttpClient _currentHttpClient;
        private int _currentResponseHttpStatusCode = TwitterException.DEFAULT_STATUS_CODE;

        public StreamTask(
            Func<string, bool> processObject,
            Func<ITwitterRequest> generateTwitterRequest,
            IExceptionHandler exceptionHandler,
            ITweetinviEvents tweetinviEvents,
            IHttpClientWebHelper httpClientWebHelper)
        {
            _processObject = processObject;
            _generateTwitterRequest = generateTwitterRequest;
            _exceptionHandler = exceptionHandler;
            _tweetinviEvents = tweetinviEvents;
            _httpClientWebHelper = httpClientWebHelper;
            _isNew = true;
        }

        public StreamState StreamState { get; private set; }
        public Exception LastException { get; private set; }

        public void Start()
        {
            if (StreamState == StreamState.Stop && !_isNew)
            {
                return;
            }

            this.Raise(StreamStarted);
            SetStreamState(StreamState.Running);

            _twitterRequest = _generateTwitterRequest();

            if (_twitterRequest.Query.TwitterCredentials == null)
            {
                throw new TwitterNullCredentialsException();
            }

            if (!_twitterRequest.Query.TwitterCredentials.AreSetupForUserAuthentication())
            {
                throw new TwitterInvalidCredentialsException(_twitterRequest.Query.TwitterCredentials);
            }

            _currentHttpClient = GetHttpClient(_twitterRequest);
            _currentStreamReader = GetStreamReader(_currentHttpClient, _twitterRequest).Result;

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
                    var json = GetJsonResponseFromReader(_currentStreamReader, _twitterRequest);

                    var isJsonResponseValid = json.IsMatchingJsonFormat();
                    if (!isJsonResponseValid)
                    {
                        if (json == string.Empty)
                        {
                            this.Raise(KeepAliveReceived);
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

            _currentStreamReader?.Dispose();
            _currentHttpClient?.Dispose();
        }

        private HttpClient GetHttpClient(ITwitterRequest request)
        {
            if (request.Query == null)
            {
                SetStreamState(StreamState.Stop);
                return null;
            }

            request.Query.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

            var queryBeforeExecuteEventArgs = new QueryBeforeExecuteEventArgs(request.Query);
            _tweetinviEvents.RaiseBeforeQueryExecute(queryBeforeExecuteEventArgs);

            if (queryBeforeExecuteEventArgs.Cancel)
            {
                SetStreamState(StreamState.Stop);
                return null;
            }

            return _httpClientWebHelper.GetHttpClient(request.Query);
        }

        private async Task<StreamReader> GetStreamReader(HttpClient client, ITwitterRequest request)
        {
            try
            {
                var twitterQuery = request.Query;
                var uri = new Uri(twitterQuery.Url);
                var endpoint = uri.GetEndpointURL();
                var queryParameters = uri.Query.Remove(0, 1);
                var httpMethod = new HttpMethod(twitterQuery.HttpMethod.ToString());

                HttpRequestMessage httpRequestMessage;

                if (httpMethod == HttpMethod.Post)
                {
                    httpRequestMessage = new HttpRequestMessage(httpMethod, endpoint)
                    {
                        Content = new StringContent(queryParameters, Encoding.UTF8, "application/x-www-form-urlencoded")
                    };
                }
                else
                {
                    httpRequestMessage = new HttpRequestMessage(httpMethod, twitterQuery.Url);
                }


                var response = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                _currentResponseHttpStatusCode = (int) response.StatusCode;
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

                if (ex is AggregateException && ex.InnerException is WebException webException)
                {
                    HandleWebException(webException);
                }

                LastException = ex;
                SetStreamState(StreamState.Stop);
            }

            return null;
        }

        private string GetJsonResponseFromReader(StreamReader reader, ITwitterRequest request)
        {
            var requestTask = reader.ReadLineAsync();

            var resultingTask = Task.WhenAny(requestTask, Task.Delay(STREAM_DISCONNECTED_DELAY)).Result;

            if (resultingTask != requestTask)
            {
                requestTask.ContinueWith(json =>
                {
                    // We want to ensure that we are properly handling reuqest Tasks exceptions
                    // so that no scheduler actually receive any potential exception received.
                }, TaskContinuationOptions.OnlyOnFaulted);

                var twitterTimeoutException = new TwitterTimeoutException(request);
                throw twitterTimeoutException;
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
                _currentStreamReader = GetStreamReader(_currentHttpClient, _twitterRequest).Result;
                return true;
            }

            if (numberOfRepeatedFailures == 2)
            {
                _currentStreamReader.Dispose();
                _currentHttpClient.Dispose();

                _currentHttpClient = GetHttpClient(_twitterRequest);
                _currentStreamReader = GetStreamReader(_currentHttpClient, _twitterRequest).Result;
                return true;
            }

            return false;
        }

        private bool ShouldContinueAfterHandlingException(Exception ex)
        {
            // NOTE : I am aware that all the paths return false.
            // But having such a method to control whether the stream needs to
            // continue running in case of Exception is great.

            if (ex is AggregateException aex)
            {
                ex = aex.InnerException;
            }

            LastException = ex;

            if (ex is TwitterTimeoutException timeoutException)
            {
                LastException = timeoutException;
                return false;
            }

            if (ex is WebException webException)
            {
                HandleWebException(webException);

                // Even though the user has asked to specifically handle WebException,
                // We need to inform him that something has gone wrong. 
                // Therefore the connection needs to be stopped with the appropriate information.
                return false;
            }

            var isExceptionThrownByStreamBeingStoppedByUser = LastException is IOException && StreamState == StreamState.Stop;
            if (isExceptionThrownByStreamBeingStoppedByUser)
            {
                LastException = null;
            }

            return false;
        }

        private void HandleWebException(WebException wex)
        {
            LastException = _exceptionHandler.GenerateTwitterException(wex, _twitterRequest, _currentResponseHttpStatusCode);

            if (!_exceptionHandler.SwallowWebExceptions)
            {
                SetStreamState(StreamState.Stop);
                throw LastException;
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
            _currentStreamReader?.Dispose();
            _currentHttpClient?.Dispose();

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