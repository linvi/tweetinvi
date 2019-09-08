using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        // Favorites
        
        public static Task<string> GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters = null)
        {
            return UserJsonController.GetFavoriteTweets(user, parameters);
        }

        public static Task<string> GetFavoriteTweets(long userId, IGetUserFavoritesParameters parameters = null)
        {
            return UserJsonController.GetFavoriteTweets(new UserIdentifier(userId), parameters);
        }

        public static Task<string> GetFavoriteTweets(string userScreenName, IGetUserFavoritesParameters parameters = null)
        {
            return UserJsonController.GetFavoriteTweets(new UserIdentifier(userScreenName), parameters);
        }
    }
}