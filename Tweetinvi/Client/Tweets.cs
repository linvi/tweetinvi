using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Client
{
    public class Tweets
    {
        private readonly TwitterClient _client;
        private readonly ITweetRequester _tweetRequester;

        public Tweets(TwitterClient client)
        {
            _client = client;
            _tweetRequester = _client.RequestExecutor.TweetRequester;
        }

        public async Task<ITweet> GetTweet(long tweetId)
        {
            var result = await _tweetRequester.GetTweet(tweetId);

            return result.Result;
        }
    }
}
