using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Parameters.HelpClient;

namespace Tweetinvi.Credentials.RateLimit
{
    public class RateLimitAwaiter : IRateLimitAwaiter
    {
        private readonly IRateLimitCacheManager _rateLimitCacheManager;
        private readonly ITaskDelayer _taskDelayer;
        private readonly IWeakEvent<EventHandler<QueryAwaitingEventArgs>> _queryAwaitingForRateLimitWeakEvent;

        public RateLimitAwaiter(
            IRateLimitCacheManager rateLimitCacheManager,
            ITaskDelayer taskDelayer,
            IWeakEvent<EventHandler<QueryAwaitingEventArgs>> queryAwaitingForRateLimitWeakEvent)
        {
            _rateLimitCacheManager = rateLimitCacheManager;
            _taskDelayer = taskDelayer;
            _queryAwaitingForRateLimitWeakEvent = queryAwaitingForRateLimitWeakEvent;
        }

        public event EventHandler<QueryAwaitingEventArgs> QueryAwaitingForRateLimit
        {
            add => _queryAwaitingForRateLimitWeakEvent.AddHandler(value);
            remove => _queryAwaitingForRateLimitWeakEvent.RemoveHandler(value);
        }

        public Task WaitForCredentialsRateLimit(ITwitterRequest request)
        {
            return WaitForCredentialsRateLimit(request.Query.Url, request.Query.TwitterCredentials, request.ExecutionContext);
        }

        public async Task WaitForCredentialsRateLimit(string query, IReadOnlyTwitterCredentials credentials, ITwitterExecutionContext executionContext)
        {
            var queryRateLimit = await _rateLimitCacheManager.GetQueryRateLimit(new GetEndpointRateLimitsParameters(query, RateLimitsSource.CacheOnly), credentials).ConfigureAwait(false);
            if (queryRateLimit == null)
            {
                return;
            }

            var timeToWait = GetTimeToWaitFromQueryRateLimit(queryRateLimit, executionContext);
            if (timeToWait > TimeSpan.Zero)
            {
                _queryAwaitingForRateLimitWeakEvent.Raise(this, new QueryAwaitingEventArgs(query, queryRateLimit, credentials));
                await _taskDelayer.Delay(timeToWait).ConfigureAwait(false);
            }
        }

        public async Task WaitForCredentialsRateLimit(IEndpointRateLimit queryRateLimit, IReadOnlyTwitterCredentials credentials, ITwitterExecutionContext executionContext)
        {
            var timeToWait = GetTimeToWaitFromQueryRateLimit(queryRateLimit, executionContext);
            if (timeToWait > TimeSpan.Zero)
            {
                _queryAwaitingForRateLimitWeakEvent.Raise(this, new QueryAwaitingEventArgs(null, queryRateLimit, credentials));
                await _taskDelayer.Delay(timeToWait).ConfigureAwait(false);
            }
        }

        public async Task<TimeSpan> TimeToWaitBeforeTwitterRequest(string query, IReadOnlyTwitterCredentials credentials, ITwitterExecutionContext twitterExecutionContext)
        {
            var queryRateLimits = await _rateLimitCacheManager.GetQueryRateLimit(new GetEndpointRateLimitsParameters(query), credentials).ConfigureAwait(false);
            return GetTimeToWaitFromQueryRateLimit(queryRateLimits, twitterExecutionContext);
        }

        public TimeSpan GetTimeToWaitFromQueryRateLimit(IEndpointRateLimit queryRateLimit, ITwitterExecutionContext executionContext)
        {
            if (queryRateLimit == null || queryRateLimit.Remaining > 0)
            {
                return TimeSpan.Zero;
            }

            var timeToWaitInMs = (int) Math.Ceiling(queryRateLimit.ResetDateTimeInMilliseconds) + executionContext.RateLimitWaitFudgeMs;
            return TimeSpan.FromMilliseconds(timeToWaitInMs);
        }
    }
}