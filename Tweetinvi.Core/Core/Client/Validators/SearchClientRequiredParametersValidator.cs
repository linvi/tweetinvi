using System;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ISearchClientRequiredParametersValidator : ISearchClientParametersValidator
    {
    }

    public class SearchClientRequiredParametersValidator : ISearchClientRequiredParametersValidator
    {
        public void Validate(ISearchTweetsParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }
    }
}