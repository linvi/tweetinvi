using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    /// <summary>
    /// A client providing all the methods related with tweets
    /// </summary>
    public class TweetsClient : ITweetsClient
    {
        private readonly ITweetsRequester _tweetsRequester;

        public TweetsClient(TwitterClient client)
        {
            _tweetsRequester = client.RequestExecutor.Tweets;
        }

        // Tweets
        
        /// <summary>
        /// Get a tweet
        /// </summary>
        /// <returns>The specified tweet</returns>
        public async Task<ITweet> GetTweet(long tweetId)
        {
            var requestResult = await _tweetsRequester.GetTweet(tweetId);
            return requestResult?.Result;
        }

        /// <summary>
        /// Get multiple tweets
        /// </summary>
        /// <returns>The specified tweets</returns>
        public async Task<ITweet[]> GetTweets(long[] tweetIds)
        {
            var requestResult = await _tweetsRequester.GetTweets(tweetIds);
            return requestResult?.Result;
        }

        // Tweets - Publish
        
        /// <summary>
        /// Publish a tweet to Twitter
        /// </summary>
        /// <returns>The published tweet</returns>
        public async Task<ITweet> PublishTweet(string text)
        {
            var requestResult = await _tweetsRequester.PublishTweet(text);
            return requestResult?.Result;
        }

        /// <summary>
        /// Publish a tweet to Twitter
        /// </summary>
        /// <returns>The published tweet</returns>
        public async Task<ITweet> PublishTweet(IPublishTweetParameters parameters)
        {
            var requestResult = await _tweetsRequester.PublishTweet(parameters);
            return requestResult?.Result;
        }

        // Tweets - Destroy
        
        /// <summary>
        /// Remove a tweet from Twitter
        /// </summary>
        /// <returns>Operation's success</returns>
        public async Task<bool> DestroyTweet(long tweetId)
        {
            var requestResult = await _tweetsRequester.DestroyTweet(tweetId);
            return requestResult?.Response?.IsSuccessStatusCode == true;
        }

        /// <summary>
        /// Remove a tweet from Twitter
        /// </summary>
        /// <returns>Operation's success</returns>
        public async Task<bool> DestroyTweet(ITweetDTO tweet)
        {
            var requestResult = await _tweetsRequester.DestroyTweet(tweet);
            return requestResult?.Response?.IsSuccessStatusCode == true;
        }

        /// <summary>
        /// Remove a tweet from Twitter
        /// </summary>
        /// <returns>Operation's success</returns>
        public Task<bool> DestroyTweet(ITweet tweet)
        {
            return DestroyTweet(tweet?.TweetDTO);
        }

        // Retweets
        
        /// <summary>
        /// Get the retweets associated with a specific tweet 
        /// </summary>
        /// <returns>Retweets</returns>
        public async Task<ITweet[]> GetRetweets(long tweetId)
        {
            var tweetIdentifier = new TweetIdentifier(tweetId);
            var requestResult = await _tweetsRequester.GetRetweets(tweetIdentifier, null);
            return requestResult?.Result;
        }

        /// <summary>
        /// Get the retweets associated with a specific tweet 
        /// </summary>
        /// <returns>Retweets</returns>
        public async Task<ITweet[]> GetRetweets(long tweetId, int maxNumberOfTweetsToRetrieve)
        {
            var tweetIdentifier = new TweetIdentifier(tweetId);
            var requestResult = await _tweetsRequester.GetRetweets(tweetIdentifier, null);
            return requestResult?.Result;
        }

        /// <summary>
        /// Get the retweets associated with a specific tweet 
        /// </summary>
        /// <returns>Retweets</returns>
        public async Task<ITweet[]> GetRetweets(ITweetIdentifier tweet)
        {
            var requestResult = await _tweetsRequester.GetRetweets(tweet, null);
            return requestResult?.Result;
        }

        /// <summary>
        /// Get the retweets associated with a specific tweet 
        /// </summary>
        /// <returns>Retweets</returns>
        public async Task<ITweet[]> GetRetweets(ITweetIdentifier tweet, int maxNumberOfTweetsToRetrieve)
        {
            var requestResult = await _tweetsRequester.GetRetweets(tweet, maxNumberOfTweetsToRetrieve);
            return requestResult?.Result;
        }

        // Retweets - Publish
        
        /// <summary>
        /// Publish a retweet 
        /// </summary>
        /// <returns>The retweet</returns>
        public async Task<ITweet> PublishRetweet(long tweetId)
        {
            var requestResult = await _tweetsRequester.PublishRetweet(new TweetIdentifier(tweetId));
            return requestResult?.Result;
        }

        /// <summary>
        /// Publish a retweet 
        /// </summary>
        /// <returns>The retweet</returns>
        public async Task<ITweet> PublishRetweet(ITweetIdentifier tweet)
        {
            var requestResult = await _tweetsRequester.PublishRetweet(tweet);
            return requestResult?.Result;
        }

        // Retweets - Destroy

        /// <summary>
        /// Destroy a retweet
        /// </summary>
        /// <returns>Whether the operation was a success</returns>
        public async Task<bool> UnRetweet(ITweetIdentifier retweet)
        {
            var requestResult = await _tweetsRequester.DestroyRetweet(retweet);
            return requestResult?.Response?.IsSuccessStatusCode == true;
        }

        /// <summary>
        /// Destroy a retweet
        /// </summary>
        /// <returns>Whether the operation was a success</returns>
        public Task<bool> UnRetweet(long retweetId)
        {
            return UnRetweet(new TweetIdentifier(retweetId));
        }
    }
}
