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
        /// Validate all the Tweets client parameters
        /// </summary>
        ITweetsClientParametersValidator ParametersValidator { get; }

        /// <inheritdoc cref="ITweetsClient.GetTweetAsync(IGetTweetParameters)" />
        Task<ITweet> GetTweetAsync(long tweetId);

        /// <summary>
        /// Get a tweet
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-show-id </para>
        /// </summary>
        /// <returns>The tweet</returns>
        Task<ITweet> GetTweetAsync(IGetTweetParameters parameters);

        /// <inheritdoc cref="ITweetsClient.GetTweetsAsync(IGetTweetsParameters)" />
        Task<ITweet[]> GetTweetsAsync(long[] tweetIds);
        /// <inheritdoc cref="ITweetsClient.GetTweetsAsync(IGetTweetsParameters)" />
        Task<ITweet[]> GetTweetsAsync(ITweetIdentifier[] tweets);

        /// <summary>
        /// Get multiple tweets
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-lookup </para>
        /// </summary>
        /// <returns>Requested tweets</returns>
        Task<ITweet[]> GetTweetsAsync(IGetTweetsParameters parameters);

        /// <inheritdoc cref="ITweetsClient.PublishTweetAsync(IPublishTweetParameters)" />
        Task<ITweet> PublishTweetAsync(string text);

        /// <summary>
        /// Publish a tweet
        /// <para>Read more : https://dev.twitter.com/rest/reference/post/statuses/update </para>
        /// </summary>
        /// <returns>Returns the published tweet</returns>
        Task<ITweet> PublishTweetAsync(IPublishTweetParameters parameters);

        /// <inheritdoc cref="ITweetsClient.DestroyTweetAsync(IDestroyTweetParameters)" />
        Task DestroyTweetAsync(long tweetId);
        /// <inheritdoc cref="ITweetsClient.DestroyTweetAsync(IDestroyTweetParameters)" />
        Task DestroyTweetAsync(ITweetIdentifier tweet);
        /// <inheritdoc cref="ITweetsClient.DestroyTweetAsync(IDestroyTweetParameters)" />
        Task DestroyTweetAsync(ITweet tweet);
        /// <inheritdoc cref="ITweetsClient.DestroyTweetAsync(IDestroyTweetParameters)" />
        Task DestroyTweetAsync(ITweetDTO tweet);

        /// <summary>
        /// Destroy a tweet
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-destroy-id </para>
        /// </summary>
        Task DestroyTweetAsync(IDestroyTweetParameters parameters);

        /// <inheritdoc cref="ITweetsClient.GetRetweetsAsync(IGetRetweetsParameters)" />
        Task<ITweet[]> GetRetweetsAsync(long tweetId);

        /// <inheritdoc cref="ITweetsClient.GetRetweetsAsync(IGetRetweetsParameters)" />
        Task<ITweet[]> GetRetweetsAsync(ITweetIdentifier tweet);

        /// <summary>
        /// Get the retweets associated with a specific tweet
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweets-id </para>
        /// </summary>
        /// <returns>Retweets</returns>
        Task<ITweet[]> GetRetweetsAsync(IGetRetweetsParameters parameters);

        /// <inheritdoc cref="ITweetsClient.PublishRetweetAsync(IPublishRetweetParameters)" />
        Task<ITweet> PublishRetweetAsync(long tweetId);
        /// <inheritdoc cref="ITweetsClient.PublishRetweetAsync(IPublishRetweetParameters)" />
        Task<ITweet> PublishRetweetAsync(ITweetIdentifier tweet);

        /// <summary>
        /// Publish a retweet
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-retweet-id </para>
        /// </summary>
        /// <returns>The retweet</returns>
        Task<ITweet> PublishRetweetAsync(IPublishRetweetParameters parameters);

        /// <inheritdoc cref="ITweetsClient.DestroyRetweetAsync(IDestroyRetweetParameters)" />
        Task DestroyRetweetAsync(long retweetId);
        /// <inheritdoc cref="ITweetsClient.DestroyRetweetAsync(IDestroyRetweetParameters)" />
        Task DestroyRetweetAsync(ITweetIdentifier retweet);

        /// <summary>
        /// Destroy a retweet
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-unretweet-id </para>
        /// </summary>
        Task DestroyRetweetAsync(IDestroyRetweetParameters parameters);

        /// <inheritdoc cref="ITweetsClient.GetRetweeterIdsAsync(IGetRetweeterIdsParameters)" />
        Task<long[]> GetRetweeterIdsAsync(long tweetId);
        /// <inheritdoc cref="ITweetsClient.GetRetweeterIdsAsync(IGetRetweeterIdsParameters)" />
        Task<long[]> GetRetweeterIdsAsync(ITweetIdentifier tweet);

        /// <summary>
        /// Get the ids of the users who retweeted a specific tweet
        /// <para> Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweeters-ids </para>
        /// </summary>
        /// <returns>List the retweeter ids</returns>
        Task<long[]> GetRetweeterIdsAsync(IGetRetweeterIdsParameters parameters);

        /// <inheritdoc cref="GetRetweeterIdsIterator(IGetRetweeterIdsParameters)" />
        ITwitterIterator<long> GetRetweeterIdsIterator(long tweetId);
        /// <inheritdoc cref="GetRetweeterIdsIterator(IGetRetweeterIdsParameters)" />
        ITwitterIterator<long> GetRetweeterIdsIterator(ITweetIdentifier tweet);

        /// <summary>
        /// Get the ids of the users who retweeted a specific tweet
        /// <para> Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweeters-ids </para>
        /// </summary>
        /// <returns>An iterator to list the retweeter ids</returns>
        ITwitterIterator<long> GetRetweeterIdsIterator(IGetRetweeterIdsParameters parameters);

        /// <inheritdoc cref="ITweetsClient.GetUserFavoriteTweetsAsync(IGetUserFavoriteTweetsParameters)" />
        Task<ITweet[]> GetUserFavoriteTweetsAsync(long userId);
        /// <inheritdoc cref="ITweetsClient.GetUserFavoriteTweetsAsync(IGetUserFavoriteTweetsParameters)" />
        Task<ITweet[]> GetUserFavoriteTweetsAsync(string username);
        /// <inheritdoc cref="ITweetsClient.GetUserFavoriteTweetsAsync(IGetUserFavoriteTweetsParameters)" />
        Task<ITweet[]> GetUserFavoriteTweetsAsync(IUserIdentifier user);

        /// <summary>
        /// Get favorite tweets of a user
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-favorites-list </para>
        /// </summary>
        /// <returns>List the favorite tweets</returns>
        Task<ITweet[]> GetUserFavoriteTweetsAsync(IGetUserFavoriteTweetsParameters parameters);

        /// <inheritdoc cref="GetUserFavoriteTweetsIterator(IGetUserFavoriteTweetsParameters)" />
        ITwitterIterator<ITweet, long?> GetUserFavoriteTweetsIterator(long userId);
        /// <inheritdoc cref="GetUserFavoriteTweetsIterator(IGetUserFavoriteTweetsParameters)" />
        ITwitterIterator<ITweet, long?> GetUserFavoriteTweetsIterator(string username);
        /// <inheritdoc cref="GetUserFavoriteTweetsIterator(IGetUserFavoriteTweetsParameters)" />
        ITwitterIterator<ITweet, long?> GetUserFavoriteTweetsIterator(IUserIdentifier user);

        /// <summary>
        /// Get favorite tweets of a user
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-favorites-list </para>
        /// </summary>
        /// <returns>An iterator to list the favorite tweets</returns>
        ITwitterIterator<ITweet, long?> GetUserFavoriteTweetsIterator(IGetUserFavoriteTweetsParameters parameters);

        /// <inheritdoc cref="ITweetsClient.FavoriteTweetAsync(IFavoriteTweetParameters)" />
        Task FavoriteTweetAsync(long tweetId);
        /// <inheritdoc cref="ITweetsClient.FavoriteTweetAsync(IFavoriteTweetParameters)" />
        Task FavoriteTweetAsync(ITweetIdentifier tweet);
        /// <inheritdoc cref="ITweetsClient.FavoriteTweetAsync(IFavoriteTweetParameters)" />
        Task FavoriteTweetAsync(ITweet tweet);
        /// <inheritdoc cref="ITweetsClient.FavoriteTweetAsync(IFavoriteTweetParameters)" />
        Task FavoriteTweetAsync(ITweetDTO tweet);

        /// <summary>
        /// Favorite a tweet
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-favorites-create </para>
        Task FavoriteTweetAsync(IFavoriteTweetParameters parameters);

        /// <inheritdoc cref="ITweetsClient.UnfavoriteTweetAsync(IUnfavoriteTweetParameters)" />
        Task UnfavoriteTweetAsync(long tweetId);
        /// <inheritdoc cref="ITweetsClient.UnfavoriteTweetAsync(IUnfavoriteTweetParameters)" />
        Task UnfavoriteTweetAsync(ITweetIdentifier tweet);
        /// <inheritdoc cref="ITweetsClient.UnfavoriteTweetAsync(IUnfavoriteTweetParameters)" />
        Task UnfavoriteTweetAsync(ITweet tweet);
        /// <inheritdoc cref="ITweetsClient.UnfavoriteTweetAsync(IUnfavoriteTweetParameters)" />
        Task UnfavoriteTweetAsync(ITweetDTO tweet);

        /// <summary>
        /// Remove the favorite of a tweet
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-favorites-destroy </para>
        Task UnfavoriteTweetAsync(IUnfavoriteTweetParameters parameters);

        /// <inheritdoc cref="ITweetsClient.GetOEmbedTweetAsync(IGetOEmbedTweetParameters)" />
        Task<IOEmbedTweet> GetOEmbedTweetAsync(ITweetIdentifier tweet);

        /// <inheritdoc cref="ITweetsClient.GetOEmbedTweetAsync(IGetOEmbedTweetParameters)" />
        Task<IOEmbedTweet> GetOEmbedTweetAsync(long tweetId);

        /// <summary>
        /// Get an oembed tweet
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-oembed </para>
        /// <returns>The generated oembed tweet</returns>
        Task<IOEmbedTweet> GetOEmbedTweetAsync(IGetOEmbedTweetParameters parameters);
    }
}