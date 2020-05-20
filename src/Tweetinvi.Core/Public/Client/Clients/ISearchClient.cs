using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.Enum;

namespace Tweetinvi.Client
{
    public interface ISearchClient
    {
        /// <summary>
        /// Validate all the Search client parameters
        /// </summary>
        ISearchClientParametersValidator ParametersValidator { get; }

        /// <inheritdoc cref="ISearchClient.SearchTweetsAsync(ISearchTweetsParameters)"/>
        Task<ITweet[]> SearchTweetsAsync(string query);

        /// <inheritdoc cref="ISearchClient.SearchTweetsWithMetadataAsync(ISearchTweetsParameters)"/>
        Task<ITweet[]> SearchTweetsAsync(IGeoCode geoCode);

        /// <summary>
        /// Search for tweets
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/tweets/search/api-reference/get-search-tweets </para>
        /// <returns>Tweets matching the search</returns>
        Task<ITweet[]> SearchTweetsAsync(ISearchTweetsParameters parameters);

        /// <inheritdoc cref="ISearchClient.SearchTweetsWithMetadataAsync(ISearchTweetsParameters)"/>
        Task<ISearchResults> SearchTweetsWithMetadataAsync(string query);

        /// <summary>
        /// Search for tweets
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/tweets/search/api-reference/get-search-tweets </para>
        /// <returns>Tweets matching the search with search metadata</returns>
        Task<ISearchResults> SearchTweetsWithMetadataAsync(ISearchTweetsParameters parameters);

        /// <inheritdoc cref="GetSearchTweetsIterator(ISearchTweetsParameters)"/>
        ITwitterIterator<ITweet, long?> GetSearchTweetsIterator(string query);

        /// <summary>
        /// Search for tweets
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/tweets/search/api-reference/get-search-tweets </para>
        /// <returns>Iterator over the search results</returns>
        ITwitterIterator<ITweet, long?> GetSearchTweetsIterator(ISearchTweetsParameters parameters);

        /// <summary>
        /// Simple set of filters for tweets
        /// </summary>
        /// <param name="tweets">Tweets you want to filter</param>
        /// <param name="filter">What type of tweets you wish to get</param>
        /// <param name="tweetsMustContainGeoInformation">Whether or not the tweet should contain geo information</param>
        /// <returns>Filtered set of tweets</returns>
        ITweet[] FilterTweets(ITweet[] tweets, OnlyGetTweetsThatAre? filter, bool tweetsMustContainGeoInformation);

        /// <inheritdoc cref="ISearchClient.SearchUsersAsync(ISearchUsersParameters)"/>
        Task<IUser[]> SearchUsersAsync(string query);

        /// <summary>
        /// Search for tweets
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-search </para>
        /// <returns>Users matching the search</returns>
        Task<IUser[]> SearchUsersAsync(ISearchUsersParameters parameters);

        /// <inheritdoc cref="GetSearchUsersIterator(ISearchUsersParameters)"/>
        ITwitterIterator<IUser, int?> GetSearchUsersIterator(string query);

        /// <summary>
        /// Search for users
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-search </para>
        /// <returns>Iterator over the search results</returns>
        ITwitterIterator<IUser, int?> GetSearchUsersIterator(ISearchUsersParameters parameters);

        /// <inheritdoc cref="ISearchClient.CreateSavedSearchAsync(ICreateSavedSearchParameters)"/>
        Task<ISavedSearch> CreateSavedSearchAsync(string query);

        /// <summary>
        /// Create a saved search
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-saved_searches-create </para>
        /// <returns>Created saved search</returns>
        Task<ISavedSearch> CreateSavedSearchAsync(ICreateSavedSearchParameters parameters);

        /// <inheritdoc cref="ISearchClient.GetSavedSearchAsync(IGetSavedSearchParameters)"/>
        Task<ISavedSearch> GetSavedSearchAsync(long savedSearchId);

        /// <summary>
        /// Get an existing saved search
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/get-saved_searches-show-id </para>
        /// <returns>Requested saved search</returns>
        Task<ISavedSearch> GetSavedSearchAsync(IGetSavedSearchParameters parameters);

        /// <inheritdoc cref="ISearchClient.ListSavedSearchesAsync(Tweetinvi.Parameters.IListSavedSearchesParameters)"/>
        Task<ISavedSearch[]> ListSavedSearchesAsync();

        /// <summary>
        /// List account's saved searches
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/get-saved_searches-list </para>
        /// <returns>Account's saved searches</returns>
        Task<ISavedSearch[]> ListSavedSearchesAsync(IListSavedSearchesParameters parameters);

        /// <inheritdoc cref="ISearchClient.DestroySavedSearchAsync(IDestroySavedSearchParameters)"/>
        Task<ISavedSearch> DestroySavedSearchAsync(long savedSearchId);

        /// <inheritdoc cref="ISearchClient.DestroySavedSearchAsync(IDestroySavedSearchParameters)"/>
        Task<ISavedSearch> DestroySavedSearchAsync(ISavedSearch savedSearch);

        /// <summary>
        /// Destroys a saved search
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-saved_searches-destroy-id </para>
        /// <returns>Deleted search</returns>
        Task<ISavedSearch> DestroySavedSearchAsync(IDestroySavedSearchParameters parameters);
    }
}