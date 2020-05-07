using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

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

        public Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerTokenAsync(ICreateBearerTokenParameters parameters)
        {
            return ExecuteRequestAsync(request =>
            {
                _validator.Validate(parameters, request);
                return _authController.CreateBearerTokenAsync(parameters, request);
            });
        }

        public Task<ITwitterResult<IAuthenticationRequest>> RequestAuthUrlAsync(IRequestAuthUrlParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _authController.RequestAuthUrlAsync(parameters, request));
        }

        public Task<ITwitterResult<ITwitterCredentials>> RequestCredentialsAsync(IRequestCredentialsParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _authController.RequestCredentialsAsync(parameters, request));
        }

        public Task<ITwitterResult<InvalidateTokenResponse>> InvalidateBearerTokenAsync(IInvalidateBearerTokenParameters parameters)
        {
            return ExecuteRequestAsync(request =>
            {
                _validator.Validate(parameters, request);
                return _authController.InvalidateBearerTokenAsync(parameters, request);
            });
        }

        public Task<ITwitterResult<InvalidateTokenResponse>> InvalidateAccessTokenAsync(IInvalidateAccessTokenParameters parameters)
        {
            return ExecuteRequestAsync(request =>
            {
                _validator.Validate(parameters, request);
                return _authController.InvalidateAccessTokenAsync(parameters, request);
            });
        }
    }
}