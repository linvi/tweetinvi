using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface ITwitterListController
    {
        // LIST
        Task<ITwitterResult<ITwitterListDTO>> CreateList(ICreateListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> GetList(IGetListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO[]>> GetListsSubscribedByUser(IGetListsSubscribedByUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> UpdateList(IUpdateListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> DestroyList(IDestroyListParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters parameters, ITwitterRequest request);

        // MEMBERS
        Task<ITwitterResult<ITwitterListDTO>> AddMemberToList(IAddMemberToListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> AddMembersToList(IAddMembersToListParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsAUserIsMemberOfIterator(IGetListsAUserIsMemberOfParameters parameters, ITwitterRequest request);
        ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfListIterator(IGetMembersOfListParameters parameters, ITwitterRequest request);

        Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsAListMember(ICheckIfUserIsMemberOfListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> RemoveMemberFromList(IRemoveMemberFromListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromList(IRemoveMembersFromListParameters parameters, ITwitterRequest request);

        // SUBSCRIBERS




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

        // GET TWEETS
        ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetTweetsFromListIterator(IGetTweetsFromListParameters parameters, ITwitterRequest request);
    }
}