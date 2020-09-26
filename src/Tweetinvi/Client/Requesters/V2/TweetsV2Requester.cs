using System.Threading.Tasks;
using Tweetinvi.Core.Controllers.V2;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.V2;
using Tweetinvi.Models.V2.Responses;
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

        public Task<ITwitterResult<TweetResponseDTO>> GetTweet(IGetTweetV2Parameters parameters)
        {
            return ExecuteRequestAsync(request => _tweetsV2Controller.GetTweetAsync(parameters, request));
        }
    }
}