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

        public Task<ITwitterResult<CreateTokenResponseDTO>> CreateBearerToken()
        {
            var request = CreateRequest();
            _validator.ValidateCreateBearerToken(request);

            return ExecuteRequest(() => _authController.CreateBearerToken(request), request);
        }

        public Task<ITwitterResult<IAuthenticationContext>> RequestAuthUrl(IRequestAuthUrlParameters parameters)
        {
            _validator.Validate(parameters);

            var request = CreateRequest();
            return ExecuteRequest(() => _authController.RequestAuthUrl(parameters, request), request);
        }

        public Task<ITwitterResult<ITwitterCredentials>> RequestCredentialsFromPinCode(IRequestCredentialsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            return ExecuteRequest(() => _authController.RequestCredentials(parameters, request), request);
        }

        public Task<ITwitterResult<ITwitterCredentials>> RequestCredentialsFromCallbackUrl(IRequestCredentialsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            return ExecuteRequest(() => _authController.RequestCredentials(parameters, request), request);
        }
    }
}