using System;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi.Core.Events
{
    public interface ITweetinviThreadEvents
    {
        event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecute;
        event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecuteAfterRateLimitAwait;
        event EventHandler<QueryAfterExecuteEventArgs> QueryAfterExecute;
    }

    public class TweetinviThreadEvents : ITweetinviThreadEvents
    {
        public event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecute;
        public event EventHandler<QueryBeforeExecuteEventArgs> QueryBeforeExecuteAfterRateLimitAwait;
        public event EventHandler<QueryAfterExecuteEventArgs> QueryAfterExecute;

        public void RaiseQueryBeforeExecute(object sender, QueryBeforeExecuteEventArgs args)
        {
            this.Raise(sender, QueryBeforeExecute, args);
        }
        public void RaiseQueryBeforeExecuteAfterRateLimitAwait(object sender, QueryBeforeExecuteEventArgs args)
        {
            this.Raise(sender, QueryBeforeExecuteAfterRateLimitAwait, args);
        }

        public void RaiseQueryAfterExecute(object sender, QueryAfterExecuteEventArgs args)
        {
            this.Raise(sender, QueryAfterExecute, args);
        }
    }
}