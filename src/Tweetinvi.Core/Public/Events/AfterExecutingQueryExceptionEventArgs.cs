using Tweetinvi.Exceptions;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public class AfterExecutingQueryExceptionEventArgs : AfterExecutingQueryEventArgs
    {
        public AfterExecutingQueryExceptionEventArgs(
            ITwitterQuery twitterQuery, 
            TwitterException exception) 
            : base(twitterQuery, null, null)
        {
            Exception = exception;
        }
    }
}
