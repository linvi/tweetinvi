using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    /// <summary>
    /// Access and manage all the information related with users.
    /// </summary>
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

        /// <summary>
        /// Generate a user from a Data Transfer Object.
        /// </summary>
        public static IUser GenerateUserFromDTO(IUserDTO userDTO)
        {
            return UserFactory.GenerateUserFromDTO(userDTO);
        }

        /// <summary>
        /// Generate a collection of users from a Data Transfer Objects.
        /// </summary>
        public static IEnumerable<IUser> GenerateUsersFromDTO(IEnumerable<IUserDTO> usersDTO)
        {
            return UserFactory.GenerateUsersFromDTO(usersDTO);
        }

        #endregion

        #region User Controller

        // Favourites

        /// <summary>
        /// Get tweets favorited by a specific user.
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters = null)
        {
            return UserController.GetFavoriteTweets(user, parameters);
        }

        /// <summary>
        /// Get tweets favorited by a specific user.
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetFavoriteTweets(long userId, IGetUserFavoritesParameters parameters = null)
        {
            return UserController.GetFavoriteTweets(new UserIdentifier(userId), parameters);
        }

        /// <summary>
        /// Get tweets favorited by a specific user.
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetFavoriteTweets(string userScreenName, IGetUserFavoritesParameters parameters = null)
        {
            return UserController.GetFavoriteTweets(new UserIdentifier(userScreenName), parameters);
        }

        // Block User

        /// <summary>
        /// Block a user on the authenticated account.
        /// </summary>
        public static Task<bool> BlockUser(IUserIdentifier user)
        {
            return UserController.BlockUser(user);
        }

        /// <summary>
        /// Block a user on the authenticated account.
        /// </summary>
        public static Task<bool> BlockUser(long userId)
        {
            return UserController.BlockUser(userId);
        }

        /// <summary>
        /// Block a user on the authenticated account.
        /// </summary>
        public static Task<bool> BlockUser(string userScreenName)
        {
            return UserController.BlockUser(userScreenName);
        }

        // Un BlockUser User

        /// <summary>
        /// Unblock a user on the authenticated account.
        /// </summary>
        public static Task<bool> UnBlockUser(IUserIdentifier user)
        {
            return UserController.UnBlockUser(user);
        }

        /// <summary>
        /// Unblock a user on the authenticated account.
        /// </summary>
        public static Task<bool> UnBlockUser(long userId)
        {
            return UserController.UnBlockUser(userId);
        }

        /// <summary>
        /// Unblock a user on the authenticated account.
        /// </summary>
        public static Task<bool> UnBlockUser(string userScreenName)
        {
            return UserController.UnBlockUser(userScreenName);
        }

        // Stream Profile Image 

        /// <summary>
        /// Get a stream to download a user profile image.
        /// </summary>
        public static System.IO.Stream GetProfileImageStream(IUser user, ImageSize imageSize = ImageSize.normal)
        {
            return UserController.GetProfileImageStream(user, imageSize);
        }

        /// <summary>
        /// Get a stream to download a user profile image.
        /// </summary>
        public static System.IO.Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            return UserController.GetProfileImageStream(userDTO, imageSize);
        }

        // Spam

        /// <summary>
        /// Report a user for spam.
        /// </summary>
        public static Task<bool> ReportUserForSpam(IUserIdentifier user)
        {
            return UserController.ReportUserForSpam(user);
        }

        public static Task<bool> ReportUserForSpam(long userId)
        {
            return UserController.ReportUserForSpam(userId);
        }

        /// <summary>
        /// Report a user for spam.
        /// </summary>
        public static Task<bool> ReportUserForSpam(string userScreenName)
        {
            return UserController.ReportUserForSpam(userScreenName);
        }

        #endregion

        #region Friendship Controller

        // Follow

        /// <summary>
        /// Follow a user.
        /// </summary>
        public static Task<bool> FollowUser(IUserIdentifier user)
        {
            return FriendshipController.CreateFriendshipWith(user);
        }

        /// <summary>
        /// Follow a user.
        /// </summary>
        public static Task<bool> FollowUser(long userScreenName)
        {
            return FriendshipController.CreateFriendshipWith(userScreenName);
        }

        /// <summary>
        /// Follow a user.
        /// </summary>
        public static Task<bool> FollowUser(string userScreenName)
        {
            return FriendshipController.CreateFriendshipWith(userScreenName);
        }

        // Un Follow

        /// <summary>
        /// Unfollow a user.
        /// </summary>
        public static Task<bool> UnFollowUser(IUserIdentifier user)
        {
            return FriendshipController.DestroyFriendshipWith(user);
        }

        /// <summary>
        /// Unfollow a user.
        /// </summary>
        public static Task<bool> UnFollowUser(long userScreenName)
        {
            return FriendshipController.DestroyFriendshipWith(userScreenName);
        }

        /// <summary>
        /// Unfollow a user.
        /// </summary>
        public static Task<bool> UnFollowUser(string userScreenName)
        {
            return FriendshipController.DestroyFriendshipWith(userScreenName);
        }

        #endregion
    }
}