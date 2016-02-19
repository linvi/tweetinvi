using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryExecutor
    {
        IEnumerable<ITweetDTO> SearchTweets(string query);
        IEnumerable<ITweetDTO> SearchTweets(ITweetSearchParameters tweetSearchParameters);
        IEnumerable<ITweetDTO> SearchRepliesTo(ITweetDTO tweetDTO, bool getRecursiveReplies);
        ISearchResultsDTO SearchTweetsWithMetadata(string searchQuery);
        IEnumerable<ISearchResultsDTO> SearchTweetsWithMetadata(ITweetSearchParameters tweetSearchParameters);

        IEnumerable<IUserDTO> SearchUsers(string searchQuery);
        IEnumerable<IUserDTO> SearchUsers(IUserSearchParameters userSearchParameters);
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

        public IEnumerable<ITweetDTO> SearchTweets(ITweetSearchParameters tweetSearchParameters)
        {
            var searchResults = SearchTweetsWithMetadata(tweetSearchParameters);
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

        public IEnumerable<ISearchResultsDTO> SearchTweetsWithMetadata(ITweetSearchParameters tweetSearchParameters)
        {
            if (tweetSearchParameters == null)
            {
                throw new ArgumentException("TweetSearch Parameters cannot be null");
            }

            List<ISearchResultsDTO> result =  new List<ISearchResultsDTO>();;
            if (tweetSearchParameters.MaximumNumberOfResults > 100)
            {
                result = SearchTweetsRecursively(tweetSearchParameters);
            }
            else
            {
                string httpQuery = _searchQueryGenerator.GetSearchTweetsQuery(tweetSearchParameters);
                
                var searchTweetResult = GetSearchTweetResultsFromQuery(httpQuery);
                if (searchTweetResult == null)
                {
                    return null;
                }

                result.Add(searchTweetResult);
            }

            UpdateMatchingTweets(result, tweetSearchParameters);

            return result;
        }

        private List<ISearchResultsDTO> SearchTweetsRecursively(ITweetSearchParameters tweetSearchParameters)
        {
            var searchParameter = _searchQueryHelper.CloneTweetSearchParameters(tweetSearchParameters);
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

            while (totalTweets.Count < tweetSearchParameters.MaximumNumberOfResults)
            {
                if (tweets.IsEmpty())
                {
                    // If Twitter does not have any result left, stop the search
                    break;
                }

                var oldestTweetId = _tweetHelper.GetOldestTweetId(tweets);
                searchParameter.MaxId = oldestTweetId - 1;
                searchParameter.MaximumNumberOfResults = Math.Min(tweetSearchParameters.MaximumNumberOfResults - totalTweets.Count, 100);
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

        private void UpdateMatchingTweets(IEnumerable<ISearchResultsDTO> searchResultsDTOs, ITweetSearchParameters tweetSearchParameters)
        {
            foreach (var searchResultsDTO in searchResultsDTOs)
            {
                var tweetDTOs = searchResultsDTO.TweetDTOs;
                if (tweetSearchParameters.TweetSearchType == TweetSearchType.OriginalTweetsOnly)
                {
                    searchResultsDTO.MatchingTweetDTOs = tweetDTOs.Where(x => x.RetweetedTweetDTO == null).ToArray();
                }

                if (tweetSearchParameters.TweetSearchType == TweetSearchType.RetweetsOnly)
                {
                    searchResultsDTO.MatchingTweetDTOs = tweetDTOs.Where(x => x.RetweetedTweetDTO != null).ToArray();
                }

                if (tweetSearchParameters.TweetSearchType == TweetSearchType.All)
                {
                    searchResultsDTO.MatchingTweetDTOs = tweetDTOs;
                }
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

        public IEnumerable<IUserDTO> SearchUsers(IUserSearchParameters userSearchParameters)
        {
            if (string.IsNullOrEmpty(userSearchParameters.SearchQuery))
            {
                return null;
            }

            var maximumNumberOfResults = Math.Min(userSearchParameters.MaximumNumberOfResults, 1000);
            var searchParameter = _searchQueryHelper.CloneUserSearchParameters(userSearchParameters);
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

                searchParameter.MaximumNumberOfResults = Math.Min(userSearchParameters.MaximumNumberOfResults - totalTweets, 20);
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