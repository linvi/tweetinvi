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
        private readonly TwitterClient _client;
        private readonly IUsersRequester _usersRequester;
        private readonly IMultiLevelCursorIteratorFactory _multiLevelCursorIteratorFactory;

        public UsersClient(TwitterClient client)
        {
            var clientContext = client.CreateTwitterExecutionContext();

            _client = client;
            _usersRequester = client.RequestExecutor.Users;
            _multiLevelCursorIteratorFactory = clientContext.Container.Resolve<IMultiLevelCursorIteratorFactory>();
        }

        public IUsersClientParametersValidator ParametersValidator => _client.ParametersValidator;

        #region GetUser

        public Task<IUser> GetUser(long? userId)
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
            if (parameters?.Users.Length == 0)
            {
                return new IUser[0];
            }

            var requestResult = await _usersRequester.GetUsers(parameters).ConfigureAwait(false);
            return requestResult?.Result;
        }

        #endregion

        #region Relationship Between Users

        public Task<IRelationshipDetails> GetRelationshipBetween(long? sourceUserId, long? targetUserId)
        {
            return GetRelationshipBetween(new GetRelationshipBetweenParameters(sourceUserId, targetUserId));
        }

        public Task<IRelationshipDetails> GetRelationshipBetween(long? sourceUserId, string targetUsername)
        {
            return GetRelationshipBetween(new GetRelationshipBetweenParameters(sourceUserId, targetUsername));
        }

        public Task<IRelationshipDetails> GetRelationshipBetween(long? sourceUserId, IUserIdentifier targetUser)
        {
            return GetRelationshipBetween(new GetRelationshipBetweenParameters(sourceUserId, targetUser));
        }

        public Task<IRelationshipDetails> GetRelationshipBetween(string sourceUsername, long? targetUserId)
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

        public Task<IRelationshipDetails> GetRelationshipBetween(IUserIdentifier sourceUser, long? targetUserId)
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
            var relationshipTwitterResult = await _usersRequester.GetRelationshipBetween(parameters);
            return relationshipTwitterResult?.Result;
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

        public ITwitterIterator<long> GetFollowerIds(long? userId)
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