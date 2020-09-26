using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.Requesters.V2
{
    public interface ITweetsV2Requester
    {
        Task<ITwitterResult<TweetResponseDTO>> GetTweet(IGetTweetV2Parameters parameters);
        Task<ITwitterResult<TweetsResponseDTO>> GetTweets(IGetTweetsV2Parameters parameters);
    }
}