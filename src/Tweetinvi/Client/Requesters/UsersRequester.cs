using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class UsersRequester : BaseRequester, IUsersRequester
    {
        private readonly IUserController _userController;
        private readonly IUsersClientRequiredParametersValidator _validator;

        public UsersRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            IUserController userController,
            IUsersClientRequiredParametersValidator validator)
        : base(client, clientEvents)
        {
            _userController = userController;
            _validator = validator;
        }

        public Task<ITwitterResult<IUserDTO>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            return ExecuteRequest(() => _userController.GetAuthenticatedUser(parameters, request), request);
        }

        public Task<ITwitterResult<IUserDTO>> GetUser(IGetUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.GetUser(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO[]>> GetUsers(IGetUsersParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.GetUsers(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIdsIterator(IGetFriendIdsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _userController.GetFriendIdsIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIdsIterator(IGetFollowerIdsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _userController.GetFollowerIdsIterator(parameters, request);
        }

        public Task<ITwitterResult<IRelationshipDetailsDTO>> GetRelationshipBetween(IGetRelationshipBetweenParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.GetRelationshipBetween(parameters, request));
        }

        // FOLLOWERS
        public Task<ITwitterResult<IUserDTO>> FollowUser(IFollowUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.FollowUser(parameters, request));
        }

        public Task<ITwitterResult<IRelationshipDetailsDTO>> UpdateRelationship(IUpdateRelationshipParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.UpdateRelationship(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> UnfollowUser(IUnfollowUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.UnfollowUser(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendshipIterator(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _userController.GetUserIdsRequestingFriendshipIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsYouRequestedToFollowIterator(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _userController.GetUserIdsYouRequestedToFollowIterator(parameters, request);
        }

        // BLOCK
        public Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.BlockUser(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.UnblockUser(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.ReportUserForSpam(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIdsIterator(IGetBlockedUserIdsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _userController.GetBlockedUserIdsIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetBlockedUsersIterator(IGetBlockedUsersParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _userController.GetBlockedUsersIterator(parameters, request);
        }

        // FRIENDSHIPS
        public Task<ITwitterResult<IRelationshipStateDTO[]>> GetRelationshipsWith(IGetRelationshipsWithParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.GetRelationshipsWith(parameters, request));
        }

        // MUTE
        public Task<ITwitterResult<long[]>> GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.GetUserIdsWhoseRetweetsAreMuted(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetMutedUserIdsIterator(IGetMutedUserIdsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _userController.GetMutedUserIdsIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMutedUsersIterator(IGetMutedUsersParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _userController.GetMutedUsersIterator(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> MuteUser(IMuteUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.MuteUser(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> UnmuteUser(IUnmuteUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.UnmuteUser(parameters, request));
        }

        public Task<System.IO.Stream> GetProfileImageStream(IGetProfileImageParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.GetProfileImageStream(parameters, request));
        }
    }
}