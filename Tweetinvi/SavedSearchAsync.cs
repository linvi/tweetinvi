using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public static class SavedSearchAsync
    {
        public static async Task<ISavedSearch> CreateSavedSearch(string query)
        {
            return await Sync.ExecuteTaskAsync(() => SavedSearch.CreateSavedSearch(query));
        }

        public static async Task<ISavedSearch> GetSavedSearch(long searchId)
        {
            return await Sync.ExecuteTaskAsync(() => SavedSearch.GetSavedSearch(searchId));
        }

        public static async Task<List<ISavedSearch>> GetSavedSearches()
        {
            return await Sync.ExecuteTaskAsync(() => SavedSearch.GetSavedSearches());
        }

        public static async Task<bool> DestroySavedSearch(ISavedSearch savedSearch)
        {
            return await Sync.ExecuteTaskAsync(() => SavedSearch.DestroySavedSearch(savedSearch));
        }

        public static async Task<bool> DestroySavedSearch(long searchId)
        {
            return await Sync.ExecuteTaskAsync(() => SavedSearch.DestroySavedSearch(searchId));
        }
    }
}
