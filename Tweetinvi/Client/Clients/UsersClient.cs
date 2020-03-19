using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class UsersClient : IUsersClient
    {
        private readonly ITwitterClient _client;
        private readonly IUsersRequester _usersRequester;
        private readonly IMultiLevelCursorIteratorFactory _multiLevelCursorIteratorFactory;

        public UsersClient(ITwitterClient client, IMultiLevelCursorIteratorFactory multiLevelCursorIteratorFactory)
        {
            _client = client;
            _usersRequester = client.Raw.Users;
            _multiLevelCursorIteratorFactory = multiLevelCursorIteratorFactory;
        }

        public IUsersClientParametersValidator ParametersValidator => _client.ParametersValidator;

        #region Authenticated User

        public Task<IAuthenticatedUser> GetAuthenticatedUser()
        {
            return GetAuthenticatedUser(new GetAuthenticatedUserParameters());
        }

        public async Task<IAuthenticatedUser> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters)
        {
            var requestResult = await _usersRequester.GetAuthenticatedUser(parameters).ConfigureAwait(false);
            return _client.Factories.CreateAuthenticatedUser(requestResult?.DataTransferObject);
        }

        #endregion

        #region GetUser

        public Task<IUser> GetUser(long userId)
        {
            return GetUser(new UserIdentifier(userId));
        }

        public Task<IUser> GetUser(string username)
        {
            return GetUser(new UserIdentifier(username));
        }

        public Task<IUser> GetUser(IUserIdentifier userIdentifier)
        {
            return GetUser(new GetUserParameters(userIdentifier));
        }

        public async Task<IUser> GetUser(IGetUserParameters parameters)
        {
            var requestResult = await _usersRequester.GetUser(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(requestResult?.DataTransferObject);
        }

        #endregion

        #region GetUsers

        public Task<IUser[]> GetUsers(IEnumerable<long> userIds)
        {
            var userIdentifiers = userIds.Select(x => new UserIdentifier(x));
            return GetUsers(userIdentifiers);
        }

        public Task<IUser[]> GetUsers(IEnumerable<string> usernames)
        {
            var userIdentifiers = usernames.Select(x => new UserIdentifier(x));
            return GetUsers(userIdentifiers);
        }

        public Task<IUser[]> GetUsers(IEnumerable<IUserIdentifier> userIdentifiers)
        {
            return GetUsers(new GetUsersParameters(userIdentifiers.ToArray()));
        }

        public async Task<IUser[]> GetUsers(IGetUsersParameters parameters)
        {
            if (parameters?.Users.Length == 0)
            {
                return new IUser[0];
            }

            var requestResult = await _usersRequester.GetUsers(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUsers(requestResult?.DataTransferObject);
        }

        #endregion

        #region Relationship Between Users

        public Task<IRelationshipDetails> GetRelationshipBetween(long sourceUserId, long targetUserId)
        {
            return GetRelationshipBetween(new GetRelationshipBetweenParameters(sourceUserId, targetUserId));
        }

        public Task<IRelationshipDetails> GetRelationshipBetween(long sourceUserId, string targetUsername)
        {
            return GetRelationshipBetween(new GetRelationshipBetweenParameters(sourceUserId, targetUsername));
        }

        public Task<IRelationshipDetails> GetRelationshipBetween(long sourceUserId, IUserIdentifier targetUser)
        {
            return GetRelationshipBetween(new GetRelationshipBetweenParameters(sourceUserId, targetUser));
        }

        public Task<IRelationshipDetails> GetRelationshipBetween(string sourceUsername, long targetUserId)
        {
            return GetRelationshipBetween(new GetRelationshipBetweenParameters(sourceUsername, targetUserId));
        }

        public Task<IRelationshipDetails> GetRelationshipBetween(string sourceUsername, string targetUsername)
        {
            return GetRelationshipBetween(new GetRelationshipBetweenParameters(sourceUsername, targetUsername));
        }

        public Task<IRelationshipDetails> GetRelationshipBetween(string sourceUsername, IUserIdentifier targetUser)
        {
            return GetRelationshipBetween(new GetRelationshipBetweenParameters(sourceUsername, targetUser));
        }

        public Task<IRelationshipDetails> GetRelationshipBetween(IUserIdentifier sourceUser, long targetUserId)
        {
            return GetRelationshipBetween(new GetRelationshipBetweenParameters(sourceUser, targetUserId));
        }

        public Task<IRelationshipDetails> GetRelationshipBetween(IUserIdentifier sourceUser, string targetUsername)
        {
            return GetRelationshipBetween(new GetRelationshipBetweenParameters(sourceUser, targetUsername));
        }

        public Task<IRelationshipDetails> GetRelationshipBetween(IUserIdentifier sourceUser, IUserIdentifier targetUser)
        {
            return GetRelationshipBetween(new GetRelationshipBetweenParameters(sourceUser, targetUser));
        }

        public async Task<IRelationshipDetails> GetRelationshipBetween(IGetRelationshipBetweenParameters parameters)
        {
            var relationshipTwitterResult = await _usersRequester.GetRelationshipBetween(parameters).ConfigureAwait(false);
            return _client.Factories.CreateRelationshipDetails(relationshipTwitterResult?.DataTransferObject);
        }

        #endregion

        #region GetFriends

        public ITwitterIterator<long> GetFriendIds(string username)
        {
            var parameters = new GetFriendIdsParameters(username);
            return GetFriendIds(parameters);
        }

        public ITwitterIterator<long> GetFriendIds(long userId)
        {
            var parameters = new GetFriendIdsParameters(userId);
            return GetFriendIds(parameters);
        }

        public ITwitterIterator<long> GetFriendIds(IUserIdentifier userIdentifier)
        {
            var parameters = new GetFriendIdsParameters(userIdentifier);
            return GetFriendIds(parameters);
        }

        public ITwitterIterator<long> GetFriendIds(IGetFriendIdsParameters parameters)
        {
            var twitterResultIterator = _usersRequester.GetFriendIdsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(twitterResultIterator, dto => dto.DataTransferObject.Ids);
        }

        public IMultiLevelCursorIterator<long, IUser> GetFriends(IGetFriendsParameters parameters)
        {
            var friendsPageIterator = _usersRequester.GetFriendIdsIterator(parameters);

            var maxPageSize = parameters.GetUsersPageSize;
            if (maxPageSize > _client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxPageSize, nameof(_client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE),
                    "page size");
            }

            return _multiLevelCursorIteratorFactory.CreateUserMultiLevelIterator(_client, friendsPageIterator, maxPageSize);
        }

        #endregion

        #region GetFollowers

        public ITwitterIterator<long> GetFollowerIds(string username)
        {
            var parameters = new GetFollowerIdsParameters(username);
            return GetFollowerIds(parameters);
        }

        public ITwitterIterator<long> GetFollowerIds(long userId)
        {
            var parameters = new GetFollowerIdsParameters(userId);
            return GetFollowerIds(parameters);
        }

        public ITwitterIterator<long> GetFollowerIds(IUserIdentifier userIdentifier)
        {
            var parameters = new GetFollowerIdsParameters(userIdentifier);
            return GetFollowerIds(parameters);
        }

        public ITwitterIterator<long> GetFollowerIds(IGetFollowerIdsParameters parameters)
        {
            var followerIdsPageIterator = _usersRequester.GetFollowerIdsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(followerIdsPageIterator, dto => dto.DataTransferObject.Ids);
        }

        public IMultiLevelCursorIterator<long, IUser> GetFollowers(IGetFollowersParameters parameters)
        {
            var followerPageIterator = _usersRequester.GetFollowerIdsIterator(parameters);

            var maxPageSize = parameters.GetUsersPageSize;
            if (maxPageSize > _client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxPageSize, nameof(_client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE),
                    "page size");
            }

            return _multiLevelCursorIteratorFactory.CreateUserMultiLevelIterator(_client, followerPageIterator, maxPageSize);
        }

        #endregion

        #region Block / Unblock

        public Task BlockUser(long userId)
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
            await _usersRequester.BlockUser(parameters).ConfigureAwait(false);
        }

        public Task UnblockUser(long userId)
        {
            return UnblockUser(new UnblockUserParameters(userId));
        }

        public Task UnblockUser(string username)
        {
            return UnblockUser(new UnblockUserParameters(username));
        }

        public Task UnblockUser(IUserIdentifier user)
        {
            return UnblockUser(new UnblockUserParameters(user));
        }

        public async Task UnblockUser(IUnblockUserParameters parameters)
        {
            await _usersRequester.UnblockUser(parameters).ConfigureAwait(false);
        }

        public Task ReportUserForSpam(long userId)
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
            await _usersRequester.ReportUserForSpam(parameters).ConfigureAwait(false);
        }

        public ITwitterIterator<long> GetBlockedUserIds()
        {
            return GetBlockedUserIds(new GetBlockedUserIdsParameters());
        }

        public ITwitterIterator<long> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetBlockedUserIdsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(twitterCursorResult, dto => dto.DataTransferObject.Ids);
        }

        public ITwitterIterator<IUser> GetBlockedUsers()
        {
            return GetBlockedUsers(new GetBlockedUsersParameters());
        }

        public ITwitterIterator<IUser> GetBlockedUsers(IGetBlockedUsersParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetBlockedUsersIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IUserCursorQueryResultDTO>, IUser>(twitterCursorResult, pageResult =>
            {
                var userDTOs = pageResult.DataTransferObject.Users;
                return _client.Factories.CreateUsers(userDTOs);
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
            await _usersRequester.FollowUser(parameters).ConfigureAwait(false);
        }

        public Task UnfollowUser(long userId)
        {
            return UnfollowUser(new UnfollowUserParameters(userId));
        }

        public Task UnfollowUser(string username)
        {
            return UnfollowUser(new UnfollowUserParameters(username));
        }

        public Task UnfollowUser(IUserIdentifier user)
        {
            return UnfollowUser(new UnfollowUserParameters(user));
        }

        public async Task UnfollowUser(IUnfollowUserParameters parameters)
        {
            await _usersRequester.UnfollowUser(parameters).ConfigureAwait(false);
        }

        #endregion

        #region Update Friendship

        public async Task UpdateRelationship(IUpdateRelationshipParameters parameters)
        {
            await _usersRequester.UpdateRelationship(parameters).ConfigureAwait(false);
        }

        #endregion

        #region Pending Followers Requests

        public ITwitterIterator<long> GetUserIdsRequestingFriendship()
        {
            return GetUserIdsRequestingFriendship(new GetUserIdsRequestingFriendshipParameters());
        }

        public ITwitterIterator<long> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var iterator = _usersRequester.GetUserIdsRequestingFriendshipIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship()
        {
            return GetUsersRequestingFriendship(new GetUsersRequestingFriendshipParameters());
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship(IGetUsersRequestingFriendshipParameters parameters)
        {
            var iterator = _usersRequester.GetUserIdsRequestingFriendshipIterator(parameters);

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
            var iterator = _usersRequester.GetUserIdsYouRequestedToFollowIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow()
        {
            return GetUsersYouRequestedToFollow(new GetUsersYouRequestedToFollowParameters());
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow(IGetUsersYouRequestedToFollowParameters parameters)
        {
            var iterator = _usersRequester.GetUserIdsYouRequestedToFollowIterator(parameters);

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
            var twitterResult = await _usersRequester.GetRelationshipsWith(parameters).ConfigureAwait(false);
            var relationshipsWith = _client.Factories.CreateRelationshipStates(twitterResult?.DataTransferObject);

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
            var twitterResult = await _usersRequester.GetUserIdsWhoseRetweetsAreMuted(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public ITwitterIterator<long> GetMutedUserIds()
        {
            return GetMutedUserIds(new GetMutedUserIdsParameters());
        }

        public ITwitterIterator<long> GetMutedUserIds(IGetMutedUserIdsParameters parameters)
        {
            var iterator = _usersRequester.GetMutedUserIdsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }

        public ITwitterIterator<IUser> GetMutedUsers()
        {
            return GetMutedUsers(new GetMutedUsersParameters());
        }

        public ITwitterIterator<IUser> GetMutedUsers(IGetMutedUsersParameters parameters)
        {
            var iterator = _usersRequester.GetMutedUsersIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IUserCursorQueryResultDTO>, IUser>(iterator, pageResult =>
            {
                var userDTOs = pageResult.DataTransferObject.Users;
                return _client.Factories.CreateUsers(userDTOs);
            });
        }

        public Task MuteUser(long userId)
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
            await _usersRequester.MuteUser(parameters).ConfigureAwait(false);
        }

        public Task UnmuteUser(long userId)
        {
            return UnmuteUser(new UnmuteUserParameters(userId));
        }

        public Task UnmuteUser(string username)
        {
            return UnmuteUser(new UnmuteUserParameters(username));
        }

        public Task UnmuteUser(IUserIdentifier user)
        {
            return UnmuteUser(new UnmuteUserParameters(user));
        }

        public async Task UnmuteUser(IUnmuteUserParameters parameters)
        {
            await _usersRequester.UnmuteUser(parameters).ConfigureAwait(false);
        }

        #endregion

        #region Profile Image

        public Task<System.IO.Stream> GetProfileImageStream(string url)
        {
            return GetProfileImageStream(new GetProfileImageParameters(url));
        }

        public Task<System.IO.Stream> GetProfileImageStream(IUser user)
        {
            return GetProfileImageStream(new GetProfileImageParameters(user));
        }

        public Task<System.IO.Stream> GetProfileImageStream(IUserDTO user)
        {
            return GetProfileImageStream(new GetProfileImageParameters(user));
        }

        public Task<System.IO.Stream> GetProfileImageStream(IGetProfileImageParameters parameters)
        {
            return _usersRequester.GetProfileImageStream(parameters);
        }

        #endregion
    }
}