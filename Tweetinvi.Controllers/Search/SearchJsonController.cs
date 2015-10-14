using System;
using System.Collections.Generic;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchJsonController
    {
        string SearchTweets(string searchQuery);
        IEnumerable<string> SearchTweets(ITweetSearchParameters tweetSearchParameters);
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
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        public IEnumerable<string> SearchTweets(ITweetSearchParameters tweetSearchParameters)
        {
            if (tweetSearchParameters.MaximumNumberOfResults > 100)
            {
                return SearchTweetsRecursively(tweetSearchParameters);
            }
            
            string query = _searchQueryGenerator.GetSearchTweetsQuery(tweetSearchParameters);
            return new[] { GetJsonResultFromQuery(query) };
        }

        private string GetJsonResultFromQuery(string query)
        {
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        private IEnumerable<string> SearchTweetsRecursively(ITweetSearchParameters tweetSearchParameters)
        {
            var searchParameter = _searchQueryHelper.CloneTweetSearchParameters(tweetSearchParameters);
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

            while (tweetDTOResult.Count < tweetSearchParameters.MaximumNumberOfResults)
            {
                if (currentResult.IsNullOrEmpty())
                {
                    // If Twitter does not any result left, stop the search
                    break;
                }

                var oldestTweetId = _tweetHelper.GetOldestTweetId(currentResult);
                searchParameter.MaxId = oldestTweetId - 1;

                searchParameter.MaximumNumberOfResults = Math.Min(tweetSearchParameters.MaximumNumberOfResults - tweetDTOResult.Count, 100);
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