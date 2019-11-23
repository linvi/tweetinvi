using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Web;

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

            return _authController.CreateBearerToken(request);
        }
    }
}