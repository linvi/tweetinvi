using System.Threading.Tasks;
using Tweetinvi.Client.Requesters.V2;
using Tweetinvi.Models.V2.Responses;
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

        public Task<TweetResponseDTO> GetTweetAsync(long tweetId)
        {
            return GetTweetAsync(new GetTweetV2Parameters(tweetId));
        }

        public async Task<TweetResponseDTO> GetTweetAsync(IGetTweetV2Parameters parameters)
        {
            var twitterResponse = await _tweetsV2Requester.GetTweetAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public Task<TweetsResponseDTO> GetTweetsAsync(long[] tweetIds)
        {
            return GetTweetsAsync(new GetTweetsV2Parameters(tweetIds));
        }

        public async Task<TweetsResponseDTO> GetTweetsAsync(IGetTweetsV2Parameters parameters)
        {
            var twitterResponse = await _tweetsV2Requester.GetTweetsAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }

        public Task<TweetHideResponseDTO> ChangeTweetReplyVisibilityAsync(long tweetId, TweetReplyVisibility visibility)
        {
            return ChangeTweetReplyVisibilityAsync(new ChangeTweetReplyVisibilityParameters(tweetId, visibility));
        }

        public async Task<TweetHideResponseDTO> ChangeTweetReplyVisibilityAsync(IChangeTweetReplyVisibilityParameters parameters)
        {
            var twitterResponse = await _tweetsV2Requester.ChangeTweetReplyVisibilityAsync(parameters).ConfigureAwait(false);
            return twitterResponse?.Model;
        }
    }
}