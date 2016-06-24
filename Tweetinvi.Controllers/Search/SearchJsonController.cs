using System;
using System.Collections.Generic;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Web;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchJsonController
    {
        string SearchTweets(string searchQuery);
        IEnumerable<string> SearchTweets(ISearchTweetsParameters searchTweetsParameters);
    }

    public class SearchJsonController : ISearchJsonController
    {
        private readonly ISearchQueryGenerator _searchQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly ISearchQueryHelper _searchQueryHelper;
        private readonly ITweetHelper _tweetHelper;

        public SearchJsonController(
            ISearchQueryGenerator searchQueryGenerator,
            ITwitterAccessor twitterAccessor,
            ISearchQueryHelper searchQueryHelper,
            ITweetHelper tweetHelper)
        {
            _searchQueryGenerator = searchQueryGenerator;
            _twitterAccessor = twitterAccessor;
            _searchQueryHelper = searchQueryHelper;
            _tweetHelper = tweetHelper;
        }

        public string SearchTweets(string searchQuery)
        {
            string query = _searchQueryGenerator.GetSearchTweetsQuery(searchQuery);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        public IEnumerable<string> SearchTweets(ISearchTweetsParameters searchTweetsParameters)
        {
            if (searchTweetsParameters.MaximumNumberOfResults > 100)
            {
                return SearchTweetsRecursively(searchTweetsParameters);
            }
            
            string query = _searchQueryGenerator.GetSearchTweetsQuery(searchTweetsParameters);
            return new[] { GetJsonResultFromQuery(query) };
        }

        private string GetJsonResultFromQuery(string query)
        {
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        private IEnumerable<string> SearchTweetsRecursively(ISearchTweetsParameters searchTweetsParameters)
        {
            var searchParameter = _searchQueryHelper.CloneTweetSearchParameters(searchTweetsParameters);
            searchParameter.MaximumNumberOfResults = Math.Min(searchParameter.MaximumNumberOfResults, 100);

            string query = _searchQueryGenerator.GetSearchTweetsQuery(searchParameter);
            var json = GetJsonResultFromQuery(query);
            var jsonResult = new List<string> { json };
            var currentResult = _searchQueryHelper.GetTweetsFromJsonResponse(json);
            var tweetDTOResult = currentResult;

            if (tweetDTOResult == null)
            {
                return jsonResult;
            }

            while (tweetDTOResult.Count < searchTweetsParameters.MaximumNumberOfResults)
            {
                if (currentResult.IsNullOrEmpty())
                {
                    // If Twitter does not any result left, stop the search
                    break;
                }

                var oldestTweetId = _tweetHelper.GetOldestTweetId(currentResult);
                searchParameter.MaxId = oldestTweetId - 1;

                searchParameter.MaximumNumberOfResults = Math.Min(searchTweetsParameters.MaximumNumberOfResults - tweetDTOResult.Count, 100);
                query = _searchQueryGenerator.GetSearchTweetsQuery(searchParameter);
                
                json = GetJsonResultFromQuery(query);

                if (json != null)
                {
                    jsonResult.Add(json);
                }

                currentResult = _searchQueryHelper.GetTweetsFromJsonResponse(json);
                tweetDTOResult.AddRangeSafely(currentResult);
            }

            return jsonResult;
        }
    }
}