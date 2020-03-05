using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class UsersRequester : BaseRequester, IUsersRequester
    {
        private readonly ITwitterClient _client;
        private readonly IUserController _userController;
        private readonly ITwitterClientFactories _factories;
        private readonly ITwitterResultFactory _twitterResultFactory;
        private readonly IUserFactory _userFactory;
        private readonly IUsersClientRequiredParametersValidator _validator;

        public UsersRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            IUserController userController,
            ITwitterClientFactories factories,
            ITwitterResultFactory twitterResultFactory,
            IUserFactory userFactory,
            IUsersClientRequiredParametersValidator validator)
        : base(client, clientEvents)
        {
            _client = client;
            _userController = userController;
            _factories = factories;
            _twitterResultFactory = twitterResultFactory;
            _userFactory = userFactory;
            _validator = validator;
        }

        public async Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            var result = await ExecuteRequest(() => _userController.GetAuthenticatedUser(parameters, request), request).ConfigureAwait(false);

            var user = result.Result;

            if (user != null)
            {
                user.Client = TwitterClient;
            }

            return result;
        }

        public Task<ITwitterResult<IUserDTO, IUser>> GetUser(IGetUserParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var twitterResult = await _userController.GetUser(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, userDTO =>
                {
                    var user = _factories.CreateUser(userDTO);

                    if (user != null)
                    {
                        user.Client = TwitterClient;
                    }

                    return user;
                });
            });
        }

        public Task<ITwitterResult<IUserDTO[], IUser[]>> GetUsers(IGetUsersParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var twitterResult = await _userController.GetUsers(parameters, request).ConfigureAwait(false);

                return _twitterResultFactory.Create(twitterResult, userDTO =>
                {
                    var users = _userFactory.GenerateUsersFromDTO(userDTO, _client);
                    users?.ForEach(x => x.Client = TwitterClient);
                    return users;
                });
            });
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

        public Task<ITwitterResult<IRelationshipDetailsDTO, IRelationshipDetails>> GetRelationshipBetween(IGetRelationshipBetweenParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var result = await _userController.GetRelationshipBetween(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(result, _factories.CreateRelationshipDetails);
            });
        }

        // FOLLOWERS
        public Task<ITwitterResult<IUserDTO>> FollowUser(IFollowUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.FollowUser(parameters, request));
        }

        public Task<ITwitterResult<IRelationshipDetailsDTO, IRelationshipDetails>> UpdateRelationship(IUpdateRelationshipParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var result = await _userController.UpdateRelationship(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(result, _factories.CreateRelationshipDetails);
            });
        }

        public Task<ITwitterResult<IUserDTO>> UnFollowUser(IUnFollowUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _userController.UnFollowUser(parameters, request));
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
        public Task<ITwitterResult<IRelationshipStateDTO[], IRelationshipState[]>> GetRelationshipsWith(IGetRelationshipsWithParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var twitterResult = await _userController.GetRelationshipsWith(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dtos => dtos?.Select(x => _factories.CreateRelationshipState(x)).ToArray());
            });
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