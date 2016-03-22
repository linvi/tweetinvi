namespace Tweetinvi.Core.Credentials
{
    public interface IAuthenticationContext
    {
        /// <summary>
        /// URL directing the user to Twitter authentication page for your application.
        /// </summary>
        string AuthorizationURL { get; set; }

        /// <summary>
        /// Internal information used through the authentication process
        /// </summary>
        IAuthenticationToken Token { get; }
    }

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