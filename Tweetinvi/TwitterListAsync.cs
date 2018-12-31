using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class TwitterListAsync
    {
        // Get Existing List
        public static Task<ITwitterList> GetExistingList(ITwitterListIdentifier twitterListIdentifier)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(twitterListIdentifier));
        }

        public static Task<ITwitterList> GetExistingList(long listId)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(listId));
        }

        public static Task<ITwitterList> GetExistingList(string slug, IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(slug, user));
        }

        public static Task<ITwitterList> GetExistingList(string slug, long userId)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(slug, userId));
        }

        public static Task<ITwitterList> GetExistingList(string slug, string userScreenName)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(slug, userScreenName));
        }

        // Owned Lists
        public static Task<IEnumerable<ITwitterList>> GetUserOwnedLists(long userId, int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_OWNED_COUNT)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetUserOwnedLists(userId, maximumNumberOfListsToRetrieve));
        }

        public static Task<IEnumerable<ITwitterList>> GetUserOwnedLists(string userScreenName, int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_OWNED_COUNT)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetUserOwnedLists(userScreenName, maximumNumberOfListsToRetrieve));
        }

        public static Task<IEnumerable<ITwitterList>> GetUserOwnedLists(IUserIdentifier user, int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_OWNED_COUNT)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetUserOwnedLists(user, maximumNumberOfListsToRetrieve));
        }

        // Create List
        public static Task<ITwitterList> CreateList(string name, PrivacyMode privacyMode, string description = null)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.CreateList(name, privacyMode, description));
        }

        // Update List
        public static Task<ITwitterList> UpdateList(ITwitterListIdentifier twitterList, ITwitterListUpdateParameters parameters)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(twitterList, parameters));
        }

        public static Task<ITwitterList> UpdateList(long listId, ITwitterListUpdateParameters parameters)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(listId, parameters));
        }

        public static Task<ITwitterList> UpdateList(string slug, IUserIdentifier owner, ITwitterListUpdateParameters parameters)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(slug, owner, parameters));
        }

        public static Task<ITwitterList> UpdateList(string slug, long ownerId, ITwitterListUpdateParameters parameters)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(slug, ownerId, parameters));
        }

        public static Task<ITwitterList> UpdateList(string slug, string ownerScreenName, ITwitterListUpdateParameters parameters)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(slug, ownerScreenName, parameters));
        }

        // Destroy List
        public static Task<bool> DestroyList(ITwitterListIdentifier twitterListIdentifier)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(twitterListIdentifier));
        }

        public static Task<bool> DestroyList(long listId)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(listId));
        }

        public static Task<bool> DestroyList(string slug, IUserIdentifier owner)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(slug, owner));
        }

        public static Task<bool> DestroyList(string slug, long ownerId)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(slug, ownerId));
        }

        public static Task<bool> DestroyList(string slug, string ownerScreenName)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(slug, ownerScreenName));
        }

        // Get Tweets from List
        public static Task<IEnumerable<ITweet>> GetTweetsFromList(long listId)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(listId));
        }

        public static Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, IUserIdentifier owner)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(slug, owner));
        }

        public static Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, string ownerScreenName)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(slug, ownerScreenName));
        }

        public static Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, long ownerId)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(slug, ownerId));
        }

        public static Task<IEnumerable<ITweet>> GetTweetsFromList(ITwitterListIdentifier twitterListIdentifier, IGetTweetsFromListParameters parameters = null)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(twitterListIdentifier, parameters));
        }

        // Get Members of List
        public static Task<IEnumerable<IUser>> GetMembersOfList(ITwitterListIdentifier twitterListIdentifier, int maxNumberOfUsersToRetrieve = 100)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(twitterListIdentifier, maxNumberOfUsersToRetrieve));
        }

        public static Task<IEnumerable<IUser>> GetMembersOfList(long listId, int maxNumberOfUsersToRetrieve = 100)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(listId, maxNumberOfUsersToRetrieve));
        }

        public static Task<IEnumerable<IUser>> GetMembersOfList(string slug, IUserIdentifier owner, int maxNumberOfUsersToRetrieve = 100)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(slug, owner, maxNumberOfUsersToRetrieve));
        }

        public static Task<IEnumerable<IUser>> GetMembersOfList(string slug, string ownerScreenName, int maxNumberOfUsersToRetrieve = 100)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(slug, ownerScreenName, maxNumberOfUsersToRetrieve));
        }

        public static Task<IEnumerable<IUser>> GetMembersOfList(string slug, long ownerId, int maxNumberOfUsersToRetrieve = 100)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(slug, ownerId, maxNumberOfUsersToRetrieve));
        }

        // Add Members To List
        public static Task<bool> AddMemberToList(ITwitterListIdentifier list, IUserIdentifier newUser)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.AddMemberToList(list, newUser));
        }

        public static Task<MultiRequestsResult> AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> users)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.AddMultipleMembersToList(list, users));
        }

        // Remove Members From List
        public static Task<bool> RemoveMemberFromList(ITwitterListIdentifier list, IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.RemoveMemberFromList(list, user));
        }

        public static Task<MultiRequestsResult> RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> usersToRemove)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.RemoveMultipleMembersFromList(list, usersToRemove));
        }

        // Check Membership
        public static Task<bool> CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.CheckIfUserIsAListMember(listIdentifier, user));
        }

        // Get Subscriptions
        public static Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(IUserIdentifier user, int maxNumberOfListsToRetrieve = TweetinviConsts.LIST_GET_USER_SUBSCRIPTIONS_COUNT)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetUserSubscribedLists(user, maxNumberOfListsToRetrieve));
        }

        public static Task<IEnumerable<IUser>> GetListSubscribers(ITwitterListIdentifier list, int maximumNumberOfUsersToRetrieve = 100)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.GetListSubscribers(list, maximumNumberOfUsersToRetrieve));
        } 

        // Subscribe
        public static Task<bool> SubscribeAuthenticatedUserToList(ITwitterListIdentifier listIdentifier, IAuthenticatedUser authenticatedUser = null)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.SubscribeAuthenticatedUserToList(listIdentifier, authenticatedUser));
        }

        // Unsubscribe
        public static Task<bool> UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier listIdentifier, IAuthenticatedUser authenticatedUser = null)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.UnSubscribeAuthenticatedUserFromList(listIdentifier, authenticatedUser));
        }

        // Check Subscription
        public static Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            return Sync.ExecuteTaskAsync(() => TwitterList.CheckIfUserIsAListSubscriber(listIdentifier, user));
        }
    }
}