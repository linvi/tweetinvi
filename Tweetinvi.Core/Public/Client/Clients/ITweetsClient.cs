using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface ITweetsClient
    {
        /// <summary>
        /// Validate all the TweetsClient parameters
        /// </summary>
        ITweetsClientParametersValidator ParametersValidator { get; }

        /// <inheritdoc cref="GetTweet(IGetTweetParameters)" />
        Task<ITweet> GetTweet(long? tweetId);

        /// <summary>
        /// Get a tweet
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-show-id </para>
        /// </summary>
        /// <returns>The tweet</returns>
        Task<ITweet> GetTweet(IGetTweetParameters parameters);

        /// <inheritdoc cref="GetTweets(IGetTweetsParameters)" />
        Task<ITweet[]> GetTweets(long[] tweetIds);
        /// <inheritdoc cref="GetTweets(IGetTweetsParameters)" />
        Task<ITweet[]> GetTweets(long?[] tweetIds);
        /// <inheritdoc cref="GetTweets(IGetTweetsParameters)" />
        Task<ITweet[]> GetTweets(ITweetIdentifier[] tweets);

        /// <summary>
        /// Get multiple tweets
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-lookup </para>
        /// </summary>
        /// <returns>Requested tweets</returns>
        Task<ITweet[]> GetTweets(IGetTweetsParameters parameters);

        /// <inheritdoc cref="PublishTweet(IPublishTweetParameters)" />
        Task<ITweet> PublishTweet(string text);

        /// <summary>
        /// Publish a tweet
        /// <para>Read more : https://dev.twitter.com/rest/reference/post/statuses/update </para>
        /// </summary>
        /// <returns>Returns the published tweet</returns>
        Task<ITweet> PublishTweet(IPublishTweetParameters parameters);

        /// <inheritdoc cref="DestroyTweet(IDestroyTweetParameters)" />
        Task DestroyTweet(long? tweetId);
        /// <inheritdoc cref="DestroyTweet(IDestroyTweetParameters)" />
        Task DestroyTweet(ITweetIdentifier tweet);
        /// <inheritdoc cref="DestroyTweet(IDestroyTweetParameters)" />
        Task DestroyTweet(ITweet tweet);
        /// <inheritdoc cref="DestroyTweet(IDestroyTweetParameters)" />
        Task DestroyTweet(ITweetDTO tweet);

        /// <summary>
        /// Destroy a tweet
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-destroy-id </para>
        /// </summary>
        Task DestroyTweet(IDestroyTweetParameters parameters);

        /// <inheritdoc cref="GetRetweets(IGetRetweetsParameters)" />
        Task<ITweet[]> GetRetweets(long? tweetId);

        /// <inheritdoc cref="GetRetweets(IGetRetweetsParameters)" />
        Task<ITweet[]> GetRetweets(ITweetIdentifier tweet);

        /// <summary>
        /// Get the retweets associated with a specific tweet
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweets-id </para>
        /// </summary>
        /// <returns>Retweets</returns>
        Task<ITweet[]> GetRetweets(IGetRetweetsParameters parameters);

        /// <inheritdoc cref="PublishRetweet(IPublishRetweetParameters)" />
        Task<ITweet> PublishRetweet(long? tweetId);
        /// <inheritdoc cref="PublishRetweet(IPublishRetweetParameters)" />
        Task<ITweet> PublishRetweet(ITweetIdentifier tweet);

        /// <summary>
        /// Publish a retweet
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-retweet-id </para>
        /// </summary>
        /// <returns>The retweet</returns>
        Task<ITweet> PublishRetweet(IPublishRetweetParameters parameters);

        /// <inheritdoc cref="DestroyRetweet(IDestroyRetweetParameters)" />
        Task DestroyRetweet(long? retweetId);
        /// <inheritdoc cref="DestroyRetweet(IDestroyRetweetParameters)" />
        Task DestroyRetweet(ITweetIdentifier retweet);

        /// <summary>
        /// Destroy a retweet
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-unretweet-id </para>
        /// </summary>
        Task DestroyRetweet(IDestroyRetweetParameters parameters);

        /// <inheritdoc cref="GetRetweeterIdsIterator(IGetRetweeterIdsParameters)" />
        ITwitterIterator<long> GetRetweeterIdsIterator(long? tweetId);
        /// <inheritdoc cref="GetRetweeterIdsIterator(IGetRetweeterIdsParameters)" />
        ITwitterIterator<long> GetRetweeterIdsIterator(ITweetIdentifier tweet);

        /// <summary>
        /// Get the ids of the users who retweeted a specific tweet
        /// <para> Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweeters-ids </para>
        /// </summary>
        /// <returns>An iterator to list the retweeter ids</returns>
        ITwitterIterator<long> GetRetweeterIdsIterator(IGetRetweeterIdsParameters parameters);

        /// <inheritdoc cref="GetFavoriteTweets(IGetFavoriteTweetsParameters)" />
        ITwitterIterator<ITweet, long?> GetFavoriteTweets(long? userId);
        /// <inheritdoc cref="GetFavoriteTweets(IGetFavoriteTweetsParameters)" />
        ITwitterIterator<ITweet, long?> GetFavoriteTweets(string username);
        /// <inheritdoc cref="GetFavoriteTweets(IGetFavoriteTweetsParameters)" />
        ITwitterIterator<ITweet, long?> GetFavoriteTweets(IUserIdentifier user);

        /// <summary>
        /// Get favorite tweets of a user
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-favorites-list </para>
        /// </summary>
        /// <returns>An iterator to list the favorite tweets</returns>
        ITwitterIterator<ITweet, long?> GetFavoriteTweets(IGetFavoriteTweetsParameters parameters);

        /// <inheritdoc cref="FavoriteTweet(IFavoriteTweetParameters)" />
        Task FavoriteTweet(long? tweetId);
        /// <inheritdoc cref="FavoriteTweet(IFavoriteTweetParameters)" />
        Task FavoriteTweet(ITweetIdentifier tweet);
        /// <inheritdoc cref="FavoriteTweet(IFavoriteTweetParameters)" />
        Task FavoriteTweet(ITweet tweet);
        /// <inheritdoc cref="FavoriteTweet(IFavoriteTweetParameters)" />
        Task FavoriteTweet(ITweetDTO tweet);

        /// <summary>
        /// Favorite a tweet
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-favorites-create </para>
        Task FavoriteTweet(IFavoriteTweetParameters parameters);

        /// <inheritdoc cref="UnfavoriteTweet(IUnfavoriteTweetParameters)" />
        Task UnfavoriteTweet(long? tweetId);
        /// <inheritdoc cref="UnfavoriteTweet(IUnfavoriteTweetParameters)" />
        Task UnfavoriteTweet(ITweetIdentifier tweet);
        /// <inheritdoc cref="UnfavoriteTweet(IUnfavoriteTweetParameters)" />
        Task UnfavoriteTweet(ITweet tweet);
        /// <inheritdoc cref="UnfavoriteTweet(IUnfavoriteTweetParameters)" />
        Task UnfavoriteTweet(ITweetDTO tweet);

        /// <summary>
        /// Remove the favorite of a tweet
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-favorites-destroy </para>
        Task UnfavoriteTweet(IUnfavoriteTweetParameters parameters);

        /// <inheritdoc cref="GetOEmbedTweet(IGetOEmbedTweetParameters)" />
        Task<IOEmbedTweet> GetOEmbedTweet(ITweetIdentifier tweet);

        /// <inheritdoc cref="GetOEmbedTweet(IGetOEmbedTweetParameters)" />
        Task<IOEmbedTweet> GetOEmbedTweet(long? tweetId);

        /// <summary>
        /// Get an oembed tweet
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-oembed </para>
        /// <returns>The generated oembed tweet</returns>
        Task<IOEmbedTweet> GetOEmbedTweet(IGetOEmbedTweetParameters parameters);
    }
}