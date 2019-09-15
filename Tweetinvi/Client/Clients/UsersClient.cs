using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

// ReSharper disable MemberCanBePrivate.Global

namespace Tweetinvi.Client
{
    /// <summary>
    /// A client providing all the methods related with users
    /// </summary>
    public class UsersClient : IUsersClient
    {
        private readonly IUsersRequester _usersRequester;
        private readonly IUserFactory _userFactory;

        public UsersClient(TwitterClient client)
        {
            _usersRequester = client.RequestExecutor.Users;
            _userFactory = TweetinviContainer.Resolve<IUserFactory>();
        }

        public async Task<IAuthenticatedUser> GetAuthenticatedUser()
        {
            var requestResult = await _usersRequester.GetAuthenticatedUser(null);
            return requestResult?.Result;
        }

        public async Task<IAuthenticatedUser> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters)
        {
            var requestResult = await _usersRequester.GetAuthenticatedUser(parameters);
            return requestResult?.Result;
        }

        #region GetUser

        public Task<IUser> GetUser(long? userId)
        {
            return GetUser(new UserIdentifier(userId));
        }

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
            var requestResult = await _usersRequester.GetUser(parameters);
            return requestResult?.Result;
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
            var requestResult = await _usersRequester.GetUsers(parameters).ConfigureAwait(false);
            return requestResult?.Result;
        }

        #endregion

        #region GetFriends

        public ISkippableResultIterator<long> GetFriendIds(string username)
        {
            var parameters = new GetFriendIdsParameters(username);
            return GetFriendIds(parameters);
        }

        public ISkippableResultIterator<long> GetFriendIds(long userId)
        {
            var parameters = new GetFriendIdsParameters(userId);
            return GetFriendIds(parameters);
        }

        public ISkippableResultIterator<long> GetFriendIds(IUserIdentifier userIdentifier)
        {
            var parameters = new GetFriendIdsParameters(userIdentifier);
            return GetFriendIds(parameters);
        }

        public ISkippableResultIterator<long> GetFriendIds(IGetFriendIdsParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetFriendIds(parameters);
            var cursorResult = new SkippableResultIterator<long, IIdsCursorQueryResultDTO>(twitterCursorResult);

            return cursorResult;
        }

        public ICursorResultIterator<IUser> GetFriends(IGetFriendsParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetFriendIds(parameters);

            var friendIdsIterator = new SkippableResultIterator<long, IIdsCursorQueryResultDTO>(twitterCursorResult);
            var userCursorIterator = new MultiLevelCursorResultIterator<IUser, long>(friendIdsIterator, friendIds =>
            {
                return new PageResultIterator<long, IUser>(friendIds, GetUsers, parameters.GetUsersPageSize);
            });
            
            return userCursorIterator;
        }

        #endregion

        #region GetFollowers

        public ISkippableResultIterator<long> GetFollowerIds(string username)
        {
            var parameters = new GetFollowerIdsParameters(username);
            return GetFollowerIds(parameters);
        }

        public ISkippableResultIterator<long> GetFollowerIds(long userId)
        {
            var parameters = new GetFollowerIdsParameters(userId);
            return GetFollowerIds(parameters);
        }

        public ISkippableResultIterator<long> GetFollowerIds(IUserIdentifier userIdentifier)
        {
            var parameters = new GetFollowerIdsParameters(userIdentifier);
            return GetFollowerIds(parameters);
        }

        public ISkippableResultIterator<long> GetFollowerIds(IGetFollowerIdsParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetFollowerIds(parameters);
            var cursorResult = new SkippableResultIterator<long, IIdsCursorQueryResultDTO>(twitterCursorResult);

            return cursorResult;
        }

        public ICursorResultIterator<IUser> GetFollowers(IGetFollowersParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetFollowerIds(parameters);
            
            var followerIdsIterator = new SkippableResultIterator<long, IIdsCursorQueryResultDTO>(twitterCursorResult);
            var userCursorIterator = new MultiLevelCursorResultIterator<IUser, long>(followerIdsIterator, followerIds =>
            {
                return new PageResultIterator<long, IUser>(followerIds, GetUsers, parameters.GetUsersPageSize);
            });

            return userCursorIterator;
        }

        #endregion

        #region Block / Unblock

        public Task<bool> BlockUser(long? userId)
        {
            return BlockUser(new BlockUserParameters(userId));
        }

        public Task<bool> BlockUser(string username)
        {
            return BlockUser(new BlockUserParameters(username));
        }

        public Task<bool> BlockUser(IUserIdentifier user)
        {
            return BlockUser(new BlockUserParameters(user));
        }

        public async Task<bool> BlockUser(IBlockUserParameters parameters)
        {
            var requestResult = await _usersRequester.BlockUser(parameters).ConfigureAwait(false);
            return requestResult?.DataTransferObject != null;
        }

        public Task<bool> UnblockUser(long? userId)
        {
            return UnblockUser(new UnblockUserParameters(userId));
        }

        public Task<bool> UnblockUser(string username)
        {
            return UnblockUser(new UnblockUserParameters(username));
        }

        public Task<bool> UnblockUser(IUserIdentifier user)
        {
            return UnblockUser(new UnblockUserParameters(user));
        }

        public async Task<bool> UnblockUser(IUnblockUserParameters parameters)
        {
            var requestResult = await _usersRequester.UnblockUser(parameters).ConfigureAwait(false);
            return requestResult?.DataTransferObject != null;
        }

        public Task<bool> ReportUserForSpam(long? userId)
        {
            return ReportUserForSpam(new ReportUserForSpamParameters(userId));
        }

        public Task<bool> ReportUserForSpam(string username)
        {
            return ReportUserForSpam(new ReportUserForSpamParameters(username));
        }

        public Task<bool> ReportUserForSpam(IUserIdentifier user)
        {
            return ReportUserForSpam(new ReportUserForSpamParameters(user));
        }

        public async Task<bool> ReportUserForSpam(IReportUserForSpamParameters parameters)
        {
            var requestResult = await _usersRequester.ReportUserForSpam(parameters).ConfigureAwait(false);
            return requestResult?.DataTransferObject != null;
        }

        public ISkippableResultIterator<long> GetBlockedUserIds()
        {
            return GetBlockedUserIds(new GetBlockedUserIdsParameters());
        }

        public ISkippableResultIterator<long> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetBlockedUserIds(parameters);
            var cursorResult = new SkippableResultIterator<long, IIdsCursorQueryResultDTO>(twitterCursorResult);

            return cursorResult;
        }

        public ISkippableResultIterator<IUser> GetBlockedUsers()
        {
            return GetBlockedUsers(new GetBlockedUsersParameters());
        }

        public ISkippableResultIterator<IUser> GetBlockedUsers(IGetBlockedUsersParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetBlockedUsers(parameters);
            var cursorResult = new SkippableResultIterator<IUser, IUserDTO, IUserCursorQueryResultDTO>(twitterCursorResult, _userFactory.GenerateUsersFromDTO);

            return cursorResult;
        }

        #endregion

        #region Follow / Unfollow

        public Task<bool> FollowUser(long userId)
        {
            return FollowUser(new FollowUserParameters(userId));
        }

        public Task<bool> FollowUser(string username)
        {
            return FollowUser(new FollowUserParameters(username));
        }

        public Task<bool> FollowUser(IUserIdentifier user)
        {
            return FollowUser(new FollowUserParameters(user));
        }

        public async Task<bool> FollowUser(IFollowUserParameters parameters)
        {
            var requestResult = await _usersRequester.FollowUser(parameters).ConfigureAwait(false);
            return requestResult?.DataTransferObject != null;
        }

        #endregion
    }
}
