using System;
using System.Collections.Generic;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi
{
    public static class Search
    {
        [ThreadStatic]
        private static ISearchController _searchController;

        /// <summary>
        /// Controller handling any Search request
        /// </summary>
        public static ISearchController SearchController
        {
            get
            {
                if (_searchController == null)
                {
                    Initialize();
                }
                
                return _searchController;
            }
        }

        public static ISearchQueryParameterGenerator SearchQueryParameterGenerator { get; set; }

        static Search()
        {
            Initialize();

            SearchQueryParameterGenerator = TweetinviContainer.Resolve<ISearchQueryParameterGenerator>();
        }

        private static void Initialize()
        {
            _searchController = TweetinviContainer.Resolve<ISearchController>();
        }

        // TWEET

        /// <summary>
        /// Search tweets based on the provided search query
        /// </summary>
        public static IEnumerable<ITweet> SearchTweets(string searchQuery)
        {
            return SearchController.SearchTweets(searchQuery);
        }

        /// <summary>
        /// Search tweets based on multiple parameters
        /// </summary>
        public static IEnumerable<ITweet> SearchTweets(ITweetSearchParameters tweetSearchParameters)
        {
            return SearchController.SearchTweets(tweetSearchParameters);
        }

        /// <summary>
        /// Search tweets with some additional metadata information
        /// </summary>
        public static ISearchResult SearchTweetsWithMetadata(string searchQuery)
        {
            return SearchController.SearchTweetsWithMetadata(searchQuery);
        }

        /// <summary>
        /// Search tweets with some additional metadata information
        /// </summary>
        public static ISearchResult SearchTweetsWithMetadata(ITweetSearchParameters tweetSearchParameters)
        {
            return SearchController.SearchTweetsWithMetadata(tweetSearchParameters);
        }

        /// <summary>
        /// Search direct replies to a specific tweet
        /// </summary>
        public static IEnumerable<ITweet> SearchDirectRepliesTo(ITweet tweet)
        {
            return SearchController.SearchDirectRepliesTo(tweet);
        }

        /// <summary>
        /// Search replies to a tweet and recursively to the replies' replies
        /// </summary>
        public static IEnumerable<ITweet> SearchRepliesTo(ITweet tweet, bool recursiveReplies)
        {
            return SearchController.SearchRepliesTo(tweet, recursiveReplies);
        }

        /// <summary>
        /// Create a parameter to search tweets for a specific query
        /// </summary>
        public static ITweetSearchParameters CreateTweetSearchParameter(string query)
        {
            return SearchQueryParameterGenerator.CreateSearchTweetParameter(query);
        }

        /// <summary>
        /// Create a parameter to search tweets for a specific GeoCode
        /// </summary>
        public static ITweetSearchParameters CreateTweetSearchParameter(IGeoCode geoCode)
        {
            return SearchQueryParameterGenerator.CreateSearchTweetParameter(geoCode);
        }

        /// <summary>
        /// Create a parameter to search tweets for some specific coordinates and radius
        /// </summary>
        public static ITweetSearchParameters CreateTweetSearchParameter(ICoordinates coordinates, int radius, DistanceMeasure measure)
        {
            return SearchQueryParameterGenerator.CreateSearchTweetParameter(coordinates, radius, measure);
        }

        /// <summary>
        /// Create a parameter to search tweets for some specific coordinates and radius
        /// </summary>
        public static ITweetSearchParameters CreateTweetSearchParameter(double longitude, double latitude, int radius, DistanceMeasure measure)
        {
            return SearchQueryParameterGenerator.CreateSearchTweetParameter(longitude, latitude, radius, measure);   
        }

        // USER

        /// <summary>
        /// Create a parameter to search users from a query
        /// </summary>
        public static IUserSearchParameters CreateUserSearchParameter(string query)
        {
            return SearchQueryParameterGenerator.CreateUserSearchParameters(query);
        }

        /// <summary>
        /// Create a parameter to search users from a query at a specific page of the result indexation
        /// </summary>
        public static IEnumerable<IUser> SearchUsers(string query, int maximumNumberOfResults = 20, int page = 0)
        {
            var searchParameters = CreateUserSearchParameter(query);
            searchParameters.Page = page;
            searchParameters.MaximumNumberOfResults = maximumNumberOfResults;

            return SearchController.SearchUsers(searchParameters);
        }
    }
}