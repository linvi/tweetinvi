using System;
using System.Linq;
using System.Collections.Generic;
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

        public void QueryExecuted(string query, ITwitterCredentials credentials, Dictionary<string, IEnumerable<string>> rateLimitHeaders)
        {
            var rateLimit = _rateLimitCacheManager.GetQueryRateLimit(query, credentials);

            if (rateLimitHeaders != null && rateLimitHeaders.Count > 0)
            {
                if (rateLimitHeaders["x-rate-limit-limit"].Count()>0)
                {
                    rateLimit.Limit = int.Parse(rateLimitHeaders["x-rate-limit-limit"].First());
                }

                if (rateLimitHeaders["x-rate-limit-remaining"].Count()>0)
                {
                    rateLimit.Remaining = int.Parse(rateLimitHeaders["x-rate-limit-remaining"].First());
                }

                if (rateLimitHeaders["x-rate-limit-reset"].Count()>0)
                {
                    rateLimit.Reset = long.Parse(rateLimitHeaders["x-rate-limit-reset"].First());
                }
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