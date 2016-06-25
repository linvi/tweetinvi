using System;
using System.Collections.Generic;
using System.IO;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Logic.QueryParameters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
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

        public UserController(
            IUserQueryExecutor userQueryExecutor,
            ITweetFactory tweetFactory,
            IUserFactory userFactory)
        {
            _userQueryExecutor = userQueryExecutor;
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;
        }

        // Friend Ids
        public IEnumerable<long> GetFriendIds(IUser user, int maxFriendsToRetrieve = 5000)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetFriendIds(user.UserDTO, maxFriendsToRetrieve);
        }

        public IEnumerable<long> GetFriendIds(IUserIdentifier userIdentifier, int maxFriendsToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFriendIds(userIdentifier, maxFriendsToRetrieve);
        }

        public IEnumerable<long> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFriendIds(new UserIdentifier(userId), maxFriendsToRetrieve);
        }

        public IEnumerable<long> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFriendIds(new UserIdentifier(userScreenName), maxFriendsToRetrieve);
        }

        // Friends
        public IEnumerable<IUser> GetFriends(IUser user, int maxFriendsToRetrieve = 250)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetFriends(user.UserDTO, maxFriendsToRetrieve);
        }

        public IEnumerable<IUser> GetFriends(IUserIdentifier userIdentifier, int maxFriendsToRetrieve = 250)
        {
            var friendIds = GetFriendIds(userIdentifier, maxFriendsToRetrieve);
            return _userFactory.GetUsersFromIds(friendIds);
        }

        public IEnumerable<IUser> GetFriends(long userId, int maxFriendsToRetrieve = 250)
        {
            var friendIds = GetFriendIds(userId, maxFriendsToRetrieve);
            return _userFactory.GetUsersFromIds(friendIds);
        }

        public IEnumerable<IUser> GetFriends(string userScreenName, int maxFriendsToRetrieve = 250)
        {
            var friendIds = GetFriendIds(userScreenName, maxFriendsToRetrieve);
            return _userFactory.GetUsersFromIds(friendIds);
        }

        // Follower Ids
        public IEnumerable<long> GetFollowerIds(IUser user, int maxFollowersToRetrieve = 5000)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetFollowerIds(user.UserDTO, maxFollowersToRetrieve);
        }

        public IEnumerable<long> GetFollowerIds(IUserIdentifier userIdentifier, int maxFollowersToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFollowerIds(userIdentifier, maxFollowersToRetrieve);
        }

        public IEnumerable<long> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFollowerIds(new UserIdentifier(userId), maxFollowersToRetrieve);
        }

        public IEnumerable<long> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFollowerIds(new UserIdentifier(userScreenName), maxFollowersToRetrieve);
        }

        // Followers
        public IEnumerable<IUser> GetFollowers(IUser user, int maxFollowersToRetrieve = 250)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetFollowers(user.UserDTO, maxFollowersToRetrieve);
        }

        public IEnumerable<IUser> GetFollowers(IUserIdentifier userIdentifier, int maxFollowersToRetrieve = 250)
        {
            var followerIds = GetFollowerIds(userIdentifier, maxFollowersToRetrieve);
            return _userFactory.GetUsersFromIds(followerIds);
        }

        public IEnumerable<IUser> GetFollowers(long userId, int maxFollowersToRetrieve = 250)
        {
            var followerIds = GetFollowerIds(userId, maxFollowersToRetrieve);
            return _userFactory.GetUsersFromIds(followerIds);
        }

        public IEnumerable<IUser> GetFollowers(string userScreenName, int maxFollowersToRetrieve = 250)
        {
            var followerIds = GetFollowerIds(userScreenName, maxFollowersToRetrieve);
            return _userFactory.GetUsersFromIds(followerIds);
        }

        // Favourites
        public IEnumerable<ITweet> GetFavoriteTweets(IUserIdentifier userIdentifier, IGetUserFavoritesParameters parameters)
        {
            var favoriteParameters = new GetUserFavoritesQueryParameters(userIdentifier, parameters);
            return GetFavoriteTweets(favoriteParameters);
        }

        public IEnumerable<ITweet> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters)
        {
            var tweetDTOs = _userQueryExecutor.GetFavoriteTweets(parameters);
            return _tweetFactory.GenerateTweetsFromDTO(tweetDTOs);
        }

        // Block User
        public bool BlockUser(IUserIdentifier userIdentifier)
        {
            return _userQueryExecutor.BlockUser(userIdentifier);
        }

        public bool BlockUser(long userId)
        {
            return _userQueryExecutor.BlockUser(new UserIdentifier(userId));
        }

        public bool BlockUser(string userScreenName)
        {
            return _userQueryExecutor.BlockUser(new UserIdentifier(userScreenName));
        }

        // UnBlock user
        public bool UnBlockUser(IUserIdentifier userIdentifier)
        {
            return _userQueryExecutor.UnBlockUser(userIdentifier);
        }

        public bool UnBlockUser(long userId)
        {
            return _userQueryExecutor.UnBlockUser(new UserIdentifier(userId));
        }

        public bool UnBlockUser(string userScreenName)
        {
            return _userQueryExecutor.UnBlockUser(new UserIdentifier(userScreenName));
        }

        public IEnumerable<long> GetBlockedUserIds(int maxUserIds)
        {
            return _userQueryExecutor.GetBlockedUserIds(maxUserIds);
        }

        public IEnumerable<IUser> GetBlockedUsers(int maxUsers = Int32.MaxValue)
        {
            var userDTOs = _userQueryExecutor.GetBlockedUsers(maxUsers);
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
        public bool ReportUserForSpam(IUserIdentifier userIdentifier)
        {
            return _userQueryExecutor.ReportUserForSpam(userIdentifier);
        }

        public bool ReportUserForSpam(long userId)
        {
            return _userQueryExecutor.ReportUserForSpam(new UserIdentifier(userId));
        }

        public bool ReportUserForSpam(string userScreenName)
        {
            return _userQueryExecutor.ReportUserForSpam(new UserIdentifier(userScreenName));
        }
    }
}