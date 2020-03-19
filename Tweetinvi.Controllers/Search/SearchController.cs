using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchController
    {
        ITwitterPageIterator<ITwitterResult<ISearchResultsDTO>, long?> SearchTweets(ISearchTweetsParameters parameters, ITwitterRequest request);

        Task<IEnumerable<ITweet>> SearchDirectRepliesTo(ITweet tweet);
        Task<IEnumerable<ITweet>> SearchRepliesTo(ITweet tweet, bool recursiveReplies);

        Task<IEnumerable<IUser>> SearchUsers(string searchQuery);
        Task<IEnumerable<IUser>> SearchUsers(ISearchUsersParameters searchUsersParameters);
    }

    public class SearchController : ISearchController
    {
        private readonly ISearchQueryExecutor _searchQueryExecutor;
        private readonly ITwitterClientFactories _factories;
        private readonly IPageCursorIteratorFactories _pageCursorIteratorFactories;
        private readonly ITweetFactory _tweetFactory;
        private readonly IUserFactory _userFactory;

        public SearchController(
            ISearchQueryExecutor searchQueryExecutor,
            ITwitterClientFactories factories,
            IPageCursorIteratorFactories pageCursorIteratorFactories,
            ITweetFactory tweetFactory,
            IUserFactory userFactory)
        {
            _searchQueryExecutor = searchQueryExecutor;
            _factories = factories;
            _pageCursorIteratorFactories = pageCursorIteratorFactories;
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;
        }

        public ITwitterPageIterator<ITwitterResult<ISearchResultsDTO>, long?> SearchTweets(ISearchTweetsParameters parameters, ITwitterRequest request)
        {
            return new TwitterPageIterator<ITwitterResult<ISearchResultsDTO>, long?>(
                parameters.MaxId,
                cursor =>
                {
                    var cursoredParameters = new SearchTweetsParameters(parameters)
                    {
                        MaxId = cursor
                    };

                    return _searchQueryExecutor.SearchTweets(cursoredParameters, new TwitterRequest(request));
                },
                page =>
                {
                    if (page?.DataTransferObject?.SearchMetadata?.NextResults == null)
                    {
                        return null;
                    }

                    return page.DataTransferObject.SearchMetadata.MaxId;
                },
                page => page?.DataTransferObject?.SearchMetadata?.NextResults == null);
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