using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Core.Web;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters.HelpClient;

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
        private readonly IRateLimitUpdaterFactory _rateLimitUpdaterFactory;
        private readonly IWebRequestExecutor _webRequestExecutor;

        public TwitterRequestHandler(
            ITweetinviEvents tweetinviEvents,
            IRateLimitAwaiter rateLimitAwaiter,
            IRateLimitUpdaterFactory rateLimitUpdaterFactory,
            IWebRequestExecutor webRequestExecutor)
        {
            _tweetinviEvents = tweetinviEvents;
            _rateLimitAwaiter = rateLimitAwaiter;
            _rateLimitUpdaterFactory = rateLimitUpdaterFactory;
            _webRequestExecutor = webRequestExecutor;
        }

        public async Task<ITwitterResponse> ExecuteQuery(ITwitterRequest request)
        {
            var rateLimitUpdater = _rateLimitUpdaterFactory.Create(request.ExecutionContext.RateLimitCacheManager);

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

                QueryCompleted(request, twitterResponse, rateLimitUpdater);

                return twitterResponse;
            }
            catch (TwitterException ex)
            {
                HandleException(request, ex, rateLimitUpdater);

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
                var credentialRateLimits = await request.ExecutionContext.RateLimitCacheManager.GetCredentialsRateLimits(twitterQuery.TwitterCredentials).ConfigureAwait(false);

                IEndpointRateLimit queryRateLimit = null;

                // If we were not able to retrieve the credentials few ms before there is no reason why it would work now.
                if (credentialRateLimits != null)
                {
                    var getEndpointRateLimitsFromCache = new GetEndpointRateLimitsParameters(twitterQuery.Url, RateLimitsSource.CacheOnly);
                    queryRateLimit = await request.ExecutionContext.RateLimitCacheManager.GetQueryRateLimit(getEndpointRateLimitsFromCache, twitterQuery.TwitterCredentials).ConfigureAwait(false);
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

        private void QueryCompleted(
        ITwitterRequest request,
        ITwitterResponse twitterResponse,
        IRateLimitUpdater rateLimitUpdater)
        {
            if (request.ExecutionContext.RateLimitTrackerMode != RateLimitTrackerMode.None)
            {
                var rateLimitHeaders = twitterResponse.Headers.Where(kvp => kvp.Key.StartsWith("x-rate-limit-")).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                rateLimitUpdater.QueryExecuted(request.Query.Url, request.Query.TwitterCredentials, rateLimitHeaders);
            }

            _tweetinviEvents.RaiseAfterQueryExecuted(new QueryAfterExecuteEventArgs(request.Query, twitterResponse.Text, twitterResponse.Headers));
        }

        private void HandleException(
            ITwitterRequest request,
            TwitterException exception,
            IRateLimitUpdater rateLimitUpdater)
        {
            var statusCode = exception.StatusCode;
            if (request.ExecutionContext.RateLimitTrackerMode != RateLimitTrackerMode.None && statusCode == TweetinviConsts.STATUS_CODE_TOO_MANY_REQUEST)
            {
                rateLimitUpdater.ClearRateLimitsForQuery(request.Query.Url, request.Query.TwitterCredentials);
            }

            _tweetinviEvents.RaiseAfterQueryExecuted(new QueryAfterExecuteExceptionEventArgs(request.Query, exception));
        }

        #endregion
    }
}