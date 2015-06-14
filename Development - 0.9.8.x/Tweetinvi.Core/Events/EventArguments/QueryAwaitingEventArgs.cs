using System;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Core.Events.EventArguments
{
    public class QueryAwaitingEventArgs : EventArgs
    {
        private readonly string _query;
        private readonly ITokenRateLimit _queryRateLimit;
        private readonly IOAuthCredentials _oAuthCredentials;

        public QueryAwaitingEventArgs(
            string query,
            ITokenRateLimit queryRateLimit,
            IOAuthCredentials oAuthCredentials)
        {
            _query = query;
            _queryRateLimit = queryRateLimit;
            _oAuthCredentials = oAuthCredentials;
        }

        public string Query { get { return _query; } }
        public ITokenRateLimit QueryRateLimit { get { return _queryRateLimit; } }
        public IOAuthCredentials Credentials { get { return _oAuthCredentials; } }

        public DateTime ResetDateTime { get { return _queryRateLimit.ResetDateTime; } }
        public int ResetInMilliseconds { get { return ((int)_queryRateLimit.ResetDateTimeInMilliseconds); } }
    }
}