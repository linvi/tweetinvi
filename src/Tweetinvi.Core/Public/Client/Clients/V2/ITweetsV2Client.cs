using System.Threading.Tasks;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.V2
{
    public interface ITweetsV2Client
    {
        Task<TweetV2Response> GetTweetAsync(long tweetId);
        Task<TweetV2Response> GetTweetAsync(IGetTweetV2Parameters parameters);

        Task<TweetsV2Response> GetTweetsAsync(long[] tweetIds);
        Task<TweetsV2Response> GetTweetsAsync(IGetTweetsV2Parameters parameters);

        Task<TweetHideV2Response> ChangeTweetReplyVisibilityAsync(long tweetId, TweetReplyVisibility visibility);
        Task<TweetHideV2Response> ChangeTweetReplyVisibilityAsync(IChangeTweetReplyVisibilityV2Parameters parameters);
    }
}