using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
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
        private readonly TwitterClient _client;
        private readonly IPagedOperationsHelper _pageOperationHelper;

        public UsersClient(TwitterClient client)
        {
            _client = client;
            _usersRequester = client.RequestExecutor.Users;
            _pageOperationHelper = TweetinviContainer.Resolve<IPagedOperationsHelper>();
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

        public Task<ICursorResult<long>> GetFriendIds(string username)
        {
            var parameters = new GetFriendIdsParameters(username);
            return GetFriendIds(parameters);
        }

        public Task<ICursorResult<long>> GetFriendIds(long userId)
        {
            var parameters = new GetFriendIdsParameters(userId);
            return GetFriendIds(parameters);
        }

        public Task<ICursorResult<long>> GetFriendIds(IUserIdentifier userIdentifier)
        {
            var parameters = new GetFriendIdsParameters(userIdentifier);
            return GetFriendIds(parameters);
        }

        public async Task<ICursorResult<long>> GetFriendIds(IGetFriendIdsParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetFriendIds(parameters);
            var cursorResult = new CursorResult<long, IIdsCursorQueryResultDTO>(twitterCursorResult);

            await cursorResult.MoveNext().ConfigureAwait(false);

            return cursorResult;
        }

        public async Task<ICursorResult<IUser>> GetFriends(IGetFriendsParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetFriendIds(parameters);

            var cursorResult = new CursorResult<IUser, long, IIdsCursorQueryResultDTO>(twitterCursorResult, async ids =>
            {
                var maxItemsPerRequest = _client.Config.Limits.Users.GetUsersMaxSize;
                return await _pageOperationHelper.IterateOverWithLimit(ids, GetUsers, maxItemsPerRequest).ConfigureAwait(false);
            });

            await cursorResult.MoveNext().ConfigureAwait(false);

            return cursorResult;
        }

        #endregion

        #region GetFollowers

        public Task<ICursorResult<long>> GetFollowerIds(string username)
        {
            var parameters = new GetFollowerIdsParameters(username);
            return GetFollowerIds(parameters);
        }

        public Task<ICursorResult<long>> GetFollowerIds(long userId)
        {
            var parameters = new GetFollowerIdsParameters(userId);
            return GetFollowerIds(parameters);
        }

        public Task<ICursorResult<long>> GetFollowerIds(IUserIdentifier userIdentifier)
        {
            var parameters = new GetFollowerIdsParameters(userIdentifier);
            return GetFollowerIds(parameters);
        }

        public async Task<ICursorResult<long>> GetFollowerIds(IGetFollowerIdsParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetFollowerIds(parameters);
            var cursorResult = new CursorResult<long, IIdsCursorQueryResultDTO>(twitterCursorResult);

            await cursorResult.MoveNext().ConfigureAwait(false);

            return cursorResult;
        }

        public async Task<ICursorResult<IUser>> GetFollowers(IGetFollowersParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetFollowerIds(parameters);

            var cursorResult = new CursorResult<IUser, long, IIdsCursorQueryResultDTO>(twitterCursorResult, async ids =>
            {
                var maxItemsPerRequest = _client.Config.Limits.Users.GetUsersMaxSize;
                return await _pageOperationHelper.IterateOverWithLimit(ids, GetUsers, maxItemsPerRequest).ConfigureAwait(false);
            });

            await cursorResult.MoveNext().ConfigureAwait(false);

            return cursorResult;
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

        public Task<ICursorResult<long>> GetBlockedUserIds()
        {
            return GetBlockedUserIds(new GetBlockedUserIdsParameters());
        }

        public async Task<ICursorResult<long>> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters)
        {
            var twitterCursorResult = _usersRequester.GetBlockedUserIds(parameters);
            var cursorResult = new CursorResult<long, IIdsCursorQueryResultDTO>(twitterCursorResult);

            await cursorResult.MoveNext().ConfigureAwait(false);

            return cursorResult;
        }

        #endregion
    }
}
