using System;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.Models;

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
            return Resources.SavedSearches_GetList;
        }

        public string GetDestroySavedSearchQuery(ISavedSearch savedSearch)
        {
            if (savedSearch == null)
            {
                return null;
            }

            return GetDestroySavedSearchQuery(savedSearch.Id);
        }

        public string GetDestroySavedSearchQuery(long searchId)
        {
            if (searchId == TweetinviSettings.DEFAULT_ID)
            {
                return null;
            }

            return string.Format(Resources.SavedSearch_Destroy, searchId);
        }
    }
}