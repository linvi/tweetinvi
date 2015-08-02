using System;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Credentials;

namespace Tweetinvi
{
    public static class CredentialsCreator
    {
        private static readonly ICredentialsCreator _credentialsCreator;
        private static readonly IWebTokenCreator _webTokenCreator;
        private static readonly ICredentialsStore _credentialsStore;

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

        // Step 1 - Authorization URL
        /// <summary>
        /// Return an URL that will let the user authenticate on twitter.
        /// If the callback url is null, the user will be redirected to PIN CODE authentication.
        /// If the callback url is defined, the user will be redirected to CALLBACK authentication.
        /// </summary>
        public static string GetAuthorizationURL(IConsumerCredentials appCredentials, string callbackURL = null)
        {
            return _webTokenCreator.GetAuthorizationURL(appCredentials, callbackURL, true);
        }

        public static string GetAuthorizationURL_StrictMode(IConsumerCredentials appCredentials, string callbackURL = null)
        {
            return _webTokenCreator.GetAuthorizationURL(appCredentials, callbackURL, false);
        }

        // Step 2 - Get the token from URL or pin code
        /// <summary>
        /// If a user has selected PIN CODE authentication, Twitter give him a pin which is the verifier code.
        /// This code needs to be used to complete the authentication of the user.
        /// </summary>
        public static ITwitterCredentials GetCredentialsFromVerifierCode(string verifierCode, IConsumerCredentials appCredentials)
        {
            return _credentialsCreator.GetCredentialsFromVerifierCode(verifierCode, appCredentials);
        }

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

        public static ITwitterCredentials GetCredentialsFromVerifierCode(string verifierCode, string authorizationId, IConsumerCredentials appCredentials = null)
        {
            appCredentials = GetCrentialsFromId(authorizationId, appCredentials);
            return _credentialsCreator.GetCredentialsFromVerifierCode(verifierCode, appCredentials);
        }

        /// <summary>
        /// PLEASE READ. If you are being redirected make sure that the ConsumerCredentials have its
        /// AuthorizationKey and AuthorizationSecret set up as they were before the redirect.
        /// 
        /// If a user has selected CALLBACK authentication, Twitter redirects to the URL you've specified.
        /// The url will contain authentication parameters. 
        /// Please provide the entire URL including the authentication parameters!
        /// </summary>
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