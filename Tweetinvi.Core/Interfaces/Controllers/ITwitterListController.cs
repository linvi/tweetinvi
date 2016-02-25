using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface ITwitterListController
    {
        IEnumerable<ITwitterList> GetUserSubscribedLists(IUserIdentifier user, bool getOwnedListsFirst);
        IEnumerable<ITwitterList> GetUserSubscribedLists(long userId, bool getOwnedListsFirst);
        IEnumerable<ITwitterList> GetUserSubscribedLists(string userScreenName, bool getOwnedListsFirst);

        IEnumerable<ITwitterList> GetUserOwnedLists(long userId, int maximumNumberOfListsToRetrieve);
        IEnumerable<ITwitterList> GetUserOwnedLists(string userScreenName, int maximumNumberOfListsToRetrieve);
        IEnumerable<ITwitterList> GetUserOwnedLists(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve);

        ITwitterList UpdateList(long listId, ITwitterListUpdateParameters parameters);
        ITwitterList UpdateList(string slug, IUserIdentifier owner, ITwitterListUpdateParameters parameters);
        ITwitterList UpdateList(string slug, long ownerId, ITwitterListUpdateParameters parameters);
        ITwitterList UpdateList(string slug, string ownerScreenName, ITwitterListUpdateParameters parameters);
        ITwitterList UpdateList(ITwitterListIdentifier list, ITwitterListUpdateParameters parameters);

        bool DestroyList(long listId);
        bool DestroyList(string slug, IUserIdentifier owner);
        bool DestroyList(string slug, string ownerScreenName);
        bool DestroyList(string slug, long ownerId);
        bool DestroyList(ITwitterListIdentifier list);

        IEnumerable<ITweet> GetTweetsFromList(long listId);
        IEnumerable<ITweet> GetTweetsFromList(string slug, IUserIdentifier owner);
        IEnumerable<ITweet> GetTweetsFromList(string slug, string ownerScreenName);
        IEnumerable<ITweet> GetTweetsFromList(string slug, long ownerId);
        IEnumerable<ITweet> GetTweetsFromList(ITwitterListIdentifier list, IGetTweetsFromListParameters parameters = null);

        // Get List Members
        IEnumerable<IUser> GetListMembers(long listId, int maximumNumberOfUsersToRetrieve = 100);
        IEnumerable<IUser> GetListMembers(string slug, IUserIdentifier owner, int maximumNumberOfUsersToRetrieve = 100);
        IEnumerable<IUser> GetListMembers(string slug, string ownerScreenName, int maximumNumberOfUsersToRetrieve = 100);
        IEnumerable<IUser> GetListMembers(string slug, long ownerId, int maximumNumberOfUsersToRetrieve = 100);
        IEnumerable<IUser> GetListMembers(ITwitterListIdentifier list, int maximumNumberOfUsersToRetrieve = 100);

        // Create List Member
        bool AddMemberToList(long listId, long newUserId);
        bool AddMemberToList(long listId, string newUserName);
        bool AddMemberToList(long listId, IUserIdentifier newUser);

        bool AddMemberToList(string slug, long ownerId, long newUserId);
        bool AddMemberToList(string slug, long ownerId, string newUserName);
        bool AddMemberToList(string slug, long ownerId, IUserIdentifier newUser);

        bool AddMemberToList(string slug, string ownerScreenName, long newUserId);
        bool AddMemberToList(string slug, string ownerScreenName, string newUserName);
        bool AddMemberToList(string slug, string ownerScreenName, IUserIdentifier newUser);

        bool AddMemberToList(string slug, IUserIdentifier owner, long newUserId);
        bool AddMemberToList(string slug, IUserIdentifier owner, string newUserName);
        bool AddMemberToList(string slug, IUserIdentifier owner, IUserIdentifier newUser);

        bool AddMemberToList(ITwitterListIdentifier list, long newUserId);
        bool AddMemberToList(ITwitterListIdentifier list, string newUserName);
        bool AddMemberToList(ITwitterListIdentifier list, IUserIdentifier newUser);

        MultiRequestsResult AddMultipleMembersToList(long listId, IEnumerable<long> userIds);
        MultiRequestsResult AddMultipleMembersToList(long listId, IEnumerable<string> userScreenNames);
        MultiRequestsResult AddMultipleMembersToList(long listId, IEnumerable<IUserIdentifier> userIdentifiers);

        MultiRequestsResult AddMultipleMembersToList(string slug, long ownerId, IEnumerable<long> userIds);
        MultiRequestsResult AddMultipleMembersToList(string slug, long ownerId, IEnumerable<string> userScreenNames);
        MultiRequestsResult AddMultipleMembersToList(string slug, long ownerId, IEnumerable<IUserIdentifier> userIdentifiers);

        MultiRequestsResult AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<long> userIds);
        MultiRequestsResult AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<string> userScreenNames);
        MultiRequestsResult AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<IUserIdentifier> userIdentifiers);

        MultiRequestsResult AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<long> userIds);
        MultiRequestsResult AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<string> userScreenNames);
        MultiRequestsResult AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<IUserIdentifier> userIdentifiers);

        MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<long> userIds);
        MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<string> userScreenNames);
        MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> userIdentifiers);

        // Remove Member from List
        bool RemoveMemberFromList(long listId, long newUserId);
        bool RemoveMemberFromList(long listId, string newUserName);
        bool RemoveMemberFromList(long listId, IUserIdentifier newUser);

        bool RemoveMemberFromList(string slug, long ownerId, long newUserId);
        bool RemoveMemberFromList(string slug, long ownerId, string newUserName);
        bool RemoveMemberFromList(string slug, long ownerId, IUserIdentifier newUser);

        bool RemoveMemberFromList(string slug, string ownerScreenName, long newUserId);
        bool RemoveMemberFromList(string slug, string ownerScreenName, string newUserName);
        bool RemoveMemberFromList(string slug, string ownerScreenName, IUserIdentifier newUser);

        bool RemoveMemberFromList(string slug, IUserIdentifier owner, long newUserId);
        bool RemoveMemberFromList(string slug, IUserIdentifier owner, string newUserName);
        bool RemoveMemberFromList(string slug, IUserIdentifier owner, IUserIdentifier newUser);

        bool RemoveMemberFromList(ITwitterListIdentifier list, long newUserId);
        bool RemoveMemberFromList(ITwitterListIdentifier list, string newUserName);
        bool RemoveMemberFromList(ITwitterListIdentifier list, IUserIdentifier newUser);

        // Remove Multiple Members
        MultiRequestsResult RemoveMultipleMembersFromList(long listId, IEnumerable<long> userIds);
        MultiRequestsResult RemoveMultipleMembersFromList(long listId, IEnumerable<string> userScreenNames);
        MultiRequestsResult RemoveMultipleMembersFromList(long listId, IEnumerable<IUserIdentifier> userIdentifiers);

        MultiRequestsResult RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<long> userIds);
        MultiRequestsResult RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<string> userScreenNames);
        MultiRequestsResult RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<IUserIdentifier> userIdentifiers);

        MultiRequestsResult RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<long> userIds);
        MultiRequestsResult RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<string> userScreenNames);
        MultiRequestsResult RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<IUserIdentifier> userIdentifiers);

        MultiRequestsResult RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<long> userIds);
        MultiRequestsResult RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<string> userScreenNames);
        MultiRequestsResult RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<IUserIdentifier> userIdentifiers);

        MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<long> userIds);
        MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<string> userScreenNames);
        MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> userIdentifiers);

        // Check Membership
        bool CheckIfUserIsAListMember(long listId, long userId);
        bool CheckIfUserIsAListMember(long listId, string userScreenName);
        bool CheckIfUserIsAListMember(long listId, IUserIdentifier userIdentifier);
        
        bool CheckIfUserIsAListMember(string slug, long ownerId, long userId);
        bool CheckIfUserIsAListMember(string slug, long ownerId, string userScreenName);
        bool CheckIfUserIsAListMember(string slug, long ownerId, IUserIdentifier userIdentifier);
        
        bool CheckIfUserIsAListMember(string slug, string ownerScreenName, long userId);
        bool CheckIfUserIsAListMember(string slug, string ownerScreenName, string userScreenName);
        bool CheckIfUserIsAListMember(string slug, string ownerScreenName, IUserIdentifier userIdentifier);

        bool CheckIfUserIsAListMember(string slug, IUserIdentifier owner, long userId);
        bool CheckIfUserIsAListMember(string slug, IUserIdentifier owner, string userScreenName);
        bool CheckIfUserIsAListMember(string slug, IUserIdentifier owner, IUserIdentifier userIdentifier);

        bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, long userId);
        bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, string userScreenName);
        bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier);

        // Subscriptions
        IEnumerable<ITwitterList> GetUserSubscribedLists(long userId, int maxNumberOfListsToRetrieve);
        IEnumerable<ITwitterList> GetUserSubscribedLists(string userName, int maxNumberOfListsToRetrieve);
        IEnumerable<ITwitterList> GetUserSubscribedLists(IUserIdentifier user, int maxNumberOfListsToRetrieve);

        // Get List subscribers
        IEnumerable<IUser> GetListSubscribers(long listId, int maximumNumberOfUsersToRetrieve = 100);
        IEnumerable<IUser> GetListSubscribers(string slug, IUserIdentifier owner, int maximumNumberOfUsersToRetrieve = 100);
        IEnumerable<IUser> GetListSubscribers(string slug, string ownerScreenName, int maximumNumberOfUsersToRetrieve = 100);
        IEnumerable<IUser> GetListSubscribers(string slug, long ownerId, int maximumNumberOfUsersToRetrieve = 100);
        IEnumerable<IUser> GetListSubscribers(ITwitterListIdentifier list, int maximumNumberOfUsersToRetrieve = 100);

        // Add subscribers
        bool SubscribeAuthenticatedUserToList(long listId);
        bool SubscribeAuthenticatedUserToList(string slug, long ownerId);
        bool SubscribeAuthenticatedUserToList(string slug, string ownerScreenName);
        bool SubscribeAuthenticatedUserToList(string slug, IUserIdentifier owner);
        bool SubscribeAuthenticatedUserToList(ITwitterListIdentifier list);

        // Remove subscriber from List
        bool UnSubscribeAuthenticatedUserFromList(long listId);
        bool UnSubscribeAuthenticatedUserFromList(string slug, long ownerId);
        bool UnSubscribeAuthenticatedUserFromList(string slug, string ownerScreenName);
        bool UnSubscribeAuthenticatedUserFromList(string slug, IUserIdentifier owner);
        bool UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier list);

        // Check Subscription
        bool CheckIfUserIsAListSubscriber(long listId, long userId);
        bool CheckIfUserIsAListSubscriber(long listId, string userScreenName);
        bool CheckIfUserIsAListSubscriber(long listId, IUserIdentifier userIdentifier);

        bool CheckIfUserIsAListSubscriber(string slug, long ownerId, long userId);
        bool CheckIfUserIsAListSubscriber(string slug, long ownerId, string userScreenName);
        bool CheckIfUserIsAListSubscriber(string slug, long ownerId, IUserIdentifier userIdentifier);

        bool CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, long userId);
        bool CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, string userScreenName);
        bool CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, IUserIdentifier userIdentifier);

        bool CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, long userId);
        bool CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, string userScreenName);
        bool CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, IUserIdentifier userIdentifier);

        bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, long userId);
        bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, string userScreenName);
        bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier);
    }
}