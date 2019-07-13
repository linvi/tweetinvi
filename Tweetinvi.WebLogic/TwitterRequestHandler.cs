using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Core.Web;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Models.Interfaces;

namespace Tweetinvi.WebLogic
{
    public interface ITwitterRequestHandler
    {
        Task<ITwitterResponse> ExecuteQuery(ITwitterRequest request);

        Task PrepareTwitterRequest(ITwitterRequest request);
    }

    public class TwitterRequestHandler : ITwitterRequestHandler
    {
        private readonly ITweetinviEvents _tweetinviEvents;
        private readonly IRateLimitAwaiter _rateLimitAwaiter;
        private readonly IRateLimitUpdater _rateLimitUpdater;
        private readonly IRateLimitCacheManager _rateLimitCacheManager;
        private readonly IWebRequestExecutor _webRequestExecutor;

        public TwitterRequestHandler(
            ITweetinviEvents tweetinviEvents,
            IRateLimitAwaiter rateLimitAwaiter,
            IRateLimitUpdater rateLimitUpdater,
            IRateLimitCacheManager rateLimitCacheManager,
            IWebRequestExecutor webRequestExecutor)
        {
            _tweetinviEvents = tweetinviEvents;
            _rateLimitAwaiter = rateLimitAwaiter;
            _rateLimitUpdater = rateLimitUpdater;
            _rateLimitCacheManager = rateLimitCacheManager;
            _webRequestExecutor = webRequestExecutor;
        }

        public async Task<ITwitterResponse> ExecuteQuery(ITwitterRequest request)
        {
            await PrepareTwitterRequest(request);

            var beforeQueryExecuteEventArgs = new QueryBeforeExecuteEventArgs(request.Query);
            _tweetinviEvents.RaiseBeforeQueryExecute(beforeQueryExecuteEventArgs);

            if (beforeQueryExecuteEventArgs.Cancel)
            {
                throw new OperationCanceledException("Operation was cancelled intentionally.");
            }

            await WaitBeforeExecutingQuery(request);

            _tweetinviEvents.RaiseBeforeExecuteAfterRateLimitAwait(beforeQueryExecuteEventArgs);

            try
            {
                ITwitterResponse twitterResponse;

                var multiPartRequest = request.Query.MultipartHttpRequest;

                if (multiPartRequest == null || multiPartRequest.Binaries.IsNullOrEmpty())
                {
                    twitterResponse = await _webRequestExecutor.ExecuteQuery(request.Query, request.TwitterClientHandler);
                }
                else
                {
                    twitterResponse = await _webRequestExecutor.ExecuteMultipartQuery(request.Query, multiPartRequest.ContentId, multiPartRequest.Binaries);
                }

                QueryCompleted(request.Query, twitterResponse, request.Config.RateLimitTrackerMode);

                return twitterResponse;
            }
            catch (TwitterException ex)
            {
                HandleException(request.Query.Url, request.Config.RateLimitTrackerMode, ex, request.Query);

                throw;
            }
        }

        public async Task PrepareTwitterRequest(ITwitterRequest request)
        {
            var twitterQuery = request.Query;
            twitterQuery.Url = CleanupQueryURL(twitterQuery.Url); // TODO : THIS LOGIC SHOULD HAPPEN BEFORE ARRIVING HERE

            var rateLimitTrackerMode = request.Config.RateLimitTrackerMode;

            if (rateLimitTrackerMode == RateLimitTrackerMode.TrackOnly ||
                rateLimitTrackerMode == RateLimitTrackerMode.TrackAndAwait)
            {
                // Use the RateLimitCacheManager instead of RateLimitHelper to get the queryRateLimits to ensure the cache is up to date!
                var credentialRateLimits = await _rateLimitCacheManager.GetCredentialsRateLimits(twitterQuery.TwitterCredentials);

                IEndpointRateLimit queryRateLimit = null;

                // If we were not able to retrieve the credentials few ms before there is no reason why it would work now.
                if (credentialRateLimits != null)
                {
                    queryRateLimit = await _rateLimitCacheManager.GetQueryRateLimit(twitterQuery.Url, twitterQuery.TwitterCredentials);
                }

                var timeToWait = _rateLimitAwaiter.GetTimeToWaitFromQueryRateLimit(queryRateLimit);

                twitterQuery.CredentialsRateLimits = credentialRateLimits;
                twitterQuery.QueryRateLimit = queryRateLimit;
                twitterQuery.DateWhenCredentialsWillHaveTheRequiredRateLimits = DateTime.UtcNow.AddMilliseconds(timeToWait);
            }
        }

        private async Task WaitBeforeExecutingQuery(ITwitterRequest twitterRequest)
        {
            var twitterQuery = twitterRequest.Query;

            if (twitterQuery.DateWhenCredentialsWillHaveTheRequiredRateLimits == null)
            {
                return;
            }

            if (twitterRequest.Config.RateLimitTrackerMode != RateLimitTrackerMode.TrackAndAwait)
            {
                return;
            }

            var timeToWait = (int)twitterQuery.DateWhenCredentialsWillHaveTheRequiredRateLimits?.Subtract(DateTime.UtcNow).TotalMilliseconds;

            await Task.Delay(timeToWait);
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

        private void QueryCompleted(ITwitterQuery twitterQuery, ITwitterResponse twitterResponse, RateLimitTrackerMode rateLimitTrackerMode)
        {
            if (rateLimitTrackerMode != RateLimitTrackerMode.None)
            {
                var rateLimitHeaders = twitterResponse.Headers.Where(kvp => kvp.Key.StartsWith("x-rate-limit-")).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                _rateLimitUpdater.QueryExecuted(twitterQuery.Url, twitterQuery.TwitterCredentials, rateLimitHeaders);
            }

            _tweetinviEvents.RaiseAfterQueryExecuted(new QueryAfterExecuteEventArgs(twitterQuery, twitterResponse.Text, twitterResponse.Headers));
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