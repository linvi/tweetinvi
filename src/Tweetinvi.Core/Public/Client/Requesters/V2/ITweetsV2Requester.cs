using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.Requesters.V2
{
    public interface ITweetsV2Requester
    {
        Task<ITwitterResult<TweetResponseDTO>> GetTweetAsync(IGetTweetV2Parameters parameters);
        Task<ITwitterResult<TweetsResponseDTO>> GetTweetsAsync(IGetTweetsV2Parameters parameters);
    }
}