using System;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi
{
    public static class Friendship
    {
        [ThreadStatic]
        private static IFriendshipController _friendshipController;
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

        static Friendship()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _friendshipController = TweetinviContainer.Resolve<IFriendshipController>();
        }

        // Get Relationship Between
        public static IRelationshipDetails GetRelationshipDetailsBetween(IUserIdentifier sourceUserIdentifier, IUserIdentifier targetUserIdentifier)
        {
            return _friendshipController.GetRelationshipBetween(sourceUserIdentifier, targetUserIdentifier);
        }

        public static IRelationshipDetails GetRelationshipDetailsBetween(IUserIdentifier sourceUserIdentifier, long targetUserId)
        {
            return _friendshipController.GetRelationshipBetween(sourceUserIdentifier, targetUserId);
        }

        public static IRelationshipDetails GetRelationshipDetailsBetween(IUserIdentifier sourceUserIdentifier, string targetUserScreenName)
        {
            return _friendshipController.GetRelationshipBetween(sourceUserIdentifier, targetUserScreenName);
        }

        public static IRelationshipDetails GetRelationshipDetailsBetween(long sourceUserId, IUserIdentifier targetUserIdentifier)
        {
            return _friendshipController.GetRelationshipBetween(sourceUserId, targetUserIdentifier);
        }

        public static IRelationshipDetails GetRelationshipDetailsBetween(string sourceUserScreenName, IUserIdentifier targetUserIdentifier)
        {
            return _friendshipController.GetRelationshipBetween(sourceUserScreenName, targetUserIdentifier);
        }

        public static IRelationshipDetails GetRelationshipDetailsBetween(long sourceUserId, long targetUserId)
        {
            return _friendshipController.GetRelationshipBetween(sourceUserId, targetUserId);
        }

        public static IRelationshipDetails GetRelationshipDetailsBetween(long sourceUserId, string targetUserScreenName)
        {
            return _friendshipController.GetRelationshipBetween(sourceUserId, targetUserScreenName);
        }

        public static IRelationshipDetails GetRelationshipDetailsBetween(string sourceUserScreenName, long targetUserId)
        {
            return _friendshipController.GetRelationshipBetween(sourceUserScreenName, targetUserId);
        }

        public static IRelationshipDetails GetRelationshipDetailsBetween(string sourceUserScreenName, string targetUserScreenName)
        {
            return _friendshipController.GetRelationshipBetween(sourceUserScreenName, targetUserScreenName);
        }
    }
}
