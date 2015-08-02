using System;
using System.Collections.Generic;
using Tweetinvi.Core.Attributes;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi
{
    public static class User
    {
        [ThreadStatic]
        private static IUserFactory _userFactory;
        public static IUserFactory UserFactory
        {
            get
            {
                if (_userFactory == null)
                {
                    Initialize();
                }

                return _userFactory;
            }
        }

        [ThreadStatic]
        private static IUserController _userController;
        public static IUserController UserController
        {
            get
            {
                if (_userController == null)
                {
                    Initialize();
                }

                return _userController;
            }
        }

        static User()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _userController = TweetinviContainer.Resolve<IUserController>();
            _userFactory = TweetinviContainer.Resolve<IUserFactory>();
        }

        #region User Factory

        [TwitterEndpoint("salut")]
        [TwitterEndpoint("salut")]
        public static ILoggedUser GetLoggedUser()
        {
            return UserFactory.GetLoggedUser();
        }

        public static ILoggedUser GetLoggedUser(ITwitterCredentials credentials)
        {
            return UserFactory.GetLoggedUser(credentials);
        }

        public static IUser GetUserFromId(long userId)
        {
            return UserFactory.GetUserFromId(userId);
        }

        /// <summary>
        /// Get a collection of users from a collection of user ids
        /// </summary>
        public static IEnumerable<IUser> GetUsersFromIds(IEnumerable<long> userIds)
        {
            return UserFactory.GetUsersFromIds(userIds);
        }

        public static IUser GetUserFromScreenName(string userName)
        {
            return UserFactory.GetUserFromScreenName(userName);
        }

        public static IEnumerable<IUser> GetUsersFromScreenNames(IEnumerable<string> screenNames)
        {
            return UserFactory.GetUsersFromScreenNames(screenNames);
        }

        public static IUser GenerateUserFromDTO(IUserDTO userDTO)
        {
            return UserFactory.GenerateUserFromDTO(userDTO);
        }

        public static IEnumerable<IUser> GenerateUsersFromDTO(IEnumerable<IUserDTO> usersDTO)
        {
            return UserFactory.GenerateUsersFromDTO(usersDTO);
        }

        public static IUserIdentifier GenerateUserIdentifierFromId(long userId)
        {
            return _userFactory.GenerateUserIdentifierFromId(userId);
        }

        public static IUserIdentifier GenerateUserIdentifierFromScreenName(string userScreenName)
        {
            return _userFactory.GenerateUserIdentifierFromScreenName(userScreenName);
        }

        #endregion

        #region User Controller

        // Friend Ids
        public static IEnumerable<long> GetFriendIds(IUser user, int maxFriendsToRetrieve = 5000)
        {
            return UserController.GetFriendIds(user, maxFriendsToRetrieve);
        }

        public static IEnumerable<long> GetFriendIds(IUserIdentifier userDTO, int maxFriendsToRetrieve = 5000)
        {
            return UserController.GetFriendIds(userDTO, maxFriendsToRetrieve);
        }

        public static IEnumerable<long> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000)
        {
            return UserController.GetFriendIds(userId, maxFriendsToRetrieve);
        }

        public static IEnumerable<long> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000)
        {
            return UserController.GetFriendIds(userScreenName, maxFriendsToRetrieve);
        }

        // Friends
        public static IEnumerable<IUser> GetFriends(IUser user, int maxFriendsToRetrieve = 250)
        {
            return UserController.GetFriends(user, maxFriendsToRetrieve);
        }

        public static IEnumerable<IUser> GetFriends(IUserIdentifier userDTO, int maxFriendsToRetrieve = 250)
        {
            return UserController.GetFriends(userDTO, maxFriendsToRetrieve);
        }

        public static IEnumerable<IUser> GetFriends(long userId, int maxFriendsToRetrieve = 250)
        {
            return UserController.GetFriends(userId, maxFriendsToRetrieve);
        }

        public static IEnumerable<IUser> GetFriends(string userScreenName, int maxFriendsToRetrieve = 250)
        {
            return UserController.GetFriends(userScreenName, maxFriendsToRetrieve);
        }

        // Follower Ids
        public static IEnumerable<long> GetFollowerIds(IUser user, int maxFollowersToRetrieve = 5000)
        {
            return UserController.GetFollowerIds(user, maxFollowersToRetrieve);
        }

        public static IEnumerable<long> GetFollowerIds(IUserIdentifier userDTO, int maxFollowersToRetrieve = 5000)
        {
            return UserController.GetFollowerIds(userDTO, maxFollowersToRetrieve);
        }

        public static IEnumerable<long> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000)
        {
            return UserController.GetFollowerIds(userId, maxFollowersToRetrieve);
        }

        public static IEnumerable<long> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000)
        {
            return UserController.GetFollowerIds(userScreenName, maxFollowersToRetrieve);
        }

        // Followers
        public static IEnumerable<IUser> GetFollowers(IUser user, int maxFollowersToRetrieve = 250)
        {
            return UserController.GetFollowers(user, maxFollowersToRetrieve);
        }

        public static IEnumerable<IUser> GetFollowers(IUserIdentifier userDTO, int maxFollowersToRetrieve = 250)
        {
            return UserController.GetFollowers(userDTO, maxFollowersToRetrieve);
        }

        public static IEnumerable<IUser> GetFollowers(long userId, int maxFollowersToRetrieve = 250)
        {
            return UserController.GetFollowers(userId, maxFollowersToRetrieve);
        }

        public static IEnumerable<IUser> GetFollowers(string userScreenName, int maxFollowersToRetrieve = 250)
        {
            return UserController.GetFollowers(userScreenName, maxFollowersToRetrieve);
        }

        // Favourites
        public static IEnumerable<ITweet> GetFavouriteTweets(IUser user, int maxFavouriteTweetsToRetrieve = 40)
        {
            return UserController.GetFavouriteTweets(user, maxFavouriteTweetsToRetrieve);
        }

        public static IEnumerable<ITweet> GetFavouriteTweets(IUserIdentifier userDTO, int maxFavouriteTweetsToRetrieve = 40)
        {
            return UserController.GetFavouriteTweets(userDTO, maxFavouriteTweetsToRetrieve);
        }

        public static IEnumerable<ITweet> GetFavouriteTweets(long userId, int maxFavouriteTweetsToRetrieve = 40)
        {
            return UserController.GetFavouriteTweets(userId, maxFavouriteTweetsToRetrieve);
        }

        public static IEnumerable<ITweet> GetFavouriteTweets(string userScreenName, int maxFavouriteTweetsToRetrieve = 40)
        {
            return UserController.GetFavouriteTweets(userScreenName, maxFavouriteTweetsToRetrieve);
        }

        // Block User
        public static bool BlockUser(IUser user)
        {
            return UserController.BlockUser(user);
        }

        public static bool BlockUser(IUserIdentifier userDTO)
        {
            return UserController.BlockUser(userDTO);
        }

        public static bool BlockUser(long userId)
        {
            return UserController.BlockUser(userId);
        }

        public static bool BlockUser(string userScreenName)
        {
            return UserController.BlockUser(userScreenName);
        }

        // Stream Profile Image 
        public static System.IO.Stream GetProfileImageStream(IUser user, ImageSize imageSize = ImageSize.normal)
        {
            return UserController.GetProfileImageStream(user, imageSize);
        }

        public static System.IO.Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            return UserController.GetProfileImageStream(userDTO, imageSize);
        }

        // Spam
        public static bool ReportUserForSpam(IUser user)
        {
            return UserController.ReportUserForSpam(user);
        }

        public static bool ReportUserForSpam(IUserIdentifier userDTO)
        {
            return UserController.ReportUserForSpam(userDTO);
        }

        public static bool ReportUserForSpam(long userId)
        {
            return UserController.ReportUserForSpam(userId);
        }

        public static bool ReportUserForSpam(string userScreenName)
        {
            return UserController.ReportUserForSpam(userScreenName);
        }

        #endregion
    }
}