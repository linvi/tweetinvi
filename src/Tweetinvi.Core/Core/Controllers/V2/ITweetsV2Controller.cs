using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Core.Controllers.V2
{
    public interface ITweetsV2Controller
    {
        Task<ITwitterResult<TweetResponseDTO>> GetTweetAsync(IGetTweetV2Parameters parameters, ITwitterRequest request);
    }
}