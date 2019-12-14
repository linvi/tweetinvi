using System;
using System.Threading.Tasks;
using Tweetinvi.Client;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters.HelpClient;

namespace Tweetinvi.Credentials.RateLimit
{
    public class RateLimitCacheManager : IRateLimitCacheManager
    {
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly IWebRequestExecutor _webRequestExecutor;
        private readonly IHelpQueryGenerator _helpQueryGenerator;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly IRateLimitHelper _rateLimitHelper;
        private readonly ITwitterQueryFactory _twitterQueryFactory;

        private IRateLimitCache _rateLimitCache;
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

        public IRateLimitCache RateLimitCache
        {
            get => _rateLimitCache;
            set => _rateLimitCache = value;
        }

        public IRateLimitsClient RateLimitsClient { get; set; }

        public async Task<IEndpointRateLimit> GetOrCreateQueryRateLimit(string query, IReadOnlyTwitterCredentials credentials)
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

        public async Task<IEndpointRateLimit> GetQueryRateLimit(string query, IReadOnlyTwitterCredentials credentials)
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

        public async Task<ICredentialsRateLimits> GetCredentialsRateLimits(IReadOnlyTwitterCredentials credentials)
        {
            var rateLimits = await _rateLimitCache.GetCredentialsRateLimits(credentials).ConfigureAwait(false);
            if (rateLimits == null)
            {
                rateLimits = await RefreshCredentialsRateLimits(credentials).ConfigureAwait(false);
            }

            return rateLimits;
        }

        public Task UpdateCredentialsRateLimits(IReadOnlyTwitterCredentials credentials, ICredentialsRateLimits credentialsRateLimits)
        {
            return _rateLimitCache.RefreshEntry(credentials, credentialsRateLimits);
        }

        public async Task<ICredentialsRateLimits> RefreshCredentialsRateLimits(IReadOnlyTwitterCredentials credentials)
        {
            var tokenRateLimits = await GetTokenRateLimitsFromTwitter(credentials).ConfigureAwait(false);
            await _rateLimitCache.RefreshEntry(credentials, tokenRateLimits).ConfigureAwait(false);
            return await _rateLimitCache.GetCredentialsRateLimits(credentials).ConfigureAwait(false);
        }

        private async Task<ICredentialsRateLimits> GetTokenRateLimitsFromTwitter(IReadOnlyTwitterCredentials credentials)
        {
            var isApplicationOnlyCreds = string.IsNullOrEmpty(credentials.AccessToken) || string.IsNullOrEmpty(credentials.AccessTokenSecret);
            if (isApplicationOnlyCreds && string.IsNullOrEmpty(credentials.BearerToken))
            {
                return null;
            }

            try
            {
                return await RateLimitsClient.GetRateLimits(new GetRateLimitsParameters
                {
                    From = RateLimitsSource.TwitterApiOnly
                });
            }
            catch (TwitterException)
            {
                return null;
            }
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