using System.Threading.Tasks;
using Tweetinvi.Core.Controllers.V2;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Tweet
{
    public class TweetsV2Controller : ITweetsV2Controller
    {
        private readonly ITweetsV2QueryExecutor _queryExecutor;

        public TweetsV2Controller(ITweetsV2QueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }

        public Task<ITwitterResult<TweetV2Response>> GetTweetAsync(IGetTweetV2Parameters parameters, ITwitterRequest request)
        {
            return _queryExecutor.GetTweetAsync(parameters, request);
        }

        public Task<ITwitterResult<TweetsV2Response>> GetTweetsAsync(IGetTweetsV2Parameters parameters, ITwitterRequest request)
        {
            return _queryExecutor.GetTweetsAsync(parameters, request);
        }

        public Task<ITwitterResult<TweetHideV2Response>> ChangeTweetReplyVisibilityAsync(IChangeTweetReplyVisibilityV2Parameters parameters, ITwitterRequest request)
        {
            return _queryExecutor.ChangeTweetReplyVisibilityAsync(parameters, request);
        }

        public Task<ITwitterResult<TweetV2Response>> PublishTweetAsync(IPublishTweetV2Parameters parameters, ITwitterRequest request)
        {
            return _queryExecutor.PublishTweetAsync(parameters, request);
        }
    }
}