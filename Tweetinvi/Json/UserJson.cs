using System;
using System.Collections.Generic;
using Tweetinvi.Controllers.User;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

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
        public static IEnumerable<string> GetFriendIds(IUserIdentifier user, int maxFriendsToRetrieve = 5000)
        {
            return UserJsonController.GetFriendIds(user, maxFriendsToRetrieve);
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
        public static IEnumerable<string> GetFollowerIds(IUserIdentifier user, int maxFollowersToRetrieve = 5000)
        {
            return UserJsonController.GetFollowerIds(user, maxFollowersToRetrieve);
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
        
        public static string GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters = null)
        {
            return UserJsonController.GetFavoriteTweets(user, parameters);
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