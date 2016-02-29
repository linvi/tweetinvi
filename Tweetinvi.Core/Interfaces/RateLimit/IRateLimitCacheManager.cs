using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Core.Interfaces.RateLimit
{
    /// <summary>
    /// Proxy used to access and refresh the rate limits cache.
    /// </summary>
    public interface IRateLimitCacheManager
    {
        /// <summary>
        /// Return the rate limits for a specific query. 
        /// If the rate limits are not located in the cache, they will be retrieved from Twitter.
        /// </summary>
        ITokenRateLimit GetQueryRateLimit(string query, ITwitterCredentials credentials);

        /// <summary>
        /// Return the all the rate limits for a specific set of credentials.
        /// If the rate limits are not located in the cache, they will be retrieved from Twitter.
        /// </summary>
        ITokenRateLimits GetTokenRateLimits(ITwitterCredentials credentials);

        /// <summary>
        /// Update the rate limit cache with a specific set of rate limits.
        /// </summary>
        void UpdateTokenRateLimits(ITwitterCredentials credentials, ITokenRateLimits tokenRateLimits);
    }
}