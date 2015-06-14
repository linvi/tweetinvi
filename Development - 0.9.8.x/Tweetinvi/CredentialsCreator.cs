using System;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Credentials;

namespace Tweetinvi
{
    public static class CredentialsCreator
    {
        private static readonly ICredentialsCreator _credentialsCreator;
        private static readonly IWebTokenCreator _webTokenCreator;

        static CredentialsCreator()
        {
            _credentialsCreator = TweetinviContainer.Resolve<ICredentialsCreator>();
            _webTokenCreator = TweetinviContainer.Resolve<IWebTokenCreator>();
        }

        // Step 0
        public static ITemporaryCredentials GenerateApplicationCredentials(string consumerKey, string consumerSecret)
        {
            return _credentialsCreator.GenerateApplicationCredentials(consumerKey, consumerSecret);
        }

        // Step 1 - Code
        public static string GetAuthorizationURL(ITemporaryCredentials temporaryCredentials)
        {
            return _webTokenCreator.GetPinCodeAuthorizationURL(temporaryCredentials);
        }

        // Step 1 - Callback URL
        public static string GetAuthorizationURLForCallback(ITemporaryCredentials temporaryCredentials, string callbackURL)
        {
            return _webTokenCreator.GetAuthorizationURL(temporaryCredentials, callbackURL);
        }

        // Step 2
        public static IOAuthCredentials GetCredentialsFromVerifierCode(string verifierCode, ITemporaryCredentials temporaryCredentials)
        {
            return _credentialsCreator.GetCredentialsFromVerifierCode(verifierCode, temporaryCredentials);
        }

        public static IOAuthCredentials GetCredentialsFromCallbackURL(string callbackURL, ITemporaryCredentials temporaryCredentials)
        {
            string verifierCode = _webTokenCreator.GetVerifierCodeFromCallbackURL(callbackURL);
            return GetCredentialsFromVerifierCode(verifierCode, temporaryCredentials);
        }

        // All steps
        public static IOAuthCredentials GetCredentialsFromCaptcha(Func<string, string> retrieveCaptchaAction, string consumerKey, string consumerSecret)
        {
            var applicationCredentials = GenerateApplicationCredentials(consumerKey, consumerSecret);
            var url = GetAuthorizationURL(applicationCredentials);
            var verifierCode = retrieveCaptchaAction(url);
            return GetCredentialsFromVerifierCode(verifierCode, applicationCredentials);
        }

        public static IOAuthCredentials GetCredentialsFromCallbackURL_UsingRedirectedCallbackURL(Func<string, string> retrieveCallbackURL, string consumerKey, string consumerSecret, string callbackURL)
        {
            var applicationCredentials = GenerateApplicationCredentials(consumerKey, consumerSecret);
            var url = GetAuthorizationURLForCallback(applicationCredentials, callbackURL);
            var redirectedURL = retrieveCallbackURL(url);
            return GetCredentialsFromCallbackURL(redirectedURL, applicationCredentials);
        }

        public static IOAuthCredentials GetCredentialsFromCallbackURL_UsingRedirectedVerifierCode(Func<string, string> retrieveVerifierCode, string consumerKey, string consumerSecret, string callbackURL)
        {
            var applicationCredentials = GenerateApplicationCredentials(consumerKey, consumerSecret);
            var url = GetAuthorizationURLForCallback(applicationCredentials, callbackURL);
            var redirectedVerifierCode = retrieveVerifierCode(url);
            return GetCredentialsFromVerifierCode(redirectedVerifierCode, applicationCredentials);
        }
    }
}