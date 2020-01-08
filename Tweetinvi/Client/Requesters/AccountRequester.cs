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

            var request = TwitterClient.CreateRequest();
            var result = await ExecuteRequest(() => _accountController.GetAuthenticatedUser(parameters, request), request).ConfigureAwait(false);

            var user = result.Result;

            if (user != null)
            {
                user.Client = TwitterClient;
            }

            return result;
        }

        // FOLLOWERS
        public Task<ITwitterResult<IUserDTO>> FollowUser(IFollowUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountController.FollowUser(parameters, request));
        }

        public Task<ITwitterResult<IRelationshipDetailsDTO, IRelationshipDetails>> UpdateRelationship(IUpdateRelationshipParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var result = await _accountController.UpdateRelationship(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(result, _friendshipFactory.GenerateRelationshipFromRelationshipDTO);
            });
        }

        public Task<ITwitterResult<IUserDTO>> UnFollowUser(IUnFollowUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountController.UnFollowUser(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendshipIterator(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetUserIdsRequestingFriendshipIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsYouRequestedToFollowIterator(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetUserIdsYouRequestedToFollowIterator(parameters, request);
        }

        // BLOCK
        public Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountController.BlockUser(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountController.UnblockUser(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountController.ReportUserForSpam(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIdsIterator(IGetBlockedUserIdsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetBlockedUserIdsIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetBlockedUsersIterator(IGetBlockedUsersParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetBlockedUsersIterator(parameters, request);
        }

        // FRIENDSHIPS
        public Task<ITwitterResult<IRelationshipStateDTO[], IRelationshipState[]>> GetRelationshipsWith(IGetRelationshipsWithParameters parameters)
        {
            _validator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var twitterResult = await _accountController.GetRelationshipsWith(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, _friendshipFactory.GenerateRelationshipStatesFromRelationshipStatesDTO);
            });
        }

        // MUTE
        public Task<ITwitterResult<long[]>> GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountController.GetUserIdsWhoseRetweetsAreMuted(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetMutedUserIdsIterator(IGetMutedUserIdsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetMutedUserIdsIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMutedUsersIterator(IGetMutedUsersParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _accountController.GetMutedUsersIterator(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> MuteUser(IMuteUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountController.MuteUser(parameters, request));
        }

        public Task<ITwitterResult<IUserDTO>> UnMuteUser(IUnMuteUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountController.UnMuteUser(parameters, request));
        }
    }
}