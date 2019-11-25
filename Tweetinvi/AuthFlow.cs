using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Credentials;
using Tweetinvi.Credentials.Models;
using Tweetinvi.Models;

namespace Tweetinvi
{
    /// <summary>
    /// Authentication Flow to help authenticate new users.
    /// If you already have credentials, use Auth class to start using Tweetinvi.
    /// </summary>
    public static class AuthFlow
    {
        private static readonly IAuthFactory _authFactory;
        private static readonly IWebTokenFactory _webTokenFactory;
        private static readonly ICredentialsStore _credentialsStore;

        static AuthFlow()
        {
            _authFactory = TweetinviContainer.Resolve<IAuthFactory>();
            _webTokenFactory = TweetinviContainer.Resolve<IWebTokenFactory>();
            _credentialsStore = TweetinviContainer.Resolve<ICredentialsStore>();
        }

        // ##############   Step 2 - Get the token from URL or pin code   ###############

        /// <summary>
        /// Get the credentials from a PIN CODE/OAUTH VERIFIER provided by twitter.com to the user.
        /// This method generates the credentials from the ConsumerCredentials used to get the Authentication URL.
        /// </summary>
        /// <param name="verifierCode">
        /// - PIN CODE Authentication : User enters the pin given on twitter.com
        /// - URL REDIRECT : Use the value of the 'oauth_verifier' url parameter.
        /// </param>
        /// <param name="authContext">Use the same credentials as the one given as a parameter to get the Authentication URL.</param>
        public static Task<ITwitterCredentials> CreateCredentialsFromVerifierCode(string verifierCode,
            IAuthenticationContext authContext)
        {
            return CreateCredentialsFromVerifierCode(verifierCode, authContext.Token);
        }

        public static Task<ITwitterCredentials> CreateCredentialsFromVerifierCode(string verifierCode,
            IAuthenticationToken authToken)
        {
            return _authFactory.GetCredentialsFromVerifierCode(verifierCode, authToken);
        }

        /// <summary>
        /// Get the credentials from a PIN CODE/OAUTH VERIFIER provided by twitter.com to the user.
        ///
        /// This method generates the credentials from the ConsumerCredentials used to get the Authentication URL.
        /// </summary>
        /// <param name="verifierCode">
        /// - PIN CODE Authentication : User enters the pin given on twitter.com
        /// - URL REDIRECT : Use the value of the 'oauth_verifier' url parameter.
        /// </param>
        /// <param name="authorizationKey">Authorization Key of the same credentials as the one given as a parameter to get the Authentication URL.</param>
        /// <param name="authorizationSecret">Authorization Secret of the same credentials as the one given as a parameter to get the Authentication URL.</param>
        /// <param name="consumerKey">Consumer Key of the same credentials as the one given as a parameter to get the Authentication URL.</param>
        /// <param name="consumerSecret">Consumer Secret of the same credentials as the one given as a parameter to get the Authentication URL.</param>
        public static Task<ITwitterCredentials> CreateCredentialsFromVerifierCode(
            string verifierCode,
            string authorizationKey,
            string authorizationSecret,
            string consumerKey,
            string consumerSecret)
        {
            var authToken = new AuthenticationToken
            {
                ConsumerCredentials = new ConsumerCredentials(consumerKey, consumerSecret),
                AuthorizationKey = authorizationKey,
                AuthorizationSecret = authorizationSecret
            };

            return _authFactory.GetCredentialsFromVerifierCode(verifierCode, authToken);
        }

        /// <summary>
        /// Get the credentials from a PIN CODE/OAUTH VERIFIER provided by twitter.com to the user.
        ///
        /// This method generates the credentials from the ConsumerCredentials used to get the Authentication URL.
        /// </summary>
        /// <param name="verifierCode">
        /// - PIN CODE Authentication : User enters the pin given on twitter.com
        /// - URL REDIRECT : Use the value of the 'oauth_verifier' url parameter.
        /// </param>
        /// <param name="authorizationId">
        /// Authorization Id used to store the credentials before a redirect.
        /// This can be retrieved from the callback request 'authorization_id' parameter.
        /// </param>
        public static Task<ITwitterCredentials> CreateCredentialsFromVerifierCode(string verifierCode, string authorizationId)
        {
            var authToken = CreateCrentialsFromId(authorizationId, null);
            return _authFactory.GetCredentialsFromVerifierCode(verifierCode, authToken);
        }

        /// <summary>
        /// Get the credentials from an entire callback URL.
        /// Please note that the appCredentials needs to contain the AuthorizationKey and AuthorizationSecret set up as they were before the redirect.
        /// </summary>
        /// <param name="callbackURL">Provide the entire URL (including the params) of the received callback request.</param>
        /// <param name="authContext">
        /// If this parameter is set, the credentials information will be extracted from it,
        /// otherwise, Tweetinvi will attempt to access the credentials associated with the 'authorization_id' parameter.
        /// </param>
        public static Task<ITwitterCredentials> CreateCredentialsFromCallbackURL(string callbackURL, IAuthenticationContext authContext = null)
        {
            string verifierCode = _webTokenFactory.GetVerifierCodeFromCallbackURL(callbackURL);
            var credentialsId = callbackURL.GetURLParameter("authorization_id");

            var authToken = CreateCrentialsFromId(credentialsId, authContext?.Token);
            return CreateCredentialsFromVerifierCode(verifierCode, authToken);
        }

        private static IAuthenticationToken CreateCrentialsFromId(string identifier, IAuthenticationToken authToken)
        {
            if (authToken == null ||
                string.IsNullOrEmpty(authToken.AuthorizationKey) ||
                string.IsNullOrEmpty(authToken.AuthorizationSecret))
            {
                IAuthenticationContext authContext;
                if (_credentialsStore.TryGetValue(identifier, out authContext))
                {
                    authToken = authContext.Token;
                    return authToken;
                }

                if (identifier != null)
                {
                    throw new ArgumentException("Identifier could not be matched to any stored AuthenticationContext.");
                }

                throw new ArgumentException("The credentials needs the AuthorizationKey and AuthorizationSecret to be set up as the URL does not contain the credentials identifier.");
            }

            return authToken;
        }
    }
}