using System;
using System.Text.RegularExpressions;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Credentials.AuthHttpHandlers;
using Tweetinvi.Credentials.Models;
using Tweetinvi.Credentials.Properties;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Credentials
{
    public class WebTokenFactory : IWebTokenFactory
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IOAuthWebRequestGenerator _oAuthWebRequestGenerator;
        private readonly ICredentialsStore _credentialsStore;
        private readonly ITwitterRequestHandler _twitterRequestHandler;

        public WebTokenFactory(
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
        public IAuthenticationContext InitAuthenticationProcess(IConsumerCredentials appCredentials, string callbackURL, bool updateQueryIsAuthorized)
        {
            try
            {
                var authContext = new AuthenticationContext(appCredentials);
                var token = authContext.Token;

                if (string.IsNullOrEmpty(callbackURL))
                {
                    callbackURL = Resources.OAuth_PINCode_CallbackURL;
                }
                else if (updateQueryIsAuthorized)
                {
                    var credsIdentifier = Guid.NewGuid();
                    _credentialsStore.CallbackAuthenticationContextStore.Add(credsIdentifier, authContext);

                    callbackURL = callbackURL.AddParameterToQuery(Resources.RedirectRequest_CredsParamId, credsIdentifier.ToString());
                }

                var callbackParameter = _oAuthWebRequestGenerator.GenerateParameter("oauth_callback", callbackURL, true, true, false);

                var authHandler = new AuthHttpHandler(callbackParameter, authContext.Token);
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

                    token.AuthorizationKey = tokenInformation.Groups["oauth_token"].Value;
                    token.AuthorizationSecret = tokenInformation.Groups["oauth_token_secret"].Value;

                    authContext.AuthorizationURL = string.Format("{0}?oauth_token={1}", Resources.OAuthRequestAuthorize, token.AuthorizationKey);

                    return authContext;
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

        public ITwitterCredentials GetCredentialsFromCallbackURL(string callbackURL, IAuthenticationToken authToken)
        {
            Match urlInformation = Regex.Match(callbackURL, Resources.OAuthToken_GetVerifierCode_Regex);

            var responseOAuthToken = urlInformation.Groups["oauth_token"].Value;
            var verifierCode = urlInformation.Groups["oauth_verifier"].Value;

            // Check that the callback URL response passed in is for our current credentials....
            if (string.Equals(responseOAuthToken, authToken.AuthorizationKey))
            {
                GetCredentialsFromVerifierCode(verifierCode, authToken);
            }

            return null;
        }

        public ITwitterCredentials GetCredentialsFromVerifierCode(string verifierCode, IAuthenticationToken authToken)
        {
            authToken.VerifierCode = verifierCode;
            return GenerateToken(authToken);
        }

        public ITwitterCredentials GenerateToken(IAuthenticationToken authToken)
        {
            var callbackParameter = _oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", authToken.VerifierCode, true, true, false);

            try
            {
                var authHandler = new AuthHttpHandler(callbackParameter, authToken);
                var response = _twitterRequestHandler.ExecuteQuery(Resources.OAuthRequestAccessToken, HttpMethod.POST, authHandler, 
                    new TwitterCredentials(authToken.ConsumerCredentials));

                if (response == null)
                {
                    return null;
                }

                Match responseInformation = Regex.Match(response, "oauth_token=(?<oauth_token>(?:\\w|\\-)*)&oauth_token_secret=(?<oauth_token_secret>(?:\\w)*)&user_id=(?<user_id>(?:\\d)*)&screen_name=(?<screen_name>(?:\\w)*)");

                var credentials = new TwitterCredentials();
                credentials.AccessToken = responseInformation.Groups["oauth_token"].Value;
                credentials.AccessTokenSecret = responseInformation.Groups["oauth_token_secret"].Value;
                credentials.ConsumerKey = authToken.ConsumerKey;
                credentials.ConsumerSecret = authToken.ConsumerSecret;

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