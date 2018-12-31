using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public static class FriendshipAsync
    {
        // Get Relationship Between
        public static Task<IRelationshipDetails> GetRelationshipDetailsBetween(IUserIdentifier sourceUserIdentifier, IUserIdentifier targetUserIdentifier)
        {
            return Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserIdentifier, targetUserIdentifier));
        }

        public static Task<IRelationshipDetails> GetRelationshipDetailsBetween(IUserIdentifier sourceUserIdentifier, long targetUserId)
        {
            return Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserIdentifier, targetUserId));
        }

        public static Task<IRelationshipDetails> GetRelationshipDetailsBetween(IUserIdentifier sourceUserIdentifier, string targetUserScreenName)
        {
            return Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserIdentifier, targetUserScreenName));
        }

        public static Task<IRelationshipDetails> GetRelationshipDetailsBetween(long sourceUserId, IUserIdentifier targetUserIdentifier)
        {
            return Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserId, targetUserIdentifier));
        }

        public static Task<IRelationshipDetails> GetRelationshipDetailsBetween(string sourceUserScreenName, IUserIdentifier targetUserIdentifier)
        {
            return Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserScreenName, targetUserIdentifier));
        }

        public static Task<IRelationshipDetails> GetRelationshipDetailsBetween(long sourceUserId, long targetUserId)
        {
            return Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserId, targetUserId));
        }

        public static Task<IRelationshipDetails> GetRelationshipDetailsBetween(long sourceUserId, string targetUserScreenName)
        {
            return Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserId, targetUserScreenName));
        }

        public static Task<IRelationshipDetails> GetRelationshipDetailsBetween(string sourceUserScreenName, long targetUserId)
        {
            return Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserScreenName, targetUserId));
        }

        public static Task<IRelationshipDetails> GetRelationshipDetailsBetween(string sourceUserScreenName, string targetUserScreenName)
        {
            return Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserScreenName, targetUserScreenName));
        }
    }
}