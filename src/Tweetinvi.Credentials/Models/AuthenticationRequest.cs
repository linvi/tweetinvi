using Tweetinvi.Core.Models.Authentication;
using Tweetinvi.Models;

namespace Tweetinvi.Credentials.Models
{
    public class AuthenticationRequest : IAuthenticationRequest
    {
        public AuthenticationRequest()
        {
        }

        public AuthenticationRequest(IReadOnlyConsumerCredentialsWithoutBearer consumerCredentials)
        {
            ConsumerKey = consumerCredentials?.ConsumerKey;
            ConsumerSecret = consumerCredentials?.ConsumerSecret;
        }

        public string Id { get; set; }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AuthorizationKey { get; set; }
        public string AuthorizationSecret { get; set; }

        public string VerifierCode { get; set; }
        public string AuthorizationURL { get; set; }

        public override string ToString()
        {
            return AuthorizationURL ?? string.Empty;
        }
    }
}