using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Friendship;
using Tweetinvi.Models;

namespace Tweetinvi.Json
{
    public static class FriendshipJson
    {
        [ThreadStatic]
        private static IFriendshipJsonController _friendshipJsonController;
        public static IFriendshipJsonController FriendshipJsonController
        {
            get { return _friendshipJsonController; }
        }

        static FriendshipJson()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _friendshipJsonController = TweetinviContainer.Resolve<IFriendshipJsonController>();
        }

        public static Task<IEnumerable<string>> GetUserIdsRequestingFriendship()
        {
            return FriendshipJsonController.GetUserIdsRequestingFriendship();
        }

        public static Task<IEnumerable<string>> GetUserIdsYouRequestedToFollow()
        {
            return FriendshipJsonController.GetUserIdsYouRequestedToFollow();
        }

        public static Task<string> CreateFriendshipWith(IUserIdentifier user)
        {
            return FriendshipJsonController.CreateFriendshipWith(user);
        }

        public static Task<string> CreateFriendshipWith(long userId)
        {
            return FriendshipJsonController.CreateFriendshipWith(userId);
        }

        public static Task<string> CreateFriendshipWith(string userScreeName)
        {
            return FriendshipJsonController.CreateFriendshipWith(userScreeName);
        }

        public static Task<string> DestroyFriendshipWith(IUserIdentifier user)
        {
            return FriendshipJsonController.DestroyFriendshipWith(user);
        }

        public static Task<string> DestroyFriendshipWith(long userId)
        {
            return FriendshipJsonController.DestroyFriendshipWith(userId);
        }

        public static Task<string> DestroyFriendshipWith(string userScreeName)
        {
            return FriendshipJsonController.DestroyFriendshipWith(userScreeName);
        }

        public static Task<string> UpdateRelationshipAuthorizationsWith(IUserIdentifier user, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return FriendshipJsonController.UpdateRelationshipAuthorizationsWith(user, retweetsEnabled, deviceNotifictionEnabled);
        }

        public static Task<string> UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return FriendshipJsonController.UpdateRelationshipAuthorizationsWith(userId, retweetsEnabled, deviceNotifictionEnabled);
        }

        public static Task<string> UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return FriendshipJsonController.UpdateRelationshipAuthorizationsWith(userScreenName, retweetsEnabled, deviceNotifictionEnabled);
        }
    }
}