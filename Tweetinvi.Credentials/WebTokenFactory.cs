using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Credentials.AuthHttpHandlers;
using Tweetinvi.Credentials.Properties;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Credentials
{
    public class WebTokenFactory : IWebTokenFactory
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IOAuthWebRequestGeneratorFactory _oAuthWebRequestGeneratorFactory;
        private readonly ITwitterRequestHandler _twitterRequestHandler;
        private readonly ITwitterQueryFactory _twitterQueryFactory;
        private readonly ITweetinviSettingsAccessor _settingsAccessor;

        public WebTokenFactory(
            IExceptionHandler exceptionHandler,
            IOAuthWebRequestGeneratorFactory oAuthWebRequestGeneratorFactory,
            ITwitterRequestHandler twitterRequestHandler,
            ITwitterQueryFactory twitterQueryFactory,
            ITweetinviSettingsAccessor settingsAccessor)
        {
            _exceptionHandler = exceptionHandler;
            _oAuthWebRequestGeneratorFactory = oAuthWebRequestGeneratorFactory;
            _twitterRequestHandler = twitterRequestHandler;
            _twitterQueryFactory = twitterQueryFactory;
            _settingsAccessor = settingsAccessor;
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

        public async Task<ITwitterCredentials> GetCredentialsFromCallbackURL(string callbackURL, IAuthenticationToken authToken)
        {
            Match urlInformation = Regex.Match(callbackURL, Resources.OAuthToken_GetVerifierCode_Regex);

            var responseOAuthToken = urlInformation.Groups["oauth_token"].Value;
            var verifierCode = urlInformation.Groups["oauth_verifier"].Value;

            // Check that the callback URL response passed in is for our current credentials....
            if (string.Equals(responseOAuthToken, authToken.AuthorizationKey))
            {
                await GetCredentialsFromVerifierCode(verifierCode, authToken);
            }

            return null;
        }

        public Task<ITwitterCredentials> GetCredentialsFromVerifierCode(string verifierCode, IAuthenticationToken authToken)
        {
            authToken.VerifierCode = verifierCode;
            return GenerateToken(authToken);
        }

        public async Task<ITwitterCredentials> GenerateToken(IAuthenticationToken authToken)
        {
            var oAuthWebRequestGenerator = _oAuthWebRequestGeneratorFactory.Create();
            var callbackParameter = oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", authToken.VerifierCode, true, true, false);

            try
            {
                var authHandler = new AuthHttpHandler(callbackParameter, authToken, oAuthWebRequestGenerator);
                var consumerCredentials = new TwitterCredentials(authToken.ConsumerKey, authToken.ConsumerSecret);

                var twitterQuery = _twitterQueryFactory.Create(Resources.OAuthRequestAccessToken, HttpMethod.POST, consumerCredentials);

                var twitterRequest = new TwitterRequest
                {
                    Query = twitterQuery,
                    TwitterClientHandler = authHandler,
                    ExecutionContext = new TwitterExecutionContext
                    {
                        RateLimitTrackerMode = _settingsAccessor.RateLimitTrackerMode
                    }
                };

                var response = await _twitterRequestHandler.ExecuteQuery(twitterRequest);

                if (response == null)
                {
                    return null;
                }

                Match responseInformation = Regex.Match(response.Text, "oauth_token=(?<oauth_token>(?:\\w|\\-)*)&oauth_token_secret=(?<oauth_token_secret>(?:\\w)*)&user_id=(?<user_id>(?:\\d)*)&screen_name=(?<screen_name>(?:\\w)*)");

                var credentials = new TwitterCredentials
                {
                    AccessToken = responseInformation.Groups["oauth_token"].Value,
                    AccessTokenSecret = responseInformation.Groups["oauth_token_secret"].Value,
                    ConsumerKey = authToken.ConsumerKey,
                    ConsumerSecret = authToken.ConsumerSecret
                };

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