using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;

namespace Tweetinvi
{
    /// <summary>
    /// Create, delete or use saved searches from Twitter.
    /// </summary>
    public static class SavedSearch
    {
        [ThreadStatic]
        private static ISavedSearchFactory _savedSearchFactory;

        /// <summary>
        /// Factory creating Saved Searches
        /// </summary>
        public static ISavedSearchFactory SavedSearchFactory
        {
            get
            {
                if (_savedSearchFactory == null)
                {
                    Initialize();
                }

                return _savedSearchFactory;
            }
        }

        [ThreadStatic]
        private static ISavedSearchController _savedSearchController;

        /// <summary>
        /// Controller handling any SavedSearch request
        /// </summary>
        public static ISavedSearchController SavedSearchController
        {
            get
            {
                if (_savedSearchController == null)
                {
                    Initialize();
                }

                return _savedSearchController;
            }
        }

        static SavedSearch()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _savedSearchFactory = TweetinviContainer.Resolve<ISavedSearchFactory>();
            _savedSearchController = TweetinviContainer.Resolve<ISavedSearchController>();
        }

        // Factory

        /// <summary>
        /// Create a saved search to be published on Twitter
        /// </summary>
        public static ISavedSearch CreateSavedSearch(string query)
        {
            return SavedSearchFactory.CreateSavedSearch(query);
        }

        /// <summary>
        /// Get a saved search from its identifier
        /// </summary>
        public static ISavedSearch GetSavedSearch(long searchId)
        {
            return SavedSearchFactory.GetSavedSearch(searchId);
        }

        // Controller

        /// <summary>
        /// Get the saved searches of the authenticated user
        /// </summary>
        public static List<ISavedSearch> GetSavedSearches()
        {
            var savedSearches = SavedSearchController.GetSavedSearches();

            if (savedSearches == null)
            {
                return null;
            }

            return savedSearches.ToList();
        }

        /// <summary>
        /// Delete a saved search own by the authenticated user
        /// </summary>
        public static bool DestroySavedSearch(ISavedSearch savedSearch)
        {
            return SavedSearchController.DestroySavedSearch(savedSearch);
        }

        /// <summary>
        /// Delete a saved search own by the authenticated user
        /// </summary>
        public static bool DestroySavedSearch(long searchId)
        {
            return SavedSearchController.DestroySavedSearch(searchId);
        }
    }
}