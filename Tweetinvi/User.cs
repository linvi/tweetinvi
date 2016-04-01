using System;
using System.Collections.Generic;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi
{
    public static class User
    {
        [ThreadStatic]
        private static IUserFactory _userFactory;

        /// <summary>
        /// Factory creating Users
        /// </summary>
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
        
        /// <summary>
        /// Controller handling any User request
        /// </summary>
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

        [ThreadStatic]
        private static IFriendshipController _friendshipController;

        /// <summary>
        /// Controller handling any Friendship request
        /// </summary>
        public static IFriendshipController FriendshipController
        {
            get
            {
                if (_friendshipController == null)
                {
                    Initialize();
                }

                return _friendshipController;
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
            _friendshipController = TweetinviContainer.Resolve<IFriendshipController>();
        }

        #region User Factory

        public static IAuthenticatedUser GetAuthenticatedUser(ITwitterCredentials credentials = null, IGetAuthenticatedUserParameters parameters = null)
        {
            return UserFactory.GetAuthenticatedUser(credentials, parameters);
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
            return UserFactory.GenerateUserIdentifierFromId(userId);
        }

        public static IUserIdentifier GenerateUserIdentifierFromScreenName(string userScreenName)
        {
            return UserFactory.GenerateUserIdentifierFromScreenName(userScreenName);
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

        public static IEnumerable<ITweet> GetFavoriteTweets(IUserIdentifier userIdentifier, IGetUserFavoritesParameters parameters = null)
        {
            return UserController.GetFavoriteTweets(userIdentifier, parameters);
        }

        public static IEnumerable<ITweet> GetFavoriteTweets(long userId, IGetUserFavoritesParameters parameters = null)
        {
            return UserController.GetFavoriteTweets(new UserIdentifier(userId), parameters);
        }

        public static IEnumerable<ITweet> GetFavoriteTweets(string userScreenName, IGetUserFavoritesParameters parameters = null)
        {
            return UserController.GetFavoriteTweets(new UserIdentifier(userScreenName), parameters);
        }

        // Block User
        public static bool BlockUser(IUserIdentifier userIdentifier)
        {
            return UserController.BlockUser(userIdentifier);
        }

        public static bool BlockUser(long userId)
        {
            return UserController.BlockUser(userId);
        }

        public static bool BlockUser(string userScreenName)
        {
            return UserController.BlockUser(userScreenName);
        }
        
        // Un BlockUser User
        public static bool UnBlockUser(IUserIdentifier userIdentifier)
        {
        return UserController.UnBlockUser(userIdentifier);
        }

        public static bool UnBlockUser(long userId)
        {
        return UserController.UnBlockUser(userId);
        }

        public static bool UnBlockUser(string userScreenName)
        {
        return UserController.UnBlockUser(userScreenName);
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

        #region Friendship Controller

        // Follow
        public static bool FollowUser(IUserIdentifier user)
        {
            return FriendshipController.CreateFriendshipWith(user);
        }

        public static bool FollowUser(long userScreenName)
        {
            return FriendshipController.CreateFriendshipWith(userScreenName);
        }

        public static bool FollowUser(string userScreenName)
        {
            return FriendshipController.CreateFriendshipWith(userScreenName);
        }

        // Un Follow
        public static bool UnFollowUser(IUserIdentifier user)
        {
            return FriendshipController.DestroyFriendshipWith(user);
        }

        public static bool UnFollowUser(long userScreenName)
        {
            return FriendshipController.DestroyFriendshipWith(userScreenName);
        }

        public static bool UnFollowUser(string userScreenName)
        {
            return FriendshipController.DestroyFriendshipWith(userScreenName);
        }

        #endregion
    }
}
