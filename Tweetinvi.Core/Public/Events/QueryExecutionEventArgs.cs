using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public class QueryExecutionEventArgs : EventArgs
    {
        private readonly ITwitterQuery _twitterQuery;

        public QueryExecutionEventArgs(ITwitterQuery twitterQuery)
        {
            _twitterQuery = twitterQuery;
        }

        /// <summary>
        /// Contains all the required information to execute a request on the Twitter REST API.
        /// </summary>
        public ITwitterQuery TwitterQuery { get { return _twitterQuery; } }

        /// <summary>
        /// Endpoint URL.
        /// </summary>
        public string QueryURL { get { return _twitterQuery.QueryURL; } }

        /// <summary>
        /// Credentials used to execute the query.
        /// </summary>
        public ITwitterCredentials Credentials { get { return _twitterQuery.TwitterCredentials; } }

        /// <summary>
        /// Endpoint Rate Limits information.
        /// </summary>
        public IEndpointRateLimit QueryRateLimit {  get { return _twitterQuery.QueryRateLimit; } }

        /// <summary>
        /// Date at which the Twitter query will be ready to be executed.
        /// </summary>
        public DateTime? DateOfQueryExecution { get { return _twitterQuery.DateWhenCredentialsWillHaveTheRequiredRateLimits; } }

        /// <summary>
        /// Recommended time to wait before executing such a query,
        /// in order to ensure that the twitter limitations won't be retuning an error.
        /// </summary>
        public int? TimeToWaitBeforeExecutingTheQueryInMilliSeconds { get { return _twitterQuery.TimeToWaitBeforeExecutingTheQueryInMilliSeconds; } }
    }
}