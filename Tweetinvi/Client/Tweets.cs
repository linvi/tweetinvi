using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Client
{
    public class Tweets
    {
        private readonly TwitterClient _client;
        private readonly ITweetsRequester _tweetsRequester;

        public Tweets(TwitterClient client)
        {
            _client = client;
            _tweetsRequester = _client.RequestExecutor.Tweets;
        }

        public async Task<ITweet> GetTweet(long tweetId)
        {
            var result = await _tweetsRequester.GetTweet(tweetId);

            return result.Result;
        }

        public async Task<ITweet[]> GetTweets(long[] tweetIds)
        {
            var result = await _tweetsRequester.GetTweets(tweetIds);

            return result.Result;
        }
    }
}
