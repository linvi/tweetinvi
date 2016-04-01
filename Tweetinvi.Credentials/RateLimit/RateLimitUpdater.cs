using System;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.RateLimit;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Credentials.RateLimit
{
    public class RateLimitUpdater : IRateLimitUpdater
    {
        private readonly IRateLimitCacheManager _rateLimitCacheManager;
        private readonly ICredentialsAccessor _credentialsAccessor;

        public RateLimitUpdater(
            IRateLimitCacheManager rateLimitCacheManager,
            ICredentialsAccessor credentialsAccessor)
        {
            _rateLimitCacheManager = rateLimitCacheManager;
            _credentialsAccessor = credentialsAccessor;
        }

        public void QueryExecuted(string query, int numberOfRequests = 1)
        {
            var currentCredentials = _credentialsAccessor.CurrentThreadCredentials;
            QueryExecuted(query, currentCredentials, numberOfRequests);
        }

        public void QueryExecuted(string query, ITwitterCredentials credentials, int numberOfRequests = 1)
        {
            var rateLimit = _rateLimitCacheManager.GetQueryRateLimit(query, credentials);

            if (rateLimit != null)
            {
                var newRemainingValue = Math.Max(rateLimit.Remaining - numberOfRequests, 0);
                rateLimit.Remaining = newRemainingValue;
            }
        }

        public void ClearRateLimitsForQuery(string query)
        {
            var currentCredentials = _credentialsAccessor.CurrentThreadCredentials;

            var rateLimit = _rateLimitCacheManager.GetQueryRateLimit(query, currentCredentials);
            if (rateLimit != null)
            {
                rateLimit.Remaining = 0;
            }
        }
    }
}