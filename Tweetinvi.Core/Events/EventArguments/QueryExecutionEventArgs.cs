using System;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Events.EventArguments
{
    public class QueryExecutionEventArgs : EventArgs
    {
        private readonly ITwitterQuery _twitterQuery;

        public QueryExecutionEventArgs(ITwitterQuery twitterQuery)
        {
            _twitterQuery = twitterQuery;
        }

        public ITwitterQuery TwitterQuery { get { return _twitterQuery; } }

        public string QueryURL { get { return _twitterQuery.QueryURL; } }
        public ITwitterCredentials Credentials { get { return _twitterQuery.TwitterCredentials; } }

        /// <summary>
        /// Date at which the Twitter query will be ready to be executed
        /// </summary>
        public DateTime? DateOfQueryExecution { get { return _twitterQuery.DateWhenCredentialsWillHaveRequiredRateLimits; } }

        public int? TimeToWaitBeforeExecutingTheQueryInMilliSeconds { get { return _twitterQuery.TimeToWaitBeforeExecutingTheQueryInMilliSeconds; } }
    }
}