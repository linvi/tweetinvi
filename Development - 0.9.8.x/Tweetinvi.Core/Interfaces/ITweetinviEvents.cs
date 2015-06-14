using System;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi.Core.Interfaces
{
    public interface ITweetinviEvents
    {
        event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecute;
        void RaiseBeforeQueryExecute(QueryBeforeExecuteEventArgs queryExecutedEventArgs);

        event EventHandler<QueryAfterExecuteEventArgs> QueryAfterExecute;
        void RaiseAfterQueryExecuted(QueryAfterExecuteEventArgs queryAfterExecuteEventArgs);
    }

    public class InternalTweetinviEvents : ITweetinviEvents
    {
        private readonly WeakEvent<EventHandler<QueryBeforeExecuteEventArgs>> _queryBeforeExecuteWeakEvent;
        private readonly WeakEvent<EventHandler<QueryAfterExecuteEventArgs>> _queryAfterExecuteWeakEvent;

        public InternalTweetinviEvents()
        {
            _queryBeforeExecuteWeakEvent = new WeakEvent<EventHandler<QueryBeforeExecuteEventArgs>>();
            _queryAfterExecuteWeakEvent = new WeakEvent<EventHandler<QueryAfterExecuteEventArgs>>();
        }

        public event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecute
        {
            add { _queryBeforeExecuteWeakEvent.AddHandler(value); }
            remove { _queryBeforeExecuteWeakEvent.RemoveHandler(value); }
        }

        public void RaiseBeforeQueryExecute(QueryBeforeExecuteEventArgs queryAfterExecuteEventArgs)
        {
            _queryBeforeExecuteWeakEvent.Raise(this, queryAfterExecuteEventArgs);
        }

        public event EventHandler<QueryAfterExecuteEventArgs> QueryAfterExecute
        {
            add { _queryAfterExecuteWeakEvent.AddHandler(value); }
            remove { _queryAfterExecuteWeakEvent.RemoveHandler(value); }
        }

        public void RaiseAfterQueryExecuted(QueryAfterExecuteEventArgs queryAfterExecuteEventArgs)
        {
            _queryAfterExecuteWeakEvent.Raise(this, queryAfterExecuteEventArgs);
        }
    }
}
