using System;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Models;

namespace Tweetinvi
{
    /// <summary>
    /// Manage friendships between users.
    /// </summary>
    public static class Friendship
    {
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

        static Friendship()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _friendshipController = TweetinviContainer.Resolve<IFriendshipController>();
        }

        // Get Relationship Between

        /// <summary>
        /// Get relationship information between 2 different users.
        /// </summary>
        public static IRelationshipDetails GetRelationshipDetailsBetween(IUserIdentifier sourceUserIdentifier, IUserIdentifier targetUserIdentifier)
        {
            return FriendshipController.GetRelationshipBetween(sourceUserIdentifier, targetUserIdentifier);
        }

        /// <summary>
        /// Get relationship information between 2 different users.
        /// </summary>
        public static IRelationshipDetails GetRelationshipDetailsBetween(IUserIdentifier sourceUserIdentifier, long targetUserId)
        {
            return FriendshipController.GetRelationshipBetween(sourceUserIdentifier, targetUserId);
        }

        /// <summary>
        /// Get relationship information between 2 different users.
        /// </summary>
        public static IRelationshipDetails GetRelationshipDetailsBetween(IUserIdentifier sourceUserIdentifier, string targetUserScreenName)
        {
            return FriendshipController.GetRelationshipBetween(sourceUserIdentifier, targetUserScreenName);
        }

        /// <summary>
        /// Get relationship information between 2 different users.
        /// </summary>
        public static IRelationshipDetails GetRelationshipDetailsBetween(long sourceUserId, IUserIdentifier targetUserIdentifier)
        {
            return FriendshipController.GetRelationshipBetween(sourceUserId, targetUserIdentifier);
        }

        /// <summary>
        /// Get relationship information between 2 different users.
        /// </summary>
        public static IRelationshipDetails GetRelationshipDetailsBetween(string sourceUserScreenName, IUserIdentifier targetUserIdentifier)
        {
            return FriendshipController.GetRelationshipBetween(sourceUserScreenName, targetUserIdentifier);
        }

        /// <summary>
        /// Get relationship information between 2 different users.
        /// </summary>
        public static IRelationshipDetails GetRelationshipDetailsBetween(long sourceUserId, long targetUserId)
        {
            return FriendshipController.GetRelationshipBetween(sourceUserId, targetUserId);
        }

        /// <summary>
        /// Get relationship information between 2 different users.
        /// </summary>
        public static IRelationshipDetails GetRelationshipDetailsBetween(long sourceUserId, string targetUserScreenName)
        {
            return FriendshipController.GetRelationshipBetween(sourceUserId, targetUserScreenName);
        }

        /// <summary>
        /// Get relationship information between 2 different users.
        /// </summary>
        public static IRelationshipDetails GetRelationshipDetailsBetween(string sourceUserScreenName, long targetUserId)
        {
            return FriendshipController.GetRelationshipBetween(sourceUserScreenName, targetUserId);
        }

        /// <summary>
        /// Get relationship information between 2 different users.
        /// </summary>
        public static IRelationshipDetails GetRelationshipDetailsBetween(string sourceUserScreenName, string targetUserScreenName)
        {
            return FriendshipController.GetRelationshipBetween(sourceUserScreenName, targetUserScreenName);
        }
    }
}