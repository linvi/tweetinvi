using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Iterators;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class AccountsClient : IAccountsClient
    {
        private readonly IAccountsRequester _accountsRequester;
        
        public AccountsClient(TwitterClient client)
        {
            _accountsRequester = client.RequestExecutor.Accounts;
        }

        public ITwitterIterator<long> GetUserIdsRequestingFriendship()
        {
            return GetUserIdsRequestingFriendship(new GetUserIdsRequestingFriendshipParameters());
        }

        public ITwitterIterator<long> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var iterator = _accountsRequester.GetUserIdsRequestingFriendship(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }
    }
}