using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Client;
using Tweetinvi.Models;
using Tweetinvi.Parameters.RateLimitsClient;

namespace Tweetinvi.Core.RateLimit
{
    /// <summary>
    /// Wait for the RateLimits before performing an operation.
    /// </summary>
    public interface IRateLimitAwaiter
    {
        /// <summary>
        /// Wait for the credentials' rate limits to be available for the specified query.
        /// </summary>
        Task WaitForCredentialsRateLimitAsync(ITwitterRequest request);

        /// <summary>
        /// Wait for the credentials' rate limits to be available for the specified query.
        /// </summary>
        Task WaitForCredentialsRateLimitAsync(IWaitForCredentialsRateLimitParameters parameters);

        /// <summary>
        /// Wait for the credentials' rate limits to be available for the specified endpoint.
        /// </summary>
        Task WaitForCredentialsRateLimitAsync(IEndpointRateLimit queryRateLimit, IReadOnlyTwitterCredentials credentials, ITwitterExecutionContext executionContext);

        /// <summary>
        /// Get the duration to wait before executing the specified query.
        /// </summary>
        Task<TimeSpan> TimeToWaitBeforeTwitterRequestAsync(string query, IReadOnlyTwitterCredentials credentials, ITwitterExecutionContext twitterExecutionContext);

        /// <summary>
        /// Get the duration (milliseconds) to wait before executing a query using the specified rate limits.
        /// </summary>
        TimeSpan GetTimeToWaitFromQueryRateLimit(IEndpointRateLimit queryRateLimit, ITwitterExecutionContext executionContext);
    }
}