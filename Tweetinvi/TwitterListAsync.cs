using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class TwitterListAsync
    {
        // Get Existing List
        public static ConfiguredTaskAwaitable<ITwitterList> GetExistingList(ITwitterListIdentifier twitterListIdentifier)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(twitterListIdentifier));
        }

        public static ConfiguredTaskAwaitable<ITwitterList> GetExistingList(long listId)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(listId));
        }

        public static ConfiguredTaskAwaitable<ITwitterList> GetExistingList(string slug, IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(slug, user));
        }

        public static ConfiguredTaskAwaitable<ITwitterList> GetExistingList(string slug, long userId)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(slug, userId));
        }

        public static ConfiguredTaskAwaitable<ITwitterList> GetExistingList(string slug, string userScreenName)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(slug, userScreenName));
        }

        // Owned Lists
        public static ConfiguredTaskAwaitable<IEnumerable<ITwitterList>> GetUserOwnedLists(long userId, int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_OWNED_COUNT)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetUserOwnedLists(userId, maximumNumberOfListsToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITwitterList>> GetUserOwnedLists(string userScreenName, int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_OWNED_COUNT)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetUserOwnedLists(userScreenName, maximumNumberOfListsToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITwitterList>> GetUserOwnedLists(IUserIdentifier user, int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_OWNED_COUNT)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetUserOwnedLists(user, maximumNumberOfListsToRetrieve));
        }

        // Create List
        public static ConfiguredTaskAwaitable<ITwitterList> CreateList(string name, PrivacyMode privacyMode, string description = null)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.CreateList(name, privacyMode, description));
        }

        // Update List
        public static ConfiguredTaskAwaitable<ITwitterList> UpdateList(ITwitterListIdentifier twitterList, ITwitterListUpdateParameters parameters)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(twitterList, parameters));
        }

        public static ConfiguredTaskAwaitable<ITwitterList> UpdateList(long listId, ITwitterListUpdateParameters parameters)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(listId, parameters));
        }

        public static ConfiguredTaskAwaitable<ITwitterList> UpdateList(string slug, IUserIdentifier owner, ITwitterListUpdateParameters parameters)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(slug, owner, parameters));
        }

        public static ConfiguredTaskAwaitable<ITwitterList> UpdateList(string slug, long ownerId, ITwitterListUpdateParameters parameters)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(slug, ownerId, parameters));
        }

        public static ConfiguredTaskAwaitable<ITwitterList> UpdateList(string slug, string ownerScreenName, ITwitterListUpdateParameters parameters)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(slug, ownerScreenName, parameters));
        }

        // Destroy List
        public static ConfiguredTaskAwaitable<bool> DestroyList(ITwitterListIdentifier twitterListIdentifier)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(twitterListIdentifier));
        }

        public static ConfiguredTaskAwaitable<bool> DestroyList(long listId)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(listId));
        }

        public static ConfiguredTaskAwaitable<bool> DestroyList(string slug, IUserIdentifier owner)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(slug, owner));
        }

        public static ConfiguredTaskAwaitable<bool> DestroyList(string slug, long ownerId)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(slug, ownerId));
        }

        public static ConfiguredTaskAwaitable<bool> DestroyList(string slug, string ownerScreenName)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(slug, ownerScreenName));
        }

        // Get Tweets from List
        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetTweetsFromList(long listId)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(listId));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetTweetsFromList(string slug, IUserIdentifier owner)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(slug, owner));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetTweetsFromList(string slug, string ownerScreenName)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(slug, ownerScreenName));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetTweetsFromList(string slug, long ownerId)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(slug, ownerId));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<ITweet>> GetTweetsFromList(ITwitterListIdentifier twitterListIdentifier, IGetTweetsFromListParameters parameters = null)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(twitterListIdentifier, parameters));
        }

        // Get Members of List
        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetMembersOfList(ITwitterListIdentifier twitterListIdentifier, int maxNumberOfUsersToRetrieve = 100)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(twitterListIdentifier, maxNumberOfUsersToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetMembersOfList(long listId, int maxNumberOfUsersToRetrieve = 100)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(listId, maxNumberOfUsersToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetMembersOfList(string slug, IUserIdentifier owner, int maxNumberOfUsersToRetrieve = 100)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(slug, owner, maxNumberOfUsersToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetMembersOfList(string slug, string ownerScreenName, int maxNumberOfUsersToRetrieve = 100)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(slug, ownerScreenName, maxNumberOfUsersToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetMembersOfList(string slug, long ownerId, int maxNumberOfUsersToRetrieve = 100)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(slug, ownerId, maxNumberOfUsersToRetrieve));
        }

        // Add Members To List
        public static ConfiguredTaskAwaitable<bool> AddMemberToList(ITwitterListIdentifier list, IUserIdentifier newUser)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.AddMemberToList(list, newUser));
        }

        public static ConfiguredTaskAwaitable<MultiRequestsResult> AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> users)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.AddMultipleMembersToList(list, users));
        }

        // Remove Members From List
        public static ConfiguredTaskAwaitable<bool> RemoveMemberFromList(ITwitterListIdentifier list, IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.RemoveMemberFromList(list, user));
        }

        public static ConfiguredTaskAwaitable<MultiRequestsResult> RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> usersToRemove)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.RemoveMultipleMembersFromList(list, usersToRemove));
        }

        // Check Membership
        public static ConfiguredTaskAwaitable<bool> CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.CheckIfUserIsAListMember(listIdentifier, user));
        }

        // Get Subscriptions
        public static ConfiguredTaskAwaitable<IEnumerable<ITwitterList>> GetUserSubscribedLists(IUserIdentifier user, int maxNumberOfListsToRetrieve = TweetinviConsts.LIST_GET_USER_SUBSCRIPTIONS_COUNT)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetUserSubscribedLists(user, maxNumberOfListsToRetrieve));
        }

        public static ConfiguredTaskAwaitable<IEnumerable<IUser>> GetListSubscribers(ITwitterListIdentifier list, int maximumNumberOfUsersToRetrieve = 100)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetListSubscribers(list, maximumNumberOfUsersToRetrieve));
        } 

        // Subscribe
        public static ConfiguredTaskAwaitable<bool> SubscribeAuthenticatedUserToList(ITwitterListIdentifier listIdentifier, IAuthenticatedUser authenticatedUser = null)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.SubscribeAuthenticatedUserToList(listIdentifier, authenticatedUser));
        }

        // Unsubscribe
        public static ConfiguredTaskAwaitable<bool> UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier listIdentifier, IAuthenticatedUser authenticatedUser = null)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.UnSubscribeAuthenticatedUserFromList(listIdentifier, authenticatedUser));
        }

        // Check Subscription
        public static ConfiguredTaskAwaitable<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.CheckIfUserIsAListSubscriber(listIdentifier, user));
        }
    }
}