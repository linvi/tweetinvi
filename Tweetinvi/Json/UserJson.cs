using System;
using System.Collections.Generic;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Json
{
    public static class UserJson
    {
        [ThreadStatic]
        private static IUserJsonController _userJsonController;

        static UserJson()
        {
            Initialize();
        }

        public static IUserJsonController UserJsonController
        {
            get
            {
                if (_userJsonController == null)
                {
                    Initialize();
                }
                
                return _userJsonController;
            }
        }

        private static void Initialize()
        {
            _userJsonController = TweetinviContainer.Resolve<IUserJsonController>();
        }

        // Friends
        public static IEnumerable<string> GetFriendIds(IUser user, int maxFriendsToRetrieve = 5000)
        {
            return UserJsonController.GetFriendIds(user, maxFriendsToRetrieve);
        }

        public static IEnumerable<string> GetFriendIds(IUserIdentifier userDTO, int maxFriendsToRetrieve = 5000)
        {
            return UserJsonController.GetFriendIds(userDTO, maxFriendsToRetrieve);
        }

        public static IEnumerable<string> GetFriendIds(long userId, int maxFriendsToRetrieve = 5000)
        {
            return UserJsonController.GetFriendIds(userId, maxFriendsToRetrieve);
        }

        public static IEnumerable<string> GetFriendIds(string userScreenName, int maxFriendsToRetrieve = 5000)
        {
            return UserJsonController.GetFriendIds(userScreenName, maxFriendsToRetrieve);
        }

        // Followers
        public static IEnumerable<string> GetFollowerIds(IUser user, int maxFollowersToRetrieve = 5000)
        {
            return UserJsonController.GetFollowerIds(user, maxFollowersToRetrieve);
        }

        public static IEnumerable<string> GetFollowerIds(IUserIdentifier userDTO, int maxFollowersToRetrieve = 5000)
        {
            return UserJsonController.GetFollowerIds(userDTO, maxFollowersToRetrieve);
        }

        public static IEnumerable<string> GetFollowerIds(long userId, int maxFollowersToRetrieve = 5000)
        {
            return UserJsonController.GetFollowerIds(userId, maxFollowersToRetrieve);
        }

        public static IEnumerable<string> GetFollowerIds(string userScreenName, int maxFollowersToRetrieve = 5000)
        {
            return UserJsonController.GetFollowerIds(userScreenName, maxFollowersToRetrieve);
        }

        // Favorites
        
        public static string GetFavoriteTweets(IUserIdentifier userIdentifier, IGetUserFavoritesParameters parameters = null)
        {
            return UserJsonController.GetFavoriteTweets(userIdentifier, parameters);
        }

        public static string GetFavoriteTweets(long userId, IGetUserFavoritesParameters parameters = null)
        {
            return UserJsonController.GetFavoriteTweets(new UserIdentifier(userId), parameters);
        }

        public static string GetFavoriteTweets(string userScreenName, IGetUserFavoritesParameters parameters = null)
        {
            return UserJsonController.GetFavoriteTweets(new UserIdentifier(userScreenName), parameters);
        }
    }
}