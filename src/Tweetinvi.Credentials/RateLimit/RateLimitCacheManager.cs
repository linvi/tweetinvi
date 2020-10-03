using System;
using System.Threading.Tasks;
using Tweetinvi.Client;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Credentials.RateLimit
{
    public class RateLimitCacheManager : IRateLimitCacheManager
    {
        private readonly IRateLimitHelper _rateLimitHelper;
        private readonly IRateLimitCache _rateLimitCache;
        public RateLimitCacheManager(
            IRateLimitCache rateLimitCache,
            IRateLimitHelper rateLimitHelper)
        {
            _rateLimitCache = rateLimitCache;
            _rateLimitHelper = rateLimitHelper;
        }

        public virtual IRateLimitCache RateLimitCache => _rateLimitCache;
        public virtual IRateLimitsClient RateLimitsClient { get; set; }

        public virtual async Task<IEndpointRateLimit> GetQueryRateLimitAsync(IGetEndpointRateLimitsParameters parameters, IReadOnlyTwitterCredentials credentials)
        {
            var credentialsRateLimits = await RateLimitsClient.GetRateLimitsAsync(parameters).ConfigureAwait(false);
            var endpointRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(parameters.Url, credentialsRateLimits, false);

            if (parameters.From == RateLimitsSource.CacheOrTwitterApi && ShouldEndpointCacheBeUpdated(endpointRateLimit))
            {
                var updatedCredentialsRateLimits = await RefreshCredentialsRateLimitsAsync(credentials).ConfigureAwait(false);
                endpointRateLimit = _rateLimitHelper.GetEndpointRateLimitFromQuery(parameters.Url, updatedCredentialsRateLimits, false);
            }

            return endpointRateLimit;
        }

        public virtual async Task<ICredentialsRateLimits> GetCredentialsRateLimitsAsync(IReadOnlyTwitterCredentials credentials)
        {
            var rateLimits = await _rateLimitCache.GetCredentialsRateLimitsAsync(credentials).ConfigureAwait(false);
            if (rateLimits == null)
            {
                rateLimits = await RefreshCredentialsRateLimitsAsync(credentials).ConfigureAwait(false);
            }

            return rateLimits;
        }

        public virtual Task UpdateCredentialsRateLimitsAsync(IReadOnlyTwitterCredentials credentials, ICredentialsRateLimits credentialsRateLimits)
        {
            return _rateLimitCache.RefreshEntryAsync(credentials, credentialsRateLimits);
        }

        public virtual async Task<ICredentialsRateLimits> RefreshCredentialsRateLimitsAsync(IReadOnlyTwitterCredentials credentials)
        {
            var tokenRateLimits = await GetTokenRateLimitsFromTwitterAsync(credentials).ConfigureAwait(false);
            await _rateLimitCache.RefreshEntryAsync(credentials, tokenRateLimits).ConfigureAwait(false);
            return await _rateLimitCache.GetCredentialsRateLimitsAsync(credentials).ConfigureAwait(false);
        }

        private async Task<ICredentialsRateLimits> GetTokenRateLimitsFromTwitterAsync(IReadOnlyTwitterCredentials credentials)
        {
            var isApplicationOnlyCreds = string.IsNullOrEmpty(credentials.AccessToken) || string.IsNullOrEmpty(credentials.AccessTokenSecret);
            if (isApplicationOnlyCreds && string.IsNullOrEmpty(credentials.BearerToken))
            {
                return null;
            }

            try
            {
                return await RateLimitsClient.GetRateLimitsAsync(new GetRateLimitsParameters
                {
                    From = RateLimitsSource.TwitterApiOnly,
                    TrackerMode = RateLimitTrackerMode.None
                }).ConfigureAwait(false);
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