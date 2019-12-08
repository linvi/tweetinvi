using Tweetinvi.Models;

namespace Tweetinvi.Parameters.Auth
{
    public interface IRequestCredentialsFromCallbackUrlParameters
    {
        /// <summary>
        /// Callback url called by Twitter auth redirection
        /// </summary>
        string CallbackUrl { get; set; }

        /// <summary>
        /// Token returned by the AuthenticationContext when
        /// </summary>
        IAuthenticationToken AuthToken { get; set; }
    }

    public class RequestCredentialsFromCallbackUrlParameters : IRequestCredentialsFromCallbackUrlParameters
    {
        public string CallbackUrl { get; set; }
        public IAuthenticationToken AuthToken { get; set; }
    }
}