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
        
        /// <summary>
        /// Get a tweet
        /// </summary>
        /// <returns>The specified tweet</returns>
        Task<ITweet> GetTweet(long? tweetId);

        /// <summary>
        /// Get multiple tweets
        /// </summary>
        /// <returns>The specified tweets</returns>
        Task<ITweet[]> GetTweets(long[] tweetIds);

        #region Publish Tweet

        /// <inheritdoc cref="PublishTweet(IPublishTweetParameters)" />
        Task<ITweet> PublishTweet(string text);

        /// <summary>
        /// Publish a tweet
        /// <para>Read more : https://dev.twitter.com/rest/reference/post/statuses/update </para>
        /// </summary>
        /// <returns>Returns the published tweet</returns>
        Task<ITweet> PublishTweet(IPublishTweetParameters parameters);

        #endregion

        /// <summary>
        /// Remove a tweet from Twitter
        /// </summary>
        /// <returns>Operation's success</returns>
        Task<bool> DestroyTweet(long tweetId);

        /// <summary>
        /// Remove a tweet from Twitter
        /// </summary>
        /// <returns>Operation's success</returns>
        Task<bool> DestroyTweet(ITweetDTO tweet);

        /// <summary>
        /// Remove a tweet from Twitter
        /// </summary>
        /// <returns>Operation's success</returns>
        Task<bool> DestroyTweet(ITweet tweet);

        /// <summary>
        /// Get the retweets associated with a specific tweet 
        /// </summary>
        /// <returns>Retweets</returns>
        Task<ITweet[]> GetRetweets(long tweetId);

        /// <summary>
        /// Get the retweets associated with a specific tweet 
        /// </summary>
        /// <returns>Retweets</returns>
        Task<ITweet[]> GetRetweets(long tweetId, int maxNumberOfTweetsToRetrieve);

        /// <summary>
        /// Get the retweets associated with a specific tweet 
        /// </summary>
        /// <returns>Retweets</returns>
        Task<ITweet[]> GetRetweets(ITweetIdentifier tweet);

        /// <summary>
        /// Get the retweets associated with a specific tweet 
        /// </summary>
        /// <returns>Retweets</returns>
        Task<ITweet[]> GetRetweets(ITweetIdentifier tweet, int maxNumberOfTweetsToRetrieve);

        /// <summary>
        /// Publish a retweet 
        /// </summary>
        /// <returns>The retweet</returns>
        Task<ITweet> PublishRetweet(long tweetId);

        /// <summary>
        /// Publish a retweet 
        /// </summary>
        /// <returns>The retweet</returns>
        Task<ITweet> PublishRetweet(ITweetIdentifier tweet);

        /// <summary>
        /// Destroy a retweet
        /// </summary>
        /// <returns>Whether the operation was a success</returns>
        Task<bool> UnRetweet(ITweetIdentifier retweet);

        /// <summary>
        /// Destroy a retweet
        /// </summary>
        /// <returns>Whether the operation was a success</returns>
        Task<bool> UnRetweet(long retweetId);


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
    }
}