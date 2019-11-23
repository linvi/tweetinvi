using Tweetinvi.Models;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IAuthClientParametersValidator
    {
        void ValidateCreateBearerToken(ITwitterRequest request);
    }

    public interface IInternalAuthClientParametersValidator : IAuthClientParametersValidator
    {
        void Initialize(ITwitterClient client);
    }

    public class AuthClientParametersValidator : IInternalAuthClientParametersValidator
    {
        private readonly IAuthClientRequiredParametersValidator _authClientRequiredParametersValidator;

        public AuthClientParametersValidator(IAuthClientRequiredParametersValidator authClientRequiredParametersValidator)
        {
            _authClientRequiredParametersValidator = authClientRequiredParametersValidator;
        }

        public void Initialize(ITwitterClient client)
        {
            // currently no need to use the client
        }

        public void ValidateCreateBearerToken(ITwitterRequest request)
        {
            _authClientRequiredParametersValidator.ValidateCreateBearerToken(request);
        }
    }
}