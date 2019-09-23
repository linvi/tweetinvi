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

        #endregion
    }
}