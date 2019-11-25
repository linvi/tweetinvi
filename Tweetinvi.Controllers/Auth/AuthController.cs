using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Extensions;
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
        private readonly ICredentialsStore _credentialsStore;
        private readonly Regex _parseStartAuthProcessResponseRegex;

        public AuthController(
            IAuthQueryExecutor authQueryExecutor,
            ICredentialsStore credentialsStore)
        {
            _authQueryExecutor = authQueryExecutor;
            _credentialsStore = credentialsStore;
            _parseStartAuthProcessResponseRegex = new Regex(Resources.Auth_RequestTokenParserRegex);
        }

        public Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerToken(ITwitterRequest request)
        {
            return _authQueryExecutor.CreateBearerToken(request);
        }

        public async Task<ITwitterResult<IAuthenticationContext>> StartAuthProcess(IStartAuthProcessParameters parameters, ITwitterRequest request)
        {
            var authContext = new AuthenticationContext(request.Query.TwitterCredentials);
            var token = authContext.Token;
            var authProcessParams = new StartAuthProcessInternalParameters(parameters, token);

            if (string.IsNullOrEmpty(parameters.CallbackUrl))
            {
                authProcessParams.CallbackUrl = Resources.Auth_PinCodeUrl;
            }
            else if (parameters.AuthorizationId != null)
            {
                _credentialsStore.CallbackAuthenticationContextStore.Add(parameters.AuthorizationId, authContext);
                authProcessParams.CallbackUrl = authProcessParams.CallbackUrl.AddParameterToQuery(Resources.Auth_ProcessIdKey, parameters.AuthorizationId);
            }

            var requestTokenResponse = await _authQueryExecutor.StartAuthProcess(authProcessParams, request).ConfigureAwait(false);

            if (string.IsNullOrEmpty(requestTokenResponse.Json) || requestTokenResponse.Json == Resources.Auth_RequestToken)
            {
                throw new TwitterAuthException("Invalid authentication response");
            }

            var tokenInformation = _parseStartAuthProcessResponseRegex.Match(requestTokenResponse.Json);

            if (!bool.TryParse(tokenInformation.Groups["oauth_callback_confirmed"].Value, out var callbackConfirmed) || !callbackConfirmed)
            {
                throw new TwitterAuthAbortedException();
            }

            token.AuthorizationKey = tokenInformation.Groups["oauth_token"].Value;
            token.AuthorizationSecret = tokenInformation.Groups["oauth_token_secret"].Value;

            authContext.AuthorizationURL = $"{Resources.Auth_AuthorizeBaseUrl}?oauth_token={token.AuthorizationKey}";
            token.AuthorizationId = parameters.AuthorizationId;

            return new TwitterResult<IAuthenticationContext>
            {
                Request = requestTokenResponse.Request,
                Response = requestTokenResponse.Response,
                DataTransferObject = authContext
            };
        }
    }
}