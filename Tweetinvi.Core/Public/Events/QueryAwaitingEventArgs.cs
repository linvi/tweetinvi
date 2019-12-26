using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public class QueryAwaitingEventArgs : EventArgs
    {
        private readonly string _query;
        private readonly IEndpointRateLimit _queryRateLimit;
        private readonly IReadOnlyTwitterCredentials _twitterCredentials;

        public QueryAwaitingEventArgs(
            string query,
            IEndpointRateLimit queryRateLimit,
            IReadOnlyTwitterCredentials twitterCredentials)
        {
            _query = query;
            _queryRateLimit = queryRateLimit;
            _twitterCredentials = twitterCredentials;
        }

        public string Query => _query;
        public IEndpointRateLimit QueryRateLimit => _queryRateLimit;
        public IReadOnlyTwitterCredentials Credentials => _twitterCredentials;
        public DateTime ResetDateTime => _queryRateLimit.ResetDateTime;
        public int ResetInMilliseconds => (int)_queryRateLimit.ResetDateTimeInMilliseconds;
    }
}