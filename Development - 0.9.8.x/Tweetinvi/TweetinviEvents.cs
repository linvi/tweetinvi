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

        public static event EventHandler<QueryAfterExecuteEventArgs> QueryAfterExecute
        {
            add { _tweetinviEvents.QueryAfterExecute += value; }
            remove { _tweetinviEvents.QueryAfterExecute -= value; }
        }

        public static event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecute
        {
            add { _tweetinviEvents.QueryBeforeExecute += value; }
            remove { _tweetinviEvents.QueryBeforeExecute -= value; }
        }
    }
}