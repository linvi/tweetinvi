using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi
{
    public static class TwitterListAsync
    {
        // Get Existing List
        public static async Task<ITwitterList> GetExistingList(ITwitterListIdentifier twitterListIdentifier)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(twitterListIdentifier));
        }

        public static async Task<ITwitterList> GetExistingList(long listId)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(listId));
        }

        public static async Task<ITwitterList> GetExistingList(string slug, IUser user)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(slug, user));
        }

        public static async Task<ITwitterList> GetExistingList(string slug, IUserIdentifier userDTO)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(slug, userDTO));
        }

        public static async Task<ITwitterList> GetExistingList(string slug, long userId)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(slug, userId));
        }

        public static async Task<ITwitterList> GetExistingList(string slug, string userScreenName)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetExistingList(slug, userScreenName));
        }

        // Owned Lists
        public static async Task<IEnumerable<ITwitterList>> GetUserOwnedLists(long userId, int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_OWNED_COUNT)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetUserOwnedLists(userId, maximumNumberOfListsToRetrieve));
        }

        public static async Task<IEnumerable<ITwitterList>> GetUserOwnedLists(string userScreenName, int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_OWNED_COUNT)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetUserOwnedLists(userScreenName, maximumNumberOfListsToRetrieve));
        }

        public static async Task<IEnumerable<ITwitterList>> GetUserOwnedLists(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_OWNED_COUNT)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetUserOwnedLists(userIdentifier, maximumNumberOfListsToRetrieve));
        }

        // Create List
        public static async Task<ITwitterList> CreateList(string name, PrivacyMode privacyMode, string description = null)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.CreateList(name, privacyMode, description));
        }

        // Update List
        public static async Task<ITwitterList> UpdateList(ITwitterListIdentifier twitterList, ITwitterListUpdateParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(twitterList, parameters));
        }

        public static async Task<ITwitterList> UpdateList(long listId, ITwitterListUpdateParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(listId, parameters));
        }

        public static async Task<ITwitterList> UpdateList(string slug, IUser owner, ITwitterListUpdateParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(slug, owner, parameters));
        }

        public static async Task<ITwitterList> UpdateList(string slug, IUserIdentifier ownerDTO, ITwitterListUpdateParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(slug, ownerDTO, parameters));
        }

        public static async Task<ITwitterList> UpdateList(string slug, long ownerId, ITwitterListUpdateParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(slug, ownerId, parameters));
        }

        public static async Task<ITwitterList> UpdateList(string slug, string ownerScreenName, ITwitterListUpdateParameters parameters)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.UpdateList(slug, ownerScreenName, parameters));
        }

        // Destroy List
        public static async Task<bool> DestroyList(ITwitterListIdentifier twitterListIdentifier)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(twitterListIdentifier));
        }

        public static async Task<bool> DestroyList(long listId)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(listId));
        }

        public static async Task<bool> DestroyList(string slug, IUser owner)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(slug, owner));
        }

        public static async Task<bool> DestroyList(string slug, IUserDTO ownerDTO)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(slug, ownerDTO));
        }

        public static async Task<bool> DestroyList(string slug, long ownerId)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(slug, ownerId));
        }

        public static async Task<bool> DestroyList(string slug, string ownerScreenName)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.DestroyList(slug, ownerScreenName));
        }

        // Get Tweets from List
        public static async Task<IEnumerable<ITweet>> GetTweetsFromList(long listId)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(listId));
        }

        public static async Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, IUser owner)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(slug, owner));
        }

        public static async Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, IUserIdentifier ownerDTO)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(slug, ownerDTO));
        }

        public static async Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, string ownerScreenName)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(slug, ownerScreenName));
        }

        public static async Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, long ownerId)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(slug, ownerId));
        }

        public static async Task<IEnumerable<ITweet>> GetTweetsFromList(ITwitterListIdentifier twitterListIdentifier, IGetTweetsFromListParameters parameters = null)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetTweetsFromList(twitterListIdentifier, parameters));
        }

        // Get Members of List
        public static async Task<IEnumerable<IUser>> GetMembersOfList(ITwitterListIdentifier twitterListIdentifier, int maxNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(twitterListIdentifier, maxNumberOfUsersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetMembersOfList(long listId, int maxNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(listId, maxNumberOfUsersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetMembersOfList(string slug, IUser owner, int maxNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(slug, owner, maxNumberOfUsersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetMembersOfList(string slug, IUserIdentifier ownerDTO, int maxNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(slug, ownerDTO, maxNumberOfUsersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetMembersOfList(string slug, string ownerScreenName, int maxNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(slug, ownerScreenName, maxNumberOfUsersToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetMembersOfList(string slug, long ownerId, int maxNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetMembersOfList(slug, ownerId, maxNumberOfUsersToRetrieve));
        }

        // Add Members To List
        public static async Task<bool> AddMemberToList(ITwitterListIdentifier list, IUserIdentifier newUser)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.AddMemberToList(list, newUser));
        }

        public static async Task<MultiRequestsResult> AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.AddMultipleMembersToList(list, userIdentifiers));
        }

        // Remove Members From List
        public static async Task<bool> RemoveMemberFromList(ITwitterListIdentifier list, IUserIdentifier user)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.RemoveMemberFromList(list, user));
        }

        public static async Task<MultiRequestsResult> RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> userIdentifiersToRemove)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.RemoveMultipleMembersFromList(list, userIdentifiersToRemove));
        }

        // Check Membership
        public static async Task<bool> CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.CheckIfUserIsAListMember(listIdentifier, userIdentifier));
        }

        // Get Subscriptions
        public static async Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(IUserIdentifier userIdentifier, int maxNumberOfListsToRetrieve = TweetinviConsts.LIST_GET_USER_SUBSCRIPTIONS_COUNT)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetUserSubscribedLists(userIdentifier, maxNumberOfListsToRetrieve));
        }

        public static async Task<IEnumerable<IUser>> GetListSubscribers(ITwitterListIdentifier list, int maximumNumberOfUsersToRetrieve = 100)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.GetListSubscribers(list, maximumNumberOfUsersToRetrieve));
        } 

        // Subscribe
        public static async Task<bool> SubscribeAuthenticatedUserToList(ITwitterListIdentifier listIdentifier, IAuthenticatedUser authenticatedUser = null)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.SubscribeAuthenticatedUserToList(listIdentifier, authenticatedUser));
        }

        // Unsubscribe
        public static async Task<bool> UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier listIdentifier, IAuthenticatedUser authenticatedUser = null)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.UnSubscribeAuthenticatedUserFromList(listIdentifier, authenticatedUser));
        }

        // Check Subscription
        public static async Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            return await Sync.ExecuteTaskAsync(() => TwitterList.CheckIfUserIsAListSubscriber(listIdentifier, userIdentifier));
        }
    }
}