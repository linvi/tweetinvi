using Tweetinvi.Models;

namespace Tweetinvi.Credentials.Models
{
    public class AuthenticationContext : IAuthenticationContext
    {
        public AuthenticationContext(IConsumerCredentials consumerCredentials)
        {
            Token = new AuthenticationToken();
            Token.ConsumerCredentials = consumerCredentials.Clone();
        }

        public string AuthorizationURL { get; set; }

        public IAuthenticationToken Token { get; }

        public override string ToString()
        {
            return AuthorizationURL ?? string.Empty;
        }
    }
}