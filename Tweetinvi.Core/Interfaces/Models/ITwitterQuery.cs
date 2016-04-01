using System;
using System.Collections.Generic;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Core.Interfaces.Models
{
    /// <summary>
    /// Create a twitter query
    /// </summary>
    public interface ITwitterQueryFactory
    {
        /// <summary>
        /// Create a twitter query.
        /// </summary>
        ITwitterQuery Create(string queryURL, HttpMethod httpMethod = HttpMethod.GET, bool withThreadCredentials = false);

        /// <summary>
        /// Create a twitter query.
        /// </summary>
        ITwitterQuery Create(string queryURL, HttpMethod httpMethod, ITwitterCredentials twitterCredentials);
    }

    /// <summary>
    /// All the information necessary for an http request to be executed.
    /// </summary>
    public interface ITwitterQuery
    {
        /// <summary>
        /// Query that will be executed.
        /// </summary>
        string QueryURL { get; set; }

        /// <summary>
        /// HTTP Method used to execute the query.
        /// </summary>
        HttpMethod HttpMethod { get; set; }
        
        /// <summary>
        /// Proxy used to perform the query
        /// </summary>
        string Proxy { get; set; }

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
        IEnumerable<IOAuthQueryParameter> QueryParameters { get; set; }

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
        /// Time to wait in milliseconds after which the required 
        /// Rate Limits will be available to execute the query.
        /// </summary>
        int? TimeToWaitBeforeExecutingTheQueryInMilliSeconds { get; }

        /// <summary>
        /// Clone the query information into a new object.
        /// </summary>
        ITwitterQuery Clone();
    }
}