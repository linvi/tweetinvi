using System;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi
{
    public static class TweetinviEvents
    {
        private static readonly ITweetinviEvents _tweetinviEvents;

        [ThreadStatic]
        private static TweetinviThreadEvents _threadEvents;

        // The difference between the public and private events are the fact that one exposes the interface
        // while the other exposes the class. It gives us the possibility to not use a cast to raise the events.
        private static TweetinviThreadEvents _currentThreadEvents
        {
            get
            {
                if (_threadEvents == null)
                {
                    _threadEvents = new TweetinviThreadEvents();
                }

                return _threadEvents;
            }
        }

        public static ITweetinviThreadEvents CurrentThreadEvents
        {
            get { return _currentThreadEvents; }
        }

        static TweetinviEvents()
        {
            _tweetinviEvents = TweetinviContainer.Resolve<ITweetinviEvents>();
            _tweetinviEvents.QueryBeforeExecute += (sender, args) =>
            {
                _currentThreadEvents.RaiseQueryBeforeExecute(sender, args);
            };

            _tweetinviEvents.QueryBeforeExecuteAfterRateLimitAwait += (sender, args) =>
            {
                _currentThreadEvents.RaiseQueryBeforeExecuteAfterRateLimitAwait(sender, args);
            };

            _tweetinviEvents.QueryAfterExecute += (sender, args) =>
            {
                _currentThreadEvents.RaiseQueryAfterExecute(sender, args);
            };
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