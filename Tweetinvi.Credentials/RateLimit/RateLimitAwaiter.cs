using System;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.RateLimit;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Credentials.RateLimit
{
    public class RateLimitAwaiter : IRateLimitAwaiter
    {
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly IRateLimitCacheManager _rateLimitCacheManager;
        private readonly IThreadHelper _threadHelper;
        private readonly IWeakEvent<EventHandler<QueryAwaitingEventArgs>> _queryAwaitingForRateLimitWeakEvent;

        public RateLimitAwaiter(
            ICredentialsAccessor credentialsAccessor,
            IRateLimitCacheManager rateLimitCacheManager,
            IThreadHelper threadHelper,
            IWeakEvent<EventHandler<QueryAwaitingEventArgs>> queryAwaitingForRateLimitWeakEvent)
        {
            _credentialsAccessor = credentialsAccessor;
            _rateLimitCacheManager = rateLimitCacheManager;
            _threadHelper = threadHelper;
            _queryAwaitingForRateLimitWeakEvent = queryAwaitingForRateLimitWeakEvent;
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

            return queryRateLimit.Remaining == 0 ? ((int) queryRateLimit.ResetDateTimeInMilliseconds) : 0;
        }
    }
}