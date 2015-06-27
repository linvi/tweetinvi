using System;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi.Core.Events
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
        public event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecute;
        public event EventHandler<QueryAfterExecuteEventArgs> QueryAfterExecute;

        public void RaiseBeforeQueryExecute(QueryBeforeExecuteEventArgs queryAfterExecuteEventArgs)
        {
            this.Raise(QueryBeforeExecute, queryAfterExecuteEventArgs);
        }

        public void RaiseAfterQueryExecuted(QueryAfterExecuteEventArgs queryAfterExecuteEventArgs)
        {
            this.Raise(QueryAfterExecute, queryAfterExecuteEventArgs);
        }
    }
}
