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
        ISearchClientParametersValidator ParametersValidator { get; }

        /// <inheritdoc cref="SearchTweets(ISearchTweetsParameters)"/>
        Task<ITweet[]> SearchTweets(string query);

        /// <inheritdoc cref="SearchTweetsWithMetadata(ISearchTweetsParameters)"/>
        Task<ITweet[]> SearchTweets(IGeoCode geoCode);

        /// <summary>
        /// Search for tweets
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/tweets/search/api-reference/get-search-tweets </para>
        /// <returns>Tweets matching the search</returns>
        Task<ITweet[]> SearchTweets(ISearchTweetsParameters parameters);

        /// <inheritdoc cref="SearchTweetsWithMetadata(ISearchTweetsParameters)"/>
        Task<ISearchResults> SearchTweetsWithMetadata(string query);

        /// <summary>
        /// Search for tweets
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/tweets/search/api-reference/get-search-tweets </para>
        /// <returns>Tweets matching the search with search metadata</returns>
        Task<ISearchResults> SearchTweetsWithMetadata(ISearchTweetsParameters parameters);

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

        /// <inheritdoc cref="SearchUsers(ISearchUsersParameters)"/>
        Task<IUser[]> SearchUsers(string query);

        /// <summary>
        /// Search for tweets
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-search </para>
        /// <returns>Users matching the search</returns>
        Task<IUser[]> SearchUsers(ISearchUsersParameters parameters);

        /// <inheritdoc cref="GetSearchUsersIterator(ISearchUsersParameters)"/>
        ITwitterIterator<IUser, int?> GetSearchUsersIterator(string query);

        /// <summary>
        /// Search for users
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-search </para>
        /// <returns>Iterator over the search results</returns>
        ITwitterIterator<IUser, int?> GetSearchUsersIterator(ISearchUsersParameters parameters);

        /// <inheritdoc cref="CreateSavedSearch(ICreateSavedSearchParameters)"/>
        Task<ISavedSearch> CreateSavedSearch(string query);

        /// <summary>
        /// Create a saved search
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-saved_searches-create </para>
        /// <returns>Created saved search</returns>
        Task<ISavedSearch> CreateSavedSearch(ICreateSavedSearchParameters parameters);

        /// <inheritdoc cref="GetSavedSearch(IGetSavedSearchParameters)"/>
        Task<ISavedSearch> GetSavedSearch(long savedSearchId);

        /// <summary>
        /// Get an existing saved search
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/get-saved_searches-show-id </para>
        /// <returns>Requested saved search</returns>
        Task<ISavedSearch> GetSavedSearch(IGetSavedSearchParameters parameters);

        /// <inheritdoc cref="ListSavedSearches(Tweetinvi.Parameters.IListSavedSearchesParameters)"/>
        Task<ISavedSearch[]> ListSavedSearches();

        /// <summary>
        /// List account's saved searches
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/get-saved_searches-list </para>
        /// <returns>Account's saved searches</returns>
        Task<ISavedSearch[]> ListSavedSearches(IListSavedSearchesParameters parameters);

        /// <inheritdoc cref="DestroySavedSearch(IDestroySavedSearchParameters)"/>
        Task<ISavedSearch> DestroySavedSearch(long savedSearchId);

        /// <inheritdoc cref="DestroySavedSearch(IDestroySavedSearchParameters)"/>
        Task<ISavedSearch> DestroySavedSearch(ISavedSearch savedSearch);

        /// <summary>
        /// Destroys a saved search
        /// </summary>
        /// <para> Read more : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-saved_searches-destroy-id </para>
        /// <returns>Deleted search</returns>
        Task<ISavedSearch> DestroySavedSearch(IDestroySavedSearchParameters parameters);
    }
}