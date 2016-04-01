using Tweetinvi.Core.Authentication;
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
        IEndpointRateLimit GetQueryRateLimit(string query, ITwitterCredentials credentials);

        /// <summary>
        /// Return the all the rate limits for a specific set of credentials.
        /// If the rate limits are not located in the cache, they will be retrieved from Twitter.
        /// </summary>
        ICredentialsRateLimits GetCredentialsRateLimits(ITwitterCredentials credentials);

        /// <summary>
        /// Update the rate limit cache with a specific set of rate limits.
        /// </summary>
        void UpdateCredentialsRateLimits(ITwitterCredentials credentials, ICredentialsRateLimits credentialsRateLimits);
    }
}