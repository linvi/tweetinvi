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
        private readonly TwitterClient _client;
        private readonly IAccountRequester _accountRequester;
        private readonly IUserFactory _userFactory;
        private readonly IMultiLevelCursorIteratorFactory _multiLevelCursorIteratorFactory;

        public AccountClient(TwitterClient client)
        {
            _client = client;
            _accountRequester = client.RequestExecutor.Account;

            _userFactory = TweetinviContainer.Resolve<IUserFactory>();
            _multiLevelCursorIteratorFactory = TweetinviContainer.Resolve<IMultiLevelCursorIteratorFactory>();
        }

        public IAccountClientParametersValidator ParametersValidator => _client.ParametersValidator;
        
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
            var requestResult = await _accountRequester.BlockUser(parameters).ConfigureAwait(false);
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

        public Task<bool> UnBlockUser(IUserIdentifier user)
        {
            return UnblockUser(new UnblockUserParameters(user));
        }

        public async Task<bool> UnblockUser(IUnblockUserParameters parameters)
        {
            var requestResult = await _accountRequester.UnblockUser(parameters).ConfigureAwait(false);
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
            var requestResult = await _accountRequester.ReportUserForSpam(parameters).ConfigureAwait(false);
            return requestResult?.DataTransferObject != null;
        }

        public ITwitterIterator<long> GetBlockedUserIds()
        {
            return GetBlockedUserIds(new GetBlockedUserIdsParameters());
        }

        public ITwitterIterator<long> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters)
        {
            var twitterCursorResult = _accountRequester.GetBlockedUserIds(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(twitterCursorResult, dto => dto.DataTransferObject.Ids);
        }

        public ITwitterIterator<IUser> GetBlockedUsers()
        {
            return GetBlockedUsers(new GetBlockedUsersParameters());
        }

        public ITwitterIterator<IUser> GetBlockedUsers(IGetBlockedUsersParameters parameters)
        {
            var twitterCursorResult = _accountRequester.GetBlockedUsers(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IUserCursorQueryResultDTO>, IUser>(twitterCursorResult, pageResult =>
            {
                var userDTOs = pageResult.DataTransferObject.Users;
                return _userFactory.GenerateUsersFromDTO(userDTOs, null);
            });
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
            var requestResult = await _accountRequester.FollowUser(parameters).ConfigureAwait(false);
            return requestResult?.DataTransferObject != null;
        }

        public Task<bool> UnFollowUser(long userId)
        {
            return UnFollowUser(new UnFollowUserParameters(userId));
        }

        public Task<bool> UnFollowUser(string username)
        {
            return UnFollowUser(new UnFollowUserParameters(username));
        }

        public Task<bool> UnFollowUser(IUserIdentifier user)
        {
            return UnFollowUser(new UnFollowUserParameters(user));
        }

        public async Task<bool> UnFollowUser(IUnFollowUserParameters parameters)
        {
            var requestResult = await _accountRequester.UnFollowUser(parameters).ConfigureAwait(false);
            return requestResult?.DataTransferObject != null;
        }

        #endregion

        #region Update Friendship

        public async Task<bool> UpdateRelationship(IUpdateRelationshipParameters parameters)
        {
            var requestResult = await _accountRequester.UpdateRelationship(parameters).ConfigureAwait(false);
            return requestResult?.Result != null;
        }

        #endregion
        
        #region Pending Followers Requests

        public ITwitterIterator<long> GetUserIdsRequestingFriendship()
        {
            return GetUserIdsRequestingFriendship(new GetUserIdsRequestingFriendshipParameters());
        }
        
        public ITwitterIterator<long> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var iterator = _accountRequester.GetUserIdsRequestingFriendship(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship()
        {
            return GetUsersRequestingFriendship(new GetUsersRequestingFriendshipParameters());
        }
        
        public IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship(IGetUsersRequestingFriendshipParameters parameters)
        {
            var iterator = _accountRequester.GetUserIdsRequestingFriendship(parameters);

            var maxPageSize = parameters.GetUsersPageSize;
            if (maxPageSize > _client.Config.Limits.USERS_GET_USERS_MAX_SIZE)
            {
                throw new TwitterArgumentLimitException($"${nameof(parameters)}.{nameof(parameters.GetUsersPageSize)}", maxPageSize, nameof(_client.Config.Limits.USERS_GET_USERS_MAX_SIZE), "page size");
            }
            
            return _multiLevelCursorIteratorFactory.CreateUserMultiLevelIterator(_client, iterator, maxPageSize);
        }

        public ITwitterIterator<long> GetUserIdsYouRequestedToFollow()
        {
            return GetUserIdsYouRequestedToFollow(new GetUserIdsYouRequestedToFollowParameters());
        }

        public ITwitterIterator<long> GetUserIdsYouRequestedToFollow(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            var iterator = _accountRequester.GetUserIdsYouRequestedToFollow(parameters);
            return new TwitterIteratorProxy<ITwitterResult<IIdsCursorQueryResultDTO>, long>(iterator, dto => dto.DataTransferObject.Ids);
        }
        
        public IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow()
        {
            return GetUsersYouRequestedToFollow(new GetUsersYouRequestedToFollowParameters());
        }

        public IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow(IGetUsersYouRequestedToFollowParameters parameters)
        {
            var iterator = _accountRequester.GetUserIdsYouRequestedToFollow(parameters);

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
            var twitterResult = await _accountRequester.GetRelationshipsWith(parameters).ConfigureAwait(false);
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

        #region Others

        public Task<long[]> GetUserIdsWhoseRetweetsAreMuted()
        {
            return GetUserIdsWhoseRetweetsAreMuted(new GetUserIdsWhoseRetweetsAreMutedParameters());
        }
        
        public async Task<long[]> GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters parameters)
        {
            var twitterResult = await _accountRequester.GetUserIdsWhoseRetweetsAreMuted(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }
        
        #endregion
    }
}