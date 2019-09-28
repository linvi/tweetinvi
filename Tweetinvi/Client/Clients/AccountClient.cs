using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
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
    /// <summary>
    /// AccountsClient contains all the operations that can be executed from an authenticated user's context.
    /// </summary>
    public class AccountClient : IAccountClient
    {
        private readonly TwitterClient _client;
        private readonly IAccountsRequester _accountsRequester;
        private readonly IUserFactory _userFactory;
        private readonly IMultiLevelCursorIteratorFactory _multiLevelCursorIteratorFactory;

        public AccountClient(TwitterClient client)
        {
            _client = client;
            _accountsRequester = client.RequestExecutor.Accounts;

            _userFactory = TweetinviContainer.Resolve<IUserFactory>();
            _multiLevelCursorIteratorFactory = TweetinviContainer.Resolve<IMultiLevelCursorIteratorFactory>();
        }

        #region Authenticated User

        /// <inheritdoc />
        public Task<IAuthenticatedUser> GetAuthenticatedUser()
        {
            return GetAuthenticatedUser(new GetAuthenticatedUserParameters());
        }

        /// <inheritdoc />
        public async Task<IAuthenticatedUser> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters)
        {
            var requestResult = await _accountsRequester.GetAuthenticatedUser(parameters);
            return requestResult?.Result;
        }

        #endregion
        
        #region Block / Unblock

        /// <inheritdoc />
        public Task<bool> BlockUser(long? userId)
        {
            return BlockUser(new BlockUserParameters(userId));
        }

        /// <inheritdoc />
        public Task<bool> BlockUser(string username)
        {
            return BlockUser(new BlockUserParameters(username));
        }

        /// <inheritdoc />
        public Task<bool> BlockUser(IUserIdentifier user)
        {
            return BlockUser(new BlockUserParameters(user));
        }

        /// <inheritdoc />
        public async Task<bool> BlockUser(IBlockUserParameters parameters)
        {
            var requestResult = await _accountsRequester.BlockUser(parameters).ConfigureAwait(false);
            return requestResult?.DataTransferObject != null;
        }

        /// <inheritdoc />
        public Task<bool> UnblockUser(long? userId)
        {
            return UnblockUser(new UnblockUserParameters(userId));
        }
        
        /// <inheritdoc />
        public Task<bool> UnblockUser(string username)
        {
            return UnblockUser(new UnblockUserParameters(username));
        }

        /// <inheritdoc />
        public Task<bool> UnBlockUser(IUserIdentifier user)
        {
            return UnblockUser(new UnblockUserParameters(user));
        }

        /// <inheritdoc />
        public async Task<bool> UnblockUser(IUnblockUserParameters parameters)
        {
            var requestResult = await _accountsRequester.UnblockUser(parameters).ConfigureAwait(false);
            return requestResult?.DataTransferObject != null;
        }

        /// <inheritdoc />
        public Task<bool> ReportUserForSpam(long? userId)
        {
            return ReportUserForSpam(new ReportUserForSpamParameters(userId));
        }

        /// <inheritdoc />
        public Task<bool> ReportUserForSpam(string username)
        {
            return ReportUserForSpam(new ReportUserForSpamParameters(username));
        }

        /// <inheritdoc />
        public Task<bool> ReportUserForSpam(IUserIdentifier user)
        {
            return ReportUserForSpam(new ReportUserForSpamParameters(user));
        }

        /// <inheritdoc />
        public async Task<bool> ReportUserForSpam(IReportUserForSpamParameters parameters)
        {
            var requestResult = await _accountsRequester.ReportUserForSpam(parameters).ConfigureAwait(false);
            return requestResult?.DataTransferObject != null;
        }

        /// <inheritdoc />
        public ITwitterIterator<long> GetBlockedUserIds()
        {
            return GetBlockedUserIds(new GetBlockedUserIdsParameters());
        }

        /// <inheritdoc />
        public ITwitterIterator<long> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters)
        {
            var twitterCursorResult = _accountsRequester.GetBlockedUserIds(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(twitterCursorResult, dto => dto.DataTransferObject.Ids);
        }

        /// <inheritdoc />
        public ITwitterIterator<IUser> GetBlockedUsers()
        {
            return GetBlockedUsers(new GetBlockedUsersParameters());
        }

        /// <inheritdoc />
        public ITwitterIterator<IUser> GetBlockedUsers(IGetBlockedUsersParameters parameters)
        {
            var twitterCursorResult = _accountsRequester.GetBlockedUsers(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IUserCursorQueryResultDTO>, IUser>(twitterCursorResult, pageResult =>
            {
                var userDTOs = pageResult.DataTransferObject.Users;
                return _userFactory.GenerateUsersFromDTO(userDTOs, null);
            });
        }

        #endregion

        #region Follow / Unfollow

        /// <inheritdoc />
        public Task<bool> FollowUser(long userId)
        {
            return FollowUser(new FollowUserParameters(userId));
        }

        /// <inheritdoc />
        public Task<bool> FollowUser(string username)
        {
            return FollowUser(new FollowUserParameters(username));
        }
        
        /// <inheritdoc />
        public Task<bool> FollowUser(IUserIdentifier user)
        {
            return FollowUser(new FollowUserParameters(user));
        }

        /// <inheritdoc />
        public async Task<bool> FollowUser(IFollowUserParameters parameters)
        {
            var requestResult = await _accountsRequester.FollowUser(parameters).ConfigureAwait(false);
            return requestResult?.DataTransferObject != null;
        }

        /// <inheritdoc />
        public Task<bool> UnFollowUser(long userId)
        {
            return UnFollowUser(new UnFollowUserParameters(userId));
        }

        /// <inheritdoc />
        public Task<bool> UnFollowUser(string username)
        {
            return UnFollowUser(new UnFollowUserParameters(username));
        }

        /// <inheritdoc />
        public Task<bool> UnFollowUser(IUserIdentifier user)
        {
            return UnFollowUser(new UnFollowUserParameters(user));
        }

        /// <inheritdoc />
        public async Task<bool> UnFollowUser(IUnFollowUserParameters parameters)
        {
            var requestResult = await _accountsRequester.UnFollowUser(parameters).ConfigureAwait(false);
            return requestResult?.DataTransferObject != null;
        }

        #endregion
        
        #region Pending Followers Requests

        /// <inheritdoc />
        public ITwitterIterator<long> GetUserIdsRequestingFriendship()
        {
            return GetUserIdsRequestingFriendship(new GetUserIdsRequestingFriendshipParameters());
        }
        
        /// <inheritdoc />
        public ITwitterIterator<long> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var iterator = _accountsRequester.GetUserIdsRequestingFriendship(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }

        /// <inheritdoc />
        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship()
        {
            return GetUsersRequestingFriendship(new GetUsersRequestingFriendshipParameters());
        }
        
        /// <inheritdoc />
        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship(IGetUsersRequestingFriendshipParameters parameters)
        {
            var iterator = _accountsRequester.GetUserIdsRequestingFriendship(parameters);

            var maxPageSize = parameters.GetUsersPageSize;
            if (maxPageSize > _client.Config.Limits.USERS_GET_USERS_MAX_SIZE)
            {
                throw new TwitterArgumentLimitException($"${nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxPageSize, nameof(_client.Config.Limits.USERS_GET_USERS_MAX_SIZE), "page size");
            }
            
            return _multiLevelCursorIteratorFactory.CreateUserMultiLevelIterator(_client, iterator, maxPageSize);
        }

        /// <inheritdoc />
        public ITwitterIterator<long> GetUserIdsYouRequestedToFollow()
        {
            return GetUserIdsYouRequestedToFollow(new GetUserIdsYouRequestedToFollowParameters());
        }

        /// <inheritdoc />
        public ITwitterIterator<long> GetUserIdsYouRequestedToFollow(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            var iterator = _accountsRequester.GetUserIdsYouRequestedToFollow(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }
        
        /// <inheritdoc />
        public IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow()
        {
            return GetUsersYouRequestedToFollow(new GetUsersYouRequestedToFollowParameters());
        }

        /// <inheritdoc />
        public IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow(IGetUsersYouRequestedToFollowParameters parameters)
        {
            var iterator = _accountsRequester.GetUserIdsYouRequestedToFollow(parameters);

            var maxPageSize = parameters.GetUsersPageSize;
            if (maxPageSize > _client.Config.Limits.USERS_GET_USERS_MAX_SIZE)
            {
                throw new TwitterArgumentLimitException($"${nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxPageSize, nameof(_client.Config.Limits.USERS_GET_USERS_MAX_SIZE), "page size");
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
            var twitterResult = await _accountsRequester.GetRelationshipsWith(parameters);
            var relationshipsWith = twitterResult.Result;

            var userRelationshipState = new UserDictionary<IRelationshipState>();

            foreach (var user in parameters.Users)
            {
                var userRelationship = relationshipsWith.First(x => x.TargetId == user.Id || x.TargetScreenName == user.ScreenName);
                userRelationshipState.AddOrUpdate(user, userRelationship);
            }

            return userRelationshipState;
        }

        #endregion
    }
}