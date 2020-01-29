using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters.ListsClient;

namespace Tweetinvi.Client
{
    public interface IListsClient
    {
        /// <inheritdoc cref="CreateList(ICreateListParameters)"/>
        Task<ITwitterList> CreateList(string name);

        /// <inheritdoc cref="CreateList(ICreateListParameters)"/>
        Task<ITwitterList> CreateList(string name, PrivacyMode privacyMode);

        /// <summary>
        /// Create a list on Twitter
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-create </para>
        /// <returns>Created list</returns>
        Task<ITwitterList> CreateList(ICreateListParameters parameters);

        /// <inheritdoc cref="GetList(IGetListParameters)"/>
        Task<ITwitterList> GetList(long? listId);

        /// <inheritdoc cref="GetList(IGetListParameters)"/>
        Task<ITwitterList> GetList(string slug, IUserIdentifier user);

        /// <inheritdoc cref="GetList(IGetListParameters)"/>
        Task<ITwitterList> GetList(ITwitterListIdentifier listId);

        /// <summary>
        /// Get a specific list from Twitter
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-show </para>
        /// <returns>List requested</returns>
        Task<ITwitterList> GetList(IGetListParameters parameters);

        /// <summary>
        /// Get lists owned by the current account
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-list </para>
        /// <returns>Account user's lists</returns>
        Task<ITwitterList[]> GetUserLists();

        /// <inheritdoc cref="GetUserLists(IGetUserListsParameters)"/>
        Task<ITwitterList[]> GetUserLists(long? userId);
        /// <inheritdoc cref="GetUserLists(IGetUserListsParameters)"/>
        Task<ITwitterList[]> GetUserLists(string username);
        /// <inheritdoc cref="GetUserLists(IGetUserListsParameters)"/>
        Task<ITwitterList[]> GetUserLists(IUserIdentifier user);

        /// <summary>
        /// Get a user's lists
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-list </para>
        /// <returns>User's lists</returns>
        Task<ITwitterList[]> GetUserLists(IGetUserListsParameters parameters);

        /// <summary>
        /// Update information of a Twitter list
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-update </para>
        /// <returns>Updated list</returns>
        Task<ITwitterList> UpdateList(IUpdateListParameters parameters);

        /// <inheritdoc cref="DestroyList(IDestroyListParameters)"/>
        Task DestroyList(long? listId);

        /// <inheritdoc cref="DestroyList(IDestroyListParameters)"/>
        Task DestroyList(string slug, IUserIdentifier user);

        /// <inheritdoc cref="DestroyList(IDestroyListParameters)"/>
        Task DestroyList(ITwitterListIdentifier listId);

        /// <summary>
        /// Destroy a list from Twitter
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-destroy </para>
        Task DestroyList(IDestroyListParameters parameters);
    }
}