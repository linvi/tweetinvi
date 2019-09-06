using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Logic.QueryParameters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.User
{
    /// <summary>
    /// Reason for change : Twitter changes the operation exposed on its REST API
    /// </summary>
    public class UserController : IUserController
    {
        private readonly IUserQueryExecutor _userQueryExecutor;
        private readonly ITweetFactory _tweetFactory;
        private readonly IUserFactory _userFactory;
        private readonly ITwitterResultFactory _twitterResultFactory;

        public UserController(
            IUserQueryExecutor userQueryExecutor,
            ITweetFactory tweetFactory,
            IUserFactory userFactory,
            ITwitterResultFactory twitterResultFactory)
        {
            _userQueryExecutor = userQueryExecutor;
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;
            _twitterResultFactory = twitterResultFactory;
        }

        public async Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters, ITwitterRequest request)
        {
            var result = await _userQueryExecutor.GetAuthenticatedUser(parameters, request);
            return _twitterResultFactory.Create(result, userDTO => _userFactory.GenerateAuthenticatedUserFromDTO(userDTO));
        }

        public async Task<ITwitterResult<IUserDTO, IUser>> GetUser(IGetUserParameters parameters, ITwitterRequest request)
        {
            var result = await _userQueryExecutor.GetUser(parameters, request);
            return _twitterResultFactory.Create(result, userDTO => _userFactory.GenerateUserFromDTO(userDTO));
        }

        public async Task<ITwitterResult<IUserDTO[], IUser[]>> GetUsers(IGetUsersParameters parameters, ITwitterRequest request)
        {
            var result = await _userQueryExecutor.GetUsers(parameters, request);
            return _twitterResultFactory.Create(result, userDTO => _userFactory.GenerateUsersFromDTO(userDTO));
        }

        // Friend Ids
        public TwitterCursorResult<long, IIdsCursorQueryResultDTO> GetFriendIds(IGetFriendIdsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterCursorResult<long, IIdsCursorQueryResultDTO>(async cursor => 
            {
                var cursoredParameters = new GetFriendIdsParameters(parameters)
                {
                    Cursor = cursor
                };

                return await _userQueryExecutor.GetFriendIds(cursoredParameters, new TwitterRequest(request));
            });
            
            return twitterCursorResult;
        }

        // Friends
        public Task<IEnumerable<IUser>> GetFriends(IUserIdentifier user, int maxFriendsToRetrieve = 250)
        {
            throw new NotImplementedException("TODO");
        }

        public Task<IEnumerable<IUser>> GetFriends(long userId, int maxFriendsToRetrieve = 250)
        {
            throw new NotImplementedException("TODO");
        }

        public Task<IEnumerable<IUser>> GetFriends(string userScreenName, int maxFriendsToRetrieve = 250)
        {
            throw new NotImplementedException("TODO");
        }
        
        // Follower Ids
        public Task<IEnumerable<long>> GetFollowerIds(IUserIdentifier user, int maxFollowersToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFollowerIds(user, maxFollowersToRetrieve);
        }

        public Task<IEnumerable<long>> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFollowerIds(new UserIdentifier(userId), maxFollowersToRetrieve);
        }

        public Task<IEnumerable<long>> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFollowerIds(new UserIdentifier(userScreenName), maxFollowersToRetrieve);
        }

        // Followers
        public async Task<IEnumerable<IUser>> GetFollowers(IUserIdentifier user, int maxFollowersToRetrieve = 250)
        {
            var followerIds = await GetFollowerIds(user, maxFollowersToRetrieve);
            return await _userFactory.GetUsersFromIds(followerIds);
        }

        public async Task<IEnumerable<IUser>> GetFollowers(long userId, int maxFollowersToRetrieve = 250)
        {
            var followerIds = await GetFollowerIds(userId, maxFollowersToRetrieve);
            return await _userFactory.GetUsersFromIds(followerIds);
        }

        public async Task<IEnumerable<IUser>> GetFollowers(string userScreenName, int maxFollowersToRetrieve = 250)
        {
            var followerIds = await GetFollowerIds(userScreenName, maxFollowersToRetrieve);
            return await _userFactory.GetUsersFromIds(followerIds);
        }

        // Favourites
        public Task<IEnumerable<ITweet>> GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters)
        {
            var favoriteParameters = new GetUserFavoritesQueryParameters(user, parameters);
            return GetFavoriteTweets(favoriteParameters);
        }

        public async Task<IEnumerable<ITweet>> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters)
        {
            var tweetDTOs = await _userQueryExecutor.GetFavoriteTweets(parameters);
            return _tweetFactory.GenerateTweetsFromDTO(tweetDTOs, null, null);
        }

        // Block User
        public Task<bool> BlockUser(IUserIdentifier user)
        {
            return _userQueryExecutor.BlockUser(user);
        }

        public Task<bool> BlockUser(long userId)
        {
            return _userQueryExecutor.BlockUser(new UserIdentifier(userId));
        }

        public Task<bool> BlockUser(string userScreenName)
        {
            return _userQueryExecutor.BlockUser(new UserIdentifier(userScreenName));
        }

        // UnBlock user
        public Task<bool> UnBlockUser(IUserIdentifier user)
        {
            return _userQueryExecutor.UnBlockUser(user);
        }

        public Task<bool> UnBlockUser(long userId)
        {
            return _userQueryExecutor.UnBlockUser(new UserIdentifier(userId));
        }

        public Task<bool> UnBlockUser(string userScreenName)
        {
            return _userQueryExecutor.UnBlockUser(new UserIdentifier(userScreenName));
        }

        public Task<IEnumerable<long>> GetBlockedUserIds(int maxUserIds)
        {
            return _userQueryExecutor.GetBlockedUserIds(maxUserIds);
        }

        public async Task<IEnumerable<IUser>> GetBlockedUsers(int maxUsers = Int32.MaxValue)
        {
            var userDTOs = await _userQueryExecutor.GetBlockedUsers(maxUsers);
            return _userFactory.GenerateUsersFromDTO(userDTOs);
        }

        // Stream Profile Image
        public Stream GetProfileImageStream(IUser user, ImageSize imageSize = ImageSize.normal)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetProfileImageStream(user.UserDTO, imageSize);
        }

        public Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            return _userQueryExecutor.GetProfileImageStream(userDTO, imageSize);
        }

        // Spam
        public Task<bool> ReportUserForSpam(IUserIdentifier user)
        {
            return _userQueryExecutor.ReportUserForSpam(user);
        }

        public Task<bool> ReportUserForSpam(long userId)
        {
            return _userQueryExecutor.ReportUserForSpam(new UserIdentifier(userId));
        }

        public Task<bool> ReportUserForSpam(string userScreenName)
        {
            return _userQueryExecutor.ReportUserForSpam(new UserIdentifier(userScreenName));
        }
    }
}