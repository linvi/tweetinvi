using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.ListsClient;

namespace Tweetinvi.Core.Controllers
{
    public interface ITwitterListController
    {
        // list
        Task<ITwitterResult<ITwitterListDTO>> CreateList(ICreateListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> GetList(IGetListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO[]>> GetUserLists(IGetUserListsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> UpdateList(IUpdateListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> DestroyList(IDestroyListParameters parameters, ITwitterRequest request);

        // members
        Task<ITwitterResult<ITwitterListDTO>> AddMemberToList(IAddMemberToListParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfListIterator(IGetMembersOfListParameters parameters, ITwitterRequest request);







        Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(IUserIdentifier user, bool getOwnedListsFirst);
        Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(long userId, bool getOwnedListsFirst);
        Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(string userScreenName, bool getOwnedListsFirst);

        Task<IEnumerable<ITwitterList>> GetUserOwnedLists(long userId, int maximumNumberOfListsToRetrieve);
        Task<IEnumerable<ITwitterList>> GetUserOwnedLists(string userScreenName, int maximumNumberOfListsToRetrieve);
        Task<IEnumerable<ITwitterList>> GetUserOwnedLists(IUserIdentifier user, int maximumNumberOfListsToRetrieve);

        Task<IEnumerable<ITweet>> GetTweetsFromList(long listId);
        Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, IUserIdentifier owner);
        Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, string ownerScreenName);
        Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, long ownerId);
        Task<IEnumerable<ITweet>> GetTweetsFromList(ITwitterListIdentifier list, IGetTweetsFromListParameters parameters = null);

        // Add Multiple Members to List
        Task<MultiRequestsResult> AddMultipleMembersToList(long listId, IEnumerable<long> userIds);
        Task<MultiRequestsResult> AddMultipleMembersToList(long listId, IEnumerable<string> userScreenNames);
        Task<MultiRequestsResult> AddMultipleMembersToList(long listId, IEnumerable<IUserIdentifier> users);

        Task<MultiRequestsResult> AddMultipleMembersToList(string slug, long ownerId, IEnumerable<long> userIds);
        Task<MultiRequestsResult> AddMultipleMembersToList(string slug, long ownerId, IEnumerable<string> userScreenNames);
        Task<MultiRequestsResult> AddMultipleMembersToList(string slug, long ownerId, IEnumerable<IUserIdentifier> users);

        Task<MultiRequestsResult> AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<long> userIds);
        Task<MultiRequestsResult> AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<string> userScreenNames);
        Task<MultiRequestsResult> AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<IUserIdentifier> users);

