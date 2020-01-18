using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Auth;

namespace Tweetinvi.Client.Requesters
{
    public class AuthRequester : BaseRequester, IAuthRequester
    {
        private readonly IAuthController _authController;
        private readonly IAuthClientRequiredParametersValidator _validator;

        public AuthRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            IAuthController authController,
            IAuthClientRequiredParametersValidator validator)
        : base(client, clientEvents)
        {
            _authController = authController;
            _validator = validator;
        }

        public Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerToken(ICreateBearerTokenParameters parameters)
        {
            return ExecuteRequest(request =>
            {
                _validator.Validate(parameters, request);
                return _authController.CreateBearerToken(parameters, request);
            });
        }

        public Task<ITwitterResult<IAuthenticationRequest>> RequestAuthUrl(IRequestAuthUrlParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _authController.RequestAuthUrl(parameters, request));
        }

        public Task<ITwitterResult<ITwitterCredentials>> RequestAuthUrl(IRequestCredentialsParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _authController.RequestCredentials(parameters, request));
        }

        public Task<ITwitterResult> InvalidateBearerToken(IInvalidateBearerTokenParameters parameters)
        {
            return ExecuteRequest(request =>
            {
                _validator.Validate(parameters, request);
                return _authController.InvalidateBearerToken(parameters, request);
            });
        }

        public Task<ITwitterResult> InvalidateAccessToken(IInvalidateAccessTokenParameters parameters)
        {
            return ExecuteRequest(request =>
            {
                _validator.Validate(parameters, request);
                return _authController.InvalidateAccessToken(parameters, request);
            });
        }
    }
}