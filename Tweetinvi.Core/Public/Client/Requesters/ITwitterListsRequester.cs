using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.ListsClient;

namespace Tweetinvi.Client.Requesters
{
    public interface ITwitterListsRequester
    {
        /// <summary>
        /// Create a list on Twitter
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-create </para>
        /// <returns>TwitterResult containing the created list</returns>
        Task<ITwitterResult<ITwitterListDTO, ITwitterList>> CreateList(ICreateListParameters parameters);

        /// <summary>
        /// Get a specific list from Twitter
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-show </para>
        /// <returns>TwitterResult containing the list</returns>
        Task<ITwitterResult<ITwitterListDTO, ITwitterList>> GetList(IGetListParameters parameters);

        /// <summary>
        /// Get a user's lists
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-list </para>
        /// <returns>TwitterResult containing the user's lists</returns>
        Task<ITwitterResult<ITwitterListDTO[], ITwitterList[]>> GetListsSubscribedByUser(IGetListsSubscribedByUserParameters parameters);

        /// <summary>
        /// Update information of a Twitter list
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-update </para>
        /// <returns>TwitterResult containing the updated list</returns>
        Task<ITwitterResult<ITwitterListDTO, ITwitterList>> UpdateList(IUpdateListParameters parameters);

        /// <summary>
        /// Destroy a list from Twitter
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-destroy </para>
        /// <returns>TwitterResult containing the destroyed list</returns>
        Task<ITwitterResult<ITwitterListDTO>> DestroyList(IDestroyListParameters parameters);

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
        /// <returns>TwitterResult</returns>
        Task<ITwitterResult<ITwitterListDTO, ITwitterList>> AddMemberToList(IAddMemberToListParameters parameters);

        /// <summary>
        /// Get the members of the specified list.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-members </para>
        /// </summary>
        /// <returns>An iterator to list the users members of the list</returns>
        ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfListIterator(IGetMembersOfListParameters parameters);


    }
}