using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi
{
    public static class FriendshipAsync
    {
        // Get Relationship Between
        public static async Task<IRelationshipDetails> GetRelationshipDetailsBetween(IUserIdentifier sourceUserIdentifier, IUserIdentifier targetUserIdentifier)
        {
            return await Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserIdentifier, targetUserIdentifier));
        }

        public static async Task<IRelationshipDetails> GetRelationshipDetailsBetween(IUserIdentifier sourceUserIdentifier, long targetUserId)
        {
            return await Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserIdentifier, targetUserId));
        }

        public static async Task<IRelationshipDetails> GetRelationshipDetailsBetween(IUserIdentifier sourceUserIdentifier, string targetUserScreenName)
        {
            return await Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserIdentifier, targetUserScreenName));
        }

        public static async Task<IRelationshipDetails> GetRelationshipDetailsBetween(long sourceUserId, IUserIdentifier targetUserIdentifier)
        {
            return await Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserId, targetUserIdentifier));
        }

        public static async Task<IRelationshipDetails> GetRelationshipDetailsBetween(string sourceUserScreenName, IUserIdentifier targetUserIdentifier)
        {
            return await Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserScreenName, targetUserIdentifier));
        }

        public static async Task<IRelationshipDetails> GetRelationshipDetailsBetween(long sourceUserId, long targetUserId)
        {
            return await Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserId, targetUserId));
        }

        public static async Task<IRelationshipDetails> GetRelationshipDetailsBetween(long sourceUserId, string targetUserScreenName)
        {
            return await Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserId, targetUserScreenName));
        }

        public static async Task<IRelationshipDetails> GetRelationshipDetailsBetween(string sourceUserScreenName, long targetUserId)
        {
            return await Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserScreenName, targetUserId));
        }

        public static async Task<IRelationshipDetails> GetRelationshipDetailsBetween(string sourceUserScreenName, string targetUserScreenName)
        {
            return await Sync.ExecuteTaskAsync(() => Friendship.GetRelationshipDetailsBetween(sourceUserScreenName, targetUserScreenName));
        }
    }
}