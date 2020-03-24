using System;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Models
{
    /// <summary>
    /// Create a twitter query
    /// </summary>
    public interface ITwitterQueryFactory
    {
        /// <summary>
        /// Create a twitter query.
        /// </summary>
        ITwitterQuery Create(string queryURL, HttpMethod httpMethod, ITwitterCredentials twitterCredentials);
    }

    /// <summary>
    /// All the information necessary for an http request to be executed.
    /// </summary>
    public interface ITwitterQuery : ITwitterRequestParameters
    {
        /// <summary>
        /// Proxy used to perform the query
        /// </summary>
        IProxyConfig ProxyConfig { get; set; }

        /// <summary>
        /// Duration after which the query is considered as having failed.
        /// </summary>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// Credentials with which the query will be executed.
        /// </summary>
        ITwitterCredentials TwitterCredentials { get; set; }

        /// <summary>
        /// OAuth request parameters.
        /// </summary>
        IOAuthQueryParameter[] QueryParameters { get; set; }

        /// <summary>
        /// RateLimit for the specific query. These can be null if the query url,
        /// could not be matched with any documented RateLimit field.
        /// </summary>
        IEndpointRateLimit QueryRateLimit { get; set; }

        /// <summary>
        /// All the endpoint RateLimits for the query credentials.
        /// </summary>
        ICredentialsRateLimits CredentialsRateLimits { get; set; }

        /// <summary>
        /// Date when the credentials will have the required rate limits to execute the query.
        /// </summary>
        DateTime? DateWhenCredentialsWillHaveTheRequiredRateLimits { get; set; }

        /// <summary>
        /// Time to wait before executing the query to ensure that we have not reached the RateLimits.
        /// </summary>
        TimeSpan? TimeToWaitBeforeExecutingTheQuery { get; }
    }
}