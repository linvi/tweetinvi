using System;
using Tweetinvi.Exceptions;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.TrendsClient;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITrendsClientRequiredParametersValidator : ITrendsClientParametersValidator
    {
    }

    public class TrendsClientRequiredParametersValidator : ITrendsClientRequiredParametersValidator
    {
        public void Validate(IGetTrendsLocationCloseToParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }

        public void Validate(IGetTrendsAtParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }

        public void Validate(IGetTrendsLocationParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }
    }
}