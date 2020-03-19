using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryExecutor
    {
        Task<ITwitterResult<ISearchResultsDTO>> SearchTweets(ISearchTweetsParameters parameters, ITwitterRequest request);
        Task<IEnumerable<ITweetDTO>> SearchRepliesTo(ITweetDTO tweetDTO, bool getRecursiveReplies);
        Task<IEnumerable<IUserDTO>> SearchUsers(string searchQuery);
        Task<IEnumerable<IUserDTO>> SearchUsers(ISearchUsersParameters searchUsersParameters);
    }

    public class SearchQueryExecutor : ISearchQueryExecutor
    {
        private readonly ISearchQueryGenerator _searchQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly ISearchQueryParameterGenerator _searchQueryParameterGenerator;

        public SearchQueryExecutor(
            ISearchQueryGenerator searchQueryGenerator,
            ITwitterAccessor twitterAccessor,
            ISearchQueryParameterGenerator searchQueryParameterGenerator)
        {
            _searchQueryGenerator = searchQueryGenerator;
            _twitterAccessor = twitterAccessor;
            _searchQueryParameterGenerator = searchQueryParameterGenerator;
        }

        public Task<ITwitterResult<ISearchResultsDTO>> SearchTweets(ISearchTweetsParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _searchQueryGenerator.GetSearchTweetsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ISearchResultsDTO>(request);
        }

        public async Task<IEnumerable<ITweetDTO>> SearchRepliesTo(ITweetDTO tweetDTO, bool recursiveReplies)
        {
            // if (tweetDTO == null)
            // {
            //     throw new ArgumentNullException(nameof(tweetDTO));
            // }
            //
            // var searchTweets = await SearchTweets(tweetDTO.CreatedBy.ScreenName);
            // var searchTweetsLists = searchTweets.ToList();
            //
            // if (recursiveReplies)
            // {
            //     return GetRecursiveReplies(searchTweetsLists, tweetDTO.Id);
            // }
            //
            // var repliesDTO = searchTweetsLists.Where(x => x.InReplyToStatusId == tweetDTO.Id);
            // return repliesDTO;

            throw new NotImplementedException();
        }

        private static IEnumerable<ITweetDTO> GetRecursiveReplies(IReadOnlyCollection<ITweetDTO> searchTweets, long sourceId)
        {
            var directReplies = searchTweets.Where(x => x.InReplyToStatusId == sourceId).ToArray();
            var results = directReplies.ToList();
            var recursiveReplies = searchTweets.Where(x =>
            {
                return x.InReplyToStatusId != null && directReplies.Select(r => r.Id).Contains(x.InReplyToStatusId.Value);
            }).ToArray();

            results.AddRange(recursiveReplies);

            while (recursiveReplies.Any())
            {
                var repliesFromPreviousLevel = recursiveReplies;
                recursiveReplies = searchTweets
                    .Where(x => { return x.InReplyToStatusId != null && repliesFromPreviousLevel.Select(r => r.Id).Contains(x.InReplyToStatusId.Value); }).ToArray();
                results.AddRange(recursiveReplies);
            }

            return results;
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

            if (searchUsersParameters.Query == null)
            {
                throw new ArgumentNullException(nameof(searchUsersParameters));
            }

            if (searchUsersParameters.Query == string.Empty)
            {
                throw new ArgumentException("Search query cannot be empty.", nameof(searchUsersParameters));
            }

            var maximumNumberOfResults = Math.Min(searchUsersParameters.MaximumNumberOfResults, 1000);
            // var searchParameter = _searchQueryHelper.CloneUserSearchParameters(searchUsersParameters);
            searchUsersParameters.MaximumNumberOfResults = Math.Min(searchUsersParameters.MaximumNumberOfResults, 20);

            string query = _searchQueryGenerator.GetSearchUsersQuery(searchUsersParameters);
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

                searchUsersParameters.MaximumNumberOfResults = Math.Min(searchUsersParameters.MaximumNumberOfResults - totalTweets, 20);
                ++searchUsersParameters.Page;
                query = _searchQueryGenerator.GetSearchUsersQuery(searchUsersParameters);
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