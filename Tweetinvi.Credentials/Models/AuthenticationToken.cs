using Tweetinvi.Models;

namespace Tweetinvi.Credentials.Models
{
    public class AuthenticationToken : IAuthenticationToken
    {
        public AuthenticationToken()
        {
        }

        public AuthenticationToken(IReadOnlyConsumerCredentialsWithoutBearer consumerCredentials)
        {
            ConsumerKey = consumerCredentials?.ConsumerKey;
            ConsumerSecret = consumerCredentials?.ConsumerSecret;
        }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AuthorizationKey { get; set; }
        public string AuthorizationSecret { get; set; }

        public string VerifierCode { get; set; }
        public string Id { get; set; }
    }
}