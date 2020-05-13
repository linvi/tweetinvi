using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Streaming;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Tweetinvi.Streams
{
    public interface IStreamTaskFactory
    {
        IStreamTask Create(Func<string, bool> onJsonReceivedCallback, Func<ITwitterRequest> createTwitterRequest);
    }

    public class StreamTaskFactory : IStreamTaskFactory
    {
        private readonly ITweetinviEvents _tweetinviEvents;
        private readonly ITwitterExceptionFactory _twitterExceptionFactory;
        private readonly IHttpClientWebHelper _httpClientWebHelper;

        public StreamTaskFactory(
            ITweetinviEvents tweetinviEvents,
            ITwitterExceptionFactory twitterExceptionFactory,
            IHttpClientWebHelper httpClientWebHelper)
        {
            _tweetinviEvents = tweetinviEvents;
            _twitterExceptionFactory = twitterExceptionFactory;
            _httpClientWebHelper = httpClientWebHelper;
        }

        public IStreamTask Create(Func<string, bool> onJsonReceivedCallback, Func<ITwitterRequest> createTwitterRequest)
        {
            return new StreamTask(onJsonReceivedCallback, createTwitterRequest, _tweetinviEvents, _twitterExceptionFactory, _httpClientWebHelper);
        }
    }

    public class StreamTaskStateChangedEventArgs
    {
        public StreamTaskStateChangedEventArgs(StreamState state)
        {
            State = state;
        }

        public StreamState State { get; set; }
        public Exception Exception { get; set; }
    }

    public interface IStreamTask
    {
        event EventHandler StreamStarted;
        event EventHandler<StreamTaskStateChangedEventArgs> StreamStateChanged;
        event EventHandler KeepAliveReceived;

        StreamState StreamState { get; }

        Task StartAsync();
        void Resume();
        void Pause();
        void Stop();
    }

    public class StreamTask : IStreamTask
    {
        public event EventHandler StreamStarted;
        public event EventHandler<StreamTaskStateChangedEventArgs> StreamStateChanged;
        public event EventHandler KeepAliveReceived;

        // https://dev.twitter.com/streaming/overview/connecting#stalls
        private const int STREAM_DISCONNECTED_DELAY = 90000;
        private const int STREAM_RESUME_DELAY = 20;

        private readonly Func<string, bool> _onJsonReceivedCallback;
        private readonly Func<ITwitterRequest> _createTwitterRequest;
        private readonly ITweetinviEvents _tweetinviEvents;
        private readonly ITwitterExceptionFactory _twitterExceptionFactory;
        private readonly IHttpClientWebHelper _httpClientWebHelper;

        private bool _isNew;

        private ITwitterRequest _twitterRequest;
        private StreamReader _currentStreamReader;
        private HttpClient _currentHttpClient;
        private int _currentResponseHttpStatusCode = -1;

        public StreamTask(
            Func<string, bool> onJsonReceivedCallback,
            Func<ITwitterRequest> createTwitterRequest,
            ITweetinviEvents tweetinviEvents,
            ITwitterExceptionFactory twitterExceptionFactory,
            IHttpClientWebHelper httpClientWebHelper)
        {
            _onJsonReceivedCallback = onJsonReceivedCallback;
            _createTwitterRequest = createTwitterRequest;
            _tweetinviEvents = tweetinviEvents;
            _twitterExceptionFactory = twitterExceptionFactory;
            _httpClientWebHelper = httpClientWebHelper;
            _isNew = true;
        }

        public StreamState StreamState { get; private set; }

        public async Task StartAsync()
        {
            if (StreamState == StreamState.Stop && !_isNew)
            {
                return;
            }

            this.Raise(StreamStarted);
            SetStreamState(StreamState.Running, null);

            _twitterRequest = _createTwitterRequest();

            if (_twitterRequest.Query.TwitterCredentials == null)
            {
                throw new TwitterNullCredentialsException();
            }

            if (!_twitterRequest.Query.TwitterCredentials.AreSetupForUserAuthentication())
            {
                throw new TwitterInvalidCredentialsException(_twitterRequest.Query.TwitterCredentials);
            }

            await RunStreamAsync().ConfigureAwait(false);
        }

        private async Task RunStreamAsync()
        {
            try
            {
                _currentHttpClient = GetHttpClient(_twitterRequest);
                _currentStreamReader = await GetStreamReaderAsync(_currentHttpClient, _twitterRequest).ConfigureAwait(false);

                var numberOfRepeatedFailures = 0;

                while (StreamState != StreamState.Stop)
                {
                    if (StreamState == StreamState.Pause)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(STREAM_RESUME_DELAY));
                        continue;
                    }

                    var json = await GetJsonResponseFromReaderAsync(_currentStreamReader, _twitterRequest).ConfigureAwait(false);

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

                    if (StreamState == StreamState.Running && !_onJsonReceivedCallback(json))
                    {
                        SetStreamState(StreamState.Stop, null);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                var exceptionToThrow = GetExceptionToThrow(ex);
                SetStreamState(StreamState.Stop, exceptionToThrow);

                if (exceptionToThrow != null)
                {
                    throw exceptionToThrow;
                }
            }
            finally
            {
                _currentStreamReader?.Dispose();
                _currentHttpClient?.Dispose();
            }
        }

        private HttpClient GetHttpClient(ITwitterRequest request)
        {
            if (request.Query == null)
            {
                SetStreamState(StreamState.Stop, null);
                return null;
            }

            request.Query.Timeout = Timeout.InfiniteTimeSpan;

            var queryBeforeExecuteEventArgs = new BeforeExecutingRequestEventArgs(request.Query);
            _tweetinviEvents.RaiseBeforeWaitingForQueryRateLimits(queryBeforeExecuteEventArgs);

            if (queryBeforeExecuteEventArgs.Cancel)
            {
                SetStreamState(StreamState.Stop, null);
                return null;
            }

            return _httpClientWebHelper.GetHttpClient(request.Query);
        }

        private async Task<StreamReader> GetStreamReaderAsync(HttpClient client, ITwitterRequest request)
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
                var body = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                return new StreamReader(body, Encoding.GetEncoding("utf-8"));
            }
            catch (Exception)
            {
                client?.Dispose();
                throw;
            }
        }

        private static async Task<string> GetJsonResponseFromReaderAsync(StreamReader reader, ITwitterRequest request)
        {
            var requestTask = reader.ReadLineAsync();
            var resultingTask = await Task.WhenAny(requestTask, Task.Delay(STREAM_DISCONNECTED_DELAY)).ConfigureAwait(false);

            var timedOut = resultingTask != requestTask;
            if (timedOut)
            {
#pragma warning disable 4014
                requestTask.ContinueWith(json =>
#pragma warning restore 4014
                {
                    // We want to ensure that we are properly handling request Tasks exceptions
                    // so that no scheduler actually receive any exception received.
                }, TaskContinuationOptions.OnlyOnFaulted);

                var twitterTimeoutException = new TwitterTimeoutException(request);
                throw twitterTimeoutException;
            }

            var jsonResponse = await requestTask.ConfigureAwait(false);
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
                _currentStreamReader = GetStreamReaderAsync(_currentHttpClient, _twitterRequest).Result;
                return true;
            }

            if (numberOfRepeatedFailures == 2)
            {
                _currentStreamReader.Dispose();
                _currentHttpClient.Dispose();

                _currentHttpClient = GetHttpClient(_twitterRequest);
                _currentStreamReader = GetStreamReaderAsync(_currentHttpClient, _twitterRequest).Result;
                return true;
            }

            return false;
        }

        private Exception GetExceptionToThrow(Exception ex)
        {
            if (ex is AggregateException aex)
            {
                ex = aex.InnerException;
            }

            if (ex is WebException webException)
            {
                return _twitterExceptionFactory.Create(webException, _twitterRequest, _currentResponseHttpStatusCode);
            }

            var exceptionThrownBecauseStreamIsBeingStoppedByUser = ex is IOException && StreamState == StreamState.Stop;
            if (exceptionThrownBecauseStreamIsBeingStoppedByUser)
            {
                return null;
            }

            return ex;
        }

        public void Resume()
        {
            SetStreamState(StreamState.Running, null);
        }

        public void Pause()
        {
            SetStreamState(StreamState.Pause, null);
        }

        public void Stop()
        {
            _currentStreamReader?.Dispose();
            _currentHttpClient?.Dispose();

            SetStreamState(StreamState.Stop, null);
        }

        private void SetStreamState(StreamState state, Exception exception)
        {
            if (StreamState == state)
            {
                return;
            }

            if (_isNew && state == StreamState.Running)
            {
                _isNew = false;
            }

            StreamState = state;

            this.Raise(StreamStateChanged, new StreamTaskStateChangedEventArgs(state)
            {
                Exception = exception
            });
        }
    }
}