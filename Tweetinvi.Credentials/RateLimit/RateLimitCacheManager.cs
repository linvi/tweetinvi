using System;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;

namespace Tweetinvi.Credentials.RateLimit
{
    public class RateLimitCacheManager : IRateLimitCacheManager
    {
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly IWebRequestExecutor _webRequestExecutor;
        private readonly IHelpQueryGenerator _helpQueryGenerator;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly IRateLimitCache _rateLimitCache;
        private readonly IRateLimitHelper _rateLimitHelper;
        private readonly ITwitterQueryFactory _twitterQueryFactory;

        public RateLimitCacheManager(
            ICredentialsAccessor credentialsAccessor,
            IWebRequestExecutor webRequestExecutor,
            IHelpQueryGenerator helpQueryGenerator,
            IJsonObjectConverter jsonObjectConverter,
            IRateLimitCache rateLimitCache,
            IRateLimitHelper rateLimitHelper,
            ITwitterQueryFactory twitterQueryFactory)
        {
            _credentialsAccessor = credentialsAccessor;
            _webRequestExecutor = webRequestExecutor;
            _helpQueryGenerator = helpQueryGenerator;
            _jsonObjectConverter = jsonObjectConverter;
            _rateLimitCache = rateLimitCache;
            _rateLimitHelper = rateLimitHelper;
            _twitterQueryFactory = twitterQueryFactory;
        }

        public IEndpointRateLimit GetOrCreateQueryRateLimit(string query, ITwitterCredentials credentials)
        {
            var rateLimits = _rateLimitCache.GetCredentialsRateLimits(credentials);
            var queryRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(query, rateLimits, true);

            if (rateLimits == null || DoesQueryNeedsToRefreshTheCacheInformation(queryRateLimit))
            {
                rateLimits = RefreshCredentialsRateLimits(credentials);
                queryRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(query, rateLimits, true);
            }

            return queryRateLimit;
        }

        public IEndpointRateLimit GetQueryRateLimit(string query, ITwitterCredentials credentials)
        {
            var rateLimits = _rateLimitCache.GetCredentialsRateLimits(credentials);
            var queryRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(query, rateLimits, false);

            if (rateLimits == null || DoesQueryNeedsToRefreshTheCacheInformation(queryRateLimit))
            {
                rateLimits = RefreshCredentialsRateLimits(credentials);
                queryRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(query, rateLimits, false);
            }

            return queryRateLimit;
        }

        public ICredentialsRateLimits GetCredentialsRateLimits(ITwitterCredentials credentials)
        {
            var rateLimits = _rateLimitCache.GetCredentialsRateLimits(credentials);
            if (rateLimits == null)
            {
                rateLimits = RefreshCredentialsRateLimits(credentials);
            }

            return rateLimits;
        }

        public void UpdateCredentialsRateLimits(ITwitterCredentials credentials, ICredentialsRateLimits credentialsRateLimits)
        {
            _rateLimitCache.RefreshEntry(credentials, credentialsRateLimits);
        }

        private ICredentialsRateLimits RefreshCredentialsRateLimits(ITwitterCredentials credentials)
        {
            var tokenRateLimits = GetTokenRateLimitsFromTwitter(credentials);
            _rateLimitCache.RefreshEntry(credentials, tokenRateLimits);
            return _rateLimitCache.GetCredentialsRateLimits(credentials);
        }

        private ICredentialsRateLimits GetTokenRateLimitsFromTwitter(ITwitterCredentials credentials)
        {
            var isApplicationOnlyCreds = string.IsNullOrEmpty(credentials.AccessToken) || string.IsNullOrEmpty(credentials.AccessTokenSecret);
            if (isApplicationOnlyCreds && string.IsNullOrEmpty(credentials.ApplicationOnlyBearerToken))
            {
                return null;
            }

            var result = _credentialsAccessor.ExecuteOperationWithCredentials(credentials, () =>
            {
                var twitterQuery = _twitterQueryFactory.Create(_helpQueryGenerator.GetCredentialsLimitsQuery(), HttpMethod.GET, credentials);

                try
                {
                    var webRequestResult = _webRequestExecutor.ExecuteQuery(twitterQuery);
                    var json = webRequestResult.Text;

                    return _jsonObjectConverter.DeserializeObject<ICredentialsRateLimits>(json);
                }
                catch (TwitterException)
                {
                    return null;
                }
            });

            return result;
        }

        private bool DoesQueryNeedsToRefreshTheCacheInformation(IEndpointRateLimit rateLimit)
        {
            if (rateLimit == null || rateLimit.IsCustomHeaderRateLimit)
            {
                return false;
            }

            return rateLimit.ResetDateTime < DateTime.Now;
        }
    }
}