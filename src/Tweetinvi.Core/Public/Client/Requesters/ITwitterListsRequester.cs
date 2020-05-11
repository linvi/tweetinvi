using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface ITwitterListsRequester
    {
        /// <summary>
        /// Create a list on Twitter
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-create </para>
        /// <returns>TwitterResult containing the created list</returns>
        Task<ITwitterResult<ITwitterListDTO>> CreateListAsync(ICreateListParameters parameters);

        /// <summary>
        /// Get a specific list from Twitter
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-show </para>
        /// <returns>TwitterResult containing the list</returns>
        Task<ITwitterResult<ITwitterListDTO>> GetListAsync(IGetListParameters parameters);

        /// <summary>
        /// Get a user's lists
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-list </para>
        /// <returns>TwitterResult containing the user's lists</returns>
        Task<ITwitterResult<ITwitterListDTO[]>> GetListsSubscribedByUserAsync(IGetListsSubscribedByUserParameters parameters);

        /// <summary>
        /// Update information of a Twitter list
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-update </para>
        /// <returns>TwitterResult containing the updated list</returns>
        Task<ITwitterResult<ITwitterListDTO>> UpdateListAsync(IUpdateListParameters parameters);

        /// <summary>
        /// Destroy a list from Twitter
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-destroy </para>
        /// <returns>TwitterResult containing the destroyed list</returns>
        Task<ITwitterResult<ITwitterListDTO>> DestroyListAsync(IDestroyListParameters parameters);

        /// <summary>
        /// Get the lists owned by a user or an account
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-ownerships </para>
        /// <returns>An iterator over the lists owned by the user or account</returns>
        ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters parameters);

        // MEMBERS

        /// <summary>
        /// Add a member to a twitter list
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-create </para>
        /// <returns>TwitterResult containing the list</returns>
        Task<ITwitterResult<ITwitterListDTO, ITwitterList>> AddMemberToListAsync(IAddMemberToListParameters parameters);

        /// <summary>
        /// Add multiple members to a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-create_all </para>
        /// </summary>
        /// <returns>TwitterResult containing the list</returns>
        Task<ITwitterResult<ITwitterListDTO, ITwitterList>> AddMembersToListAsync(IAddMembersToListParameters parameters);

        /// <summary>
        /// Get the lists a user is a member of
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships </para>
        /// <returns>An iterator over the lists a user is a member of</returns>
        ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListMembershipsIterator(IGetUserListMembershipsParameters parameters);

        /// <summary>
        /// Get the members of the specified list.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-members </para>
        /// </summary>
        /// <returns>An iterator to list the users members of the list</returns>
        ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfListIterator(IGetMembersOfListParameters parameters);

        /// <summary>
        /// Check if a user is a member of a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-members-show </para>
        /// </summary>
        /// <returns>TwitterResult containing the list </returns>
        Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsAListMemberAsync(ICheckIfUserIsMemberOfListParameters parameters);

        /// <summary>
        /// Remove a member from a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-destroy </para>
        /// </summary>
        /// <returns>TwitterResult containing the list </returns>
        Task<ITwitterResult<ITwitterListDTO>> RemoveMemberFromListAsync(IRemoveMemberFromListParameters parameters);

        /// <summary>
        /// Remove multiple members from a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-destroy_all </para>
        /// </summary>
        /// <returns>TwitterResult containing the list</returns>
        Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromListAsync(IRemoveMembersFromListParameters parameters);

        // SUBSCRIBER

        /// <summary>
        /// Subscribe the authenticated account to the specified list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-subscribers-create </para>
        /// </summary>
        /// <returns>Twitter result containing the latest version of the list</returns>
        Task<ITwitterResult<ITwitterListDTO>> SubscribeToListAsync(ISubscribeToListParameters parameters);

        /// <summary>
        /// Unsubscribe the authenticated account from the specified list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-subscribers-destroy </para>
        /// </summary>
        /// <returns>Twitter result containing the latest version of the list</returns>
        Task<ITwitterResult<ITwitterListDTO>> UnsubscribeFromListAsync(IUnsubscribeFromListParameters parameters);

        /// <summary>
        /// Returns the users subscribed to a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscribers </para>
        /// </summary>
        /// <returns>An iterator to get through the subscribers of the list</returns>
        ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetListSubscribersIterator(IGetListSubscribersParameters parameters);

        /// <summary>
        /// Returns the lists a user subscribed to
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscriptions </para>
        /// </summary>
        /// <returns>An iterator the get though the lists a user subscribed to</returns>
        ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListSubscriptionsIterator(IGetUserListSubscriptionsParameters parameters);

        /// <summary>
        /// Check if a user is a subscriber of a list
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscribers-show </para>
        /// </summary>
        /// <exception cref="Tweetinvi.Exceptions.TwitterException">If the user is not subscribed</exception>
        /// <returns>TwitterResult with the latest version of the list.</returns>
        Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsSubscriberOfListAsync(ICheckIfUserIsSubscriberOfListParameters parameters);

        // GET TWEETS

        /// <summary>
        /// Returns the tweets authored by the members of the list.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-statuses </para>
        /// </summary>
        /// <returns>An iterator to get through the tweets of a list</returns>
        ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetTweetsFromListIterator(IGetTweetsFromListParameters parameters);
    }
}