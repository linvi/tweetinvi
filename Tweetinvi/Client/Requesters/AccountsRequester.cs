using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.QueryJsonConverters;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IInternalAccountsRequester : IAccountsRequester, IBaseRequester
    {
    }

    public class AccountsRequester : BaseRequester, IInternalAccountsRequester
    {
        private readonly IAccountController _accountController;

        public AccountsRequester(IAccountController accountController)
        {
            _accountController = accountController;
        }
        
        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetUserIdsRequestingFriendship(parameters, request);
        }
    }
}