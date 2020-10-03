using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.Requesters.V2
{
    public interface ITweetsV2Requester
    {
        Task<ITwitterResult<TweetV2Response>> GetTweetAsync(IGetTweetV2Parameters parameters);
        Task<ITwitterResult<TweetsV2Response>> GetTweetsAsync(IGetTweetsV2Parameters parameters);
        Task<ITwitterResult<TweetHideV2Response>> ChangeTweetReplyVisibilityAsync(IChangeTweetReplyVisibilityParameters parameters);
    }
}