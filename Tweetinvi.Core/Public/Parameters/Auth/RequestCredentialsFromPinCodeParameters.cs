using Tweetinvi.Models;

namespace Tweetinvi.Parameters.Auth
{
    public interface IRequestCredentialsFromPinCodeParameters
    {
        /// <summary>
        /// PinCode entered by the user
        /// </summary>
        string PinCode { get; set; }

        /// <summary>
        /// Token returned by the AuthenticationContext when
        /// </summary>
        IAuthenticationToken AuthToken { get; set; }
    }

    public class RequestCredentialsFromPinCodeParameters : IRequestCredentialsFromPinCodeParameters
    {

        public string PinCode { get; set; }
        public IAuthenticationToken AuthToken { get; set; }
    }
}