using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Auth;

namespace Tweetinvi.Client.Requesters
{
    public interface IInternalAuthRequester : IAuthRequester, IBaseRequester
    {
    }

    public class AuthRequester : BaseRequester, IInternalAuthRequester
    {
        private readonly IAuthController _authController;
        private readonly IAuthClientRequiredParametersValidator _validator;

        public AuthRequester(
            IAuthController authController,
            IAuthClientRequiredParametersValidator validator)
        {
            _authController = authController;
            _validator = validator;
        }

        public Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerToken(ICreateBearerTokenParameters parameters)
        {
            var request = CreateRequest();
            _validator.Validate(parameters, request);

            return ExecuteRequest(() => _authController.CreateBearerToken(parameters, request), request);
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
            var request = CreateRequest();
            _validator.Validate(parameters, request);

            return ExecuteRequest(() => _authController.InvalidateBearerToken(parameters, request), request);
        }

        public Task<ITwitterResult> InvalidateAccessToken(IInvalidateAccessTokenParameters parameters)
        {
            var request = CreateRequest();
            _validator.Validate(parameters, request);

            return ExecuteRequest(() => _authController.InvalidateAccessToken(parameters, request), request);
        }
    }
}