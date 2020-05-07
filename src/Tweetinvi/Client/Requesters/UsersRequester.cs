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

        public Task<ITwitterResult<IUserDTO>> GetAuthenticatedUserAsync(IGetAuthenticatedUserParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            return ExecuteRequestAsync(() => _userController.GetAuthenticatedUserAsync(parameters, request), request);
        }

        public Task<ITwitterResult<IUserDTO>> GetUserAsync(IGetUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.GetUserAsync(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO[]>> GetUsersAsync(IGetUsersParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.GetUsersAsync(parameters, request));
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

        public Task<ITwitterResult<IRelationshipDetailsDTO>> GetRelationshipBetweenAsync(IGetRelationshipBetweenParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.GetRelationshipBetweenAsync(parameters, request));
        }

        // FOLLOWERS
        public Task<ITwitterResult<IUserDTO>> FollowUserAsync(IFollowUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.FollowUserAsync(parameters, request));
        }

        public Task<ITwitterResult<IRelationshipDetailsDTO>> UpdateRelationshipAsync(IUpdateRelationshipParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.UpdateRelationshipAsync(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> UnfollowUserAsync(IUnfollowUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.UnfollowUserAsync(parameters, request));
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
        public Task<ITwitterResult<IUserDTO>> BlockUserAsync(IBlockUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.BlockUserAsync(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUserAsync(IUnblockUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.UnblockUserAsync(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpamAsync(IReportUserForSpamParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.ReportUserForSpamAsync(parameters, request));
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
        public Task<ITwitterResult<IRelationshipStateDTO[]>> GetRelationshipsWithAsync(IGetRelationshipsWithParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.GetRelationshipsWithAsync(parameters, request));
        }

        // MUTE
        public Task<ITwitterResult<long[]>> GetUserIdsWhoseRetweetsAreMutedAsync(IGetUserIdsWhoseRetweetsAreMutedParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.GetUserIdsWhoseRetweetsAreMutedAsync(parameters, request));
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

        public Task<ITwitterResult<IUserDTO>> MuteUserAsync(IMuteUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.MuteUserAsync(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> UnmuteUserAsync(IUnmuteUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.UnmuteUserAsync(parameters, request));
        }

        public Task<System.IO.Stream> GetProfileImageStream(IGetProfileImageParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _userController.GetProfileImageStreamAsync(parameters, request));
        }
    }
}