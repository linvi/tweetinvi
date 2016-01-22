using System.Net;

namespace Tweetinvi.Core.Exceptions
{
    public class UserStreamFailedToInitialize : WebException
    {
        public UserStreamFailedToInitialize(string message) : base(message)
        {
        }
    }
}