using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.RateLimitsClient;

namespace Tweetinvi.Credentials.RateLimit
{
    public class RateLimitAwaiter : IRateLimitAwaiter
    {
        private readonly IRateLimitCacheManager _rateLimitCacheManager;
        private readonly ITaskDelayer _taskDelayer;

        public RateLimitAwaiter(
            IRateLimitCacheManager rateLimitCacheManager,
            ITaskDelayer taskDelayer)
        {
            _rateLimitCacheManager = rateLimitCacheManager;
            _taskDelayer = taskDelayer;
        }

        public Task WaitForCredentialsRateLimitAsync(ITwitterRequest request)
        {
            var credentialsRateLimitParameters = new WaitForCredentialsRateLimitParameters(request)
            {
                From = RateLimitsSource.CacheOnly
            };

            return WaitForCredentialsRateLimitAsync(credentialsRateLimitParameters);
        }

        public async Task WaitForCredentialsRateLimitAsync(IWaitForCredentialsRateLimitParameters parameters)
        {
            var queryRateLimit = await _rateLimitCacheManager.GetQueryRateLimitAsync(new GetEndpointRateLimitsParameters(parameters.Url, parameters.From), parameters.Credentials).ConfigureAwait(false);
            if (queryRateLimit == null)
            {
                return;
            }

            var timeToWait = GetTimeToWaitFromQueryRateLimit(queryRateLimit, parameters.ExecutionContext);
            if (timeToWait > TimeSpan.Zero)
            {
                if (parameters.Request != null)
                {
                    parameters.ExecutionContext.Events.RaiseWaitingForRateLimit(new WaitingForRateLimitEventArgs(parameters.Request, queryRateLimit));
                }
                else
                {
                    parameters.ExecutionContext.Events.RaiseWaitingForRateLimit(new WaitingForRateLimitEventArgs(parameters.Request, queryRateLimit));
                }

                await _taskDelayer.Delay(timeToWait).ConfigureAwait(false);
            }
        }

        public async Task WaitForCredentialsRateLimitAsync(IEndpointRateLimit queryRateLimit, IReadOnlyTwitterCredentials credentials, ITwitterExecutionContext executionContext)
        {
            var timeToWait = GetTimeToWaitFromQueryRateLimit(queryRateLimit, executionContext);
            if (timeToWait > TimeSpan.Zero)
            {
                executionContext.Events.RaiseWaitingForRateLimit(new WaitingForRateLimitEventArgs(null, queryRateLimit, credentials));
                await _taskDelayer.Delay(timeToWait).ConfigureAwait(false);
            }
        }

        public async Task<TimeSpan> TimeToWaitBeforeTwitterRequestAsync(string query, IReadOnlyTwitterCredentials credentials, ITwitterExecutionContext twitterExecutionContext)
        {
            var queryRateLimits = await _rateLimitCacheManager.GetQueryRateLimitAsync(new GetEndpointRateLimitsParameters(query), credentials).ConfigureAwait(false);
            return GetTimeToWaitFromQueryRateLimit(queryRateLimits, twitterExecutionContext);
        }

        public TimeSpan GetTimeToWaitFromQueryRateLimit(IEndpointRateLimit queryRateLimit, ITwitterExecutionContext executionContext)
        {
            if (queryRateLimit == null || queryRateLimit.Remaining > 0)
            {
                return TimeSpan.Zero;
            }

            var timeToWaitInMs = (int) Math.Ceiling(queryRateLimit.ResetDateTimeInMilliseconds) + executionContext.RateLimitWaitFudge.TotalMilliseconds;
            return TimeSpan.FromMilliseconds(timeToWaitInMs);
        }
    }
}