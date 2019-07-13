using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public class QueryExecutionEventArgs : EventArgs
    {

        public QueryExecutionEventArgs(ITwitterQuery twitterQuery)
        {
            TwitterQuery = twitterQuery;
        }

        /// <summary>
        /// Contains all the required information to execute a request on the Twitter REST API.
        /// </summary>
        public ITwitterQuery TwitterQuery { get; }

        /// <summary>
        /// Endpoint URL.
        /// </summary>
        public string Url => TwitterQuery.Url;

        /// <summary>
        /// Credentials used to execute the query.
        /// </summary>
        public ITwitterCredentials Credentials => TwitterQuery.TwitterCredentials;

        /// <summary>
        /// Endpoint Rate Limits information.
        /// </summary>
        public IEndpointRateLimit QueryRateLimit => TwitterQuery.QueryRateLimit;

        /// <summary>
        /// Date at which the Twitter query will be ready to be executed.
        /// </summary>
        public DateTime? DateOfQueryExecution => TwitterQuery.DateWhenCredentialsWillHaveTheRequiredRateLimits;

        /// <summary>
        /// Recommended time to wait before executing such a query,
        /// in order to ensure that the twitter limitations won't be retuning an error.
        /// </summary>
        public int? TimeToWaitBeforeExecutingTheQueryInMilliSeconds => TwitterQuery.TimeToWaitBeforeExecutingTheQueryInMilliSeconds;
    }
}