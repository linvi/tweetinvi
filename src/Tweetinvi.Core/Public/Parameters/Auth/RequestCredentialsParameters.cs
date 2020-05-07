using System;
using System.Threading.Tasks;
using Tweetinvi.Auth;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/basics/authentication/api-reference/access_token
    /// </summary>
    public interface IRequestCredentialsParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The verification code returned by Twitter also known as oauth_verifier
        /// </summary>
        string VerifierCode { get; set; }

        /// <summary>
        /// Token returned by the AuthenticationContext when
        /// </summary>
        IAuthenticationRequest AuthRequest { get; set; }
    }

    public class RequestCredentialsParameters : CustomRequestParameters, IRequestCredentialsParameters
    {
        /// <summary>
        /// Generate request credentials parameters to authenticate with pinCode
        /// </summary>
        public static IRequestCredentialsParameters FromPinCode(string pinCode, IAuthenticationRequest authRequest)
        {
            return new RequestCredentialsParameters(pinCode, authRequest);
        }

        /// <inheritdoc cref="FromCallbackUrl(string, IAuthenticationRequest)"/>
        public static IRequestCredentialsParameters FromCallbackUrl(Uri callbackUri, IAuthenticationRequest authenticationRequestStore)
        {
            return FromCallbackUrl(callbackUri.AbsoluteUri, authenticationRequestStore);
        }

        public static IRequestCredentialsParameters FromCallbackUrl(string callbackUrl, IAuthenticationRequest authRequest)
        {
            var oAuthVerifier = callbackUrl.GetURLParameter("oauth_verifier");
            return new RequestCredentialsParameters(oAuthVerifier, authRequest);
        }

        /// <summary>
        /// Generate the request credentials parameters from an AuthenticationTokenProvider
        /// If the url does not contain the expected input or the token provider cannot find
        /// the authentication token, this will return an error.
        /// </summary>
        /// <exception cref="ArgumentException">When callback url is not properly formatted</exception>
        public static async Task<IRequestCredentialsParameters> FromCallbackUrlAsync(string callbackUrl, IAuthenticationRequestStore authenticationRequestStore)
        {
            var tokenId = authenticationRequestStore.ExtractAuthenticationRequestIdFromCallbackUrl(callbackUrl);

            var authToken = await authenticationRequestStore.GetAuthenticationRequestFromIdAsync(tokenId).ConfigureAwait(false);
            if (authToken == null)
            {
                throw new Exception("Could not retrieve the authentication token");
            }

            await authenticationRequestStore.RemoveAuthenticationTokenAsync(tokenId).ConfigureAwait(false);

            var oAuthVerifier = callbackUrl.GetURLParameter("oauth_verifier");
            if (oAuthVerifier == null)
            {
                throw new ArgumentException($"oauth_verifier query parameter not found, this is required to authenticate the user", nameof(callbackUrl));
            }

            return new RequestCredentialsParameters(oAuthVerifier, authToken);
        }

        public RequestCredentialsParameters(string verifierCode, IAuthenticationRequest authenticationRequest)
        {
            VerifierCode = verifierCode;
            AuthRequest = authenticationRequest;
        }

        public string VerifierCode { get; set; }
        public IAuthenticationRequest AuthRequest { get; set; }
    }
}