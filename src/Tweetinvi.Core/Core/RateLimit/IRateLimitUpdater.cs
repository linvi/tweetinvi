using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Models.Authentication;
using Tweetinvi.Models;

namespace Tweetinvi.Core.RateLimit
{
    public interface IRateLimitUpdaterFactory
    {
        IRateLimitUpdater Create(IRateLimitCacheManager rateLimitCacheManager);
    }

    /// <summary>
    /// Update the rate limit cached information.
    /// </summary>
    public interface IRateLimitUpdater
    {
        /// <summary>
        /// Inform the updater a specific query has been executed with a specific set of credentials.
        /// </summary>
        Task QueryExecutedAsync(string query, ITwitterCredentials credentials, int numberOfRequests = 1);

        /// <summary>
        /// Inform the updater a specific query has been executed with a specific set of credentials.
        /// </summary>
        Task QueryExecutedAsync(string query, ITwitterCredentials credentials, Dictionary<string, IEnumerable<string>> rateLimitHeaders);

        /// <summary>
        /// Inform that you want to query rate limits to be set to 0.
        /// </summary>
        Task ClearRateLimitsForQueryAsync(string query, IReadOnlyTwitterCredentials credentials);
    }
}