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
        /// Get a tweet
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-show-id </para>
        /// </summary>
        /// <returns>TwitterResult containing specified tweet</returns>
        Task<ITwitterResult<ITweetDTO, ITweet>> GetTweet(IGetTweetParameters parameters);
        
        /// <summary>
        /// Publish a tweet
        /// <para>Read more : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-show-id </para>
        /// </summary>
        /// <returns>TwitterResult containing the published tweet</returns>
        Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(IPublishTweetParameters parameters);
        
        /// <summary>
        /// Destroy a tweet
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-destroy-id </para>
        /// </summary>
        /// <returns>TwitterResult containing the destroyed tweet</returns>
        Task<ITwitterResult<ITweetDTO>> DestroyTweet(IDestroyTweetParameters parameters);
        
        /// <summary>
        /// Get favorite tweets of a user
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-favorites-list </para>
        /// </summary>
        /// <returns>Iterator over the list of tweets favorited by a user</returns>
        ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetFavoriteTweets(IGetFavoriteTweetsParameters parameters);
        
        
        /// <summary>
        /// Get multiple tweets
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-lookup </para>
        /// </summary>
        /// <returns>TwitterResult containing requested tweets</returns>
        Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetTweets(IGetTweetsParameters parameters);
        
        /// <summary>
        /// Get the retweets associated with a specific tweet 
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweets-id </para>
        /// </summary>
        /// <returns>TwitterResult containing the retweets</returns>
        Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetRetweets(IGetRetweetsParameters parameters);

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