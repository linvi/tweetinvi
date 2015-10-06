using System;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi
{
    public static class TweetinviEvents
    {
        private static readonly ITweetinviEvents _tweetinviEvents;

        static TweetinviEvents()
        {
            _tweetinviEvents = TweetinviContainer.Resolve<ITweetinviEvents>();
        }

        /// <summary>
        /// Before an operation executes and the RateLimits are checked, this event will let you log, modify, cancel a query.
        /// In addition it is the perfect location to handle the RateLimits
        /// </summary>
        public static event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecute
        {
            add { _tweetinviEvents.QueryBeforeExecute += value; }
            remove { _tweetinviEvents.QueryBeforeExecute -= value; }
        }

        /// <summary>
        /// Before an operation executes, this event will let you log, modify, cancel a query.
        /// This event is better than QueryBeforeExecute for logging as it is happening just before the query is being executed.
        /// </summary>
        public static event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecuteAfterRateLimitAwait
        {
            add { _tweetinviEvents.QueryBeforeExecuteAfterRateLimitAwait += value; }
            remove { _tweetinviEvents.QueryBeforeExecuteAfterRateLimitAwait -= value; }
        }

        /// <summary>
        /// After an operation executes and the RateLimits are checked, this event will let you log a query and check the result/exception.
        /// </summary>
        public static event EventHandler<QueryAfterExecuteEventArgs> QueryAfterExecute
        {
            add { _tweetinviEvents.QueryAfterExecute += value; }
            remove { _tweetinviEvents.QueryAfterExecute -= value; }
        }
    }
}