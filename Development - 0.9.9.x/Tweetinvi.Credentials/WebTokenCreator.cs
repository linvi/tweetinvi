using System;
using System.Text.RegularExpressions;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Credentials.AuthHttpHandlers;
using Tweetinvi.Credentials.Properties;
using Tweetinvi.Logic.Exceptions;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Credentials
{
    public class WebTokenCreator : IWebTokenCreator
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IOAuthWebRequestGenerator _oAuthWebRequestGenerator;
        private readonly ICredentialsStore _credentialsStore;
        private readonly ITwitterRequestHandler _twitterRequestHandler;

        public WebTokenCreator(
            IExceptionHandler exceptionHandler,
            IOAuthWebRequestGenerator oAuthWebRequestGenerator,
            ICredentialsStore credentialsStore,
            ITwitterRequestHandler twitterRequestHandler)
        {
            _exceptionHandler = exceptionHandler;
            _oAuthWebRequestGenerator = oAuthWebRequestGenerator;
            _credentialsStore = credentialsStore;
            _twitterRequestHandler = twitterRequestHandler;
        }

        // Step 1 - Generate Authorization URL
        public string GetAuthorizationURL(IConsumerCredentials appCredentials, string callbackURL, bool updateQueryIsAuthorized)
        {
            try
            {
                if (string.IsNullOrEmpty(callbackURL))
                {
                    callbackURL = Resources.OAuth_PINCode_CallbackURL;
                }
                else if (updateQueryIsAuthorized)
                {
                    var credsIdentifier = Guid.NewGuid();
                    _credentialsStore.CallbackCredentialsStore.Add(credsIdentifier, appCredentials);

                    callbackURL = callbackURL.AddParameterToQuery(Resources.RedirectRequest_CredsParamId, credsIdentifier.ToString());
                }

                var callbackParameter = _oAuthWebRequestGenerator.GenerateParameter("oauth_callback", callbackURL, true, true, false);

                var authHandler = new AuthHttpHandler(callbackParameter);
                var requestTokenResponse = _twitterRequestHandler.ExecuteQuery(Resources.OAuthRequestToken, HttpMethod.POST, authHandler,
                    new TwitterCredentials(appCredentials));

                if (!string.IsNullOrEmpty(requestTokenResponse) && requestTokenResponse != Resources.OAuthRequestToken)
                {
                    Match tokenInformation = Regex.Match(requestTokenResponse, Resources.OAuthTokenRequestRegex);

                    bool callbackConfirmed = Boolean.Parse(tokenInformation.Groups["oauth_callback_confirmed"].Value);
                    if (!callbackConfirmed)
                    {
                        return null;
                    }

                    appCredentials.AuthorizationKey = tokenInformation.Groups["oauth_token"].Value;
                    appCredentials.AuthorizationSecret = tokenInformation.Groups["oauth_token_secret"].Value;

                    return String.Format("{0}?oauth_token={1}", Resources.OAuthRequestAuthorize, appCredentials.AuthorizationKey);
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

        public ITwitterCredentials GetCredentialsFromCallbackURL(string callbackURL, IConsumerCredentials appCredentials)
        {
            Match urlInformation = Regex.Match(callbackURL, Resources.OAuthToken_GetVerifierCode_Regex);

            String responseOAuthToken = urlInformation.Groups["oauth_token"].Value;
            String verifierCode = urlInformation.Groups["oauth_verifier"].Value;

            // Check that the callback URL response passed in is for our current credentials....
            if (String.Equals(responseOAuthToken, appCredentials.AuthorizationKey))
            {
                GetCredentialsFromVerifierCode(verifierCode, appCredentials);
            }

            return null;
        }

        public ITwitterCredentials GetCredentialsFromVerifierCode(string verifierCode, IConsumerCredentials appCredentials)
        {
            appCredentials.VerifierCode = verifierCode;
            return GenerateToken(appCredentials);
        }

        public ITwitterCredentials GenerateToken(IConsumerCredentials appCredentials)
        {
            var callbackParameter = _oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", appCredentials.VerifierCode, true, true, false);

            try
            {
                var authHandler = new AuthHttpHandler(callbackParameter);
                var response = _twitterRequestHandler.ExecuteQuery(Resources.OAuthRequestAccessToken, HttpMethod.POST, authHandler, 
                    new TwitterCredentials(appCredentials));

                if (response == null)
                {
                    return null;
                }

                Match responseInformation = Regex.Match(response, "oauth_token=(?<oauth_token>(?:\\w|\\-)*)&oauth_token_secret=(?<oauth_token_secret>(?:\\w)*)&user_id=(?<user_id>(?:\\d)*)&screen_name=(?<screen_name>(?:\\w)*)");

                var credentials = new TwitterCredentials();
                credentials.AccessToken = responseInformation.Groups["oauth_token"].Value;
                credentials.AccessTokenSecret = responseInformation.Groups["oauth_token_secret"].Value;
                credentials.ConsumerKey = appCredentials.ConsumerKey;
                credentials.ConsumerSecret = appCredentials.ConsumerSecret;

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