using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
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
        private readonly ITwitterResultFactory _twitterResultFactory;
        private readonly IFriendshipFactory _friendshipFactory;

        public AccountsRequester(
            IAccountController accountController, 
            ITwitterResultFactory twitterResultFactory, 
            IFriendshipFactory friendshipFactory)
        {
            _accountController = accountController;
            _twitterResultFactory = twitterResultFactory;
            _friendshipFactory = friendshipFactory;
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

        // FOLLOWERS
        public Task<ITwitterResult<IUserDTO>> FollowUser(IFollowUserParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _accountController.FollowUser(parameters, request), request);
        }
        
        public async Task<ITwitterResult<IRelationshipDetailsDTO, IRelationshipDetails>> UpdateRelationship(IUpdateRelationshipParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            var result = await ExecuteRequest(() => _accountController.UpdateRelationship(parameters, request), request).ConfigureAwait(false);
            
            return _twitterResultFactory.Create(result, _friendshipFactory.GenerateRelationshipFromRelationshipDTO);
        }

        public Task<ITwitterResult<IUserDTO>> UnFollowUser(IUnFollowUserParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _accountController.UnFollowUser(parameters, request), request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetUserIdsRequestingFriendship(parameters, request);
        }
        
        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsYouRequestedToFollow(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetUserIdsYouRequestedToFollow(parameters, request);
        }

        // BLOCK
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

        // FRIENDSHIPS
        public async Task<ITwitterResult<IRelationshipStateDTO[], IRelationshipState[]>> GetRelationshipsWith(IGetRelationshipsWithParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            var twitterResult = await _accountController.GetRelationshipsWith(parameters, request).ConfigureAwait(false);

            return _twitterResultFactory.Create(twitterResult, _friendshipFactory.GenerateRelationshipStatesFromRelationshipStatesDTO);
        }
    }
}