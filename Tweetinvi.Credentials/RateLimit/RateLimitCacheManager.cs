using System;
using System.Threading.Tasks;
using Tweetinvi.Client;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters.HelpClient;

namespace Tweetinvi.Credentials.RateLimit
{
    public class RateLimitCacheManager : IRateLimitCacheManager
    {
        private readonly IRateLimitHelper _rateLimitHelper;

        private IRateLimitCache _rateLimitCache;
        public RateLimitCacheManager(
            IRateLimitCache rateLimitCache,
            IRateLimitHelper rateLimitHelper)
        {
            _rateLimitCache = rateLimitCache;
            _rateLimitHelper = rateLimitHelper;
        }

        public virtual IRateLimitCache RateLimitCache
        {
            get => _rateLimitCache;
            set => _rateLimitCache = value;
        }

        public virtual IRateLimitsClient RateLimitsClient { get; set; }

        public virtual async Task<IEndpointRateLimit> GetQueryRateLimit(IGetEndpointRateLimitsParameters parameters, IReadOnlyTwitterCredentials credentials)
        {
            var credentialsRateLimits = await RateLimitsClient.GetRateLimits(parameters).ConfigureAwait(false);
            var endpointRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(parameters.Url, credentialsRateLimits, false);

            if (parameters.From == RateLimitsSource.CacheOrTwitterApi && ShouldEndpointCacheBeUpdated(endpointRateLimit))
            {
                var updatedCredentialsRateLimits = await RefreshCredentialsRateLimits(credentials).ConfigureAwait(false);
                endpointRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(parameters.Url, updatedCredentialsRateLimits, false);
            }

            return endpointRateLimit;
        }

        public virtual async Task<ICredentialsRateLimits> GetCredentialsRateLimits(IReadOnlyTwitterCredentials credentials)
        {
            var rateLimits = await _rateLimitCache.GetCredentialsRateLimits(credentials).ConfigureAwait(false);
            if (rateLimits == null)
            {
                rateLimits = await RefreshCredentialsRateLimits(credentials).ConfigureAwait(false);
            }

            return rateLimits;
        }

        public virtual Task UpdateCredentialsRateLimits(IReadOnlyTwitterCredentials credentials, ICredentialsRateLimits credentialsRateLimits)
        {
            return _rateLimitCache.RefreshEntry(credentials, credentialsRateLimits);
        }

        public virtual async Task<ICredentialsRateLimits> RefreshCredentialsRateLimits(IReadOnlyTwitterCredentials credentials)
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
                    From = RateLimitsSource.TwitterApiOnly,
                    TrackerMode = RateLimitTrackerMode.None
                });
            }
            catch (TwitterException)
            {
                return null;
            }
        }

        public virtual bool ShouldEndpointCacheBeUpdated(IEndpointRateLimit rateLimit)
        {
            if (rateLimit == null || rateLimit.IsCustomHeaderRateLimit)
            {
                return false;
            }

            return rateLimit.ResetDateTime < DateTime.Now;
        }
    }
}