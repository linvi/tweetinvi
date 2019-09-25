using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class AccountsClient : IAccountsClient
    {
        private readonly TwitterClient _client;
        private readonly IAccountsRequester _accountsRequester;
        private readonly IMultiLevelCursorIteratorFactory _multiLevelCursorIteratorFactory;

        public AccountsClient(TwitterClient client)
        {
            _client = client;
            _accountsRequester = client.RequestExecutor.Accounts;
            _multiLevelCursorIteratorFactory = TweetinviContainer.Resolve<IMultiLevelCursorIteratorFactory>();
        }

        #region Pending Followers Requests

        public ITwitterIterator<long> GetUserIdsRequestingFriendship()
        {
            return GetUserIdsRequestingFriendship(new GetUserIdsRequestingFriendshipParameters());
        }

        public ITwitterIterator<long> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var iterator = _accountsRequester.GetUserIdsRequestingFriendship(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship()
        {
            return GetUsersRequestingFriendship(new GetUsersRequestingFriendshipParameters());
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship(IGetUsersRequestingFriendshipParameters parameters)
        {
            var iterator = _accountsRequester.GetUserIdsRequestingFriendship(parameters);
            var maxPageSize = parameters.GetUsersPageSize;
            return _multiLevelCursorIteratorFactory.CreateUserMultiLevelIterator(_client, iterator, maxPageSize);
        }

        #endregion
    }
}