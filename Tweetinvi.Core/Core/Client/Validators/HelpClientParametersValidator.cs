using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IHelpClientParametersValidator
    {
        void Validate(IGetRateLimitsParameters parameters);
        void Validate(IGetTwitterConfigurationParameters parameters);
        void Validate(IGetSupportedLanguagesParameters parameters);
    }

    public interface IInternalHelpClientParametersValidator : IHelpClientParametersValidator
    {
        void Initialize(ITwitterClient client);
    }

    public class HelpClientParametersValidator : IInternalHelpClientParametersValidator
    {
        private readonly IHelpClientRequiredParametersValidator _helpClientRequiredParametersValidator;

        public HelpClientParametersValidator(IHelpClientRequiredParametersValidator helpClientRequiredParametersValidator)
        {
            _helpClientRequiredParametersValidator = helpClientRequiredParametersValidator;
        }

        public void Initialize(ITwitterClient client)
        {
        }

        public void Validate(IGetRateLimitsParameters parameters)
        {
            _helpClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetTwitterConfigurationParameters parameters)
        {
            _helpClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetSupportedLanguagesParameters parameters)
        {
            _helpClientRequiredParametersValidator.Validate(parameters);
        }
    }
}