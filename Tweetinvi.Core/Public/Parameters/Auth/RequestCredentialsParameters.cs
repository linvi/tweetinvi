using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi.Auth;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters.Auth
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/basics/authentication/api-reference/access_token
    /// </summary>
    public interface IRequestCredentialsParameters
    {
        /// <summary>
        /// The verification code returned by Twitter also known as oauth_verifier
        /// </summary>
        string VerifierCode { get; set; }

        /// <summary>
        /// Token returned by the AuthenticationContext when
        /// </summary>
        IAuthenticationRequestToken AuthRequestToken { get; set; }
    }

    public class RequestCredentialsParameters : IRequestCredentialsParameters
    {
        /// <summary>
        /// Regex used to extract oAuthVerifier and oAuthToken from callback url
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public static Regex AuthExtractOAuthVerifierFromCallbackUrlRegex { get; set; } = new Regex("(?:\\?|%3f|&|%26)oauth_token(?:=|%3d)(?<oauth_token>(?:\\w|\\-)*)(?:&|%26)oauth_verifier(?:=|%3d)(?<oauth_verifier>(?:\\w|\\-)*)");

        public static IRequestCredentialsParameters FromPinCode(string pinCode, IAuthenticationRequestToken authRequestToken)
        {
            return new RequestCredentialsParameters(pinCode, authRequestToken);
        }

        /// <summary>
        /// Generate the request credentials parameters from an AuthenticationTokenProvider
        /// If the url does not contain the expected input or the token provider cannot find
        /// the authentication token, this will return an error.
        /// </summary>
        /// <exception cref="ArgumentException">When callback url is not properly formatted</exception>
        public static async Task<IRequestCredentialsParameters> FromCallbackUrl(string callbackUrl, IAuthenticationTokenProvider authenticationTokenProvider)
        {
            var tokenId = authenticationTokenProvider.ExtractTokenIdFromCallbackUrl(callbackUrl);

            var authToken = await authenticationTokenProvider.GetAuthenticationTokenFromId(tokenId);
            if (authToken == null)
            {
                throw new Exception("Could not retrieve the authentication token");
            }

            await authenticationTokenProvider.RemoveAuthenticationToken(tokenId);

            var oAuthVerifier = callbackUrl.GetURLParameter("oauth_verifier");

            if (oAuthVerifier == null)
            {
                throw new ArgumentException($"oauth_verifier query parameter not found, this is required to authenticate the user", nameof(callbackUrl));
            }

            return new RequestCredentialsParameters(oAuthVerifier, authToken);
        }

        public static IRequestCredentialsParameters FromCallbackUrl(string callbackUrl, IAuthenticationRequestToken authRequestToken)
        {
            var urlInformation = AuthExtractOAuthVerifierFromCallbackUrlRegex.Match(callbackUrl);
            var oAuthVerifier = urlInformation.Groups["oauth_verifier"].Value;
            return new RequestCredentialsParameters(oAuthVerifier, authRequestToken);
        }

        public RequestCredentialsParameters(string verifierCode, IAuthenticationRequestToken authenticationRequestToken)
        {
            VerifierCode = verifierCode;
            AuthRequestToken = authenticationRequestToken;
        }

        public string VerifierCode { get; set; }
        public IAuthenticationRequestToken AuthRequestToken { get; set; }
    }
}