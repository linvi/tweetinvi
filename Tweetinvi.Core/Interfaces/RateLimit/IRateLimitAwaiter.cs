using System;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Core.Interfaces.RateLimit
{
    /// <summary>
    /// Wait for the RateLimits before performing an operation.
    /// </summary>
    public interface IRateLimitAwaiter
    {
        /// <summary>
        /// Inform that a query is currently waiting in the RateLimitAwaiter
        /// for the appropriate RateLimits to be refreshed before being executed.
        /// </summary>
        event EventHandler<QueryAwaitingEventArgs> QueryAwaitingForRateLimit;

        /// <summary>
        /// Wait for the rate limits to be available for the specified query.
        /// </summary>
        void WaitForCurrentCredentialsRateLimit(string query);

        /// <summary>
        /// Wait for the credentials' rate limits to be available for the specified query.
        /// </summary>
        void WaitForCredentialsRateLimit(string query, ITwitterCredentials credentials);

        /// <summary>
        /// Wait before executing a query using the specified rate limits.
        /// </summary>
        void WaitForCredentialsRateLimit(IEndpointRateLimit endpointRateLimit);

        /// <summary>
        /// Wrapper to wait for a specific amount of time safely.
        /// </summary>
        void Wait(int timeToWait);

        /// <summary>
        /// Get the duration (milliseconds) to wait before executing the specified query.
        /// </summary>
        int TimeToWaitBeforeTwitterRequest(string query, ITwitterCredentials credentials);

        /// <summary>
        /// Get the duration (milliseconds) to wait before executing a query using the specified rate limits.
        /// </summary>
        int GetTimeToWaitFromQueryRateLimit(IEndpointRateLimit queryRateLimit);
    }
}