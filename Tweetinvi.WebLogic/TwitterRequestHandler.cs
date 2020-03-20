using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Core.Web;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.WebLogic
{
    public interface ITwitterRequestHandler
    {
        Task<ITwitterResponse> ExecuteQuery(ITwitterRequest request);

        Task PrepareTwitterRequest(ITwitterRequest request);
    }

    public class TwitterRequestHandler : ITwitterRequestHandler
    {
        private readonly IRateLimitAwaiter _rateLimitAwaiter;
        private readonly IRateLimitCacheManager _rateLimitCacheManager;
        private readonly IRateLimitUpdaterFactory _rateLimitUpdaterFactory;
        private readonly IWebRequestExecutor _webRequestExecutor;

        public TwitterRequestHandler(
            IRateLimitAwaiter rateLimitAwaiter,
            IRateLimitCacheManager rateLimitCacheManager,
            IRateLimitUpdaterFactory rateLimitUpdaterFactory,
            IWebRequestExecutor webRequestExecutor)
        {
            _rateLimitAwaiter = rateLimitAwaiter;
            _rateLimitCacheManager = rateLimitCacheManager;
            _rateLimitUpdaterFactory = rateLimitUpdaterFactory;
            _webRequestExecutor = webRequestExecutor;
        }

        public async Task<ITwitterResponse> ExecuteQuery(ITwitterRequest request)
        {
            var rateLimitUpdater = _rateLimitUpdaterFactory.Create(_rateLimitCacheManager);

            await PrepareTwitterRequest(request).ConfigureAwait(false);

            var beforeQueryExecuteEventArgs = new BeforeExecutingRequestEventArgs(request.Query);
            request.ExecutionContext.Events.RaiseBeforeWaitingForQueryRateLimits(beforeQueryExecuteEventArgs);

            if (beforeQueryExecuteEventArgs.Cancel)
            {
                throw new OperationCanceledException("Operation was cancelled intentionally.");
            }

            await WaitBeforeExecutingQuery(request).ConfigureAwait(false);

            request.ExecutionContext.Events.RaiseBeforeExecutingQuery(beforeQueryExecuteEventArgs);

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
            twitterQuery.Url = CleanupQueryUrl(twitterQuery.Url); // TODO : THIS LOGIC SHOULD HAPPEN BEFORE ARRIVING HERE

            var rateLimitTrackerMode = request.ExecutionContext.RateLimitTrackerMode;
            if (rateLimitTrackerMode == RateLimitTrackerMode.None)
            {
                return;
            }

            // Use the RateLimitCacheManager instead of RateLimitHelper to get the queryRateLimits to ensure the cache is up to date!
            var credentialRateLimits = await _rateLimitCacheManager.GetCredentialsRateLimits(twitterQuery.TwitterCredentials).ConfigureAwait(false);

            IEndpointRateLimit queryRateLimit = null;

            // If we were not able to retrieve the credentials few ms before there is no reason why it would work now.
            if (credentialRateLimits != null)
            {
                var getEndpointRateLimitsFromCache = new GetEndpointRateLimitsParameters(twitterQuery.Url, RateLimitsSource.CacheOnly);
                queryRateLimit = await _rateLimitCacheManager.GetQueryRateLimit(getEndpointRateLimitsFromCache, twitterQuery.TwitterCredentials).ConfigureAwait(false);
            }

            var timeToWait = _rateLimitAwaiter.GetTimeToWaitFromQueryRateLimit(queryRateLimit, request.ExecutionContext);

            twitterQuery.CredentialsRateLimits = credentialRateLimits;
            twitterQuery.QueryRateLimit = queryRateLimit;
            twitterQuery.DateWhenCredentialsWillHaveTheRequiredRateLimits = DateTime.UtcNow.Add(timeToWait);
        }

        private async Task WaitBeforeExecutingQuery(ITwitterRequest twitterRequest)
        {
            var twitterQuery = twitterRequest.Query;
            if (twitterQuery.DateWhenCredentialsWillHaveTheRequiredRateLimits == null)
            {
                return;
            }

            if (twitterRequest.ExecutionContext.RateLimitTrackerMode == RateLimitTrackerMode.TrackAndAwait)
            {
                await _rateLimitAwaiter.WaitForCredentialsRateLimit(twitterRequest).ConfigureAwait(false);
            }
        }

        #region Helper Methods

        private static string CleanupQueryUrl(string query)
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

            request.ExecutionContext.Events.RaiseAfterExecutingQuery(new AfterExecutingQueryEventArgs(request.Query, twitterResponse.Text, twitterResponse.Headers));
        }

        private void HandleException(
            ITwitterRequest request,
            TwitterException exception,
            IRateLimitUpdater rateLimitUpdater)
        {
            var statusCode = exception.StatusCode;
            const int tooManyRequestStatusCode = 429;
            if (request.ExecutionContext.RateLimitTrackerMode != RateLimitTrackerMode.None && statusCode == tooManyRequestStatusCode)
            {
                rateLimitUpdater.ClearRateLimitsForQuery(request.Query.Url, request.Query.TwitterCredentials);
            }

            request.ExecutionContext.Events.RaiseAfterExecutingQuery(new AfterExecutingQueryExceptionEventArgs(request.Query, exception));
        }

        #endregion
    }
}