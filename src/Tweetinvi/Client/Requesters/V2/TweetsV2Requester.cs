using System.Threading.Tasks;
using Tweetinvi.Core.Controllers.V2;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.Requesters.V2
{
    public class TweetsV2Requester : BaseRequester, ITweetsV2Requester
    {
        private readonly ITweetsV2Controller _tweetsV2Controller;

        public TweetsV2Requester(
            ITwitterClient client,
            ITwitterClientEvents twitterClientEvents,
            ITweetsV2Controller tweetsV2Controller) : base(client, twitterClientEvents)
        {
            _tweetsV2Controller = tweetsV2Controller;
        }

        public Task<ITwitterResult<TweetV2Response>> GetTweetAsync(IGetTweetV2Parameters parameters)
        {
            return ExecuteRequestAsync(request => _tweetsV2Controller.GetTweetAsync(parameters, request));
        }

        public Task<ITwitterResult<TweetsV2Response>> GetTweetsAsync(IGetTweetsV2Parameters parameters)
        {
            return ExecuteRequestAsync(request => _tweetsV2Controller.GetTweetsAsync(parameters, request));
        }

        public Task<ITwitterResult<TweetHideV2Response>> ChangeTweetReplyVisibilityAsync(IChangeTweetReplyVisibilityV2Parameters parameters)
        {
            return ExecuteRequestAsync(request => _tweetsV2Controller.ChangeTweetReplyVisibilityAsync(parameters, request));
        }

        public Task<ITwitterResult<TweetV2Response>> PublishTweetAsync(IPublishTweetV2Parameters parameters)
        {
            return ExecuteRequestAsync(request => _tweetsV2Controller.PublishTweetAsync(parameters, request));
        }
    }
}