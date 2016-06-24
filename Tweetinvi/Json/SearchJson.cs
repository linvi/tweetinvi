using System;
using System.Collections.Generic;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Parameters;

namespace Tweetinvi.Json
{
    public static class SearchJson
    {
        [ThreadStatic]
        private static ISearchJsonController _searchJsonController;
        public static ISearchJsonController SearchJsonController
        {
            get
            {
                if (_searchJsonController == null)
                {
                    Initialize();
                }

                return _searchJsonController;
            }
        }

        static SearchJson()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _searchJsonController = TweetinviContainer.Resolve<ISearchJsonController>();
        }

        /// <summary>
        /// Search tweets based on the provided search query
        /// </summary>
        public static string SearchTweets(string searchQuery)
        {
            return SearchJsonController.SearchTweets(searchQuery);
        }

        /// <summary>
        /// Search tweets based on multiple parameters.
        /// </summary>
        /// <returns>This can returns a collection of json responses when the MaximumNumberOfResults parameter is bigger than 100</returns>
        public static IEnumerable<string> SearchTweets(ISearchTweetsParameters searchTweetsParameters)
        {
            return SearchJsonController.SearchTweets(searchTweetsParameters);
        }
    }
}