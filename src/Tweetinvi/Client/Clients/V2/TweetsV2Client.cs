using System.Threading.Tasks;
using Tweetinvi.Client.Requesters.V2;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.V2
{
    public class TweetsV2Client : ITweetsV2Client
    {
        private readonly ITweetsV2Requester _tweetsV2Requester;

        public TweetsV2Client(ITweetsV2Requester tweetsV2Requester)
        {
            _tweetsV2Requester = tweetsV2Requester;
        }

        public Task<TweetV2Response> GetTweetAsync(long tweetId)
        {
            return GetTweetAsync(new GetTweetV2Parameters(tweetId));
        }

        public Task<TweetV2Response> GetTweetAsync(string tweetId)
        {
            return GetTweetAsync(new GetTweetV2Parameters(tweetId));
        }

        public async Task<TweetV2Response> GetTweetAsync(IGetTweetV2Parameters parameters)
        {
            var twitterResponse = await _tweetsV2Requester.GetTweetAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public Task<TweetsV2Response> GetTweetsAsync(long[] tweetIds)
        {
            return GetTweetsAsync(new GetTweetsV2Parameters(tweetIds));
        }

        public Task<TweetsV2Response> GetTweetsAsync(string[] tweetIds)
        {
            return GetTweetsAsync(new GetTweetsV2Parameters(tweetIds));
        }

        public async Task<TweetsV2Response> GetTweetsAsync(IGetTweetsV2Parameters parameters)
        {
            var twitterResponse = await _tweetsV2Requester.GetTweetsAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public Task<TweetHideV2Response> ChangeTweetReplyVisibilityAsync(long tweetId, TweetReplyVisibility visibility)
        {
            return ChangeTweetReplyVisibilityAsync(new ChangeTweetReplyVisibilityV2Parameters(tweetId, visibility));
        }

        public Task<TweetHideV2Response> ChangeTweetReplyVisibilityAsync(string tweetId, TweetReplyVisibility visibility)
        {
            return ChangeTweetReplyVisibilityAsync(new ChangeTweetReplyVisibilityV2Parameters(tweetId, visibility));
        }

        public async Task<TweetHideV2Response> ChangeTweetReplyVisibilityAsync(IChangeTweetReplyVisibilityV2Parameters parameters)
        {
            var twitterResponse = await _tweetsV2Requester.ChangeTweetReplyVisibilityAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public async Task<TweetV2Response> PublishTweetAsync(IPublishTweetV2Parameters parameters)
        {
            var twitterResponse = await _tweetsV2Requester.PublishTweetAsync(parameters);
            return twitterResponse.Model;
        }
    }
}