using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public static class SavedSearchAsync
    {
        public static Task<ISavedSearch> CreateSavedSearch(string query)
        {
            return Sync.ExecuteTaskAsync(() => SavedSearch.CreateSavedSearch(query));
        }

        public static Task<ISavedSearch> GetSavedSearch(long searchId)
        {
            return Sync.ExecuteTaskAsync(() => SavedSearch.GetSavedSearch(searchId));
        }

        public static Task<List<ISavedSearch>> GetSavedSearches()
        {
            return Sync.ExecuteTaskAsync(() => SavedSearch.GetSavedSearches());
        }

        public static Task<bool> DestroySavedSearch(ISavedSearch savedSearch)
        {
            return Sync.ExecuteTaskAsync(() => SavedSearch.DestroySavedSearch(savedSearch));
        }

        public static Task<bool> DestroySavedSearch(long searchId)
        {
            return Sync.ExecuteTaskAsync(() => SavedSearch.DestroySavedSearch(searchId));
        }
    }
}
