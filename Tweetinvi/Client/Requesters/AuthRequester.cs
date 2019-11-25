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
            var request = TwitterClient.CreateRequest();
            _validator.ValidateCreateBearerToken(request);

            return ExecuteRequest(() => _authController.CreateBearerToken(request), request);
        }

        public Task<ITwitterResult<IAuthenticationContext>> StartAuthProcess(IStartAuthProcessParameters parameters)
        {
            var request = TwitterClient.CreateRequest();
            return ExecuteRequest(() => _authController.StartAuthProcess(parameters, request), request);
        }

    }
}