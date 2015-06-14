using System;
using System.Text.RegularExpressions;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Credentials.Properties;
using Tweetinvi.Logic.Exceptions;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Credentials
{
    public class WebTokenCreator : IWebTokenCreator
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IFactory<ITemporaryCredentials> _applicationCredentialsUnityFactory;
        private readonly IOAuthWebRequestGenerator _oAuthWebRequestGenerator;
        private readonly ITwitterRequestHandler _twitterRequestHandler;

        public WebTokenCreator(
            IExceptionHandler exceptionHandler,
            IFactory<ITemporaryCredentials> applicationCredentialsUnityFactory,
            IOAuthWebRequestGenerator oAuthWebRequestGenerator,
            ITwitterRequestHandler twitterRequestHandler)
        {
            _exceptionHandler = exceptionHandler;
            _applicationCredentialsUnityFactory = applicationCredentialsUnityFactory;
            _oAuthWebRequestGenerator = oAuthWebRequestGenerator;
            _twitterRequestHandler = twitterRequestHandler;
        }

        // Step 0 - Generate Application Credentials
        public ITemporaryCredentials GenerateApplicationCredentials(string consumerKey, string consumerSecret)
        {
            var consumerKeyParameterOverride = _applicationCredentialsUnityFactory.GenerateParameterOverrideWrapper("consumerKey", consumerKey);
            var consumerSecretParameterOverride = _applicationCredentialsUnityFactory.GenerateParameterOverrideWrapper("consumerSecret", consumerSecret);

            return _applicationCredentialsUnityFactory.Create(consumerKeyParameterOverride, consumerSecretParameterOverride);
        }

        // Step 1 - Generate Authorization URL
        public string GetPinCodeAuthorizationURL(ITemporaryCredentials temporaryCredentials)
        {
            return GetAuthorizationURL(temporaryCredentials, Resources.OAuth_PINCode_CallbackURL);
        }

        public string GetAuthorizationURL(ITemporaryCredentials temporaryCredentials, string callbackURL)
        {
            try
            {
                var callbackParameter = _oAuthWebRequestGenerator.GenerateParameter("oauth_callback", callbackURL, true, true, false);
                var requestTokenResponse = _twitterRequestHandler.ExecuteQueryWithTemporaryCredentials(Resources.OAuthRequestToken, HttpMethod.POST, temporaryCredentials, new[] {callbackParameter});

                if (!string.IsNullOrEmpty(requestTokenResponse) && requestTokenResponse != Resources.OAuthRequestToken)
                {
                    Match tokenInformation = Regex.Match(requestTokenResponse, Resources.OAuthTokenRequestRegex);

                    bool callbackConfirmed = Boolean.Parse(tokenInformation.Groups["oauth_callback_confirmed"].Value);
                    if (!callbackConfirmed)
                    {
                        return null;
                    }

                    temporaryCredentials.AuthorizationKey = tokenInformation.Groups["oauth_token"].Value;
                    temporaryCredentials.AuthorizationSecret = tokenInformation.Groups["oauth_token_secret"].Value;

                    return String.Format("{0}?oauth_token={1}", Resources.OAuthRequestAuthorize, temporaryCredentials.AuthorizationKey);
                }
            }
            catch (TwitterException ex)
            {
                LogExceptionOrThrow(ex);
            }

            return null;
        }

        // Step 2 - Generate Credentials
        public string GetVerifierCodeFromCallbackURL(string callbackURL)
        {
            if (callbackURL == null)
            {
                return null;
            }

            Match urlInformation = Regex.Match(callbackURL, Resources.OAuthToken_GetVerifierCode_Regex);
            return urlInformation.Groups["oauth_verifier"].Value;
        }

        public IOAuthCredentials GetCredentialsFromCallbackURL(string callbackURL, ITemporaryCredentials temporaryCredentials)
        {
            Match urlInformation = Regex.Match(callbackURL, Resources.OAuthToken_GetVerifierCode_Regex);

            String responseOAuthToken = urlInformation.Groups["oauth_token"].Value;
            String verifierCode = urlInformation.Groups["oauth_verifier"].Value;

            // Check that the callback URL response passed in is for our current credentials....
            if (String.Equals(responseOAuthToken, temporaryCredentials.AuthorizationKey))
            {
                GetCredentialsFromVerifierCode(verifierCode, temporaryCredentials);
            }

            return null;
        }

        public IOAuthCredentials GetCredentialsFromVerifierCode(string verifierCode, ITemporaryCredentials temporaryCredentials)
        {
            temporaryCredentials.VerifierCode = verifierCode;
            return GenerateToken(temporaryCredentials);
        }

        public IOAuthCredentials GenerateToken(ITemporaryCredentials temporaryCredentials)
        {
            var callbackParameter = _oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", temporaryCredentials.VerifierCode, true, true, false);

            try
            {
                var response = _twitterRequestHandler.ExecuteQueryWithTemporaryCredentials(Resources.OAuthRequestAccessToken, HttpMethod.POST, temporaryCredentials, new[] { callbackParameter });
                if (response == null)
                {
                    return null;
                }

                Match responseInformation = Regex.Match(response, "oauth_token=(?<oauth_token>(?:\\w|\\-)*)&oauth_token_secret=(?<oauth_token_secret>(?:\\w)*)&user_id=(?<user_id>(?:\\d)*)&screen_name=(?<screen_name>(?:\\w)*)");

                var credentials = new OAuthCredentials();
                credentials.AccessToken = responseInformation.Groups["oauth_token"].Value;
                credentials.AccessTokenSecret = responseInformation.Groups["oauth_token_secret"].Value;
                credentials.ConsumerKey = temporaryCredentials.ConsumerKey;
                credentials.ConsumerSecret = temporaryCredentials.ConsumerSecret;

                return credentials;
            }
            catch (TwitterException ex)
            {
                LogExceptionOrThrow(ex);
            }

            return null;
        }

        private void LogExceptionOrThrow(TwitterException ex)
        {
            if (_exceptionHandler.LogExceptions)
            {
                _exceptionHandler.AddTwitterException(ex);
            }

            if (!_exceptionHandler.SwallowWebExceptions)
            {
                throw ex;
            }
        }
    }
}