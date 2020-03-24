using Tweetinvi.Core.Web;

namespace Tweetinvi.Exceptions
{
    public class TwitterAuthAbortedException : TwitterAuthException
    {
        public TwitterAuthAbortedException(ITwitterResult twitterResult) : base(twitterResult, "Authentication did not proceed until the end")
        {
        }
    }
}