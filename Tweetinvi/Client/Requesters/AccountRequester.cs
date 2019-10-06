using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
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
    public interface IInternalAccountRequester : IAccountRequester, IBaseRequester
    {
    }

    public class AccountRequester : BaseRequester, IInternalAccountRequester
    {
        private readonly IAccountController _accountController;
        private readonly ITwitterResultFactory _twitterResultFactory;
        private readonly IFriendshipFactory _friendshipFactory;
        private readonly IAccountClientRequiredParametersValidator _validator;

        public AccountRequester(
            IAccountController accountController, 
            ITwitterResultFactory twitterResultFactory, 
            IFriendshipFactory friendshipFactory,
            IAccountClientRequiredParametersValidator validator)
        {
            _accountController = accountController;
            _twitterResultFactory = twitterResultFactory;
            _friendshipFactory = friendshipFactory;
            _validator = validator;
        }
        
        public async Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters)
        {
            _validator.Validate(parameters);
            
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
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _accountController.FollowUser(parameters, request), request);
        }
        
        public async Task<ITwitterResult<IRelationshipDetailsDTO, IRelationshipDetails>> UpdateRelationship(IUpdateRelationshipParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            var result = await ExecuteRequest(() => _accountController.UpdateRelationship(parameters, request), request).ConfigureAwait(false);
            
            return _twitterResultFactory.Create(result, _friendshipFactory.GenerateRelationshipFromRelationshipDTO);
        }

        public Task<ITwitterResult<IUserDTO>> UnFollowUser(IUnFollowUserParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _accountController.UnFollowUser(parameters, request), request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetUserIdsRequestingFriendship(parameters, request);
        }
        
        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsYouRequestedToFollow(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetUserIdsYouRequestedToFollow(parameters, request);
        }

        // BLOCK
        public Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _accountController.BlockUser(parameters, request), request);
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _accountController.UnblockUser(parameters, request), request);
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _accountController.ReportUserForSpam(parameters, request), request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetBlockedUserIds(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetBlockedUsers(IGetBlockedUsersParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetBlockedUsers(parameters, request);
        }

        // FRIENDSHIPS
        public async Task<ITwitterResult<IRelationshipStateDTO[], IRelationshipState[]>> GetRelationshipsWith(IGetRelationshipsWithParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            var twitterResult = await _accountController.GetRelationshipsWith(parameters, request).ConfigureAwait(false);

            return _twitterResultFactory.Create(twitterResult, _friendshipFactory.GenerateRelationshipStatesFromRelationshipStatesDTO);
        }
        
        // MUTE
        public Task<ITwitterResult<long[]>> GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            return _accountController.GetUserIdsWhoseRetweetsAreMuted(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetMutedUserIds(IGetMutedUserIdsParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetMutedUserIds(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMutedUsers(IGetMutedUsersParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetMutedUsers(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> MuteUser(IMuteUserParameters parameters)
        {
            _validator.Validate(parameters);
            var request = _twitterClient.CreateRequest();
            return _accountController.MuteUser(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UnMuteUser(IUnMuteUserParameters parameters)
        {
            _validator.Validate(parameters);
            var request = _twitterClient.CreateRequest();
            return _accountController.UnMuteUser(parameters, request);
        }
    }
}