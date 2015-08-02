using System;
using System.Collections.Generic;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi
{
    public static class Search
    {
        [ThreadStatic]
        private static ISearchController _searchController;
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

        [ThreadStatic]
        private static ISearchQueryParameterGenerator _searchQueryParameterGenerator;
        public static ISearchQueryParameterGenerator SearchQueryParameterGenerator
        {
            get
            {
                if (_searchQueryParameterGenerator == null)
                {
                    Initialize();
                }

                return _searchQueryParameterGenerator;
            }
        }

        static Search()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _searchController = TweetinviContainer.Resolve<ISearchController>();
            _searchQueryParameterGenerator = TweetinviContainer.Resolve<ISearchQueryParameterGenerator>();
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

        public static ISearchResult SearchTweetsWithMetadata(string searchQuery)
        {
            return SearchController.SearchTweetsWithMetadata(searchQuery);
        }

        public static ISearchResult SearchTweetsWithMetadata(ITweetSearchParameters tweetSearchParameters)
        {
            return SearchController.SearchTweetsWithMetadata(tweetSearchParameters);
        }

        public static IEnumerable<ITweet> SearchDirectRepliesTo(ITweet tweet)
        {
            return SearchController.SearchDirectRepliesTo(tweet);
        }

        public static IEnumerable<ITweet> SearchRepliesTo(ITweet tweet, bool recursiveReplies)
        {
            return SearchController.SearchRepliesTo(tweet, recursiveReplies);
        }

        public static ITweetSearchParameters CreateTweetSearchParameter(string query)
        {
            return SearchQueryParameterGenerator.CreateSearchTweetParameter(query);
        }

        public static ITweetSearchParameters CreateTweetSearchParameter(IGeoCode geoCode)
        {
            return SearchQueryParameterGenerator.CreateSearchTweetParameter(geoCode);
        }

        public static ITweetSearchParameters CreateTweetSearchParameter(ICoordinates coordinates, int radius, DistanceMeasure measure)
        {
            return SearchQueryParameterGenerator.CreateSearchTweetParameter(coordinates, radius, measure);
        }

        public static ITweetSearchParameters CreateTweetSearchParameter(double longitude, double latitude, int radius, DistanceMeasure measure)
        {
            return SearchQueryParameterGenerator.CreateSearchTweetParameter(longitude, latitude, radius, measure);   
        }

        // USER

        public static IUserSearchParameters CreateUserSearchParameter(string query)
        {
            return _searchQueryParameterGenerator.CreateUserSearchParameters(query);
        }

        public static IEnumerable<IUser> SearchUsers(string query)
        {
            return SearchUsers(query, 0);
        }

        public static IEnumerable<IUser> SearchUsers(string query, int maximumNumberOfResults = 20, int page = 0)
        {
            var searchParameters = CreateUserSearchParameter(query);
            searchParameters.Page = page;
            searchParameters.MaximumNumberOfResults = maximumNumberOfResults;

            return _searchController.SearchUsers(searchParameters);
        }
    }
}