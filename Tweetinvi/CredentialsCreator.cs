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
        public static Dictionary<Guid, IConsumerCredentials> CallbackCredentialsStore
        {
            get { return _credentialsStore.CallbackCredentialsStore; }
        }

        static CredentialsCreator()
        {
            _credentialsCreator = TweetinviContainer.Resolve<ICredentialsCreator>();
            _webTokenCreator = TweetinviContainer.Resolve<IWebTokenCreator>();
            _credentialsStore = TweetinviContainer.Resolve<ICredentialsStore>();
        }

        // ##############   Step 1 - Authorization URL   ###############

        /// <summary>
        /// Return an URL that will let the user authenticate on twitter.
        /// If the callback url is null, the user will be redirected to PIN CODE authentication.
        /// If the callback url is defined, the user will be redirected to CALLBACK authentication.
        /// </summary>
        public static string GetAuthorizationURL(IConsumerCredentials appCredentials, string callbackURL = null)
        {
            return _webTokenCreator.GetAuthorizationURL(appCredentials, callbackURL, true);
        }

        /// <summary>
        /// Return an URL that will let the user authenticate on twitter.
        /// If the callback url is null, the user will be redirected to PIN CODE authentication.
        /// If the callback url is defined, the user will be redirected to CALLBACK authentication.
        /// 
        /// The 'authorization_id' parameter is added by Tweetinvi to simplify the retrieval of information to 
        /// generate the credentials. Strict Mode removes this parameter from your query and let you handle it your way.
        /// </summary>
        public static string GetAuthorizationURL_StrictMode(IConsumerCredentials appCredentials, string callbackURL = null)
        {
            return _webTokenCreator.GetAuthorizationURL(appCredentials, callbackURL, false);
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
        /// <param name="appCredentials">Use the same credentials as the one given as a parameter to get the Authentication URL.</param>
        public static ITwitterCredentials GetCredentialsFromVerifierCode(string verifierCode, IConsumerCredentials appCredentials)
        {
            return _credentialsCreator.GetCredentialsFromVerifierCode(verifierCode, appCredentials);
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
            var appCredentials = new ConsumerCredentials(consumerKey, consumerSecret)
            {
                AuthorizationKey = authorizationKey,
                AuthorizationSecret = authorizationSecret
            };

            return _credentialsCreator.GetCredentialsFromVerifierCode(verifierCode, appCredentials);
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
        /// <param name="appCredentials">
        /// If this parameter is set, the authorizationId will be used only if this object misses some required information.
        /// </param>
        public static ITwitterCredentials GetCredentialsFromVerifierCode(string verifierCode, string authorizationId, IConsumerCredentials appCredentials = null)
        {
            appCredentials = GetCrentialsFromId(authorizationId, appCredentials);
            return _credentialsCreator.GetCredentialsFromVerifierCode(verifierCode, appCredentials);
        }

        /// <summary>
        /// Get the credentials from an entire callback URL.
        /// Please note that the appCredentials needs to contain the AuthorizationKey and AuthorizationSecret set up as they were before the redirect.
        /// </summary>
        /// <param name="callbackURL">Provide the entire URL (including the params) of the received callback request.</param>
        /// <param name="appCredentials">
        /// If this parameter is set, the credentials information will be extracted from it, 
        /// otherwise, Tweetinvi will attempt to access the credentials associated with the 'authorization_id' parameter.
        /// </param>
        public static ITwitterCredentials GetCredentialsFromCallbackURL(string callbackURL, IConsumerCredentials appCredentials = null)
        {
            string verifierCode = _webTokenCreator.GetVerifierCodeFromCallbackURL(callbackURL);
            var credentialsId = callbackURL.GetURLParameter("authorization_id");
            
            appCredentials = GetCrentialsFromId(credentialsId, appCredentials);
            return GetCredentialsFromVerifierCode(verifierCode, appCredentials);
        }

        private static IConsumerCredentials GetCrentialsFromId(string identifier, IConsumerCredentials appCredentials)
        {
            if (appCredentials == null ||
                string.IsNullOrEmpty(appCredentials.AuthorizationKey) ||
                string.IsNullOrEmpty(appCredentials.AuthorizationSecret))
            {
                IConsumerCredentials creds;
                if (_credentialsStore.TryGetValue(identifier, out creds))
                {
                    appCredentials = creds;
                }
                else
                {
                    if (appCredentials == null)
                    {
                        throw new ArgumentException("The credentials are required as the URL does not contain the credentials identifier.");
                    }

                    throw new ArgumentException("The credentials needs the AuthorizationKey and AuthorizationSecret to be set up as the URL does not contain the credentials identifier.");
                }
            }

            return appCredentials;
        }
    }
}