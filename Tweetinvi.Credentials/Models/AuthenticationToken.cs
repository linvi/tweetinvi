using Tweetinvi.Core.Authentication;

namespace Tweetinvi.Credentials.Models
{
    public class AuthenticationToken : IAuthenticationToken
    {
        public IConsumerCredentials ConsumerCredentials { get; set; }
        public string ConsumerKey { get { return ConsumerCredentials.ConsumerKey; } }
        public string ConsumerSecret { get { return ConsumerCredentials.ConsumerSecret; } }
        public string AuthorizationKey { get; set; }
        public string AuthorizationSecret { get; set; }

        public string VerifierCode { get; set; }
    }
}