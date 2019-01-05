using System;
using Tweetinvi.Core;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Events;
using Tweetinvi.Models;

namespace Tweetinvi.Credentials.RateLimit
{
    public class RateLimitAwaiter : IRateLimitAwaiter
    {
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly IRateLimitCacheManager _rateLimitCacheManager;
        private readonly IThreadHelper _threadHelper;
        private readonly IWeakEvent<EventHandler<QueryAwaitingEventArgs>> _queryAwaitingForRateLimitWeakEvent;
        private readonly ITweetinviSettingsAccessor _settingsAccessor;

        public RateLimitAwaiter(
            ICredentialsAccessor credentialsAccessor,
            IRateLimitCacheManager rateLimitCacheManager,
            IThreadHelper threadHelper,
            IWeakEvent<EventHandler<QueryAwaitingEventArgs>> queryAwaitingForRateLimitWeakEvent,
            ITweetinviSettingsAccessor settingsAccessor)
        {
            _credentialsAccessor = credentialsAccessor;
            _rateLimitCacheManager = rateLimitCacheManager;
            _threadHelper = threadHelper;
            _queryAwaitingForRateLimitWeakEvent = queryAwaitingForRateLimitWeakEvent;
            _settingsAccessor = settingsAccessor;
        }

        public event EventHandler<QueryAwaitingEventArgs> QueryAwaitingForRateLimit
        {
            add { _queryAwaitingForRateLimitWeakEvent.AddHandler(value);}
            remove { _queryAwaitingForRateLimitWeakEvent.RemoveHandler(value);}
        }

        public void WaitForCurrentCredentialsRateLimit(string query)
        {
            var credentials = _credentialsAccessor.CurrentThreadCredentials;
            WaitForCredentialsRateLimit(query, credentials);
        }

        public void WaitForCredentialsRateLimit(string query, ITwitterCredentials credentials)
        {
            var queryRateLimit = _rateLimitCacheManager.GetQueryRateLimit(query, credentials);
            var timeToWait = GetTimeToWaitFromQueryRateLimit(queryRateLimit);

            if (timeToWait > 0)
            {
                _queryAwaitingForRateLimitWeakEvent.Raise(this, new QueryAwaitingEventArgs(query, queryRateLimit, credentials));
                _threadHelper.Sleep(timeToWait);
            }
        }

        public void WaitForCredentialsRateLimit(IEndpointRateLimit endpointRateLimit)
        {
            var timeToWait = GetTimeToWaitFromQueryRateLimit(endpointRateLimit);

            Wait(timeToWait);
        }

        public void Wait(int timeToWait)
        {
            if (timeToWait > 0)
            {
                _threadHelper.Sleep(timeToWait);
            }
        }

        public int TimeToWaitBeforeTwitterRequest(string query, ITwitterCredentials credentials)
        {
            var queryRateLimits = _rateLimitCacheManager.GetQueryRateLimit(query, credentials);

            return GetTimeToWaitFromQueryRateLimit(queryRateLimits);
        }

        public int GetTimeToWaitFromQueryRateLimit(IEndpointRateLimit queryRateLimit)
        {
            if (queryRateLimit == null)
            {
                return 0;
            }

            return queryRateLimit.Remaining == 0
                ? ((int) Math.Ceiling(queryRateLimit.ResetDateTimeInMilliseconds)) +
                  _settingsAccessor.RateLimitWaitFudgeMs
                : 0;
        }
    }
}