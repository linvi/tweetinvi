using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.Models;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Auth
{
    public class AuthController : IAuthController
    {
        private readonly IAuthQueryExecutor _authQueryExecutor;
        private readonly Regex _parseRequestUrlResponseRegex;

        public AuthController(IAuthQueryExecutor authQueryExecutor)
        {
            _authQueryExecutor = authQueryExecutor;
            _parseRequestUrlResponseRegex = new Regex(Resources.Auth_RequestTokenParserRegex);
        }

        public Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerTokenAsync(ICreateBearerTokenParameters parameters, ITwitterRequest request)
        {
            return _authQueryExecutor.CreateBearerTokenAsync(parameters, request);
        }

        public async Task<ITwitterResult<IAuthenticationRequest>> RequestAuthUrlAsync(IRequestAuthUrlParameters parameters, ITwitterRequest request)
        {
            var authToken = new AuthenticationRequest(request.Query.TwitterCredentials);

            var authProcessParams = new RequestAuthUrlInternalParameters(parameters, authToken);

            if (string.IsNullOrEmpty(parameters.CallbackUrl))
            {
                authProcessParams.CallbackUrl = Resources.Auth_PinCodeUrl;
            }

            var requestTokenResponse = await _authQueryExecutor.RequestAuthUrlAsync(authProcessParams, request).ConfigureAwait(false);

            if (string.IsNullOrEmpty(requestTokenResponse.RawResult) || requestTokenResponse.RawResult == Resources.Auth_RequestToken)
            {
                throw new TwitterAuthException(requestTokenResponse, "Invalid authentication response");
            }

            var tokenInformation = _parseRequestUrlResponseRegex.Match(requestTokenResponse.RawResult);

            if (!bool.TryParse(tokenInformation.Groups["oauth_callback_confirmed"].Value, out var callbackConfirmed) || !callbackConfirmed)
            {
                throw new TwitterAuthAbortedException(requestTokenResponse);
            }

            authToken.AuthorizationKey = tokenInformation.Groups["oauth_token"].Value;
            authToken.AuthorizationSecret = tokenInformation.Groups["oauth_token_secret"].Value;

            var authorizationUrl = new StringBuilder(Resources.Auth_AuthorizeBaseUrl);
            authorizationUrl.AddParameterToQuery("oauth_token", authToken.AuthorizationKey);
            authorizationUrl.AddParameterToQuery("force_login", parameters.ForceLogin);
            authorizationUrl.AddParameterToQuery("screen_name", parameters.ScreenName);

            authToken.AuthorizationURL = authorizationUrl.ToString();

            return new TwitterResult<IAuthenticationRequest>
            {
                Request = requestTokenResponse.Request,
                Response = requestTokenResponse.Response,
                DataTransferObject = authToken
            };
        }

        public async Task<ITwitterResult<ITwitterCredentials>> RequestCredentialsAsync(IRequestCredentialsParameters parameters, ITwitterRequest request)
        {
            var twitterResult = await _authQueryExecutor.RequestCredentialsAsync(parameters, request).ConfigureAwait(false);

            var oAuthToken = twitterResult.RawResult.GetURLParameter("oauth_token");
            var oAuthTokenSecret = twitterResult.RawResult.GetURLParameter("oauth_token_secret");

            if (oAuthToken == null || oAuthTokenSecret == null)
            {
                throw new TwitterAuthException(twitterResult, "Invalid authentication response");
            }

            var credentials = new TwitterCredentials(
                parameters.AuthRequest.ConsumerKey,
                parameters.AuthRequest.ConsumerSecret,
                oAuthToken,
                oAuthTokenSecret);

            return new TwitterResult<ITwitterCredentials>
            {
                Request = twitterResult.Request,
                Response = twitterResult.Response,
                DataTransferObject = credentials
            };
        }

        public Task<ITwitterResult<InvalidateTokenResponse>> InvalidateBearerTokenAsync(IInvalidateBearerTokenParameters parameters, ITwitterRequest request)
        {
            return _authQueryExecutor.InvalidateBearerTokenAsync(parameters, request);
        }

        public Task<ITwitterResult<InvalidateTokenResponse>> InvalidateAccessTokenAsync(IInvalidateAccessTokenParameters parameters, ITwitterRequest request)
        {
            return _authQueryExecutor.InvalidateAccessTokenAsync(parameters, request);
        }
    }
}