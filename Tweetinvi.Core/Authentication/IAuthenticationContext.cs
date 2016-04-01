namespace Tweetinvi.Core.Authentication
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
}