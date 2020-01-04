using System;
using Tweetinvi.Events;

namespace Tweetinvi.Core.Events
{
    public interface ITweetinviEvents : ITwitterClientEvents
    {
    }

    public class TweetinviEvents : TwitterClientEvents, ITweetinviEvents
    {
    }

    public interface IExternalClientEvents
    {
        event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecute;
        event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecuteAfterRateLimitAwait;
        event EventHandler<QueryAfterExecuteEventArgs> QueryAfterExecute;
    }

    public interface ITwitterClientEvents : IExternalClientEvents
    {
        void RaiseBeforeQueryExecute(QueryBeforeExecuteEventArgs queryExecutedEventArgs);
        void RaiseBeforeExecuteAfterRateLimitAwait(QueryBeforeExecuteEventArgs queryExecutedEventArgs);
        void RaiseAfterQueryExecuted(QueryAfterExecuteEventArgs queryAfterExecuteEventArgs);
    }

    public class TwitterClientEvents : ITwitterClientEvents
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