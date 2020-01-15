using System;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IHelpClientRequiredParametersValidator : IHelpClientParametersValidator
    {
    }

    public class HelpClientRequiredParametersValidator : IHelpClientRequiredParametersValidator
    {
        public void Validate(IGetRateLimitsParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }

        public void Validate(IGetTwitterConfigurationParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }

        public void Validate(IGetSupportedLanguagesParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }
    }
}