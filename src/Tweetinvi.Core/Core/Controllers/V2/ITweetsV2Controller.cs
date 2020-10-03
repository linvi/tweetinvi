using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Core.Controllers.V2
{
    public interface ITweetsV2Controller
    {
        Task<ITwitterResult<TweetV2Response>> GetTweetAsync(IGetTweetV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<TweetsV2Response>> GetTweetsAsync(IGetTweetsV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<TweetHideV2Response>> ChangeTweetReplyVisibilityAsync(IChangeTweetReplyVisibilityParameters parameters, ITwitterRequest request);
    }
}