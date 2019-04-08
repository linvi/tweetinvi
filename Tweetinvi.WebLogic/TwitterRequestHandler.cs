using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Core.Web;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi.WebLogic
{
    public interface ITwitterRequestHandler
    {
        Task<IWebRequestResult> ExecuteQuery(
            string queryURL,
            HttpMethod method,
            TwitterClientHandler handler = null,
            ITwitterCredentials credentials = null,
            HttpContent httpContent = null);

        Task<string> ExecuteMultipartQuery(IMultipartHttpRequestParameters parameters);
        Task<byte[]> DownloadBinary(string url);

        Task<ITwitterQuery> GetTwitterQuery(
            IHttpRequestParameters requestParameters,
            RateLimitTrackerMode rateLimitTrackerMode,
            ITwitterCredentials credentials = null);
    }

    public class TwitterRequestHandler : ITwitterRequestHandler
    {
        private readonly ITweetinviEvents _tweetinviEvents;
        private readonly IRateLimitAwaiter _rateLimitAwaiter;
        private readonly IRateLimitUpdater _rateLimitUpdater;
        private readonly IRateLimitCacheManager _rateLimitCacheManager;
        private readonly IWebRequestExecutor _webRequestExecutor;
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;
        private readonly ITwitterQueryFactory _twitterQueryFactory;
        private readonly IOAuthWebRequestGenerator _oAuthWebRequestGenerator;

        public TwitterRequestHandler(
            ITweetinviEvents tweetinviEvents,
            IRateLimitAwaiter rateLimitAwaiter,
            IRateLimitUpdater rateLimitUpdater,
            IRateLimitCacheManager rateLimitCacheManager,
            IWebRequestExecutor webRequestExecutor,
            ICredentialsAccessor credentialsAccessor,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor,
            ITwitterQueryFactory twitterQueryFactory,
            IOAuthWebRequestGenerator oAuthWebRequestGenerator)
        {
            _tweetinviEvents = tweetinviEvents;
            _rateLimitAwaiter = rateLimitAwaiter;
            _rateLimitUpdater = rateLimitUpdater;
            _rateLimitCacheManager = rateLimitCacheManager;
            _webRequestExecutor = webRequestExecutor;
            _credentialsAccessor = credentialsAccessor;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
            _twitterQueryFactory = twitterQueryFactory;
            _oAuthWebRequestGenerator = oAuthWebRequestGenerator;
        }

        public async Task<IWebRequestResult> ExecuteQuery(
            string queryURL,
            HttpMethod httpMethod,
            TwitterClientHandler handler = null,
            ITwitterCredentials credentials = null,
            HttpContent httpContent = null)
        {
            queryURL = CleanupQueryURL(queryURL);
            var rateLimitTrackerOption = _tweetinviSettingsAccessor.RateLimitTrackerMode;

            var requestParameters = new HttpRequestParameters
            {
                Query = queryURL,
                HttpMethod = httpMethod,
                HttpContent = httpContent
            };

            var asyncOperation = await TryPrepareRequest(requestParameters, rateLimitTrackerOption, credentials, true);

            if (!asyncOperation.Success)
            {
                return null;
            }

            var twitterQuery = asyncOperation.Result;

            try
            {
                var webRequestResult = await _webRequestExecutor.ExecuteQuery(twitterQuery, handler);

                QueryCompleted(twitterQuery, webRequestResult, rateLimitTrackerOption);

                return webRequestResult;
            }
            catch (TwitterException ex)
            {
                HandleException(queryURL, rateLimitTrackerOption, ex, twitterQuery);

                throw;
            }
        }

        public async Task<string> ExecuteMultipartQuery(IMultipartHttpRequestParameters parameters)
        {
            var queryURL = parameters.Query;

            CleanupQueryURL(queryURL);
            var rateLimitTrackerOption = _tweetinviSettingsAccessor.RateLimitTrackerMode;

            parameters.Timeout = parameters.Timeout ?? TimeSpan.FromMilliseconds(_tweetinviSettingsAccessor.UploadTimeout);

            var asyncOperation = await TryPrepareRequest(parameters, rateLimitTrackerOption, null, true);

            if (!asyncOperation.Success)
            {
                return null;
            }

            var twitterQuery = asyncOperation.Result;

            try
            {
                IWebRequestResult webRequestResult;

                if (parameters.Binaries.IsNullOrEmpty())
                {
                    webRequestResult = await _webRequestExecutor.ExecuteQuery(twitterQuery);
                }
                else
                {
                    webRequestResult = await _webRequestExecutor.ExecuteMultipartQuery(twitterQuery, parameters.ContentId, parameters.Binaries);
                }

                QueryCompleted(twitterQuery, webRequestResult, rateLimitTrackerOption);

                return webRequestResult.Text;
            }
            catch (TwitterException ex)
            {
                HandleException(queryURL, rateLimitTrackerOption, ex, twitterQuery);

                throw;
            }
        }

        public async Task<byte[]> DownloadBinary(string url)
        {
            var rateLimitTrackerMode = _tweetinviSettingsAccessor.RateLimitTrackerMode;
            var requestParameters = new HttpRequestParameters
            {
                Query = url,
                HttpMethod = HttpMethod.GET,
                HttpContent = null
            };

            var asyncOperation = await TryPrepareRequest(requestParameters, rateLimitTrackerMode, null, true);

            if (!asyncOperation.Success)
            {
                return null;
            }

            var twitterQuery = asyncOperation.Result;

            try
            {
                byte[] binary = null;

                var webRequestResult = await _webRequestExecutor.ExecuteQuery(twitterQuery);

                if (webRequestResult.IsSuccessStatusCode)
                {
                    binary = webRequestResult.Binary;
                }

                QueryCompleted(twitterQuery, webRequestResult, rateLimitTrackerMode);

                return binary;
            }
            catch (TwitterException ex)
            {
                HandleException(url, rateLimitTrackerMode, ex, twitterQuery);

                throw;
            }
        }

        public async Task<ITwitterQuery> GetTwitterQuery(
            IHttpRequestParameters requestParameters,
            RateLimitTrackerMode rateLimitTrackerMode,
            ITwitterCredentials credentials = null)
        {
            var asyncOperation  = await TryPrepareRequest(requestParameters, rateLimitTrackerMode, credentials, false);

            if (asyncOperation.Success)
            {
                var twitterQuery = asyncOperation.Result;
                _oAuthWebRequestGenerator.SetTwitterQueryAuthorizationHeader(twitterQuery);
                return twitterQuery;
            }

            return null;
        }

        #region Helper Methods
        private string CleanupQueryURL(string query)
        {
            var index = query.IndexOf("?", StringComparison.OrdinalIgnoreCase);
            if (index > 0)
            {
                if (query.Length == index + 1)
                {
                    query = query.Remove(index);
                    return query;
                }

                if (query.Length > index && query[index + 1] == '&')
                {
                    query = query.Remove(index + 1, 1);
                }
            }

            return query;
        }



        private async Task<AsyncOperation<ITwitterQuery>> TryPrepareRequest(
            IHttpRequestParameters requestParameters,
            RateLimitTrackerMode rateLimitTrackerMode,
            ITwitterCredentials credentials,
            bool raiseEvents)
        {
            credentials = credentials ?? _credentialsAccessor.CurrentThreadCredentials;

            if (credentials == null)
            {
                throw new TwitterNullCredentialsException();
            }

            var twitterQuery = _twitterQueryFactory.Create(requestParameters.Query, requestParameters.HttpMethod, credentials);
            twitterQuery.HttpContent = requestParameters.HttpContent;
            twitterQuery.Timeout = requestParameters.Timeout ?? twitterQuery.Timeout;

            var beforeQueryExecuteEventArgs = new QueryBeforeExecuteEventArgs(twitterQuery);


            if (rateLimitTrackerMode == RateLimitTrackerMode.TrackOnly ||
                rateLimitTrackerMode == RateLimitTrackerMode.TrackAndAwait)
            {
                // Use the RateLimitCacheManager instead of RateLimitHelper to get the queryRateLimits to ensure the cache is up to date!
                var credentialRateLimits = await _rateLimitCacheManager.GetCredentialsRateLimits(twitterQuery.TwitterCredentials);

                IEndpointRateLimit queryRateLimit = null;

                // If we were not able to retrieve the credentials few ms before there is no reason why it would work now.
                if (credentialRateLimits != null)
                {
                    queryRateLimit = await _rateLimitCacheManager.GetQueryRateLimit(requestParameters.Query, twitterQuery.TwitterCredentials);
                }

                var timeToWait = _rateLimitAwaiter.GetTimeToWaitFromQueryRateLimit(queryRateLimit);

                twitterQuery.CredentialsRateLimits = credentialRateLimits;
                twitterQuery.QueryRateLimit = queryRateLimit;
                twitterQuery.DateWhenCredentialsWillHaveTheRequiredRateLimits = DateTime.Now.AddMilliseconds(timeToWait);

                if (raiseEvents)
                {
                    _tweetinviEvents.RaiseBeforeQueryExecute(beforeQueryExecuteEventArgs);
                }


                if (beforeQueryExecuteEventArgs.Cancel)
                {
                    return new FailedAsyncOperation<ITwitterQuery>();
                }

                if (rateLimitTrackerMode == RateLimitTrackerMode.TrackAndAwait)
                {
                    _rateLimitAwaiter.Wait(timeToWait);
                }
            }
            else
            {
                if (raiseEvents)
                {
                    _tweetinviEvents.RaiseBeforeQueryExecute(beforeQueryExecuteEventArgs);
                }

                if (beforeQueryExecuteEventArgs.Cancel)
                {
                    return new FailedAsyncOperation<ITwitterQuery>();
                }
            }

            if (raiseEvents)
            {
                _tweetinviEvents.RaiseBeforeExecuteAfterRateLimitAwait(beforeQueryExecuteEventArgs);
            }

            return new AsyncOperation<ITwitterQuery>
            {
                Result = twitterQuery
            };
        }

        private void QueryCompleted(ITwitterQuery twitterQuery, IWebRequestResult webRequestResult, RateLimitTrackerMode rateLimitTrackerMode)
        {
            if (rateLimitTrackerMode != RateLimitTrackerMode.None)
            {
                var rateLimitHeaders = webRequestResult.Headers.Where(kvp => kvp.Key.StartsWith("x-rate-limit-")).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                _rateLimitUpdater.QueryExecuted(twitterQuery.QueryURL, twitterQuery.TwitterCredentials, rateLimitHeaders);
            }

            _tweetinviEvents.RaiseAfterQueryExecuted(new QueryAfterExecuteEventArgs(twitterQuery, webRequestResult.Text, webRequestResult.Headers));
        }

        private void HandleException(
            string queryURL, 
            RateLimitTrackerMode rateLimitTrackerMode, 
            TwitterException exception, 
            ITwitterQuery queryParameter)
        {
            var statusCode = exception.StatusCode;
            if (rateLimitTrackerMode != RateLimitTrackerMode.None && statusCode == TweetinviConsts.STATUS_CODE_TOO_MANY_REQUEST)
            {
                _rateLimitUpdater.ClearRateLimitsForQuery(queryURL);
            }

            _tweetinviEvents.RaiseAfterQueryExecuted(new QueryAfterExecuteExceptionEventArgs(queryParameter, exception));
        }

        #endregion
    }
}