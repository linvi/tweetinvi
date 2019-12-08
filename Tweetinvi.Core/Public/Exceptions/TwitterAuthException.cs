using Tweetinvi.Core.Web;

namespace Tweetinvi.Exceptions
{
    public class TwitterAuthException : TwitterResponseException
    {
        public TwitterAuthException(ITwitterResult twitterResult, string message) : base(twitterResult, message)
        {
        }
    }
}