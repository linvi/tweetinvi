using Tweetinvi.Models;

namespace Tweetinvi.Credentials.Models
{
    public class AuthenticationContext : IAuthenticationContext
    {
        public AuthenticationContext(IReadOnlyConsumerCredentials consumerCredentials)
        {
            Token = new AuthenticationToken
            {
                ConsumerCredentials = new ConsumerCredentials(consumerCredentials)
            };
        }

        public string AuthorizationURL { get; set; }

        public IAuthenticationToken Token { get; }

        public override string ToString()
        {
            return AuthorizationURL ?? string.Empty;
        }
    }
}