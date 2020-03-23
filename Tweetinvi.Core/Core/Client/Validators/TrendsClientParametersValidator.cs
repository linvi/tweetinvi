using Tweetinvi.Exceptions;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.TrendsClient;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITrendsClientParametersValidator
    {
        void Validate(IGetTrendsLocationCloseToParameters parameters);
        void Validate(IGetTrendsAtParameters parameters);
        void Validate(IGetTrendsLocationParameters parameters);
    }

    public class TrendsClientParametersValidator : ITrendsClientParametersValidator
    {
        private readonly ITrendsClientRequiredParametersValidator _requiredParametersValidator;

        public TrendsClientParametersValidator(ITrendsClientRequiredParametersValidator requiredParametersValidator)
        {
            _requiredParametersValidator = requiredParametersValidator;
        }

        public void Validate(IGetTrendsLocationCloseToParameters parameters)
        {
            _requiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetTrendsAtParameters parameters)
        {
            _requiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetTrendsLocationParameters parameters)
        {
            _requiredParametersValidator.Validate(parameters);
        }
    }
}