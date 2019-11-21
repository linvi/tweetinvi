using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.Enum;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryExecutor
    {
        Task<IEnumerable<ITweetDTO>> SearchTweets(string query);
        Task<IEnumerable<ITweetDTO>> SearchTweets(ISearchTweetsParameters searchTweetsParameters);
        Task<IEnumerable<ITweetDTO>> SearchRepliesTo(ITweetDTO tweetDTO, bool getRecursiveReplies);
        Task<ISearchResultsDTO> SearchTweetsWithMetadata(string searchQuery);
        Task<ISearchResultsDTO[]> SearchTweetsWithMetadata(ISearchTweetsParameters searchTweetsParameters);

        Task<IEnumerable<IUserDTO>> SearchUsers(string searchQuery);
        Task<IEnumerable<IUserDTO>> SearchUsers(ISearchUsersParameters searchUsersParameters);
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

        public async Task<IEnumerable<ITweetDTO>> SearchTweets(string searchQuery)
        {
            var searchTweetsParameters = _searchQueryParameterGenerator.CreateSearchTweetParameter(searchQuery);
            return await SearchTweets(searchTweetsParameters);
        }

        public async Task<IEnumerable<ITweetDTO>> SearchTweets(ISearchTweetsParameters searchTweetsParameters)
        {
            var searchResults = await SearchTweetsWithMetadata(searchTweetsParameters);
            if (searchResults == null)
            {
                return null;
            }

            return searchResults.SelectMany(x => x.MatchingTweetDTOs);
        }

        public async Task<ISearchResultsDTO> SearchTweetsWithMetadata(string searchQuery)
        {
            var searchTweetsParameters = _searchQueryParameterGenerator.CreateSearchTweetParameter(searchQuery);
            var searchesWithMetadata = await SearchTweetsWithMetadata(searchTweetsParameters);

            return searchesWithMetadata.FirstOrDefault();
        }

        public async Task<ISearchResultsDTO[]> SearchTweetsWithMetadata(ISearchTweetsParameters searchTweetsParameters)
        {
            if (searchTweetsParameters == null)
            {
                throw new ArgumentException("TweetSearch Parameters cannot be null");
            }

            var result = new List<ISearchResultsDTO>();
            if (searchTweetsParameters.MaximumNumberOfResults > 100)
            {
                result = await SearchTweetsRecursively(searchTweetsParameters);
            }
            else
            {
                string httpQuery = _searchQueryGenerator.GetSearchTweetsQuery(searchTweetsParameters);

                var searchTweetResult = await GetSearchTweetResultsFromQuery(httpQuery);
                if (searchTweetResult == null)
                {
                    return null;
                }

                result.Add(searchTweetResult);
            }

            UpdateMatchingTweets(result, searchTweetsParameters);

            return result.ToArray();
        }

        private async Task<List<ISearchResultsDTO>> SearchTweetsRecursively(ISearchTweetsParameters searchTweetsParameters)
        {
            var searchParameter = _searchQueryHelper.CloneTweetSearchParameters(searchTweetsParameters);
            searchParameter.MaximumNumberOfResults = Math.Min(searchParameter.MaximumNumberOfResults, 100);

            string query = _searchQueryGenerator.GetSearchTweetsQuery(searchParameter);

            var currentResult = await GetSearchTweetResultsFromQuery(query);
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
                currentResult = await GetSearchTweetResultsFromQuery(query);

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
                IEnumerable<ITweetWithSearchMetadataDTO> matchingTweetDTOs = null;

                if (searchTweetsParameters.TweetSearchType == TweetSearchType.OriginalTweetsOnly)
                {
                    matchingTweetDTOs = tweetDTOs.Where(x => x.RetweetedTweetDTO == null).ToArray();
                }

                if (searchTweetsParameters.TweetSearchType == TweetSearchType.RetweetsOnly)
                {
                    matchingTweetDTOs = tweetDTOs.Where(x => x.RetweetedTweetDTO != null).ToArray();
                }

                if (searchTweetsParameters.TweetSearchType == TweetSearchType.All)
                {
                    matchingTweetDTOs = tweetDTOs;
                }

                if (matchingTweetDTOs != null && searchTweetsParameters.FilterTweetsNotContainingGeoInformation)
                {
                    matchingTweetDTOs = matchingTweetDTOs.Where(x => x.Coordinates != null || x.Place != null);
                }

                searchResultsDTO.MatchingTweetDTOs = matchingTweetDTOs?.ToArray();
            }
        }

        public async Task<IEnumerable<ITweetDTO>> SearchRepliesTo(ITweetDTO tweetDTO, bool recursiveReplies)
        {
            if (tweetDTO == null)
            {
                throw new ArgumentNullException(nameof(tweetDTO));
            }

            if (tweetDTO.Id == null)
            {
                throw new ArgumentNullException($"{nameof(tweetDTO)}.{nameof(tweetDTO.Id)}");
            }

            var searchTweets = await SearchTweets(string.Format(tweetDTO.CreatedBy.ScreenName));
            var searchTweetsLists = searchTweets.ToList();

            if (recursiveReplies)
            {
                return GetRecursiveReplies(searchTweetsLists, tweetDTO.Id.Value);
            }

            var repliesDTO = searchTweetsLists.Where(x => x.InReplyToStatusId == tweetDTO.Id);
            return repliesDTO;
        }

        private static IEnumerable<ITweetDTO> GetRecursiveReplies(IReadOnlyCollection<ITweetDTO> searchTweets, long sourceId)
        {
            var directReplies = searchTweets.Where(x => x.InReplyToStatusId == sourceId).ToList();
            var results = directReplies.ToList();
            var recursiveReplies = searchTweets.Where(x => directReplies.Select(r => r.Id).Contains(x.InReplyToStatusId)).ToArray();
            
            results.AddRange(recursiveReplies);

            while (recursiveReplies.Any())
            {
                var repliesFromPreviousLevel = recursiveReplies;
                recursiveReplies = searchTweets.Where(x => repliesFromPreviousLevel.Select(r => r.Id).Contains(x.InReplyToStatusId)).ToArray();
                results.AddRange(recursiveReplies);
            }

            return results;
        }

        private Task<ISearchResultsDTO> GetSearchTweetResultsFromQuery(string query)
        {
            return _twitterAccessor.ExecuteGETQuery<ISearchResultsDTO>(query);
        }

        public Task<IEnumerable<IUserDTO>> SearchUsers(string searchQuery)
        {
            var searchUsersParameters = _searchQueryParameterGenerator.CreateUserSearchParameters(searchQuery);
            return SearchUsers(searchUsersParameters);
        }

        public async Task<IEnumerable<IUserDTO>> SearchUsers(ISearchUsersParameters searchUsersParameters)
        {
            if (searchUsersParameters == null)
            {
                throw new ArgumentNullException(nameof(searchUsersParameters));
            }

            if (searchUsersParameters.SearchQuery == null)
            {
                throw new ArgumentNullException(nameof(searchUsersParameters));
            }

            if (searchUsersParameters.SearchQuery == string.Empty)
            {
                throw new ArgumentException("Search query cannot be empty.", nameof(searchUsersParameters));
            }

            var maximumNumberOfResults = Math.Min(searchUsersParameters.MaximumNumberOfResults, 1000);
            var searchParameter = _searchQueryHelper.CloneUserSearchParameters(searchUsersParameters);
            searchParameter.MaximumNumberOfResults = Math.Min(searchParameter.MaximumNumberOfResults, 20);

            string query = _searchQueryGenerator.GetSearchUsersQuery(searchParameter);
            var currentResult = await _twitterAccessor.ExecuteGETQuery<List<IUserDTO>>(query);
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
                currentResult = await _twitterAccessor.ExecuteGETQuery<List<IUserDTO>>(query);

                var searchIsComplete = currentResult == null;

                if (!searchIsComplete)
                {
                    foreach (var userDTO in currentResult)
                    {
                        if (userDTO?.Id == null)
                        {
                            continue;
                        }

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