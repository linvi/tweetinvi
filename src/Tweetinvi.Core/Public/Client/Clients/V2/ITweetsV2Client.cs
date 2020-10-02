using System.Threading.Tasks;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.V2
{
    public interface ITweetsV2Client
    {
        Task<TweetResponseDTO> GetTweetAsync(long tweetId);
        Task<TweetResponseDTO> GetTweetAsync(IGetTweetV2Parameters parameters);

        Task<TweetsResponseDTO> GetTweetsAsync(long[] tweetIds);
        Task<TweetsResponseDTO> GetTweetsAsync(IGetTweetsV2Parameters parameters);

        Task<TweetHideResponseDTO> ChangeTweetReplyVisibilityAsync(long tweetId, TweetReplyVisibility visibility);
        Task<TweetHideResponseDTO> ChangeTweetReplyVisibilityAsync(IChangeTweetReplyVisibilityParameters parameters);
    }
}