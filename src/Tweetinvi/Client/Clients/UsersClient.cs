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
            if (parameters.Users == null || parameters.Users.Length == 0)
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

        public Task<long[]> GetFriendIds(string username)
        {
            return GetFriendIds(new GetFriendIdsParameters(username));
        }

        public Task<long[]> GetFriendIds(long userId)
        {
            return GetFriendIds(new GetFriendIdsParameters(userId));
        }

        public Task<long[]> GetFriendIds(IUserIdentifier user)
        {
            return GetFriendIds(new GetFriendIdsParameters(user));
        }

        public async Task<long[]> GetFriendIds(IGetFriendIdsParameters parameters)
        {
            var iterator = GetFriendIdsIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<long> GetFriendIdsIterator(string username)
        {
            var parameters = new GetFriendIdsParameters(username);
            return GetFriendIdsIterator(parameters);
        }

        public ITwitterIterator<long> GetFriendIdsIterator(long userId)
        {
            var parameters = new GetFriendIdsParameters(userId);
            return GetFriendIdsIterator(parameters);
        }

        public ITwitterIterator<long> GetFriendIdsIterator(IUserIdentifier userIdentifier)
        {
            var parameters = new GetFriendIdsParameters(userIdentifier);
            return GetFriendIdsIterator(parameters);
        }

        public ITwitterIterator<long> GetFriendIdsIterator(IGetFriendIdsParameters parameters)
        {
            var twitterResultIterator = _usersRequester.GetFriendIdsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(twitterResultIterator, dto => dto.DataTransferObject.Ids);
        }

        public Task<IUser[]> GetFriends(long userId)
        {
            return GetFriends(new GetFriendsParameters(userId));
        }

        public Task<IUser[]> GetFriends(string username)
        {
            return GetFriends(new GetFriendsParameters(username));
        }

        public Task<IUser[]> GetFriends(IUserIdentifier user)
        {
            return GetFriends(new GetFriendsParameters(user));
        }

        public async Task<IUser[]> GetFriends(IGetFriendsParameters parameters)
        {
            var iterator = GetFriendsIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
        }

        public IMultiLevelCursorIterator<long, IUser> GetFriendsIterator(IGetFriendsParameters parameters)
        {
            var friendsPageIterator = _usersRequester.GetFriendIdsIterator(parameters);

            var maxPageSize = _client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE;
            if (parameters.GetUsersPageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxPageSize, nameof(_client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE),
                    "page size");
            }

            return _multiLevelCursorIteratorFactory.CreateUserMultiLevelIterator(_client, friendsPageIterator, maxPageSize);
        }

        #endregion

        #region GetFollowers

        public Task<long[]> GetFollowerIds(long userId)
        {
            return GetFollowerIds(new GetFollowerIdsParameters(userId));
        }

        public Task<long[]> GetFollowerIds(string username)
        {
            return GetFollowerIds(new GetFollowerIdsParameters(username));
        }

        public Task<long[]> GetFollowerIds(IUserIdentifier user)
        {
            return GetFollowerIds(new GetFollowerIdsParameters(user));
        }

        public async Task<long[]> GetFollowerIds(IGetFollowerIdsParameters parameters)
        {
            var iterator = GetFollowerIdsIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<long> GetFollowerIdsIterator(string username)
        {
            var parameters = new GetFollowerIdsParameters(username);
            return GetFollowerIdsIterator(parameters);
        }

        public ITwitterIterator<long> GetFollowerIdsIterator(long userId)
        {
            var parameters = new GetFollowerIdsParameters(userId);
            return GetFollowerIdsIterator(parameters);
        }

        public ITwitterIterator<long> GetFollowerIdsIterator(IUserIdentifier userIdentifier)
        {
            var parameters = new GetFollowerIdsParameters(userIdentifier);
            return GetFollowerIdsIterator(parameters);
        }

        public ITwitterIterator<long> GetFollowerIdsIterator(IGetFollowerIdsParameters parameters)
        {
            var followerIdsPageIterator = _usersRequester.GetFollowerIdsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(followerIdsPageIterator, dto => dto.DataTransferObject.Ids);
        }

        public Task<IUser[]> GetFollowers(long userId)
        {
            return GetFollowers(new GetFollowersParameters(userId));
        }

        public Task<IUser[]> GetFollowers(string username)
        {
            return GetFollowers(new GetFollowersParameters(username));
        }

        public Task<IUser[]> GetFollowers(IUserIdentifier user)
        {
            return GetFollowers(new GetFollowersParameters(user));
        }

        public async Task<IUser[]> GetFollowers(IGetFollowersParameters parameters)
        {
            var iterator = GetFollowersIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
        }

        public IMultiLevelCursorIterator<long, IUser> GetFollowersIterator(IGetFollowersParameters parameters)
        {
            var followerPageIterator = _usersRequester.GetFollowerIdsIterator(parameters);

            var maxPageSize = _client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE;
            if (parameters.GetUsersPageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxPageSize, nameof(_client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE),
                    "page size");
            }

            return _multiLevelCursorIteratorFactory.CreateUserMultiLevelIterator(_client, followerPageIterator, maxPageSize);
        }

        #endregion

        #region Block / Unblock

        public Task<IUser> BlockUser(long userId)
        {
            return BlockUser(new BlockUserParameters(userId));
        }

        public Task<IUser> BlockUser(string username)
        {
            return BlockUser(new BlockUserParameters(username));
        }

        public Task<IUser> BlockUser(IUserIdentifier user)
        {
            return BlockUser(new BlockUserParameters(user));
        }

        public async Task<IUser> BlockUser(IBlockUserParameters parameters)
        {
            var twitterResult = await _usersRequester.BlockUser(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.DataTransferObject);
        }

        public Task<IUser> UnblockUser(long userId)
        {
            return UnblockUser(new UnblockUserParameters(userId));
        }

        public Task<IUser> UnblockUser(string username)
        {
            return UnblockUser(new UnblockUserParameters(username));
        }

        public Task<IUser> UnblockUser(IUserIdentifier user)
        {
            return UnblockUser(new UnblockUserParameters(user));
        }

        public async Task<IUser> UnblockUser(IUnblockUserParameters parameters)
        {
            var twitterResult = await _usersRequester.UnblockUser(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.DataTransferObject);
        }

        public Task<IUser> ReportUserForSpam(long userId)
        {
            return ReportUserForSpam(new ReportUserForSpamParameters(userId));
        }

        public Task<IUser> ReportUserForSpam(string username)
        {
            return ReportUserForSpam(new ReportUserForSpamParameters(username));
        }

        public Task<IUser> ReportUserForSpam(IUserIdentifier user)
        {
            return ReportUserForSpam(new ReportUserForSpamParameters(user));
        }

        public async Task<IUser> ReportUserForSpam(IReportUserForSpamParameters parameters)
        {
            var twitterResult = await _usersRequester.ReportUserForSpam(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.DataTransferObject);
        }

        public Task<long[]> GetBlockedUserIds()
        {
            return GetBlockedUserIds(new GetBlockedUserIdsParameters());
        }

        public async Task<long[]> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters)
        {
            var iterator = GetBlockedUserIdsIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<long> GetBlockedUserIdsIterator()
        {
            return GetBlockedUserIdsIterator(new GetBlockedUserIdsParameters());
        }

        public ITwitterIterator<long> GetBlockedUserIdsIterator(IGetBlockedUserIdsParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetBlockedUserIdsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(twitterCursorResult, dto => dto.DataTransferObject.Ids);
        }

        public Task<IUser[]> GetBlockedUsers()
        {
            return GetBlockedUsers(new GetBlockedUsersParameters());
        }

        public async Task<IUser[]> GetBlockedUsers(IGetBlockedUsersParameters parameters)
        {
            var iterator = GetBlockedUsersIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<IUser> GetBlockedUsersIterator()
        {
            return GetBlockedUsersIterator(new GetBlockedUsersParameters());
        }

        public ITwitterIterator<IUser> GetBlockedUsersIterator(IGetBlockedUsersParameters parameters)
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

        public Task<IUser> FollowUser(long userId)
        {
            return FollowUser(new FollowUserParameters(userId));
        }

        public Task<IUser> FollowUser(string username)
        {
            return FollowUser(new FollowUserParameters(username));
        }

        public Task<IUser> FollowUser(IUserIdentifier user)
        {
            return FollowUser(new FollowUserParameters(user));
        }

        public async Task<IUser> FollowUser(IFollowUserParameters parameters)
        {
            var twitterResult = await _usersRequester.FollowUser(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.DataTransferObject);
        }

        public Task<IUser> UnfollowUser(long userId)
        {
            return UnfollowUser(new UnfollowUserParameters(userId));
        }

        public Task<IUser> UnfollowUser(string username)
        {
            return UnfollowUser(new UnfollowUserParameters(username));
        }

        public Task<IUser> UnfollowUser(IUserIdentifier user)
        {
            return UnfollowUser(new UnfollowUserParameters(user));
        }

        public async Task<IUser> UnfollowUser(IUnfollowUserParameters parameters)
        {
            var twitterResult = await _usersRequester.UnfollowUser(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.DataTransferObject);
        }

        #endregion

        #region Update Friendship

        public async Task UpdateRelationship(IUpdateRelationshipParameters parameters)
        {
            await _usersRequester.UpdateRelationship(parameters).ConfigureAwait(false);
        }

        #endregion

        #region Pending Followers Requests

        public Task<long[]> GetUserIdsRequestingFriendship()
        {
            return GetUserIdsRequestingFriendship(new GetUserIdsRequestingFriendshipParameters());
        }

        public async Task<long[]> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var iterator = GetUserIdsRequestingFriendshipIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<long> GetUserIdsRequestingFriendshipIterator()
        {
            return GetUserIdsRequestingFriendshipIterator(new GetUserIdsRequestingFriendshipParameters());
        }

        public ITwitterIterator<long> GetUserIdsRequestingFriendshipIterator(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var iterator = _usersRequester.GetUserIdsRequestingFriendshipIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }

        public Task<IUser[]> GetUsersRequestingFriendship()
        {
            return GetUsersRequestingFriendship(new GetUsersRequestingFriendshipParameters());
        }

        public async Task<IUser[]> GetUsersRequestingFriendship(IGetUsersRequestingFriendshipParameters parameters)
        {
            var iterator = GetUsersRequestingFriendshipIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendshipIterator()
        {
            return GetUsersRequestingFriendshipIterator(new GetUsersRequestingFriendshipParameters());
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendshipIterator(IGetUsersRequestingFriendshipParameters parameters)
        {
            var iterator = _usersRequester.GetUserIdsRequestingFriendshipIterator(parameters);

            var maxPageSize =_client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE;
            if (parameters.GetUsersPageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxPageSize, nameof(_client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE), "page size");
            }

            return _multiLevelCursorIteratorFactory.CreateUserMultiLevelIterator(_client, iterator, maxPageSize);
        }

        public Task<long[]> GetUserIdsYouRequestedToFollow()
        {
            return GetUserIdsYouRequestedToFollow(new GetUsersYouRequestedToFollowParameters());
        }

        public async Task<long[]> GetUserIdsYouRequestedToFollow(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            var iterator = GetUserIdsYouRequestedToFollowIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<long> GetUserIdsYouRequestedToFollowIterator()
        {
            return GetUserIdsYouRequestedToFollowIterator(new GetUserIdsYouRequestedToFollowParameters());
        }

        public ITwitterIterator<long> GetUserIdsYouRequestedToFollowIterator(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            var iterator = _usersRequester.GetUserIdsYouRequestedToFollowIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }

        public Task<IUser[]> GetUsersYouRequestedToFollow()
        {
            return GetUsersYouRequestedToFollow(new GetUsersYouRequestedToFollowParameters());
        }

        public async Task<IUser[]> GetUsersYouRequestedToFollow(IGetUsersYouRequestedToFollowParameters parameters)
        {
            var iterator = GetUsersYouRequestedToFollowIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollowIterator()
        {
            return GetUsersYouRequestedToFollowIterator(new GetUsersYouRequestedToFollowParameters());
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollowIterator(IGetUsersYouRequestedToFollowParameters parameters)
        {
            var iterator = _usersRequester.GetUserIdsYouRequestedToFollowIterator(parameters);

            var maxPageSize = _client.ClientSettings.Limits.USERS_GET_USERS_MAX_SIZE;
            if (parameters.GetUsersPageSize > maxPageSize)
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
            if (parameters.Users == null || parameters.Users.Length == 0)
            {
                return new UserDictionary<IRelationshipState>();
            }

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

        public Task<long[]> GetMutedUserIds()
        {
            return GetMutedUserIds(new GetMutedUserIdsParameters());
        }

        public async Task<long[]> GetMutedUserIds(IGetMutedUserIdsParameters parameters)
        {
            var iterator = GetMutedUserIdsIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<long> GetMutedUserIdsIterator()
        {
            return GetMutedUserIdsIterator(new GetMutedUserIdsParameters());
        }

        public ITwitterIterator<long> GetMutedUserIdsIterator(IGetMutedUserIdsParameters parameters)
        {
            var iterator = _usersRequester.GetMutedUserIdsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }

        public Task<IUser[]> GetMutedUsers()
        {
            return GetMutedUsers(new GetMutedUsersParameters());
        }

        public async Task<IUser[]> GetMutedUsers(IGetMutedUsersParameters parameters)
        {
            var iterator = GetMutedUsersIterator(parameters);
            return (await iterator.MoveToNextPage().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<IUser> GetMutedUsersIterator()
        {
            return GetMutedUsersIterator(new GetMutedUsersParameters());
        }

        public ITwitterIterator<IUser> GetMutedUsersIterator(IGetMutedUsersParameters parameters)
        {
            var iterator = _usersRequester.GetMutedUsersIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IUserCursorQueryResultDTO>, IUser>(iterator, pageResult =>
            {
                var userDTOs = pageResult.DataTransferObject.Users;
                return _client.Factories.CreateUsers(userDTOs);
            });
        }

        public Task<IUser> MuteUser(long userId)
        {
            return MuteUser(new MuteUserParameters(userId));
        }

        public Task<IUser> MuteUser(string username)
        {
            return MuteUser(new MuteUserParameters(username));
        }

        public Task<IUser> MuteUser(IUserIdentifier user)
        {
            return MuteUser(new MuteUserParameters(user));
        }

        public async Task<IUser> MuteUser(IMuteUserParameters parameters)
        {
            var twitterResult = await _usersRequester.MuteUser(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.DataTransferObject);
        }

        public Task<IUser> UnmuteUser(long userId)
        {
            return UnmuteUser(new UnmuteUserParameters(userId));
        }

        public Task<IUser> UnmuteUser(string username)
        {
            return UnmuteUser(new UnmuteUserParameters(username));
        }

        public Task<IUser> UnmuteUser(IUserIdentifier user)
        {
            return UnmuteUser(new UnmuteUserParameters(user));
        }

        public async Task<IUser> UnmuteUser(IUnmuteUserParameters parameters)
        {
            var twitterResult = await _usersRequester.UnmuteUser(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.DataTransferObject);
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