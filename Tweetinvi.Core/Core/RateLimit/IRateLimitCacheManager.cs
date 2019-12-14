using System.Threading.Tasks;
using Tweetinvi.Client;
using Tweetinvi.Models;

namespace Tweetinvi.Core.RateLimit
{
    /// <summary>
    /// Proxy used to access and refresh the rate limits cache.
    /// </summary>
    public interface IRateLimitCacheManager
    {
        IRateLimitCache RateLimitCache { get; set; }
        IRateLimitsClient RateLimitsClient { get; set; }

        /// <summary>
        /// Return the rate limits for a specific query.
        /// If the query url cannot be mapped, a new one is created in the OtherQueryRateLimits.
        /// If the credentials rate limits are not located in the cache, they will be retrieved from Twitter.
        /// </summary>
        Task<IEndpointRateLimit> GetOrCreateQueryRateLimit(string query, IReadOnlyTwitterCredentials credentials);

        /// <summary>
        /// Return the rate limits for a specific query.
        /// If the rate limits are not located in the cache, they will be retrieved from Twitter.
        /// </summary>
        Task<IEndpointRateLimit> GetQueryRateLimit(string query, IReadOnlyTwitterCredentials credentials);

        /// <summary>
        /// Return the all the rate limits for a specific set of credentials.
        /// If the rate limits are not located in the cache, they will be retrieved from Twitter.
        /// </summary>
        Task<ICredentialsRateLimits> GetCredentialsRateLimits(IReadOnlyTwitterCredentials credentials);

        /// <summary>
        /// Update the rate limit cache with a specific set of rate limits.
        /// </summary>
        Task UpdateCredentialsRateLimits(IReadOnlyTwitterCredentials credentials, ICredentialsRateLimits credentialsRateLimits);

        Task<ICredentialsRateLimits> RefreshCredentialsRateLimits(IReadOnlyTwitterCredentials credentials);
    }
}