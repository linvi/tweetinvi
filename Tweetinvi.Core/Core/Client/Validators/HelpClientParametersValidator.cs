using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IHelpClientParametersValidator
    {
        void Validate(IGetRateLimitsParameters parameters);
        void Validate(IGetTwitterConfigurationParameters parameters);
        void Validate(IGetSupportedLanguagesParameters parameters);
    }

    public class HelpClientParametersValidator : IHelpClientParametersValidator
    {
        private readonly IHelpClientRequiredParametersValidator _helpClientRequiredParametersValidator;

        public HelpClientParametersValidator(IHelpClientRequiredParametersValidator helpClientRequiredParametersValidator)
        {
            _helpClientRequiredParametersValidator = helpClientRequiredParametersValidator;
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