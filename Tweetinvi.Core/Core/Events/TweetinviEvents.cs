using System;
using Tweetinvi.Events;

namespace Tweetinvi.Core.Events
{
    public interface ITweetinviEvents
    {
        event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecute;
        void RaiseBeforeQueryExecute(QueryBeforeExecuteEventArgs queryExecutedEventArgs);

        event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecuteAfterRateLimitAwait;
        void RaiseBeforeExecuteAfterRateLimitAwait(QueryBeforeExecuteEventArgs queryExecutedEventArgs);

        event EventHandler<QueryAfterExecuteEventArgs> QueryAfterExecute;
        void RaiseAfterQueryExecuted(QueryAfterExecuteEventArgs queryAfterExecuteEventArgs);
    }

    public class InternalTweetinviEvents : ITweetinviEvents
    {
        public event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecute;
        public void RaiseBeforeQueryExecute(QueryBeforeExecuteEventArgs queryAfterExecuteEventArgs)
        {
            this.Raise(QueryBeforeExecute, queryAfterExecuteEventArgs);
        }

        public event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecuteAfterRateLimitAwait;
        public void RaiseBeforeExecuteAfterRateLimitAwait(QueryBeforeExecuteEventArgs queryExecutedEventArgs)
        {
            this.Raise(QueryBeforeExecuteAfterRateLimitAwait, queryExecutedEventArgs);
        }

        public event EventHandler<QueryAfterExecuteEventArgs> QueryAfterExecute;
        public void RaiseAfterQueryExecuted(QueryAfterExecuteEventArgs queryAfterExecuteEventArgs)
        {
            this.Raise(QueryAfterExecute, queryAfterExecuteEventArgs);
        }
    }
}