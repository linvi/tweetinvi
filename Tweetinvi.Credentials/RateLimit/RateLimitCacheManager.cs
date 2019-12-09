using System;
using System.Threading.Tasks;
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

        public async Task<IEndpointRateLimit> GetOrCreateQueryRateLimit(string query, ITwitterCredentials credentials)
        {
            var rateLimits = await _rateLimitCache.GetCredentialsRateLimits(credentials).ConfigureAwait(false);
            var queryRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(query, rateLimits, true);

            if (rateLimits == null || DoesQueryNeedsToRefreshTheCacheInformation(queryRateLimit))
            {
                rateLimits = await RefreshCredentialsRateLimits(credentials);
                queryRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(query, rateLimits, true);
            }

            return queryRateLimit;
        }

        public async Task<IEndpointRateLimit> GetQueryRateLimit(string query, ITwitterCredentials credentials)
        {
            var rateLimits = await _rateLimitCache.GetCredentialsRateLimits(credentials).ConfigureAwait(false);
            var queryRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(query, rateLimits, false);

            if (rateLimits == null || DoesQueryNeedsToRefreshTheCacheInformation(queryRateLimit))
            {
                rateLimits = await RefreshCredentialsRateLimits(credentials).ConfigureAwait(false);
                queryRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(query, rateLimits, false);
            }

            return queryRateLimit;
        }

        public async Task<ICredentialsRateLimits> GetCredentialsRateLimits(ITwitterCredentials credentials)
        {
            var rateLimits = await _rateLimitCache.GetCredentialsRateLimits(credentials).ConfigureAwait(false);
            if (rateLimits == null)
            {
                rateLimits = await RefreshCredentialsRateLimits(credentials).ConfigureAwait(false);
            }

            return rateLimits;
        }

        public void UpdateCredentialsRateLimits(ITwitterCredentials credentials, ICredentialsRateLimits credentialsRateLimits)
        {
            _rateLimitCache.RefreshEntry(credentials, credentialsRateLimits);
        }

        private async Task<ICredentialsRateLimits> RefreshCredentialsRateLimits(ITwitterCredentials credentials)
        {
            var tokenRateLimits = await GetTokenRateLimitsFromTwitter(credentials).ConfigureAwait(false);
            await _rateLimitCache.RefreshEntry(credentials, tokenRateLimits).ConfigureAwait(false);
            return await _rateLimitCache.GetCredentialsRateLimits(credentials).ConfigureAwait(false);
        }

        private async Task<ICredentialsRateLimits> GetTokenRateLimitsFromTwitter(ITwitterCredentials credentials)
        {
            var isApplicationOnlyCreds = string.IsNullOrEmpty(credentials.AccessToken) || string.IsNullOrEmpty(credentials.AccessTokenSecret);
            if (isApplicationOnlyCreds && string.IsNullOrEmpty(credentials.BearerToken))
            {
                return null;
            }

            var result = await _credentialsAccessor.ExecuteOperationWithCredentials(credentials, async () =>
            {
                var twitterQuery = _twitterQueryFactory.Create(_helpQueryGenerator.GetCredentialsLimitsQuery(), HttpMethod.GET, credentials);
                var request = new TwitterRequest
                {
                    Query = twitterQuery
                };

                try
                {
                    var webRequestResult = await _webRequestExecutor.ExecuteQuery(request).ConfigureAwait(false);
                    var json = webRequestResult.Text;

                    return _jsonObjectConverter.DeserializeObject<ICredentialsRateLimits>(json);
                }
                catch (TwitterException)
                {
                    return null;
                }
            }).ConfigureAwait(false);

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