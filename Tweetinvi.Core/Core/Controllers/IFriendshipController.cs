using System.Collections.Generic;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Controllers
{
    public interface IFriendshipController
    {
        IEnumerable<long> GetUserIdsRequestingFriendship(
            int maximumUserIdsToRetrieve = TweetinviConsts.FRIENDSHIPS_INCOMING_IDS_MAX_PER_REQ);
        IEnumerable<IUser> GetUsersRequestingFriendship(
            int maximumUsersToRetrieve = TweetinviConsts.FRIENDSHIPS_INCOMING_USERS_MAX_PER_REQ);

        IEnumerable<long> GetUserIdsYouRequestedToFollow(
            int maximumUsersToRetrieve = TweetinviConsts.FRIENDSHIPS_OUTGOING_IDS_MAX_PER_REQ);
        IEnumerable<IUser> GetUsersYouRequestedToFollow(
            int maximumUsersToRetrieve = TweetinviConsts.FRIENDSHIPS_OUTGOING_USERS_MAX_PER_REQ);

        // Create Friendship with
        bool CreateFriendshipWith(IUserIdentifier user);
        bool CreateFriendshipWith(long userId);
        bool CreateFriendshipWith(string userScreeName);

        // Destroy Friendship with
        bool DestroyFriendshipWith(IUserIdentifier user);
        bool DestroyFriendshipWith(long userId);
        bool DestroyFriendshipWith(string userScreeName);

        // Update Friendship Authorizations
        bool UpdateRelationshipAuthorizationsWith(IUserIdentifier user, bool retweetsEnabled, bool deviceNotificationEnabled);
        bool UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotificationEnabled);
        bool UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotificationEnabled);

        // Relationship
        IRelationshipDetails GetRelationshipBetween(IUserIdentifier sourceUserIdentifier, IUserIdentifier targetUserIdentifier);

        IRelationshipDetails GetRelationshipBetween(IUserIdentifier sourceUserIdentifier, long targetUserId);
        IRelationshipDetails GetRelationshipBetween(IUserIdentifier sourceUserIdentifier, string targetUserScreenName);
        IRelationshipDetails GetRelationshipBetween(long sourceUserId, IUserIdentifier targetUserIdentifier);
        IRelationshipDetails GetRelationshipBetween(string sourceUserScreenName, IUserIdentifier targetUserIdentifier);

        IRelationshipDetails GetRelationshipBetween(long sourceUserId, long targetUserId);
        IRelationshipDetails GetRelationshipBetween(long sourceUserId, string targetUserScreenName);

        IRelationshipDetails GetRelationshipBetween(string sourceUserScreenName, string targetUserScreenName);
        IRelationshipDetails GetRelationshipBetween(string sourceUserScreenName, long targetUserId);

        // Get Relationships between the current user and a list of users
        Dictionary<IUser, IRelationshipState> GetRelationshipStatesAssociatedWith(IEnumerable<IUser> targetUsers);

        IEnumerable<IRelationshipState> GetMultipleRelationships(IEnumerable<IUserIdentifier> targetUserIdentifiers);
        IEnumerable<IRelationshipState> GetMultipleRelationships(IEnumerable<long> targetUsersId);
        IEnumerable<IRelationshipState> GetMultipleRelationships(IEnumerable<string> targetUsersScreenName);

        // Retweet Muted Friends
        IEnumerable<long> GetUserIdsWhoseRetweetsAreMuted();
        IEnumerable<IUser> GetUsersWhoseRetweetsAreMuted();
    }
}