using System.Net;

namespace Tweetinvi.Exceptions
{
    public class UserStreamFailedToInitializeException : WebException
    {
        public UserStreamFailedToInitializeException(string message) : base(message)
        {
        }
    }
}