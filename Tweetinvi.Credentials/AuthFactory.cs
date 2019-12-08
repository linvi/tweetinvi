using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Credentials.AuthHttpHandlers;
using Tweetinvi.Credentials.Properties;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Credentials
{
    public interface IAuthFactory
    {

        Task<ITwitterCredentials> GetCredentialsFromVerifierCode(string verifierCode, IAuthenticationRequestToken authRequestToken);
        Task<bool> InvalidateCredentials(ITwitterCredentials credentials);
    }

    public class AuthFactory : IAuthFactory
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ITwitterRequestHandler _twitterRequestHandler;
        private readonly IOAuthWebRequestGeneratorFactory _oAuthWebRequestGeneratorFactory;
        private readonly IJObjectStaticWrapper _jObjectStaticWrapper;
        private readonly ITwitterQueryFactory _twitterQueryFactory;
        private readonly ITweetinviSettingsAccessor _settingsAccessor;

        public AuthFactory(
            IExceptionHandler exceptionHandler,
            ITwitterRequestHandler twitterRequestHandler,
            IOAuthWebRequestGeneratorFactory oAuthWebRequestGeneratorFactory,
            IJObjectStaticWrapper jObjectStaticWrapper,
            ITwitterQueryFactory twitterQueryFactory,
            ITweetinviSettingsAccessor settingsAccessor)
        {
            _exceptionHandler = exceptionHandler;
            _twitterRequestHandler = twitterRequestHandler;
            _oAuthWebRequestGeneratorFactory = oAuthWebRequestGeneratorFactory;
            _jObjectStaticWrapper = jObjectStaticWrapper;
            _twitterQueryFactory = twitterQueryFactory;
            _settingsAccessor = settingsAccessor;
        }

        // Step 2 - Generate User Credentials
        public async Task<ITwitterCredentials> GetCredentialsFromVerifierCode(string verifierCode, IAuthenticationRequestToken authRequestToken)
        {
            try
            {
                if (authRequestToken == null)
                {
                    throw new ArgumentNullException(nameof(authRequestToken), "Authentication Token cannot be null.");
                }

                if (verifierCode == null)
                {
                    throw new ArgumentNullException(nameof(verifierCode),
                        "If you've received a verifier code that is null, " +
                        "it means that authentication has failed!");
                }

                var oAuthWebRequestGenerator = _oAuthWebRequestGeneratorFactory.Create();
                var callbackParameter = oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", verifierCode, true,
                    true, false);

                var authHandler = new AuthHttpHandler(callbackParameter, authRequestToken, oAuthWebRequestGenerator);

                var consumerCredentials = new TwitterCredentials(authRequestToken.ConsumerKey, authRequestToken.ConsumerSecret);
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

                var responseInformation = Regex.Match(response.Text, Resources.OAuthTokenAccessRegex);
                if (responseInformation.Groups["oauth_token"] == null ||
                    responseInformation.Groups["oauth_token_secret"] == null)
                {
                    return null;
                }

                var credentials = new TwitterCredentials(
                    authRequestToken.ConsumerKey,
                    authRequestToken.ConsumerSecret,
                    responseInformation.Groups["oauth_token"].Value,
                    responseInformation.Groups["oauth_token_secret"].Value);

                return credentials;
            }
            catch (TwitterException ex)
            {
                if (_exceptionHandler.LogExceptions)
                {
                    _exceptionHandler.AddTwitterException(ex);
                }

                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }
            }

            return null;
        }

        public async Task<bool> InvalidateCredentials(ITwitterCredentials credentials)
        {
            var url = "https://api.twitter.com/oauth2/invalidate_token";

            var twitterQuery = _twitterQueryFactory.Create(url, HttpMethod.POST, credentials);
            var oAuthWebRequestGenerator = _oAuthWebRequestGeneratorFactory.Create();

            var twitterRequest = new TwitterRequest
            {
                Query = twitterQuery,
                TwitterClientHandler = new InvalidateTokenHttpHandler(oAuthWebRequestGenerator),
                ExecutionContext = new TwitterExecutionContext
                {
                    RateLimitTrackerMode = _settingsAccessor.RateLimitTrackerMode
                }
            };

            var response = await _twitterRequestHandler.ExecuteQuery(twitterRequest);

            var jobject = _jObjectStaticWrapper.GetJobjectFromJson(response.Text);

            JToken unused;
            if (jobject.TryGetValue("access_token", out unused))
            {
                return true;
            }

            try
            {
                var errorsObject = jobject["errors"];
                var errors = _jObjectStaticWrapper.ToObject<ITwitterExceptionInfo[]>(errorsObject);

                _exceptionHandler.TryLogExceptionInfos(errors, url);
            }
            catch (Exception)
            {
                // Something is definitely wrong!
            }

            return false;
        }
    }
}