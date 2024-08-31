using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Core.Controllers.V2
{
    public interface ITweetsV2Controller
    {
        Task<ITwitterResult<TweetV2Response>> GetTweetAsync(IGetTweetV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<TweetsV2Response>> GetTweetsAsync(IGetTweetsV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<TweetHideV2Response>> ChangeTweetReplyVisibilityAsync(IChangeTweetReplyVisibilityV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<TweetV2Response>> PublishTweetAsync(IPublishTweetV2Parameters parameters, ITwitterRequest request);
    }
}