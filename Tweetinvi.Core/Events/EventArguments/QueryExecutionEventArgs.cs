using System;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.WebLogic;

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
        public ITwitterCredentials Credentials { get { return _twitterQuery.TwitterCredentials; }}
    }
}