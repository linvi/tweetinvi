using System;
using Tweetinvi.Controllers.SavedSearch;
using Tweetinvi.Factories.SavedSearch;
using Tweetinvi.Models;

namespace Tweetinvi.Json
{
    public static class SavedSearchJson
    {
        [ThreadStatic]
        private static ISavedSearchJsonFactory _savedSearchJsonFactory;
        public static ISavedSearchJsonFactory SavedSearchJsonFactory
        {
            get
            {
                if (_savedSearchJsonFactory == null)
                {
                    Initialize();
                }

                return _savedSearchJsonFactory;
            }
        }

        [ThreadStatic]
        private static ISavedSearchJsonController _savedSearchJsonController;
        public static ISavedSearchJsonController SavedSearchJsonController
        {
            get
            {
                if (_savedSearchJsonController == null)
                {
                    Initialize();
                }

                return _savedSearchJsonController;
            }
        }

        static SavedSearchJson()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _savedSearchJsonFactory = TweetinviContainer.Resolve<ISavedSearchJsonFactory>();
            _savedSearchJsonController = TweetinviContainer.Resolve<ISavedSearchJsonController>();
        }

        // Factory

        /// <summary>
        /// Create a saved search in Twitter for the specified query
        /// </summary>
        public static string CreateSavedSearch(string query)
        {
            return SavedSearchJsonFactory.CreateSavedSearch(query);
        }

        /// <summary>
        /// Get an existing search query saved in Twitter
        /// </summary>
        public static string GetSavedSearch(long searchId)
        {
            return SavedSearchJsonFactory.GetSavedSearch(searchId);
        }

        // Controller

        /// <summary>
        /// Get the search queries saved in the Twitter account
        /// </summary>
        public static string GetSavedSearches()
        {
            return SavedSearchJsonController.GetSavedSearches();
        }

        /// <summary>
        /// Destroy a search saved in the Twitter account
        /// </summary>
        public static string DestroySavedSearch(ISavedSearch savedSearch)
        {
            return SavedSearchJsonController.DestroySavedSearch(savedSearch);
        }

        /// <summary>
        /// Destroy a search saved in the Twitter account
        /// </summary>
        public static string DestroySavedSearch(long searchId)
        {
            return SavedSearchJsonController.DestroySavedSearch(searchId);
        }
    }
}