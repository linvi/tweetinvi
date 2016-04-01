using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Credentials.AuthHttpHandlers;
using Tweetinvi.Credentials.Properties;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Credentials
{
    public interface IAuthFactory
    {
        void InitializeApplicationBearer(ITwitterCredentials credentials);
        
        ITwitterCredentials GetCredentialsFromVerifierCode(string verifierCode, IAuthenticationToken authToken);
        bool InvalidateCredentials(ITwitterCredentials credentials);
    }

    public class AuthFactory : IAuthFactory
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ITwitterRequestHandler _twitterRequestHandler;
        private readonly IOAuthWebRequestGenerator _oAuthWebRequestGenerator;
        private readonly IJObjectStaticWrapper _jObjectStaticWrapper;

        public AuthFactory(
            IExceptionHandler exceptionHandler,
            ITwitterRequestHandler twitterRequestHandler,
            IOAuthWebRequestGenerator oAuthWebRequestGenerator,
            IJObjectStaticWrapper jObjectStaticWrapper)
        {
            _exceptionHandler = exceptionHandler;
            _twitterRequestHandler = twitterRequestHandler;
            _oAuthWebRequestGenerator = oAuthWebRequestGenerator;
            _jObjectStaticWrapper = jObjectStaticWrapper;
        }

        // Step 2 - Generate User Credentials
        public ITwitterCredentials GetCredentialsFromVerifierCode(string verifierCode, IAuthenticationToken authToken)
        {
            try
            {
                if (verifierCode == null)
                {
                    throw new ArgumentNullException("VerifierCode", "If you've received a verifier code that is null, " +
                                                                    "it means that authentication has failed!");
                }

                var callbackParameter = _oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", verifierCode, true, true, false);

                var authHandler = new AuthHttpHandler(callbackParameter, authToken);
                var response = _twitterRequestHandler.ExecuteQuery(Resources.OAuthRequestAccessToken, HttpMethod.POST, authHandler, 
                    new TwitterCredentials(authToken.ConsumerCredentials));

                if (response == null)
                {
                    return null;
                }

                var responseInformation = Regex.Match(response, Resources.OAuthTokenAccessRegex);
                if (responseInformation.Groups["oauth_token"] == null || responseInformation.Groups["oauth_token_secret"] == null)
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

        public void InitializeApplicationBearer(ITwitterCredentials credentials)
        {
            if (credentials == null)
            {
                throw new TwitterNullCredentialsException();
            }

            if (string.IsNullOrEmpty(credentials.AccessToken) ||
                string.IsNullOrEmpty(credentials.AccessTokenSecret))
            {
                var json = _twitterRequestHandler.ExecuteQuery("https://api.twitter.com/oauth2/token", HttpMethod.POST, new BearerHttpHandler(), credentials);
                var accessToken = Regex.Match(json, "access_token\":\"(?<value>.*)\"").Groups["value"].Value;
                credentials.ApplicationOnlyBearerToken = accessToken;
            }
        }

        public bool InvalidateCredentials(ITwitterCredentials credentials)
        {
            var url = "https://api.twitter.com/oauth2/invalidate_token";

            var json = _twitterRequestHandler.ExecuteQuery(url, HttpMethod.POST, new InvalidateTokenHttpHandler(), credentials);
            var jobject = _jObjectStaticWrapper.GetJobjectFromJson(json);

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