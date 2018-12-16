using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public static class SavedSearchAsync
    {
        public static ConfiguredTaskAwaitable<ISavedSearch> CreateSavedSearch(string query)
        {
            return Sync.ExecuteTaskAsync(() => SavedSearch.CreateSavedSearch(query));
        }

        public static ConfiguredTaskAwaitable<ISavedSearch> GetSavedSearch(long searchId)
        {
            return Sync.ExecuteTaskAsync(() => SavedSearch.GetSavedSearch(searchId));
        }

        public static ConfiguredTaskAwaitable<List<ISavedSearch>> GetSavedSearches()
        {
            return Sync.ExecuteTaskAsync(() => SavedSearch.GetSavedSearches());
        }

        public static ConfiguredTaskAwaitable<bool> DestroySavedSearch(ISavedSearch savedSearch)
        {
            return Sync.ExecuteTaskAsync(() => SavedSearch.DestroySavedSearch(savedSearch));
        }

        public static ConfiguredTaskAwaitable<bool> DestroySavedSearch(long searchId)
        {
            return Sync.ExecuteTaskAsync(() => SavedSearch.DestroySavedSearch(searchId));
        }
    }
}
