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

        public void QueryExecuted(string query, ITwitterCredentials credentials, int numberOfRequests = 1, Dictionary<string, IEnumerable<string>> rateLimitHeaders = null)
        {
            //IEndpointRateLimit rateLimit = null;

            Dictionary<string, string> RATE_LIMIT_Headers = new Dictionary<string, string>() {
                {"x-rate-limit-limit","Limit" },
                {"x-rate-limit-remaining", "Remaining"},
                {"x-rate-limit-reset", "Reset"},
            };

            
            var rateLimit = _rateLimitCacheManager.GetQueryRateLimit(query, credentials);
            
            if (rateLimit != null)
            {
                if (rateLimitHeaders != null && rateLimitHeaders.Count > 0)
                {
                    if (rateLimitHeaders.ContainsKey("x-rate-limit-limit"))
                    {
                        //rateLimit.Limit = int.Parse(rateLimitHeaders["x-rate-limit-limit"].FirstOrDefault());
                    }

                    if (rateLimitHeaders.ContainsKey("x-rate-limit-remaining"))
                    {
                        rateLimit.Remaining = int.Parse(rateLimitHeaders["x-rate-limit-remaining"].FirstOrDefault());
                    }

                    if (rateLimitHeaders.ContainsKey("x-rate-limit-reset"))
                    {
                        //rateLimit.Reset = long.Parse(rateLimitHeaders["x-rate-limit-reset"].FirstOrDefault());
                    }
                    //var iEndpointRateLimittype = rateLimit.GetType();
                    //foreach (var item in RATE_LIMIT_Headers)
                    //{
                    //    
                    //    iEndpointRateLimittype.GetProperty(item.Value).SetValue(rateLimit,value.FirstOrDefault() , null);
                    //}
                }
                else
                {
                    var newRemainingValue = Math.Max(rateLimit.Remaining - numberOfRequests, 0);
                    rateLimit.Remaining = newRemainingValue;
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