using System;

namespace Tweetinvi.Models
{
    /// <summary>
    /// Give information regarding the rate limits for a specific
    /// endpoint of the Twitter API.
    /// </summary>
    public interface IEndpointRateLimit
    {
        /// <summary>
        /// Remaining operation authorized with the associated credentials.
        /// </summary>
        int Remaining { get; set; }

        /// <summary>
        /// Reset DateTime in UTC.
        /// </summary>
        long Reset { get; set; }

        /// <summary>
        /// Maximum number of query execution authorized in a 
        /// rate limit lifecycle (usually 15 minutes).
        /// </summary>
        int Limit { get; set; }

        /// <summary>
        /// Remaining seconds to wait before being able to perform such queries again.
        /// </summary>
        double ResetDateTimeInSeconds { get; }

        /// <summary>
        /// Remaining milliseconds to wait before being able to perform such queries again.
        /// </summary>
        double ResetDateTimeInMilliseconds { get; }

        /// <summary>
        /// DateTime when the rate limit lifecycle reset.
        /// </summary>
        DateTime ResetDateTime { get; }

        bool IsCustomHeaderRateLimit { get; set; }
    }
}