        Task<MultiRequestsResult> AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<long> userIds);
        Task<MultiRequestsResult> AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<string> userScreenNames);
        Task<MultiRequestsResult> AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<IUserIdentifier> users);

        Task<MultiRequestsResult> AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<long> userIds);
        Task<MultiRequestsResult> AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<string> userScreenNames);
        Task<MultiRequestsResult> AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> users);

        // Get List Memberships
        Task<IEnumerable<ITwitterList>> GetUserListsMemberships(IUserIdentifier userIdentifier, IGetUserListMembershipsParameters parameters);
        Task<IEnumerable<ITwitterList>> GetUserListsMemberships(IGetUserListMembershipsQueryParameters parameters);

        // Remove Member from List
        Task<bool> RemoveMemberFromList(long listId, long newUserId);
        Task<bool> RemoveMemberFromList(long listId, string newUserName);
        Task<bool> RemoveMemberFromList(long listId, IUserIdentifier newUser);

        Task<bool> RemoveMemberFromList(string slug, long ownerId, long newUserId);
        Task<bool> RemoveMemberFromList(string slug, long ownerId, string newUserName);
        Task<bool> RemoveMemberFromList(string slug, long ownerId, IUserIdentifier newUser);

        Task<bool> RemoveMemberFromList(string slug, string ownerScreenName, long newUserId);
        Task<bool> RemoveMemberFromList(string slug, string ownerScreenName, string newUserName);
        Task<bool> RemoveMemberFromList(string slug, string ownerScreenName, IUserIdentifier newUser);

        Task<bool> RemoveMemberFromList(string slug, IUserIdentifier owner, long newUserId);
        Task<bool> RemoveMemberFromList(string slug, IUserIdentifier owner, string newUserName);
        Task<bool> RemoveMemberFromList(string slug, IUserIdentifier owner, IUserIdentifier newUser);

        Task<bool> RemoveMemberFromList(ITwitterListIdentifier list, long newUserId);
        Task<bool> RemoveMemberFromList(ITwitterListIdentifier list, string newUserName);
        Task<bool> RemoveMemberFromList(ITwitterListIdentifier list, IUserIdentifier newUser);

        // Remove Multiple Members
        Task<MultiRequestsResult> RemoveMultipleMembersFromList(long listId, IEnumerable<long> userIds);
        Task<MultiRequestsResult> RemoveMultipleMembersFromList(long listId, IEnumerable<string> userScreenNames);
        Task<MultiRequestsResult> RemoveMultipleMembersFromList(long listId, IEnumerable<IUserIdentifier> users);

        Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<long> userIds);
        Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<string> userScreenNames);
        Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<IUserIdentifier> users);

        Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<long> userIds);
        Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<string> userScreenNames);
        Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<IUserIdentifier> users);

        Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<long> userIds);
        Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<string> userScreenNames);
        Task<MultiRequestsResult> RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<IUserIdentifier> users);

        Task<MultiRequestsResult> RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<long> userIds);
        Task<MultiRequestsResult> RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<string> userScreenNames);
        Task<MultiRequestsResult> RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> users);

        // Check Membership
        Task<bool> CheckIfUserIsAListMember(long listId, long userId);
        Task<bool> CheckIfUserIsAListMember(long listId, string userScreenName);
        Task<bool> CheckIfUserIsAListMember(long listId, IUserIdentifier user);

        Task<bool> CheckIfUserIsAListMember(string slug, long ownerId, long userId);
        Task<bool> CheckIfUserIsAListMember(string slug, long ownerId, string userScreenName);
        Task<bool> CheckIfUserIsAListMember(string slug, long ownerId, IUserIdentifier user);

        Task<bool> CheckIfUserIsAListMember(string slug, string ownerScreenName, long userId);
        Task<bool> CheckIfUserIsAListMember(string slug, string ownerScreenName, string userScreenName);
        Task<bool> CheckIfUserIsAListMember(string slug, string ownerScreenName, IUserIdentifier user);

        Task<bool> CheckIfUserIsAListMember(string slug, IUserIdentifier owner, long userId);
        Task<bool> CheckIfUserIsAListMember(string slug, IUserIdentifier owner, string userScreenName);
        Task<bool> CheckIfUserIsAListMember(string slug, IUserIdentifier owner, IUserIdentifier user);

        Task<bool> CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, long userId);
        Task<bool> CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, string userScreenName);
        Task<bool> CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier user);

        // Subscriptions
        Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(long userId, int maxNumberOfListsToRetrieve);
        Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(string userName, int maxNumberOfListsToRetrieve);
        Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(IUserIdentifier user, int maxNumberOfListsToRetrieve);

        // Get List subscribers
        Task<IEnumerable<IUser>> GetListSubscribers(long listId, int maximumNumberOfUsersToRetrieve = 100);
        Task<IEnumerable<IUser>> GetListSubscribers(string slug, IUserIdentifier owner, int maximumNumberOfUsersToRetrieve = 100);
        Task<IEnumerable<IUser>> GetListSubscribers(string slug, string ownerScreenName, int maximumNumberOfUsersToRetrieve = 100);
        Task<IEnumerable<IUser>> GetListSubscribers(string slug, long ownerId, int maximumNumberOfUsersToRetrieve = 100);
        Task<IEnumerable<IUser>> GetListSubscribers(ITwitterListIdentifier list, int maximumNumberOfUsersToRetrieve = 100);

        // Add subscribers
        Task<bool> SubscribeAuthenticatedUserToList(long listId);
        Task<bool> SubscribeAuthenticatedUserToList(string slug, long ownerId);
        Task<bool> SubscribeAuthenticatedUserToList(string slug, string ownerScreenName);
        Task<bool> SubscribeAuthenticatedUserToList(string slug, IUserIdentifier owner);
        Task<bool> SubscribeAuthenticatedUserToList(ITwitterListIdentifier list);

        // Remove subscriber from List
        Task<bool> UnSubscribeAuthenticatedUserFromList(long listId);
        Task<bool> UnSubscribeAuthenticatedUserFromList(string slug, long ownerId);
        Task<bool> UnSubscribeAuthenticatedUserFromList(string slug, string ownerScreenName);
        Task<bool> UnSubscribeAuthenticatedUserFromList(string slug, IUserIdentifier owner);
        Task<bool> UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier list);

        // Check Subscription
        Task<bool> CheckIfUserIsAListSubscriber(long listId, long userId);
        Task<bool> CheckIfUserIsAListSubscriber(long listId, string userScreenName);
        Task<bool> CheckIfUserIsAListSubscriber(long listId, IUserIdentifier user);

        Task<bool> CheckIfUserIsAListSubscriber(string slug, long ownerId, long userId);
        Task<bool> CheckIfUserIsAListSubscriber(string slug, long ownerId, string userScreenName);
        Task<bool> CheckIfUserIsAListSubscriber(string slug, long ownerId, IUserIdentifier user);

        Task<bool> CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, long userId);
        Task<bool> CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, string userScreenName);
        Task<bool> CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, IUserIdentifier user);

        Task<bool> CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, long userId);
        Task<bool> CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, string userScreenName);
        Task<bool> CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, IUserIdentifier user);

        Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, long userId);
        Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, string userScreenName);
        Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier user);
    }
}