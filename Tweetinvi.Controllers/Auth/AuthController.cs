using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.Models;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Auth;

namespace Tweetinvi.Controllers.Auth
{
    public class AuthController : IAuthController
    {
        private readonly IAuthQueryExecutor _authQueryExecutor;
        private readonly Regex _parseStartAuthProcessResponseRegex;
        private readonly Regex _parseRequestTokenResponseRegex;

        public AuthController(IAuthQueryExecutor authQueryExecutor)
        {
            _authQueryExecutor = authQueryExecutor;
            _parseStartAuthProcessResponseRegex = new Regex(Resources.Auth_RequestTokenParserRegex);
            _parseRequestTokenResponseRegex = new Regex(Resources.Auth_RequestAccessTokenRegex);
        }

        public Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerToken(ITwitterRequest request)
        {
            return _authQueryExecutor.CreateBearerToken(request);
        }

        public async Task<ITwitterResult<IAuthenticationRequest>> RequestAuthUrl(IRequestAuthUrlParameters parameters, ITwitterRequest request)
        {
            var authToken = new AuthenticationRequest(request.Query.TwitterCredentials);

            var authProcessParams = new RequestAuthUrlInternalParameters(parameters, authToken);

            if (string.IsNullOrEmpty(parameters.CallbackUrl))
            {
                authProcessParams.CallbackUrl = Resources.Auth_PinCodeUrl;
            }

            var requestTokenResponse = await _authQueryExecutor.RequestAuthUrl(authProcessParams, request).ConfigureAwait(false);

            if (string.IsNullOrEmpty(requestTokenResponse.Json) || requestTokenResponse.Json == Resources.Auth_RequestToken)
            {
                throw new TwitterAuthException(requestTokenResponse, "Invalid authentication response");
            }

            var tokenInformation = _parseStartAuthProcessResponseRegex.Match(requestTokenResponse.Json);

            if (!bool.TryParse(tokenInformation.Groups["oauth_callback_confirmed"].Value, out var callbackConfirmed) || !callbackConfirmed)
            {
                throw new TwitterAuthAbortedException(requestTokenResponse);
            }

            authToken.AuthorizationKey = tokenInformation.Groups["oauth_token"].Value;
            authToken.AuthorizationSecret = tokenInformation.Groups["oauth_token_secret"].Value;
            authToken.AuthorizationURL = $"{Resources.Auth_AuthorizeBaseUrl}?oauth_token={authToken.AuthorizationKey}";

            return new TwitterResult<IAuthenticationRequest>
            {
                Request = requestTokenResponse.Request,
                Response = requestTokenResponse.Response,
                DataTransferObject = authToken
            };
        }

        public async Task<ITwitterResult<ITwitterCredentials>> RequestCredentials(IRequestCredentialsParameters parameters, ITwitterRequest request)
        {
            var twitterResult = await _authQueryExecutor.RequestCredentials(parameters, request).ConfigureAwait(false);
            var responseInformation = _parseRequestTokenResponseRegex.Match(twitterResult.Json);

            if (responseInformation.Groups["oauth_token"] == null ||
                responseInformation.Groups["oauth_token_secret"] == null)
            {
                throw new TwitterAuthException(twitterResult, "Invalid authentication response");
            }

            var credentials = new TwitterCredentials(
                parameters.AuthRequest.ConsumerKey,
                parameters.AuthRequest.ConsumerSecret,
                responseInformation.Groups["oauth_token"].Value,
                responseInformation.Groups["oauth_token_secret"].Value);

            return new TwitterResult<ITwitterCredentials>
            {
                Request = twitterResult.Request,
                Response = twitterResult.Response,
                DataTransferObject = credentials
            };
        }
    }
}