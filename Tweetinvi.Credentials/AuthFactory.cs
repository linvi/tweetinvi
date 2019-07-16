using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Web;
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
        Task<bool> InitializeApplicationBearer(ITwitterCredentials credentials);

        Task<ITwitterCredentials> GetCredentialsFromVerifierCode(string verifierCode, IAuthenticationToken authToken);
        Task<bool> InvalidateCredentials(ITwitterCredentials credentials);
    }

    public class AuthFactory : IAuthFactory
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ITwitterRequestHandler _twitterRequestHandler;
        private readonly IOAuthWebRequestGenerator _oAuthWebRequestGenerator;
        private readonly IJObjectStaticWrapper _jObjectStaticWrapper;
        private readonly ITwitterQueryFactory _twitterQueryFactory;
        private readonly ITweetinviSettingsAccessor _settingsAccessor;

        public AuthFactory(
            IExceptionHandler exceptionHandler,
            ITwitterRequestHandler twitterRequestHandler,
            IOAuthWebRequestGenerator oAuthWebRequestGenerator,
            IJObjectStaticWrapper jObjectStaticWrapper,
            ITwitterQueryFactory twitterQueryFactory,
            ITweetinviSettingsAccessor settingsAccessor)
        {
            _exceptionHandler = exceptionHandler;
            _twitterRequestHandler = twitterRequestHandler;
            _oAuthWebRequestGenerator = oAuthWebRequestGenerator;
            _jObjectStaticWrapper = jObjectStaticWrapper;
            _twitterQueryFactory = twitterQueryFactory;
            _settingsAccessor = settingsAccessor;
        }

        // Step 2 - Generate User Credentials
        public async Task<ITwitterCredentials> GetCredentialsFromVerifierCode(string verifierCode, IAuthenticationToken authToken)
        {
            try
            {
                if (authToken == null)
                {
                    throw new ArgumentNullException(nameof(authToken), "Authentication Token cannot be null.");
                }

                if (verifierCode == null)
                {
                    throw new ArgumentNullException(nameof(verifierCode),
                        "If you've received a verifier code that is null, " +
                        "it means that authentication has failed!");
                }

                var callbackParameter = _oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", verifierCode, true,
                    true, false);

                var authHandler = new AuthHttpHandler(callbackParameter, authToken);

                var consumerCredentials = new TwitterCredentials(authToken.ConsumerCredentials);
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
                    authToken.ConsumerKey,
                    authToken.ConsumerSecret,
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

        public async Task<bool> InitializeApplicationBearer(ITwitterCredentials credentials)
        {
            if (credentials == null)
            {
                throw new TwitterNullCredentialsException();
            }

            if (string.IsNullOrEmpty(credentials.AccessToken) ||
                string.IsNullOrEmpty(credentials.AccessTokenSecret))
            {
                try
                {
                    var twitterQuery = _twitterQueryFactory.Create("https://api.twitter.com/oauth2/token", HttpMethod.POST, credentials);

                    var twitterRequest = new TwitterRequest
                    {
                        Query = twitterQuery,
                        TwitterClientHandler = new BearerHttpHandler(),
                        ExecutionContext = new TwitterExecutionContext
                        {
                            RateLimitTrackerMode = _settingsAccessor.RateLimitTrackerMode
                        }
                    };

                    var response = await _twitterRequestHandler.ExecuteQuery(twitterRequest);
                    var accessToken = Regex.Match(response.Text, "access_token\":\"(?<value>.*)\"").Groups["value"].Value;
                    credentials.ApplicationOnlyBearerToken = accessToken;

                    return true;
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
            }

            return false;
        }

        public async Task<bool> InvalidateCredentials(ITwitterCredentials credentials)
        {
            var url = "https://api.twitter.com/oauth2/invalidate_token";

            var twitterQuery = _twitterQueryFactory.Create(url, HttpMethod.POST, credentials);

            var twitterRequest = new TwitterRequest
            {
                Query = twitterQuery,
                TwitterClientHandler = new InvalidateTokenHttpHandler(),
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