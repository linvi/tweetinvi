using System;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.SavedSearch
{
    public interface ISavedSearchQueryGenerator
    {
        string GetSavedSearchesQuery();

        string GetDestroySavedSearchQuery(ISavedSearch savedSearch);
        string GetDestroySavedSearchQuery(long searchId);
    }

    public class SavedSearchQueryGenerator : ISavedSearchQueryGenerator
    {
        public string GetSavedSearchesQuery()
        {
            return Resources.SavedSearches_List;
        }

        public string GetDestroySavedSearchQuery(ISavedSearch savedSearch)
        {
            if (savedSearch == null)
            {
                throw new ArgumentNullException(nameof(savedSearch));
            }

            return GetDestroySavedSearchQuery(savedSearch.Id);
        }

        public string GetDestroySavedSearchQuery(long searchId)
        {
            if (searchId <= 0)
            {
                throw new ArgumentException("Search Id must be set.");
            }

            return string.Format(Resources.SavedSearch_Destroy, searchId);
        }
    }
}