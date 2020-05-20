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

        public void Validate(ISearchUsersParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Query == null)
            {
                throw new ArgumentNullException($"{nameof(parameters.Query)}");
            }
        }

        public void Validate(ICreateSavedSearchParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (string.IsNullOrEmpty(parameters.Query))
            {
                throw new ArgumentException($"{nameof(parameters.Query)}");
            }
        }

        public void Validate(IGetSavedSearchParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.SavedSearchId <= 0)
            {
                throw new ArgumentNullException($"{nameof(parameters.SavedSearchId)}");
            }
        }

        public void Validate(IListSavedSearchesParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }

        public void Validate(IDestroySavedSearchParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.SavedSearchId <= 0)
            {
                throw new ArgumentNullException($"{nameof(parameters.SavedSearchId)}");
            }
        }
    }
}