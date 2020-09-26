using System.Threading.Tasks;
using Tweetinvi.Core.Controllers.V2;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Models.V2.Responses;
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

        public Task<ITwitterResult<TweetResponseDTO>> GetTweetAsync(IGetTweetV2Parameters parameters, ITwitterRequest request)
        {
            return _queryExecutor.GetTweetAsync(parameters, request);
        }
    }
}