using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Core.Web;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;

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
            await PrepareTwitterRequest(request).ConfigureAwait(false);

            var beforeQueryExecuteEventArgs = new QueryBeforeExecuteEventArgs(request.Query);
            _tweetinviEvents.RaiseBeforeQueryExecute(beforeQueryExecuteEventArgs);

            if (beforeQueryExecuteEventArgs.Cancel)
            {
                throw new OperationCanceledException("Operation was cancelled intentionally.");
            }

            await WaitBeforeExecutingQuery(request).ConfigureAwait(false);

            _tweetinviEvents.RaiseBeforeExecuteAfterRateLimitAwait(beforeQueryExecuteEventArgs);

            try
            {
                ITwitterResponse twitterResponse;

                if (!(request.Query is IMultipartTwitterQuery))
                {
                    twitterResponse = await _webRequestExecutor.ExecuteQuery(request, request.TwitterClientHandler).ConfigureAwait(false);
                }
                else
                {
                    twitterResponse = await _webRequestExecutor.ExecuteMultipartQuery(request).ConfigureAwait(false);
                }

                QueryCompleted(request, twitterResponse, request.ExecutionContext.RateLimitTrackerMode);

                return twitterResponse;
            }
            catch (TwitterException ex)
            {
                HandleException(request.Query.Url, request.ExecutionContext.RateLimitTrackerMode, ex, request);

                throw;
            }
        }

        public async Task PrepareTwitterRequest(ITwitterRequest request)
        {
            var twitterQuery = request.Query;
            twitterQuery.Url = CleanupQueryURL(twitterQuery.Url); // TODO : THIS LOGIC SHOULD HAPPEN BEFORE ARRIVING HERE

            var rateLimitTrackerMode = request.ExecutionContext.RateLimitTrackerMode;

            if (rateLimitTrackerMode == RateLimitTrackerMode.TrackOnly ||
                rateLimitTrackerMode == RateLimitTrackerMode.TrackAndAwait)
            {
                // Use the RateLimitCacheManager instead of RateLimitHelper to get the queryRateLimits to ensure the cache is up to date!
                var credentialRateLimits = await _rateLimitCacheManager.GetCredentialsRateLimits(twitterQuery.TwitterCredentials).ConfigureAwait(false);

                IEndpointRateLimit queryRateLimit = null;

                // If we were not able to retrieve the credentials few ms before there is no reason why it would work now.
                if (credentialRateLimits != null)
                {
                    queryRateLimit = await _rateLimitCacheManager.GetQueryRateLimit(twitterQuery.Url, twitterQuery.TwitterCredentials).ConfigureAwait(false);
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

            if (twitterRequest.ExecutionContext.RateLimitTrackerMode != RateLimitTrackerMode.TrackAndAwait)
            {
                return;
            }

            var timeToWait = (int)twitterQuery.DateWhenCredentialsWillHaveTheRequiredRateLimits?.Subtract(DateTime.UtcNow).TotalMilliseconds;

            await Task.Delay(timeToWait).ConfigureAwait(false);
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

        private void QueryCompleted(ITwitterRequest request, ITwitterResponse twitterResponse, RateLimitTrackerMode rateLimitTrackerMode)
        {
            if (rateLimitTrackerMode != RateLimitTrackerMode.None)
            {
                var rateLimitHeaders = twitterResponse.Headers.Where(kvp => kvp.Key.StartsWith("x-rate-limit-")).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                _rateLimitUpdater.QueryExecuted(request.Query.Url, request.Query.TwitterCredentials, rateLimitHeaders);
            }

            _tweetinviEvents.RaiseAfterQueryExecuted(new QueryAfterExecuteEventArgs(request.Query, twitterResponse.Text, twitterResponse.Headers));
        }

        private void HandleException(
            string queryURL,
            RateLimitTrackerMode rateLimitTrackerMode,
            TwitterException exception,
            ITwitterRequest request)
        {
            var statusCode = exception.StatusCode;
            if (rateLimitTrackerMode != RateLimitTrackerMode.None && statusCode == TweetinviConsts.STATUS_CODE_TOO_MANY_REQUEST)
            {
                _rateLimitUpdater.ClearRateLimitsForQuery(queryURL);
            }

            _tweetinviEvents.RaiseAfterQueryExecuted(new QueryAfterExecuteExceptionEventArgs(request.Query, exception));
        }

        #endregion
    }
}