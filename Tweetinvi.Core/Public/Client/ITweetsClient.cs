using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public interface ITweetsClient
    {
        /// <summary>
        /// Get a tweet
        /// </summary>
        /// <returns>The specified tweet</returns>
        Task<ITweet> GetTweet(long tweetId);

        /// <summary>
        /// Get multiple tweets
        /// </summary>
        /// <returns>The specified tweets</returns>
        Task<ITweet[]> GetTweets(long[] tweetIds);

        /// <summary>
        /// Publish a tweet to Twitter
        /// </summary>
        /// <returns>The published tweet</returns>
        Task<ITweet> PublishTweet(string text);

        /// <summary>
        /// Publish a tweet to Twitter
        /// </summary>
        /// <returns>The published tweet</returns>
        Task<ITweet> PublishTweet(IPublishTweetParameters parameters);

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
    }
}
