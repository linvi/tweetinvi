using System;
using System.Collections.Generic;
using System.IO;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
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
        public IEnumerable<long> GetFriendIds(IUserIdentifier user, int maxFriendsToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFriendIds(user, maxFriendsToRetrieve);
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
        public IEnumerable<IUser> GetFriends(IUserIdentifier user, int maxFriendsToRetrieve = 250)
        {
            var friendIds = GetFriendIds(user, maxFriendsToRetrieve);
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
        public IEnumerable<long> GetFollowerIds(IUserIdentifier user, int maxFollowersToRetrieve = 5000)
        {
            return _userQueryExecutor.GetFollowerIds(user, maxFollowersToRetrieve);
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
        public IEnumerable<IUser> GetFollowers(IUserIdentifier user, int maxFollowersToRetrieve = 250)
        {
            var followerIds = GetFollowerIds(user, maxFollowersToRetrieve);
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
        public IEnumerable<ITweet> GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters)
        {
            var favoriteParameters = new GetUserFavoritesQueryParameters(user, parameters);
            return GetFavoriteTweets(favoriteParameters);
        }

        public IEnumerable<ITweet> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters)
        {
            var tweetDTOs = _userQueryExecutor.GetFavoriteTweets(parameters);
            return _tweetFactory.GenerateTweetsFromDTO(tweetDTOs);
        }

        // Block User
        public bool BlockUser(IUserIdentifier user)
        {
            return _userQueryExecutor.BlockUser(user);
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
        public bool UnBlockUser(IUserIdentifier user)
        {
            return _userQueryExecutor.UnBlockUser(user);
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
        public bool ReportUserForSpam(IUserIdentifier user)
        {
            return _userQueryExecutor.ReportUserForSpam(user);
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