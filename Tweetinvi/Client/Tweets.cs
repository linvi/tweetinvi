using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class Tweets
    {
        private readonly ITweetsRequester _tweetsRequester;

        public Tweets(TwitterClient client)
        {
            _tweetsRequester = client.RequestExecutor.Tweets;
        }

        public async Task<ITweet> GetTweet(long tweetId)
        {
            var requestResult = await _tweetsRequester.GetTweet(tweetId);
            return requestResult?.Result;
        }

        public async Task<ITweet[]> GetTweets(long[] tweetIds)
        {
            var requestResult = await _tweetsRequester.GetTweets(tweetIds);
            return requestResult?.Result;
        }

        public async Task<ITweet> PublishTweet(string text)
        {
            var requestResult = await _tweetsRequester.PublishTweet(text);
            return requestResult?.Result;
        }

        public async Task<ITweet> PublishTweet(IPublishTweetParameters parameters)
        {
            var requestResult = await _tweetsRequester.PublishTweet(parameters);
            return requestResult?.Result;
        }

        // Publish Retweet
        public async Task<ITweet> PublishRetweet(long tweetId)
        {
            var requestResult = await _tweetsRequester.PublishRetweet(tweetId);
            return requestResult?.Result;
        }

        public async Task<ITweet> PublishRetweet(ITweetDTO tweet)
        {
            var requestResult = await _tweetsRequester.PublishRetweet(tweet);
            return requestResult?.Result;
        }

        public Task<ITweet> PublishRetweet(ITweet tweet)
        {
            return PublishRetweet(tweet?.TweetDTO);
        }


        // Destroy Tweet
        public async Task<bool> DestroyTweet(long tweetId)
        {
            var requestResult = await _tweetsRequester.DestroyTweet(tweetId);
            return requestResult?.Response?.IsSuccessStatusCode == true;
        }

        public async Task<bool> DestroyTweet(ITweetDTO tweet)
        {
            var requestResult = await _tweetsRequester.DestroyTweet(tweet);
            return requestResult?.Response?.IsSuccessStatusCode == true;
        }

        public Task<bool> DestroyTweet(ITweet tweet)
        {
            return DestroyTweet(tweet?.TweetDTO);
        }
    }
}
