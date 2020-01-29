using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
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
        Task<ITwitterResult<ITwitterListDTO[], ITwitterList[]>> GetUserLists(IGetUserListsParameters parameters);

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
    }
}