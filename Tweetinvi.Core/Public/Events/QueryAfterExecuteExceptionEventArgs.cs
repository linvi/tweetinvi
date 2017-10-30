using Tweetinvi.Exceptions;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public class QueryAfterExecuteExceptionEventArgs : QueryAfterExecuteEventArgs
    {
        public QueryAfterExecuteExceptionEventArgs(
            ITwitterQuery twitterQuery, 
            TwitterException exception) 
            : base(twitterQuery, null, null)
        {
            Exception = exception;
        }
    }
}
