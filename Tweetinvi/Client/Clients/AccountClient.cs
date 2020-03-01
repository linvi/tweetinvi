using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class AccountClient : IAccountClient
    {
        private readonly ITwitterClient _client;
        private readonly IAccountRequester _accountRequester;
        private readonly IUserFactory _userFactory;
        private readonly IMultiLevelCursorIteratorFactory _multiLevelCursorIteratorFactory;

        public AccountClient(
            ITwitterClient client,
            IUserFactory userFactory,
            IMultiLevelCursorIteratorFactory multiLevelCursorIteratorFactory)
        {
            _client = client;
            _userFactory = userFactory;
            _accountRequester = client.RequestExecutor.Account;
            _multiLevelCursorIteratorFactory = multiLevelCursorIteratorFactory;
        }

        #region Authenticated User

        public Task<IAuthenticatedUser> GetAuthenticatedUser()
        {
            return GetAuthenticatedUser(new GetAuthenticatedUserParameters());
        }

        public async Task<IAuthenticatedUser> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters)
        {
            var requestResult = await _accountRequester.GetAuthenticatedUser(parameters).ConfigureAwait(false);
            return requestResult?.Result;
        }

        #endregion

        #region Block / Unblock

        public Task BlockUser(long? userId)
        {
            return BlockUser(new BlockUserParameters(userId));
        }

        public Task BlockUser(string username)
        {
            return BlockUser(new BlockUserParameters(username));
        }

        public Task BlockUser(IUserIdentifier user)
        {
            return BlockUser(new BlockUserParameters(user));
        }

        public async Task BlockUser(IBlockUserParameters parameters)
        {
            await _accountRequester.BlockUser(parameters).ConfigureAwait(false);
        }

        public Task UnblockUser(long? userId)
        {
            return UnblockUser(new UnblockUserParameters(userId));
        }

        public Task UnblockUser(string username)
        {
            return UnblockUser(new UnblockUserParameters(username));
        }

        public Task UnBlockUser(IUserIdentifier user)
        {
            return UnblockUser(new UnblockUserParameters(user));
        }

        public async Task UnblockUser(IUnblockUserParameters parameters)
        {
            await _accountRequester.UnblockUser(parameters).ConfigureAwait(false);
        }

        public Task ReportUserForSpam(long? userId)
        {
            return ReportUserForSpam(new ReportUserForSpamParameters(userId));
        }

        public Task ReportUserForSpam(string username)
        {
            return ReportUserForSpam(new ReportUserForSpamParameters(username));
        }

        public Task ReportUserForSpam(IUserIdentifier user)
        {
            return ReportUserForSpam(new ReportUserForSpamParameters(user));
        }

        public async Task ReportUserForSpam(IReportUserForSpamParameters parameters)
        {
            await _accountRequester.ReportUserForSpam(parameters).ConfigureAwait(false);
        }

        public ITwitterIterator<long> GetBlockedUserIds()
        {
            return GetBlockedUserIds(new GetBlockedUserIdsParameters());
        }

        public ITwitterIterator<long> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters)
        {
            var twitterCursorResult = _accountRequester.GetBlockedUserIdsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(twitterCursorResult, dto => dto.DataTransferObject.Ids);
        }

        public ITwitterIterator<IUser> GetBlockedUsers()
        {
            return GetBlockedUsers(new GetBlockedUsersParameters());
        }

        public ITwitterIterator<IUser> GetBlockedUsers(IGetBlockedUsersParameters parameters)
        {
            var twitterCursorResult = _accountRequester.GetBlockedUsersIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IUserCursorQueryResultDTO>, IUser>(twitterCursorResult, pageResult =>
            {
                var userDTOs = pageResult.DataTransferObject.Users;
                return _userFactory.GenerateUsersFromDTO(userDTOs, _client);
            });
        }

        #endregion

        #region Follow / Unfollow

        public Task FollowUser(long userId)
        {
            return FollowUser(new FollowUserParameters(userId));
        }

        public Task FollowUser(string username)
        {
            return FollowUser(new FollowUserParameters(username));
        }

        public Task FollowUser(IUserIdentifier user)
        {
            return FollowUser(new FollowUserParameters(user));
        }

        public async Task FollowUser(IFollowUserParameters parameters)
        {
            await _accountRequester.FollowUser(parameters).ConfigureAwait(false);
        }

        public Task UnFollowUser(long userId)
        {
            return UnFollowUser(new UnFollowUserParameters(userId));
        }

        public Task UnFollowUser(string username)
        {
            return UnFollowUser(new UnFollowUserParameters(username));
        }

        public Task UnFollowUser(IUserIdentifier user)
        {
            return UnFollowUser(new UnFollowUserParameters(user));
        }

        public async Task UnFollowUser(IUnFollowUserParameters parameters)
        {
            await _accountRequester.UnFollowUser(parameters).ConfigureAwait(false);
        }

        #endregion

        #region Update Friendship

        public async Task UpdateRelationship(IUpdateRelationshipParameters parameters)
        {
            await _accountRequester.UpdateRelationship(parameters).ConfigureAwait(false);
        }

        #endregion

        #region Pending Followers Requests

        public ITwitterIterator<long> GetUserIdsRequestingFriendship()
        {
            return GetUserIdsRequestingFriendship(new GetUserIdsRequestingFriendshipParameters());
        }

        public ITwitterIterator<long> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var iterator = _accountRequester.GetUserIdsRequestingFriendshipIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship()
        {
            return GetUsersRequestingFriendship(new GetUsersRequestingFriendshipParameters());
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship(IGetUsersRequestingFriendshipParameters parameters)
        {
            var iterator = _accountRequester.GetUserIdsRequestingFriendshipIterator(parameters);

            var maxPageSize = parameters.GetUsersPageSize;
            if (maxPageSize > _client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxPageSize, nameof(_client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE), "page size");
            }

            return _multiLevelCursorIteratorFactory.CreateUserMultiLevelIterator(_client, iterator, maxPageSize);
        }

        public ITwitterIterator<long> GetUserIdsYouRequestedToFollow()
        {
            return GetUserIdsYouRequestedToFollow(new GetUserIdsYouRequestedToFollowParameters());
        }

        public ITwitterIterator<long> GetUserIdsYouRequestedToFollow(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            var iterator = _accountRequester.GetUserIdsYouRequestedToFollowIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow()
        {
            return GetUsersYouRequestedToFollow(new GetUsersYouRequestedToFollowParameters());
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow(IGetUsersYouRequestedToFollowParameters parameters)
        {
            var iterator = _accountRequester.GetUserIdsYouRequestedToFollowIterator(parameters);

            var maxPageSize = parameters.GetUsersPageSize;
            if (maxPageSize > _client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxPageSize, nameof(_client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE), "page size");
            }

            return _multiLevelCursorIteratorFactory.CreateUserMultiLevelIterator(_client, iterator, maxPageSize);
        }

        #endregion

        #region Relationships With

        public Task<IUserDictionary<IRelationshipState>> GetRelationshipsWith(long[] userIds)
        {
            return GetRelationshipsWith(new GetRelationshipsWithParameters(userIds));
        }

        public Task<IUserDictionary<IRelationshipState>> GetRelationshipsWith(long?[] userIds)
        {
            return GetRelationshipsWith(new GetRelationshipsWithParameters(userIds));
        }

        public Task<IUserDictionary<IRelationshipState>> GetRelationshipsWith(string[] usernames)
        {
            return GetRelationshipsWith(new GetRelationshipsWithParameters(usernames));
        }

        public Task<IUserDictionary<IRelationshipState>> GetRelationshipsWith(IUserIdentifier[] users)
        {
            return GetRelationshipsWith(new GetRelationshipsWithParameters(users));
        }

        public Task<IUserDictionary<IRelationshipState>> GetRelationshipsWith(IUser[] users)
        {
            return GetRelationshipsWith(new GetRelationshipsWithParameters(users));
        }

        public async Task<IUserDictionary<IRelationshipState>> GetRelationshipsWith(IGetRelationshipsWithParameters parameters)
        {
            var twitterResult = await _accountRequester.GetRelationshipsWith(parameters).ConfigureAwait(false);
            var relationshipsWith = twitterResult.Result;

            var userRelationshipState = new UserDictionary<IRelationshipState>();

            foreach (var user in parameters.Users)
            {
                var userRelationship = relationshipsWith.FirstOrDefault(x => x.TargetId == user.Id || x.TargetScreenName.ToLowerInvariant() == user.ScreenName.ToLowerInvariant());
                if (userRelationship != null)
                {
                    userRelationshipState.AddOrUpdate(user, userRelationship);
                }
            }

            return userRelationshipState;
        }

        #endregion

        #region MUTE

        public Task<long[]> GetUserIdsWhoseRetweetsAreMuted()
        {
            return GetUserIdsWhoseRetweetsAreMuted(new GetUserIdsWhoseRetweetsAreMutedParameters());
        }

        public async Task<long[]> GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters parameters)
        {
            var twitterResult = await _accountRequester.GetUserIdsWhoseRetweetsAreMuted(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public ITwitterIterator<long> GetMutedUserIds()
        {
            return GetMutedUserIds(new GetMutedUserIdsParameters());
        }

        public ITwitterIterator<long> GetMutedUserIds(IGetMutedUserIdsParameters parameters)
        {
            var iterator = _accountRequester.GetMutedUserIdsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }

        public ITwitterIterator<IUser> GetMutedUsers()
        {
            return GetMutedUsers(new GetMutedUsersParameters());
        }

        public ITwitterIterator<IUser> GetMutedUsers(IGetMutedUsersParameters parameters)
        {
            var iterator = _accountRequester.GetMutedUsersIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IUserCursorQueryResultDTO>, IUser>(iterator, pageResult =>
            {
                var userDTOs = pageResult.DataTransferObject.Users;
                return _userFactory.GenerateUsersFromDTO(userDTOs, _client);
            });
        }

        public Task MuteUser(long? userId)
        {
            return MuteUser(new MuteUserParameters(userId));
        }

        public Task MuteUser(string username)
        {
            return MuteUser(new MuteUserParameters(username));
        }

        public Task MuteUser(IUserIdentifier user)
        {
            return MuteUser(new MuteUserParameters(user));
        }

        public async Task MuteUser(IMuteUserParameters parameters)
        {
            await _accountRequester.MuteUser(parameters).ConfigureAwait(false);
        }

        public Task UnMuteUser(long? userId)
        {
            return UnMuteUser(new UnMuteUserParameters(userId));
        }

        public Task UnMuteUser(string username)
        {
            return UnMuteUser(new UnMuteUserParameters(username));
        }

        public Task UnMuteUser(IUserIdentifier user)
        {
            return UnMuteUser(new UnMuteUserParameters(user));
        }

        public async Task UnMuteUser(IUnMuteUserParameters parameters)
        {
            await _accountRequester.UnMuteUser(parameters).ConfigureAwait(false);
        }

        #endregion
    }
}