using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Core.Interfaces.RateLimit
{
    /// <summary>
    /// Cache storing the RateLimits to reduce the number of access to the Twitter API rate limits.
    /// Access to the rate limit cache should be done via the RateLimitCacheManager.
    /// </summary>
    public interface IRateLimitCache
    {
        /// <summary>
        /// Clear the rate limits entry associated with a specific set of credentials.
        /// </summary>
        void Clear(ITwitterCredentials credentials);

        /// <summary>
        /// Clear all the rate limit entries from the cache.
        /// </summary>
        void ClearAll();

        /// <summary>
        /// Manually set a rate limit entry for a specific set of credentials.
        /// </summary>
        void RefreshEntry(ITwitterCredentials credentials, ICredentialsRateLimits credentialsRateLimits);

        /// <summary>
        /// Return the rate limits entry for a set of credentials.
        /// </summary>
        ICredentialsRateLimits GetCredentialsRateLimits(ITwitterCredentials credentials);
    }
}