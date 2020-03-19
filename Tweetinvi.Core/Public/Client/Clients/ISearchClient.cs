using System.Threading.Tasks;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.Enum;

namespace Tweetinvi.Client
{
    public interface ISearchClient
    {
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
    }
}