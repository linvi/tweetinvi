using System;
using System.Linq;
using System.Collections.Generic;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Models;

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
            if (rateLimitHeaders != null && rateLimitHeaders.Count > 0)
            {
                var rateLimit = _rateLimitCacheManager.GetOrCreateQueryRateLimit(query, credentials);

                // If the user runs out of RateLimit requests
                if (rateLimit == null)
                {
                    return;
                }

                IEnumerable<string> limitHeaders;
                if (rateLimitHeaders.TryGetValue("x-rate-limit-limit", out limitHeaders))
                {
                    var limit = limitHeaders.FirstOrDefault();
                    if (limit != null)
                    {
                        rateLimit.Limit = int.Parse(limit);
                    }
                }

                IEnumerable<string> remainingHeaders;
                if (rateLimitHeaders.TryGetValue("x-rate-limit-remaining", out remainingHeaders))
                {
                    var remaining = remainingHeaders.FirstOrDefault();
                    if (remaining != null)
                    {
                        rateLimit.Remaining = int.Parse(remaining);
                    }
                }

                IEnumerable<string> resetHeaders;
                if (rateLimitHeaders.TryGetValue("x-rate-limit-reset", out resetHeaders))
                {
                    var reset = resetHeaders.FirstOrDefault();
                    if (reset != null)
                    {
                        rateLimit.Reset = int.Parse(reset);
                    }
                }
            }
            else
            {
                QueryExecuted(query, credentials);
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