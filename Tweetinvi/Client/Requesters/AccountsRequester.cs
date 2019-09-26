using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.QueryJsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
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
        
        public async Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            var result = await ExecuteRequest(() => _accountController.GetAuthenticatedUser(parameters, request), request).ConfigureAwait(false);

            var user = result.Result;

            if (user != null)
            {
                user.Client = _twitterClient;
            }

            return result;
        }

        public Task<ITwitterResult<IUserDTO>> FollowUser(IFollowUserParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _accountController.FollowUser(parameters, request), request);
        }

        public Task<ITwitterResult<IUserDTO>> UnFollowUser(IUnFollowUserParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _accountController.UnFollowUser(parameters, request), request);
        }
        
        public Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _accountController.BlockUser(parameters, request), request);
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _accountController.UnblockUser(parameters, request), request);
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _accountController.ReportUserForSpam(parameters, request), request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetBlockedUserIds(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetBlockedUsers(IGetBlockedUsersParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetBlockedUsers(parameters, request);
        }
        
        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetUserIdsRequestingFriendship(parameters, request);
        }
    }
}