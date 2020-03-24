using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IHelpClientParametersValidator
    {
        void Validate(IGetRateLimitsParameters parameters);
        void Validate(IGetTwitterConfigurationParameters parameters);
        void Validate(IGetSupportedLanguagesParameters parameters);

        void Validate(IGetPlaceParameters parameters);
        void Validate(IGeoSearchParameters parameters);
        void Validate(IGeoSearchReverseParameters parameters);
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

        public void Validate(IGetPlaceParameters parameters)
        {
            _helpClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGeoSearchParameters parameters)
        {
            _helpClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGeoSearchReverseParameters parameters)
        {
            _helpClientRequiredParametersValidator.Validate(parameters);
        }
    }
}