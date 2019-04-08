using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Controllers
{
    public interface IFriendshipController
    {
        Task<IEnumerable<long>> GetUserIdsRequestingFriendship(
            int maximumUserIdsToRetrieve = TweetinviConsts.FRIENDSHIPS_INCOMING_IDS_MAX_PER_REQ);
        Task<IEnumerable<IUser>> GetUsersRequestingFriendship(
            int maximumUsersToRetrieve = TweetinviConsts.FRIENDSHIPS_INCOMING_USERS_MAX_PER_REQ);

        Task<IEnumerable<long>> GetUserIdsYouRequestedToFollow(
            int maximumUsersToRetrieve = TweetinviConsts.FRIENDSHIPS_OUTGOING_IDS_MAX_PER_REQ);
        Task<IEnumerable<IUser>> GetUsersYouRequestedToFollow(
            int maximumUsersToRetrieve = TweetinviConsts.FRIENDSHIPS_OUTGOING_USERS_MAX_PER_REQ);

        // Create Friendship with
        Task<bool> CreateFriendshipWith(IUserIdentifier user);
        Task<bool> CreateFriendshipWith(long userId);
        Task<bool> CreateFriendshipWith(string userScreeName);

        // Destroy Friendship with
        Task<bool> DestroyFriendshipWith(IUserIdentifier user);
        Task<bool> DestroyFriendshipWith(long userId);
        Task<bool> DestroyFriendshipWith(string userScreeName);

        // Update Friendship Authorizations
        Task<bool> UpdateRelationshipAuthorizationsWith(IUserIdentifier user, bool retweetsEnabled, bool deviceNotificationEnabled);
        Task<bool> UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotificationEnabled);
        Task<bool> UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotificationEnabled);

        // Relationship
        Task<IRelationshipDetails> GetRelationshipBetween(IUserIdentifier sourceUserIdentifier, IUserIdentifier targetUserIdentifier);

        Task<IRelationshipDetails> GetRelationshipBetween(IUserIdentifier sourceUserIdentifier, long targetUserId);
        Task<IRelationshipDetails> GetRelationshipBetween(IUserIdentifier sourceUserIdentifier, string targetUserScreenName);
        Task<IRelationshipDetails> GetRelationshipBetween(long sourceUserId, IUserIdentifier targetUserIdentifier);
        Task<IRelationshipDetails> GetRelationshipBetween(string sourceUserScreenName, IUserIdentifier targetUserIdentifier);

        Task<IRelationshipDetails> GetRelationshipBetween(long sourceUserId, long targetUserId);
        Task<IRelationshipDetails> GetRelationshipBetween(long sourceUserId, string targetUserScreenName);

        Task<IRelationshipDetails> GetRelationshipBetween(string sourceUserScreenName, string targetUserScreenName);
        Task<IRelationshipDetails> GetRelationshipBetween(string sourceUserScreenName, long targetUserId);

        // Get Relationships between the current user and a list of users
        Task<Dictionary<IUser, IRelationshipState>> GetRelationshipStatesAssociatedWith(IEnumerable<IUser> targetUsers);

        Task<IEnumerable<IRelationshipState>> GetMultipleRelationships(IEnumerable<IUserIdentifier> targetUserIdentifiers);
        Task<IEnumerable<IRelationshipState>> GetMultipleRelationships(IEnumerable<long> targetUsersId);
        Task<IEnumerable<IRelationshipState>> GetMultipleRelationships(IEnumerable<string> targetUsersScreenName);

        // Retweet Muted Friends
        Task<long[]> GetUserIdsWhoseRetweetsAreMuted();
        Task<IEnumerable<IUser>> GetUsersWhoseRetweetsAreMuted();
    }
}