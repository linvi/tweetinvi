using Tweetinvi.Models;

namespace Tweetinvi.Core.RateLimit
{
    /// <summary>
    /// Helper class used to read the flags information from the rate limits
    /// and return the rate limits associated with a query.
    /// </summary>
    public interface IRateLimitHelper
    {
        /// <summary>
        /// Return the specified query rate limits if the query can be identified in the credentialsRateLimits.
        /// </summary>
        IEndpointRateLimit GetEndpointRateLimitFromQuery(string query, ICredentialsRateLimits rateLimits, bool createIfNotExist);
    }
}