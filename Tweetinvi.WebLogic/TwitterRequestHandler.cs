using System;
using System.Linq;
using System.Net.Http;
using Tweetinvi.Core;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Extensions;
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
        string ExecuteQuery(
            string queryURL,
            HttpMethod method,
            TwitterClientHandler handler = null,
            ITwitterCredentials twitterCredentials = null,
            HttpContent httpContent = null);

        string ExecuteMultipartQuery(IMultipartHttpRequestParameters parameters);
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

        public TwitterRequestHandler(
            ITweetinviEvents tweetinviEvents,
            IRateLimitAwaiter rateLimitAwaiter,
            IRateLimitUpdater rateLimitUpdater,
            IRateLimitCacheManager rateLimitCacheManager,
            IWebRequestExecutor webRequestExecutor,
            ICredentialsAccessor credentialsAccessor,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor,
            ITwitterQueryFactory twitterQueryFactory)
        {
            _tweetinviEvents = tweetinviEvents;
            _rateLimitAwaiter = rateLimitAwaiter;
            _rateLimitUpdater = rateLimitUpdater;
            _rateLimitCacheManager = rateLimitCacheManager;
            _webRequestExecutor = webRequestExecutor;
            _credentialsAccessor = credentialsAccessor;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
            _twitterQueryFactory = twitterQueryFactory;
        }

        public string ExecuteQuery(
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

            ITwitterQuery twitterQuery;
            if (!TryPrepareRequest(requestParameters, rateLimitTrackerOption, credentials, out twitterQuery))
            {
                return null;
            }

            try
            {
                var webRequestResult = _webRequestExecutor.ExecuteQuery(twitterQuery, handler);

                QueryCompleted(twitterQuery, webRequestResult, rateLimitTrackerOption);

                return webRequestResult.Response;
            }
            catch (TwitterException ex)
            {
                HandleException(queryURL, rateLimitTrackerOption, ex.StatusCode, twitterQuery);

                throw;
            }
        }

        public string ExecuteMultipartQuery(IMultipartHttpRequestParameters parameters)
        {
            var queryURL = parameters.Query;

            CleanupQueryURL(queryURL);
            var rateLimitTrackerOption = _tweetinviSettingsAccessor.RateLimitTrackerMode;

            parameters.Timeout = parameters.Timeout ?? TimeSpan.FromMilliseconds(_tweetinviSettingsAccessor.UploadTimeout);

            ITwitterQuery twitterQuery;
            if (!TryPrepareRequest(parameters, rateLimitTrackerOption, null, out twitterQuery))
            {
                return null;
            }

            try
            {
                IWebRequestResult webRequestResult;

                if (parameters.Binaries.IsNullOrEmpty())
                {
                    webRequestResult = _webRequestExecutor.ExecuteQuery(twitterQuery);
                }
                else
                {
                    webRequestResult = _webRequestExecutor.ExecuteMultipartQuery(twitterQuery, parameters.ContentId, parameters.Binaries);
                }

                QueryCompleted(twitterQuery, webRequestResult, rateLimitTrackerOption);

                return webRequestResult.Response;
            }
            catch (TwitterException ex)
            {
                HandleException(queryURL, rateLimitTrackerOption, ex.StatusCode, twitterQuery);

                throw;
            }
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

        private bool TryPrepareRequest(
            IHttpRequestParameters requestParameters,
            RateLimitTrackerMode rateLimitTrackerMode,
            ITwitterCredentials credentials,
            out ITwitterQuery twitterQuery)
        {
            credentials = credentials ?? _credentialsAccessor.CurrentThreadCredentials;

            if (credentials == null)
            {
                throw new TwitterNullCredentialsException();
            }

            twitterQuery = _twitterQueryFactory.Create(requestParameters.Query, requestParameters.HttpMethod, credentials);
            twitterQuery.HttpContent = requestParameters.HttpContent;
            twitterQuery.Timeout = requestParameters.Timeout ?? twitterQuery.Timeout;

            var beforeQueryExecuteEventArgs = new QueryBeforeExecuteEventArgs(twitterQuery);


            if (rateLimitTrackerMode == RateLimitTrackerMode.TrackOnly ||
                rateLimitTrackerMode == RateLimitTrackerMode.TrackAndAwait)
            {
                // Use the RateLimitCacheManager instead of RateLimitHelper to get the queryRateLimits to ensure the cache is up to date!
                var credentialRateLimits = _rateLimitCacheManager.GetCredentialsRateLimits(twitterQuery.TwitterCredentials);

                IEndpointRateLimit queryRateLimit = null;

                // If we were not able to retrieve the credentials few ms before there is no reason why it would work now.
                if (credentialRateLimits != null)
                {
                    queryRateLimit = _rateLimitCacheManager.GetQueryRateLimit(requestParameters.Query, twitterQuery.TwitterCredentials);
                }

                var timeToWait = _rateLimitAwaiter.GetTimeToWaitFromQueryRateLimit(queryRateLimit);

                twitterQuery.CredentialsRateLimits = credentialRateLimits;
                twitterQuery.QueryRateLimit = queryRateLimit;
                twitterQuery.DateWhenCredentialsWillHaveTheRequiredRateLimits = DateTime.Now.AddMilliseconds(timeToWait);

                _tweetinviEvents.RaiseBeforeQueryExecute(beforeQueryExecuteEventArgs);

                if (beforeQueryExecuteEventArgs.Cancel)
                {
                    twitterQuery = null;
                    return false;
                }

                if (rateLimitTrackerMode == RateLimitTrackerMode.TrackAndAwait)
                {
                    _rateLimitAwaiter.Wait(timeToWait);
                }
            }
            else
            {
                _tweetinviEvents.RaiseBeforeQueryExecute(beforeQueryExecuteEventArgs);

                if (beforeQueryExecuteEventArgs.Cancel)
                {
                    twitterQuery = null;
                    return false;
                }
            }

            _tweetinviEvents.RaiseBeforeExecuteAfterRateLimitAwait(beforeQueryExecuteEventArgs);

            return true;
        }

        private void QueryCompleted(ITwitterQuery twitterQuery, IWebRequestResult webRequestResult, RateLimitTrackerMode rateLimitTrackerMode)
        {
            if (rateLimitTrackerMode != RateLimitTrackerMode.None)
            {
                var rateLimitHeaders = webRequestResult.Headers.Where(kvp => kvp.Key.StartsWith("x-rate-limit-")).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                _rateLimitUpdater.QueryExecuted(twitterQuery.QueryURL, twitterQuery.TwitterCredentials, rateLimitHeaders);
            }

            _tweetinviEvents.RaiseAfterQueryExecuted(new QueryAfterExecuteEventArgs(twitterQuery, webRequestResult.Response, webRequestResult.Headers));
        }

        private void HandleException(string queryURL, RateLimitTrackerMode rateLimitTrackerMode, int statusCode, ITwitterQuery queryParameter)
        {
            if (rateLimitTrackerMode != RateLimitTrackerMode.None && statusCode == TweetinviConsts.STATUS_CODE_TOO_MANY_REQUEST)
            {
                _rateLimitUpdater.ClearRateLimitsForQuery(queryURL);
            }

            _tweetinviEvents.RaiseAfterQueryExecuted(new QueryAfterExecuteEventArgs(queryParameter, null, null));
        }
        #endregion
    }
}