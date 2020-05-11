using Tweetinvi.Core.Web;

namespace Tweetinvi.Exceptions
{
    /// <summary>
    /// This exception informs that the authentication process failed.
    /// </summary>
    public class TwitterAuthException : TwitterResponseException
    {
        public TwitterAuthException(ITwitterResult twitterResult, string message) : base(twitterResult, message)
        {
        }
    }
}