using System.Collections.Generic;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Controllers
{
    public interface IFriendshipController
    {
        IEnumerable<long> GetUserIdsRequestingFriendship(int maximumUserIdsToRetrieve = 75000);
        IEnumerable<IUser> GetUsersRequestingFriendship(int maximumUsersToRetrieve = 75000);

        IEnumerable<long> GetUserIdsYouRequestedToFollow(int maximumUsersToRetrieve = 75000);
        IEnumerable<IUser> GetUsersYouRequestedToFollow(int maximumUsersToRetrieve = 75000);

        // Create Friendship with
        bool CreateFriendshipWith(IUser user);
        bool CreateFriendshipWith(IUserIdentifier userIdentifier);
        bool CreateFriendshipWith(long userId);
        bool CreateFriendshipWith(string userScreeName);

        // Destroy Friendship with
        bool DestroyFriendshipWith(IUserIdentifier userIdentifier);
        bool DestroyFriendshipWith(long userId);
        bool DestroyFriendshipWith(string userScreeName);

        // Update Friendship Authorizations
        bool UpdateRelationshipAuthorizationsWith(IUser user, bool retweetsEnabled, bool deviceNotifictionEnabled);
        bool UpdateRelationshipAuthorizationsWith(IUserIdentifier userIdentifier, bool retweetsEnabled, bool deviceNotifictionEnabled);
        bool UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled);
        bool UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled);

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