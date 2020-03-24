using Tweetinvi.Models;

namespace Tweetinvi.Parameters
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
        IAuthenticationRequest AuthRequest { get; set; }
    }

    public class RequestCredentialsFromCallbackUrlParameters : IRequestCredentialsFromCallbackUrlParameters
    {
        public string CallbackUrl { get; set; }
        public IAuthenticationRequest AuthRequest { get; set; }
    }
}