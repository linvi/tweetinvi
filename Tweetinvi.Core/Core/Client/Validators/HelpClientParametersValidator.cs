using Tweetinvi.Models;
using Tweetinvi.Parameters.HelpClient;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IHelpClientParametersValidator
    {
        void Validate(IGetRateLimitsParameters parameters);
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
    }
}