using System.Threading.Tasks;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface ISearchRequester
    {
        /// <summary>
        /// Search for tweets
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/tweets/search/api-reference/get-search-tweets </para>
        /// <returns>Iterator over the search results</returns>
        ITwitterPageIterator<ITwitterResult<ISearchResultsDTO>, long?> GetSearchTweetsIterator(ISearchTweetsParameters parameters);

        /// <summary>
        /// Search for users
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-search </para>
        /// <returns>Iterator over the search results</returns>
        ITwitterPageIterator<ITwitterResult<UserDTO[]>, int?> GetSearchUsersIterator(ISearchUsersParameters parameters);

        /// <summary>
        /// Create a saved search
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-saved_searches-create </para>
        /// <returns>Twitter result containing the created saved search</returns>
        Task<ITwitterResult<SavedSearchDTO>> CreateSavedSearch(ICreateSavedSearchParameters parameters);

        /// <summary>
        /// Get an existing saved search
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/get-saved_searches-show-id </para>
        /// <returns>Twitter result containing the requested saved search</returns>
        Task<ITwitterResult<SavedSearchDTO>> GetSavedSearch(IGetSavedSearchParameters parameters);

        /// <summary>
        /// List account's saved searches
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/get-saved_searches-list </para>
        /// <returns>Twitter result containing the account's saved searches</returns>
        Task<ITwitterResult<SavedSearchDTO[]>> ListSavedSearches(IListSavedSearchesParameters parameters);

        /// <summary>
        /// Destroys a saved search
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-saved_searches-destroy-id </para>
        /// <returns>Twitter result containing the deleted search</returns>
        Task<ITwitterResult<SavedSearchDTO>> DestroySavedSearch(IDestroySavedSearchParameters parameters);
    }
}