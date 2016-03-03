using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Core.Interfaces.RateLimit
{
    /// <summary>
    /// Helper class used to read the flags information from the rate limits
    /// and return the rate limits associated with a query.
    /// </summary>
    public interface IRateLimitHelper
    {
        /// <summary>
        /// Detect if the query can be identified as being a rate limited query.
        /// </summary>
        bool IsQueryAssociatedWithEndpointRateLimit(string query, ICredentialsRateLimits rateLimits);

        /// <summary>
        /// Return the specified query rate limits if the query can be identified in the credentialsRateLimits.
        /// </summary>
        IEndpointRateLimit GetEndpointRateLimitFromQuery(string query, ICredentialsRateLimits rateLimits);
    }
}