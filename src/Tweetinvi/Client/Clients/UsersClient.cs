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

        public Task<IAuthenticatedUser> GetAuthenticatedUserAsync()
        {
            return GetAuthenticatedUserAsync(new GetAuthenticatedUserParameters());
        }

        public async Task<IAuthenticatedUser> GetAuthenticatedUserAsync(IGetAuthenticatedUserParameters parameters)
        {
            var requestResult = await _usersRequester.GetAuthenticatedUserAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateAuthenticatedUser(requestResult?.Model);
        }

        #endregion

        #region GetUser

        public Task<IUser> GetUserAsync(long userId)
        {
            return GetUserAsync(new UserIdentifier(userId));
        }

        public Task<IUser> GetUserAsync(string username)
        {
            return GetUserAsync(new UserIdentifier(username));
        }

        public Task<IUser> GetUserAsync(IUserIdentifier userIdentifier)
        {
            return GetUserAsync(new GetUserParameters(userIdentifier));
        }

        public async Task<IUser> GetUserAsync(IGetUserParameters parameters)
        {
            var requestResult = await _usersRequester.GetUserAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(requestResult?.Model);
        }

        #endregion

        #region GetUsers

        public Task<IUser[]> GetUsersAsync(IEnumerable<long> userIds)
        {
            var userIdentifiers = userIds.Select(x => new UserIdentifier(x));
            return GetUsersAsync(userIdentifiers);
        }

        public Task<IUser[]> GetUsersAsync(IEnumerable<string> usernames)
        {
            var userIdentifiers = usernames.Select(x => new UserIdentifier(x));
            return GetUsersAsync(userIdentifiers);
        }

        public Task<IUser[]> GetUsersAsync(IEnumerable<IUserIdentifier> userIdentifiers)
        {
            return GetUsersAsync(new GetUsersParameters(userIdentifiers.ToArray()));
        }

        public async Task<IUser[]> GetUsersAsync(IGetUsersParameters parameters)
        {
            if (parameters.Users == null || parameters.Users.Length == 0)
            {
                return new IUser[0];
            }

            var requestResult = await _usersRequester.GetUsersAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUsers(requestResult?.Model);
        }

        #endregion

        #region Relationship Between Users

        public Task<IRelationshipDetails> GetRelationshipBetweenAsync(long sourceUserId, long targetUserId)
        {
            return GetRelationshipBetweenAsync(new GetRelationshipBetweenParameters(sourceUserId, targetUserId));
        }

        public Task<IRelationshipDetails> GetRelationshipBetweenAsync(long sourceUserId, string targetUsername)
        {
            return GetRelationshipBetweenAsync(new GetRelationshipBetweenParameters(sourceUserId, targetUsername));
        }

        public Task<IRelationshipDetails> GetRelationshipBetweenAsync(long sourceUserId, IUserIdentifier targetUser)
        {
            return GetRelationshipBetweenAsync(new GetRelationshipBetweenParameters(sourceUserId, targetUser));
        }

        public Task<IRelationshipDetails> GetRelationshipBetweenAsync(string sourceUsername, long targetUserId)
        {
            return GetRelationshipBetweenAsync(new GetRelationshipBetweenParameters(sourceUsername, targetUserId));
        }

        public Task<IRelationshipDetails> GetRelationshipBetweenAsync(string sourceUsername, string targetUsername)
        {
            return GetRelationshipBetweenAsync(new GetRelationshipBetweenParameters(sourceUsername, targetUsername));
        }

        public Task<IRelationshipDetails> GetRelationshipBetweenAsync(string sourceUsername, IUserIdentifier targetUser)
        {
            return GetRelationshipBetweenAsync(new GetRelationshipBetweenParameters(sourceUsername, targetUser));
        }

        public Task<IRelationshipDetails> GetRelationshipBetweenAsync(IUserIdentifier sourceUser, long targetUserId)
        {
            return GetRelationshipBetweenAsync(new GetRelationshipBetweenParameters(sourceUser, targetUserId));
        }

        public Task<IRelationshipDetails> GetRelationshipBetweenAsync(IUserIdentifier sourceUser, string targetUsername)
        {
            return GetRelationshipBetweenAsync(new GetRelationshipBetweenParameters(sourceUser, targetUsername));
        }

        public Task<IRelationshipDetails> GetRelationshipBetweenAsync(IUserIdentifier sourceUser, IUserIdentifier targetUser)
        {
            return GetRelationshipBetweenAsync(new GetRelationshipBetweenParameters(sourceUser, targetUser));
        }

        public async Task<IRelationshipDetails> GetRelationshipBetweenAsync(IGetRelationshipBetweenParameters parameters)
        {
            var relationshipTwitterResult = await _usersRequester.GetRelationshipBetweenAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateRelationshipDetails(relationshipTwitterResult?.Model);
        }

        #endregion

        #region GetFriends

        public Task<long[]> GetFriendIdsAsync(string username)
        {
            return GetFriendIdsAsync(new GetFriendIdsParameters(username));
        }

        public Task<long[]> GetFriendIdsAsync(long userId)
        {
            return GetFriendIdsAsync(new GetFriendIdsParameters(userId));
        }

        public Task<long[]> GetFriendIdsAsync(IUserIdentifier user)
        {
            return GetFriendIdsAsync(new GetFriendIdsParameters(user));
        }

        public async Task<long[]> GetFriendIdsAsync(IGetFriendIdsParameters parameters)
        {
            var iterator = GetFriendIdsIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
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
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(twitterResultIterator, dto => dto.Model.Ids);
        }

        public Task<IUser[]> GetFriendsAsync(long userId)
        {
            return GetFriendsAsync(new GetFriendsParameters(userId));
        }

        public Task<IUser[]> GetFriendsAsync(string username)
        {
            return GetFriendsAsync(new GetFriendsParameters(username));
        }

        public Task<IUser[]> GetFriendsAsync(IUserIdentifier user)
        {
            return GetFriendsAsync(new GetFriendsParameters(user));
        }

        public async Task<IUser[]> GetFriendsAsync(IGetFriendsParameters parameters)
        {
            var iterator = GetFriendsIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
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

        public Task<long[]> GetFollowerIdsAsync(long userId)
        {
            return GetFollowerIdsAsync(new GetFollowerIdsParameters(userId));
        }

        public Task<long[]> GetFollowerIdsAsync(string username)
        {
            return GetFollowerIdsAsync(new GetFollowerIdsParameters(username));
        }

        public Task<long[]> GetFollowerIdsAsync(IUserIdentifier user)
        {
            return GetFollowerIdsAsync(new GetFollowerIdsParameters(user));
        }

        public async Task<long[]> GetFollowerIdsAsync(IGetFollowerIdsParameters parameters)
        {
            var iterator = GetFollowerIdsIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
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
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(followerIdsPageIterator, dto => dto.Model.Ids);
        }

        public Task<IUser[]> GetFollowersAsync(long userId)
        {
            return GetFollowersAsync(new GetFollowersParameters(userId));
        }

        public Task<IUser[]> GetFollowersAsync(string username)
        {
            return GetFollowersAsync(new GetFollowersParameters(username));
        }

        public Task<IUser[]> GetFollowersAsync(IUserIdentifier user)
        {
            return GetFollowersAsync(new GetFollowersParameters(user));
        }

        public async Task<IUser[]> GetFollowersAsync(IGetFollowersParameters parameters)
        {
            var iterator = GetFollowersIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
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

        public Task<IUser> BlockUserAsync(long userId)
        {
            return BlockUserAsync(new BlockUserParameters(userId));
        }

        public Task<IUser> BlockUserAsync(string username)
        {
            return BlockUserAsync(new BlockUserParameters(username));
        }

        public Task<IUser> BlockUserAsync(IUserIdentifier user)
        {
            return BlockUserAsync(new BlockUserParameters(user));
        }

        public async Task<IUser> BlockUserAsync(IBlockUserParameters parameters)
        {
            var twitterResult = await _usersRequester.BlockUserAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.Model);
        }

        public Task<IUser> UnblockUserAsync(long userId)
        {
            return UnblockUserAsync(new UnblockUserParameters(userId));
        }

        public Task<IUser> UnblockUserAsync(string username)
        {
            return UnblockUserAsync(new UnblockUserParameters(username));
        }

        public Task<IUser> UnblockUserAsync(IUserIdentifier user)
        {
            return UnblockUserAsync(new UnblockUserParameters(user));
        }

        public async Task<IUser> UnblockUserAsync(IUnblockUserParameters parameters)
        {
            var twitterResult = await _usersRequester.UnblockUserAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.Model);
        }

        public Task<IUser> ReportUserForSpamAsync(long userId)
        {
            return ReportUserForSpamAsync(new ReportUserForSpamParameters(userId));
        }

        public Task<IUser> ReportUserForSpamAsync(string username)
        {
            return ReportUserForSpamAsync(new ReportUserForSpamParameters(username));
        }

        public Task<IUser> ReportUserForSpamAsync(IUserIdentifier user)
        {
            return ReportUserForSpamAsync(new ReportUserForSpamParameters(user));
        }

        public async Task<IUser> ReportUserForSpamAsync(IReportUserForSpamParameters parameters)
        {
            var twitterResult = await _usersRequester.ReportUserForSpamAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.Model);
        }

        public Task<long[]> GetBlockedUserIdsAsync()
        {
            return GetBlockedUserIdsAsync(new GetBlockedUserIdsParameters());
        }

        public async Task<long[]> GetBlockedUserIdsAsync(IGetBlockedUserIdsParameters parameters)
        {
            var iterator = GetBlockedUserIdsIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<long> GetBlockedUserIdsIterator()
        {
            return GetBlockedUserIdsIterator(new GetBlockedUserIdsParameters());
        }

        public ITwitterIterator<long> GetBlockedUserIdsIterator(IGetBlockedUserIdsParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetBlockedUserIdsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(twitterCursorResult, dto => dto.Model.Ids);
        }

        public Task<IUser[]> GetBlockedUsersAsync()
        {
            return GetBlockedUsersAsync(new GetBlockedUsersParameters());
        }

        public async Task<IUser[]> GetBlockedUsersAsync(IGetBlockedUsersParameters parameters)
        {
            var iterator = GetBlockedUsersIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
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
                var userDTOs = pageResult.Model.Users;
                return _client.Factories.CreateUsers(userDTOs);
            });
        }

        #endregion

        #region Follow / Unfollow

        public Task<IUser> FollowUserAsync(long userId)
        {
            return FollowUserAsync(new FollowUserParameters(userId));
        }

        public Task<IUser> FollowUserAsync(string username)
        {
            return FollowUserAsync(new FollowUserParameters(username));
        }

        public Task<IUser> FollowUserAsync(IUserIdentifier user)
        {
            return FollowUserAsync(new FollowUserParameters(user));
        }

        public async Task<IUser> FollowUserAsync(IFollowUserParameters parameters)
        {
            var twitterResult = await _usersRequester.FollowUserAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.Model);
        }

        public Task<IUser> UnfollowUserAsync(long userId)
        {
            return UnfollowUserAsync(new UnfollowUserParameters(userId));
        }

        public Task<IUser> UnfollowUserAsync(string username)
        {
            return UnfollowUserAsync(new UnfollowUserParameters(username));
        }

        public Task<IUser> UnfollowUserAsync(IUserIdentifier user)
        {
            return UnfollowUserAsync(new UnfollowUserParameters(user));
        }

        public async Task<IUser> UnfollowUserAsync(IUnfollowUserParameters parameters)
        {
            var twitterResult = await _usersRequester.UnfollowUserAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.Model);
        }

        #endregion

        #region Update Friendship

        public async Task UpdateRelationshipAsync(IUpdateRelationshipParameters parameters)
        {
            await _usersRequester.UpdateRelationshipAsync(parameters).ConfigureAwait(false);
        }

        #endregion

        #region Pending Followers Requests

        public Task<long[]> GetUserIdsRequestingFriendshipAsync()
        {
            return GetUserIdsRequestingFriendshipAsync(new GetUserIdsRequestingFriendshipParameters());
        }

        public async Task<long[]> GetUserIdsRequestingFriendshipAsync(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var iterator = GetUserIdsRequestingFriendshipIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<long> GetUserIdsRequestingFriendshipIterator()
        {
            return GetUserIdsRequestingFriendshipIterator(new GetUserIdsRequestingFriendshipParameters());
        }

        public ITwitterIterator<long> GetUserIdsRequestingFriendshipIterator(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var iterator = _usersRequester.GetUserIdsRequestingFriendshipIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.Model.Ids);
        }

        public Task<IUser[]> GetUsersRequestingFriendshipAsync()
        {
            return GetUsersRequestingFriendshipAsync(new GetUsersRequestingFriendshipParameters());
        }

        public async Task<IUser[]> GetUsersRequestingFriendshipAsync(IGetUsersRequestingFriendshipParameters parameters)
        {
            var iterator = GetUsersRequestingFriendshipIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
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

        public Task<long[]> GetUserIdsYouRequestedToFollowAsync()
        {
            return GetUserIdsYouRequestedToFollowAsync(new GetUserIdsYouRequestedToFollowParameters());
        }

        public async Task<long[]> GetUserIdsYouRequestedToFollowAsync(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            var iterator = GetUserIdsYouRequestedToFollowIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<long> GetUserIdsYouRequestedToFollowIterator()
        {
            return GetUserIdsYouRequestedToFollowIterator(new GetUserIdsYouRequestedToFollowParameters());
        }

        public ITwitterIterator<long> GetUserIdsYouRequestedToFollowIterator(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            var iterator = _usersRequester.GetUserIdsYouRequestedToFollowIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.Model.Ids);
        }

        public Task<IUser[]> GetUsersYouRequestedToFollowAsync()
        {
            return GetUsersYouRequestedToFollowAsync(new GetUsersYouRequestedToFollowParameters());
        }

        public async Task<IUser[]> GetUsersYouRequestedToFollowAsync(IGetUsersYouRequestedToFollowParameters parameters)
        {
            var iterator = GetUsersYouRequestedToFollowIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
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

        public Task<IUserDictionary<IRelationshipState>> GetRelationshipsWithAsync(long[] userIds)
        {
            return GetRelationshipsWithAsync(new GetRelationshipsWithParameters(userIds));
        }

        public Task<IUserDictionary<IRelationshipState>> GetRelationshipsWithAsync(string[] usernames)
        {
            return GetRelationshipsWithAsync(new GetRelationshipsWithParameters(usernames));
        }

        public Task<IUserDictionary<IRelationshipState>> GetRelationshipsWithAsync(IUserIdentifier[] users)
        {
            return GetRelationshipsWithAsync(new GetRelationshipsWithParameters(users));
        }

        public Task<IUserDictionary<IRelationshipState>> GetRelationshipsWithAsync(IUser[] users)
        {
            return GetRelationshipsWithAsync(new GetRelationshipsWithParameters(users));
        }

        public async Task<IUserDictionary<IRelationshipState>> GetRelationshipsWithAsync(IGetRelationshipsWithParameters parameters)
        {
            if (parameters.Users == null || parameters.Users.Length == 0)
            {
                return new UserDictionary<IRelationshipState>();
            }

            var twitterResult = await _usersRequester.GetRelationshipsWithAsync(parameters).ConfigureAwait(false);
            var relationshipsWith = _client.Factories.CreateRelationshipStates(twitterResult?.Model);

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

        public Task<long[]> GetUserIdsWhoseRetweetsAreMutedAsync()
        {
            return GetUserIdsWhoseRetweetsAreMutedAsync(new GetUserIdsWhoseRetweetsAreMutedParameters());
        }

        public async Task<long[]> GetUserIdsWhoseRetweetsAreMutedAsync(IGetUserIdsWhoseRetweetsAreMutedParameters parameters)
        {
            var twitterResult = await _usersRequester.GetUserIdsWhoseRetweetsAreMutedAsync(parameters).ConfigureAwait(false);
            return twitterResult?.Model;
        }

        public Task<long[]> GetMutedUserIdsAsync()
        {
            return GetMutedUserIdsAsync(new GetMutedUserIdsParameters());
        }

        public async Task<long[]> GetMutedUserIdsAsync(IGetMutedUserIdsParameters parameters)
        {
            var iterator = GetMutedUserIdsIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
        }

        public ITwitterIterator<long> GetMutedUserIdsIterator()
        {
            return GetMutedUserIdsIterator(new GetMutedUserIdsParameters());
        }

        public ITwitterIterator<long> GetMutedUserIdsIterator(IGetMutedUserIdsParameters parameters)
        {
            var iterator = _usersRequester.GetMutedUserIdsIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.Model.Ids);
        }

        public Task<IUser[]> GetMutedUsersAsync()
        {
            return GetMutedUsersAsync(new GetMutedUsersParameters());
        }

        public async Task<IUser[]> GetMutedUsersAsync(IGetMutedUsersParameters parameters)
        {
            var iterator = GetMutedUsersIterator(parameters);
            return (await iterator.NextPageAsync().ConfigureAwait(false)).ToArray();
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
                var userDTOs = pageResult.Model.Users;
                return _client.Factories.CreateUsers(userDTOs);
            });
        }

        public Task<IUser> MuteUserAsync(long userId)
        {
            return MuteUserAsync(new MuteUserParameters(userId));
        }

        public Task<IUser> MuteUserAsync(string username)
        {
            return MuteUserAsync(new MuteUserParameters(username));
        }

        public Task<IUser> MuteUserAsync(IUserIdentifier user)
        {
            return MuteUserAsync(new MuteUserParameters(user));
        }

        public async Task<IUser> MuteUserAsync(IMuteUserParameters parameters)
        {
            var twitterResult = await _usersRequester.MuteUserAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.Model);
        }

        public Task<IUser> UnmuteUserAsync(long userId)
        {
            return UnmuteUserAsync(new UnmuteUserParameters(userId));
        }

        public Task<IUser> UnmuteUserAsync(string username)
        {
            return UnmuteUserAsync(new UnmuteUserParameters(username));
        }

        public Task<IUser> UnmuteUserAsync(IUserIdentifier user)
        {
            return UnmuteUserAsync(new UnmuteUserParameters(user));
        }

        public async Task<IUser> UnmuteUserAsync(IUnmuteUserParameters parameters)
        {
            var twitterResult = await _usersRequester.UnmuteUserAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateUser(twitterResult?.Model);
        }

        #endregion

        #region Profile Image

        public Task<System.IO.Stream> GetProfileImageStreamAsync(string url)
        {
            return GetProfileImageStreamAsync(new GetProfileImageParameters(url));
        }

        public Task<System.IO.Stream> GetProfileImageStreamAsync(IUser user)
        {
            return GetProfileImageStreamAsync(new GetProfileImageParameters(user));
        }

        public Task<System.IO.Stream> GetProfileImageStreamAsync(IUserDTO user)
        {
            return GetProfileImageStreamAsync(new GetProfileImageParameters(user));
        }

        public Task<System.IO.Stream> GetProfileImageStreamAsync(IGetProfileImageParameters parameters)
        {
            return _usersRequester.GetProfileImageStream(parameters);
        }

        #endregion
    }
}