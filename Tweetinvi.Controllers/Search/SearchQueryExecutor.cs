using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryExecutor
    {
        IEnumerable<ITweetDTO> SearchTweets(string query);
        IEnumerable<ITweetDTO> SearchTweets(ISearchTweetsParameters searchTweetsParameters);
        IEnumerable<ITweetDTO> SearchRepliesTo(ITweetDTO tweetDTO, bool getRecursiveReplies);
        ISearchResultsDTO SearchTweetsWithMetadata(string searchQuery);
        IEnumerable<ISearchResultsDTO> SearchTweetsWithMetadata(ISearchTweetsParameters searchTweetsParameters);

        IEnumerable<IUserDTO> SearchUsers(string searchQuery);
        IEnumerable<IUserDTO> SearchUsers(ISearchUsersParameters searchUsersParameters);
    }

    public class SearchQueryExecutor : ISearchQueryExecutor
    {
        private readonly ISearchQueryGenerator _searchQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly ISearchQueryHelper _searchQueryHelper;
        private readonly ITweetHelper _tweetHelper;
        private readonly ISearchQueryParameterGenerator _searchQueryParameterGenerator;

        public SearchQueryExecutor(
            ISearchQueryGenerator searchQueryGenerator,
            ITwitterAccessor twitterAccessor,
            ISearchQueryHelper searchQueryHelper,
            ITweetHelper tweetHelper,
            ISearchQueryParameterGenerator searchQueryParameterGenerator)
        {
            _searchQueryGenerator = searchQueryGenerator;
            _twitterAccessor = twitterAccessor;
            _searchQueryHelper = searchQueryHelper;
            _tweetHelper = tweetHelper;
            _searchQueryParameterGenerator = searchQueryParameterGenerator;
        }

        public IEnumerable<ITweetDTO> SearchTweets(string searchQuery)
        {
            var searchTweetsParameters = _searchQueryParameterGenerator.CreateSearchTweetParameter(searchQuery);
            return SearchTweets(searchTweetsParameters);
        }

        public IEnumerable<ITweetDTO> SearchTweets(ISearchTweetsParameters searchTweetsParameters)
        {
            var searchResults = SearchTweetsWithMetadata(searchTweetsParameters);
            if (searchResults == null)
            {
                return null;
            }

            return searchResults.SelectMany(x => x.MatchingTweetDTOs);
        }

        public ISearchResultsDTO SearchTweetsWithMetadata(string searchQuery)
        {
            var searchTweetsParameters = _searchQueryParameterGenerator.CreateSearchTweetParameter(searchQuery);
            return SearchTweetsWithMetadata(searchTweetsParameters).FirstOrDefault();
        }

        public IEnumerable<ISearchResultsDTO> SearchTweetsWithMetadata(ISearchTweetsParameters searchTweetsParameters)
        {
            if (searchTweetsParameters == null)
            {
                throw new ArgumentException("TweetSearch Parameters cannot be null");
            }

            var result = new List<ISearchResultsDTO>();
            if (searchTweetsParameters.MaximumNumberOfResults > 100)
            {
                result = SearchTweetsRecursively(searchTweetsParameters);
            }
            else
            {
                string httpQuery = _searchQueryGenerator.GetSearchTweetsQuery(searchTweetsParameters);

                var searchTweetResult = GetSearchTweetResultsFromQuery(httpQuery);
                if (searchTweetResult == null)
                {
                    return null;
                }

                result.Add(searchTweetResult);
            }

            UpdateMatchingTweets(result, searchTweetsParameters);

            return result;
        }

        private List<ISearchResultsDTO> SearchTweetsRecursively(ISearchTweetsParameters searchTweetsParameters)
        {
            var searchParameter = _searchQueryHelper.CloneTweetSearchParameters(searchTweetsParameters);
            searchParameter.MaximumNumberOfResults = Math.Min(searchParameter.MaximumNumberOfResults, 100);

            string query = _searchQueryGenerator.GetSearchTweetsQuery(searchParameter);

            var currentResult = GetSearchTweetResultsFromQuery(query);
            if (currentResult == null)
            {
                return new List<ISearchResultsDTO>();
            }

            var result = new List<ISearchResultsDTO> { currentResult };
            var tweets = currentResult.TweetDTOs;
            var totalTweets = currentResult.TweetDTOs.ToList();

            while (totalTweets.Count < searchTweetsParameters.MaximumNumberOfResults)
            {
                if (tweets.IsEmpty())
                {
                    // If Twitter does not have any result left, stop the search
                    break;
                }

                var oldestTweetId = _tweetHelper.GetOldestTweetId(tweets);
                searchParameter.MaxId = oldestTweetId - 1;
                searchParameter.MaximumNumberOfResults = Math.Min(searchTweetsParameters.MaximumNumberOfResults - totalTweets.Count, 100);
                query = _searchQueryGenerator.GetSearchTweetsQuery(searchParameter);
                currentResult = GetSearchTweetResultsFromQuery(query);

                if (currentResult == null)
                {
                    break;
                }

                tweets = currentResult.TweetDTOs;
                totalTweets.AddRange(tweets);
                result.Add(currentResult);
            }

            return result;
        }

        private void UpdateMatchingTweets(IEnumerable<ISearchResultsDTO> searchResultsDTOs, ISearchTweetsParameters searchTweetsParameters)
        {
            foreach (var searchResultsDTO in searchResultsDTOs)
            {
                var tweetDTOs = searchResultsDTO.TweetDTOs;
                IEnumerable<ITweetWithSearchMetadataDTO> matchingTweetDTOS = null;

                if (searchTweetsParameters.TweetSearchType == TweetSearchType.OriginalTweetsOnly)
                {
                    matchingTweetDTOS = tweetDTOs.Where(x => x.RetweetedTweetDTO == null).ToArray();
                }

                if (searchTweetsParameters.TweetSearchType == TweetSearchType.RetweetsOnly)
                {
                    matchingTweetDTOS = tweetDTOs.Where(x => x.RetweetedTweetDTO != null).ToArray();
                }

                if (searchTweetsParameters.TweetSearchType == TweetSearchType.All)
                {
                    matchingTweetDTOS = tweetDTOs;
                }

                if (matchingTweetDTOS != null && searchTweetsParameters.FilterTweetsNotContainingGeoInformation)
                {
                    matchingTweetDTOS = matchingTweetDTOS.Where(x => x.Coordinates != null || x.Place != null);
                }

                searchResultsDTO.MatchingTweetDTOs = matchingTweetDTOS?.ToArray();
            }
        }

        public IEnumerable<ITweetDTO> SearchRepliesTo(ITweetDTO tweetDTO, bool recursiveReplies)
        {
            if (tweetDTO == null)
            {
                throw new ArgumentException("TweetDTO cannot be null");
            }

            var searchTweets = SearchTweets(string.Format(tweetDTO.CreatedBy.ScreenName)).ToList();

            if (recursiveReplies)
            {
                return GetRecursiveReplies(searchTweets, tweetDTO.Id);
            }

            var repliesDTO = searchTweets.Where(x => x.InReplyToStatusId == tweetDTO.Id);
            return repliesDTO;
        }

        private IEnumerable<ITweetDTO> GetRecursiveReplies(List<ITweetDTO> searchTweets, long sourceId)
        {
            var directReplies = searchTweets.Where(x => x.InReplyToStatusId == sourceId).ToList();
            List<ITweetDTO> results = directReplies.ToList();

            var recursiveReplies = searchTweets.Where(x => directReplies.Select(r => r.Id as long?).Contains(x.InReplyToStatusId));
            results.AddRange(recursiveReplies);

            while (recursiveReplies.Any())
            {
                var repliesFromPreviousLevel = recursiveReplies;
                recursiveReplies = searchTweets.Where(x => repliesFromPreviousLevel.Select(r => r.Id as long?).Contains(x.InReplyToStatusId));
                results.AddRange(recursiveReplies);
            }

            return results;
        }

        private ISearchResultsDTO GetSearchTweetResultsFromQuery(string query)
        {
            return _twitterAccessor.ExecuteGETQuery<ISearchResultsDTO>(query);
        }

        public IEnumerable<IUserDTO> SearchUsers(string searchQuery)
        {
            var searchUsersParameters = _searchQueryParameterGenerator.CreateUserSearchParameters(searchQuery);
            return SearchUsers(searchUsersParameters);
        }

        public IEnumerable<IUserDTO> SearchUsers(ISearchUsersParameters searchUsersParameters)
        {
            if (searchUsersParameters == null)
            {
                throw new ArgumentNullException("Search parameters cannot be null.");
            }

            if (searchUsersParameters.SearchQuery == null)
            {
                throw new ArgumentNullException("Search query cannot be null.");
            }

            if (searchUsersParameters.SearchQuery == string.Empty)
            {
                throw new ArgumentException("Search query cannot be empty.");
            }

            var maximumNumberOfResults = Math.Min(searchUsersParameters.MaximumNumberOfResults, 1000);
            var searchParameter = _searchQueryHelper.CloneUserSearchParameters(searchUsersParameters);
            searchParameter.MaximumNumberOfResults = Math.Min(searchParameter.MaximumNumberOfResults, 20);

            string query = _searchQueryGenerator.GetSearchUsersQuery(searchParameter);
            var currentResult = _twitterAccessor.ExecuteGETQuery<List<IUserDTO>>(query);
            if (currentResult == null)
            {
                return null;
            }

            var result = currentResult.ToDictionary(user => user.Id);
            var totalTweets = result.Count;

            while (totalTweets < maximumNumberOfResults)
            {
                if (currentResult.Count == 0)
                {
                    // If Twitter does not have any result left, stop the search
                    break;
                }

                searchParameter.MaximumNumberOfResults = Math.Min(searchUsersParameters.MaximumNumberOfResults - totalTweets, 20);
                ++searchParameter.Page;
                query = _searchQueryGenerator.GetSearchUsersQuery(searchParameter);
                currentResult = _twitterAccessor.ExecuteGETQuery<List<IUserDTO>>(query);

                bool searchIsComplete = currentResult == null;

                if (!searchIsComplete)
                {
                    foreach (var userDTO in currentResult)
                    {
                        if (result.ContainsKey(userDTO.Id))
                        {
                            searchIsComplete = true;
                        }
                        else
                        {
                            result.Add(userDTO.Id, userDTO);
                            ++totalTweets;
                        }
                    }
                }

                if (searchIsComplete)
                {
                    break;
                }
            }

            return result.Values;
        }
    }
}