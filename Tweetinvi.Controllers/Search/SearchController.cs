using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchController
    {
        Task<IEnumerable<ITweet>> SearchTweets(string searchQuery);
        Task<IEnumerable<ITweet>> SearchTweets(ISearchTweetsParameters searchTweetsParameters);

        Task<ISearchResult> SearchTweetsWithMetadata(string searchQuery);
        Task<ISearchResult> SearchTweetsWithMetadata(ISearchTweetsParameters searchTweetsParameters);

        Task<IEnumerable<ITweet>> SearchDirectRepliesTo(ITweet tweet);
        Task<IEnumerable<ITweet>> SearchRepliesTo(ITweet tweet, bool recursiveReplies);

        Task<IEnumerable<IUser>> SearchUsers(string searchQuery);
        Task<IEnumerable<IUser>> SearchUsers(ISearchUsersParameters searchUsersParameters);
    }

    public class SearchController : ISearchController
    {
        private readonly ISearchQueryExecutor _searchQueryExecutor;
        private readonly ITwitterClientFactories _factories;
        private readonly ITweetFactory _tweetFactory;
        private readonly IUserFactory _userFactory;

        public SearchController(
            ISearchQueryExecutor searchQueryExecutor,
            ITwitterClientFactories factories,
            ITweetFactory tweetFactory,
            IUserFactory userFactory)
        {
            _searchQueryExecutor = searchQueryExecutor;
            _factories = factories;
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;
        }

        public async Task<IEnumerable<ITweet>> SearchTweets(string searchQuery)
        {
            var tweetsDTO = await _searchQueryExecutor.SearchTweets(searchQuery);
            return _tweetFactory.GenerateTweetsFromDTO(tweetsDTO, null, null);
        }

        public async Task<IEnumerable<ITweet>> SearchTweets(ISearchTweetsParameters searchTweetsParameters)
        {
            var tweetsDTO = await _searchQueryExecutor.SearchTweets(searchTweetsParameters);
            return _tweetFactory.GenerateTweetsFromDTO(tweetsDTO, null, null);
        }

        public async Task<ISearchResult> SearchTweetsWithMetadata(string searchQuery)
        {
            var searchResultsDTO = await _searchQueryExecutor.SearchTweetsWithMetadata(searchQuery);
            return _factories.CreateSearchResult(new [] { searchResultsDTO });
        }

        public async Task<ISearchResult> SearchTweetsWithMetadata(ISearchTweetsParameters searchTweetsParameters)
        {
            var searchResultsDTO = (await _searchQueryExecutor.SearchTweetsWithMetadata(searchTweetsParameters)).ToArray();
            return _factories.CreateSearchResult(searchResultsDTO);
        }

        public Task<IEnumerable<ITweet>> SearchDirectRepliesTo(ITweet tweet)
        {
            return SearchRepliesTo(tweet, false);
        }

        public async Task<IEnumerable<ITweet>> SearchRepliesTo(ITweet tweet, bool recursiveReplies)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            var repliesDTO = await _searchQueryExecutor.SearchRepliesTo(tweet.TweetDTO, recursiveReplies);
            return _tweetFactory.GenerateTweetsFromDTO(repliesDTO, null, null);
        }

        public async Task<IEnumerable<IUser>> SearchUsers(string searchQuery)
        {
            var userDTOs = await _searchQueryExecutor.SearchUsers(searchQuery);
            return _userFactory.GenerateUsersFromDTO(userDTOs, null);
        }

        public async Task<IEnumerable<IUser>> SearchUsers(ISearchUsersParameters searchUsersParameters)
        {
            var userDTOs = await _searchQueryExecutor.SearchUsers(searchUsersParameters);
            return _userFactory.GenerateUsersFromDTO(userDTOs, null);
        }
    }
}