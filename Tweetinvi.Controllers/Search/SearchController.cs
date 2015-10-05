using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchController
    {
        IEnumerable<ITweet> SearchTweets(string searchQuery);
        IEnumerable<ITweet> SearchTweets(ITweetSearchParameters tweetSearchParameters);

        ISearchResult SearchTweetsWithMetadata(string searchQuery);
        ISearchResult SearchTweetsWithMetadata(ITweetSearchParameters tweetSearchParameters);

        IEnumerable<ITweet> SearchDirectRepliesTo(ITweet tweet);
        IEnumerable<ITweet> SearchRepliesTo(ITweet tweet, bool recursiveReplies);

        IEnumerable<IUser> SearchUsers(string searchQuery);
        IEnumerable<IUser> SearchUsers(IUserSearchParameters userSearchParameters);
    }

    public class SearchController : ISearchController
    {
        private readonly ISearchQueryExecutor _searchQueryExecutor;
        private readonly ISearchResultFactory _searchResultFactory;
        private readonly ITweetFactory _tweetFactory;
        private readonly IUserFactory _userFactory;

        public SearchController(
            ISearchQueryExecutor searchQueryExecutor, 
            ISearchResultFactory searchResultFactory,
            ITweetFactory tweetFactory,
            IUserFactory userFactory)
        {
            _searchQueryExecutor = searchQueryExecutor;
            _searchResultFactory = searchResultFactory;
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;
        }

        public IEnumerable<ITweet> SearchTweets(string searchQuery)
        {
            var tweetsDTO = _searchQueryExecutor.SearchTweets(searchQuery);
            return _tweetFactory.GenerateTweetsFromDTO(tweetsDTO);
        }

        public IEnumerable<ITweet> SearchTweets(ITweetSearchParameters tweetSearchParameters)
        {
            var tweetsDTO = _searchQueryExecutor.SearchTweets(tweetSearchParameters);
            return _tweetFactory.GenerateTweetsFromDTO(tweetsDTO);
        }

        public ISearchResult SearchTweetsWithMetadata(string searchQuery)
        {
            var searchResultsDTO = _searchQueryExecutor.SearchTweetsWithMetadata(searchQuery);
            return _searchResultFactory.Create(new [] { searchResultsDTO });
        }

        public ISearchResult SearchTweetsWithMetadata(ITweetSearchParameters tweetSearchParameters)
        {
            var searchResultsDTO = _searchQueryExecutor.SearchTweetsWithMetadata(tweetSearchParameters).ToArray();
            return _searchResultFactory.Create(searchResultsDTO);
        }

        public IEnumerable<ITweet> SearchDirectRepliesTo(ITweet tweet)
        {
            return SearchRepliesTo(tweet, false);
        }

        public IEnumerable<ITweet> SearchRepliesTo(ITweet tweet, bool recursiveReplies)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            var repliesDTO = _searchQueryExecutor.SearchRepliesTo(tweet.TweetDTO, recursiveReplies);
            return _tweetFactory.GenerateTweetsFromDTO(repliesDTO);
        }

        public IEnumerable<IUser> SearchUsers(string searchQuery)
        {
            var userDTOs = _searchQueryExecutor.SearchUsers(searchQuery);
            return _userFactory.GenerateUsersFromDTO(userDTOs);
        }

        public IEnumerable<IUser> SearchUsers(IUserSearchParameters userSearchParameters)
        {
            var userDTOs = _searchQueryExecutor.SearchUsers(userSearchParameters);
            return _userFactory.GenerateUsersFromDTO(userDTOs);
        }
    }
}