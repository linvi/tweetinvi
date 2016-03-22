using System;
using System.Collections.Generic;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Credentials;

namespace Tweetinvi
{
    /// <summary>
    /// Provide tools to request and create new credentials to access the Twitter API.
    /// </summary>
    public static class CredentialsCreator
    {
        private static readonly ICredentialsCreator _credentialsCreator;
        private static readonly IWebTokenCreator _webTokenCreator;
        private static readonly ICredentialsStore _credentialsStore;

        /// <summary>
        /// Static objet storing the credentials for Callbacks Authentication
        /// </summary>
        public static Dictionary<Guid, IAuthenticationContext> CallbackAuthenticationContextStore
        {
            get { return _credentialsStore.CallbackAuthenticationContextStore; }
        }

        static CredentialsCreator()
        {
            _credentialsCreator = TweetinviContainer.Resolve<ICredentialsCreator>();
            _webTokenCreator = TweetinviContainer.Resolve<IWebTokenCreator>();
            _credentialsStore = TweetinviContainer.Resolve<ICredentialsStore>();
        }

        // ##############   Step 1 - Authorization URL   ###############

        /// <summary>
        /// Return an authentication context object containing an url that will let the user authenticate on twitter.
        /// If the callback url is null, the user will be redirected to PIN CODE authentication.
        /// If the callback url is defined, the user will be redirected to CALLBACK authentication.
        /// </summary>
        public static IAuthenticationContext InitAuthentication(IConsumerCredentials appCredentials, string callbackURL = null)
        {
            return _webTokenCreator.InitAuthenticationProcess(appCredentials, callbackURL, true);
        }

        /// <summary>
        /// Return an authentication context object containing an url that will let the user authenticate on twitter.
        /// If the callback url is null, the user will be redirected to PIN CODE authentication.
        /// If the callback url is defined, the user will be redirected to CALLBACK authentication.
        /// 
        /// The 'authorization_id' parameter is added by Tweetinvi to simplify the retrieval of information to 
        /// generate the credentials. Strict mode removes this parameter from your query and let you handle it your own way.
        /// </summary>
        public static IAuthenticationContext InitAuthentication_StrictMode(IConsumerCredentials appCredentials, string callbackURL = null)
        {
            return _webTokenCreator.InitAuthenticationProcess(appCredentials, callbackURL, false);
        }


        // ##############   Step 2 - Get the token from URL or pin code   ###############


        /// <summary>
        /// Get the credentials from a PIN CODE/OAUTH VERIFIER provided by twitter.com to the user.
        /// 
        /// This method generates the credentials from the ConsumerCredentials used to get the Authentication URL.
        /// </summary>
        /// <param name="verifierCode">
        /// - PIN CODE Authentication : User enters the pin given on twitter.com
        /// - URL REDIRECT : Use the value of the 'oauth_verifier' url parameter.
        /// </param>
        /// <param name="authContext">Use the same credentials as the one given as a parameter to get the Authentication URL.</param>
        public static ITwitterCredentials GetCredentialsFromVerifierCode(string verifierCode, IAuthenticationContext authContext)
        {
            return GetCredentialsFromVerifierCode(verifierCode, authContext.Token);
        }

        private static ITwitterCredentials GetCredentialsFromVerifierCode(string verifierCode, IAuthenticationToken authToken)
        {
            return _credentialsCreator.GetCredentialsFromVerifierCode(verifierCode, authToken);
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
        public static ITwitterCredentials GetCredentialsFromVerifierCode(
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

            return _credentialsCreator.GetCredentialsFromVerifierCode(verifierCode, authToken);
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
        /// <param name="authContext">
        /// If this parameter is set, the authorizationId will be used only if this object misses some required information.
        /// </param>
        public static ITwitterCredentials GetCredentialsFromVerifierCode(string verifierCode, string authorizationId, IAuthenticationContext authContext = null)
        {
            var authToken = GetCrentialsFromId(authorizationId, authContext?.Token);
            return _credentialsCreator.GetCredentialsFromVerifierCode(verifierCode, authToken);
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
        public static ITwitterCredentials GetCredentialsFromCallbackURL(string callbackURL, IAuthenticationContext authContext = null)
        {
            string verifierCode = _webTokenCreator.GetVerifierCodeFromCallbackURL(callbackURL);
            var credentialsId = callbackURL.GetURLParameter("authorization_id");

            var authToken = GetCrentialsFromId(credentialsId, authContext?.Token);
            return GetCredentialsFromVerifierCode(verifierCode, authToken);
        }

        private static IAuthenticationToken GetCrentialsFromId(string identifier, IAuthenticationToken authToken)
        {
            if (authToken == null ||
                string.IsNullOrEmpty(authToken.AuthorizationKey) ||
                string.IsNullOrEmpty(authToken.AuthorizationSecret))
            {
                IAuthenticationContext authContext;
                if (_credentialsStore.TryGetValue(identifier, out authContext))
                {
                    authToken = authContext.Token;
                }
                else
                {
                    if (authToken == null)
                    {
                        throw new ArgumentException("The credentials are required as the URL does not contain the credentials identifier.");
                    }

                    throw new ArgumentException("The credentials needs the AuthorizationKey and AuthorizationSecret to be set up as the URL does not contain the credentials identifier.");
                }
            }

            return authToken;
        }
    }
}