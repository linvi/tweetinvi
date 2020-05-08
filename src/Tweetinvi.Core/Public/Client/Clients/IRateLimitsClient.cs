using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface IRateLimitsClient
    {
        /// <summary>
        /// Load the client's rate limits in the cache
        /// </summary>
        Task InitializeRateLimitsManagerAsync();

        /// <inheritdoc cref="IRateLimitsClient.GetRateLimitsAsync(IGetRateLimitsParameters)" />
        Task<ICredentialsRateLimits> GetRateLimitsAsync();

        /// <inheritdoc cref="IRateLimitsClient.GetRateLimitsAsync(IGetRateLimitsParameters)" />
        Task<ICredentialsRateLimits> GetRateLimitsAsync(RateLimitsSource from);

        /// <summary>
        /// Get the rate limits of the current client
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/developer-utilities/rate-limit-status/api-reference/get-application-rate_limit_status </para>
        /// <returns>The client's rate limits</returns>
        Task<ICredentialsRateLimits> GetRateLimitsAsync(IGetRateLimitsParameters parameters);

        /// <inheritdoc cref="IRateLimitsClient.GetEndpointRateLimitAsync(IGetEndpointRateLimitsParameters)"/>
        Task<IEndpointRateLimit> GetEndpointRateLimitAsync(string url);

        /// <inheritdoc cref="IRateLimitsClient.GetEndpointRateLimitAsync(IGetEndpointRateLimitsParameters)"/>
        Task<IEndpointRateLimit> GetEndpointRateLimitAsync(string url, RateLimitsSource from);

        /// <summary>
        /// Get a specific endpoint's rate limits of the current client
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/developer-utilities/rate-limit-status/api-reference/get-application-rate_limit_status </para>
        /// <returns>The endpoint's rate limits, or null if the endpoint is not support by Tweetinvi</returns>
        Task<IEndpointRateLimit> GetEndpointRateLimitAsync(IGetEndpointRateLimitsParameters parameters);

        /// <inheritdoc cref="IRateLimitsClient.WaitForQueryRateLimitAsync(IEndpointRateLimit)" />
        Task WaitForQueryRateLimitAsync(string url);

        /// <summary>
        /// Wait for new requests to a specific endpoint become available
        /// </summary>
        Task WaitForQueryRateLimitAsync(IEndpointRateLimit endpointRateLimit);

        /// <summary>
        /// Clear the rate limits cached for a specific set of credentials
        /// </summary>
        Task ClearRateLimitCacheAsync(IReadOnlyTwitterCredentials credentials);

        /// <summary>
        /// Clear the rate limits cached for the client's credentials
        /// </summary>
        Task ClearRateLimitCacheAsync();

        /// <summary>
        /// Clear the rate limits of all the credentials
        /// </summary>
        Task ClearAllRateLimitCacheAsync();
    }
}