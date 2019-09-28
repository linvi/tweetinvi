using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    /// <summary>
    /// A client providing all the methods related with tweets.
    /// The results from this client contain additional metadata.
    /// </summary>
    public interface ITweetsRequester
    {
        /// <summary>
        /// Publish a tweet
        /// <para>Read more : https://dev.twitter.com/rest/reference/post/statuses/update </para>
        /// </summary>
        /// <returns>TwitterResult containing the published tweet</returns>
        Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(IPublishTweetParameters parameters);
        
        /// <summary>
        /// Get favorite tweets of a user
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-favorites-list </para>
        /// </summary>
        /// <returns>Iterator over the list of tweets favorited by a user</returns>
        ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetFavoriteTweets(IGetFavoriteTweetsParameters parameters);
        
        // Tweets
        
        /// <summary>
        /// Get a tweet
        /// </summary>
        /// <returns>TwitterResult containing specified tweet</returns>
        Task<ITwitterResult<ITweetDTO, ITweet>> GetTweet(long tweetId);
        
        /// <summary>
        /// Get multiple tweets
        /// </summary>
        /// <returns>TwitterResult containing multiple tweets</returns>
        Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetTweets(long[] tweetIds);
        
        

        /// <summary>
        /// Destroying a tweet
        /// </summary>
        /// <returns>TwitterResult with the success status of the request</returns>
        Task<ITwitterResult> DestroyTweet(long tweetId);
        
        /// <summary>
        /// Destroying a tweet
        /// </summary>
        /// <returns>TwitterResult with the success status of the request</returns>
        Task<ITwitterResult> DestroyTweet(ITweetDTO tweet);

        // Retweets
        
        /// <summary>
        /// Get the retweets associated with a specific tweet
        /// </summary>
        /// <returns>TwitterResult containing the retweets</returns>
        Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetRetweets(ITweetIdentifier tweet, int? maxRetweetsToRetrieve);

        /// <summary>
        /// Publish a retweet 
        /// </summary>
        /// <returns>TwitterResult containing the published retweet</returns>
        Task<ITwitterResult<ITweetDTO, ITweet>> PublishRetweet(ITweetIdentifier tweet);
        
        /// <summary>
        /// Destroy a retweet
        /// </summary>
        /// <returns>TwitterResult containing the success status of the request</returns>
        Task<ITwitterResult> DestroyRetweet(ITweetIdentifier retweetId);
    }
}