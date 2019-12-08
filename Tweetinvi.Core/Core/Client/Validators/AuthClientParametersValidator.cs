using System;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Auth;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IAuthClientParametersValidator
    {
        void ValidateCreateBearerToken(ITwitterRequest request);
        void Validate(IRequestAuthUrlParameters parameters);

        void Validate(IRequestCredentialsParameters parameters);
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

        public void Validate(IRequestAuthUrlParameters parameters)
        {
            _authClientRequiredParametersValidator.Validate(parameters);

            if (parameters.CallbackUrl != null)
            {
                if (parameters.CallbackUrl != "oob" && !Uri.IsWellFormedUriString(parameters.CallbackUrl, UriKind.Absolute))
                {
                    throw new ArgumentException("Invalid format, must be absolute uri or have a value of 'oob'", $"{nameof(parameters)}{nameof(parameters.CallbackUrl)}");
                }
            }
        }

        public void Validate(IRequestCredentialsParameters parameters)
        {
            _authClientRequiredParametersValidator.Validate(parameters);
        }
    }
}