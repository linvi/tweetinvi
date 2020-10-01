using System.Threading.Tasks;
using Tweetinvi.Core.QueryGenerators.V2;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetsV2QueryExecutor
    {
        Task<ITwitterResult<TweetResponseDTO>> GetTweetAsync(IGetTweetV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<TweetsResponseDTO>> GetTweetsAsync(IGetTweetsV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<TweetHideResponseDTO>> ChangeTweetReplyVisibilityAsync(IChangeTweetReplyVisibilityParameters parameters, ITwitterRequest request);
    }

    public class TweetsV2QueryExecutor : ITweetsV2QueryExecutor
    {
        private readonly JsonContentFactory _jsonContentFactory;
        private readonly ITweetsV2QueryGenerator _tweetQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public TweetsV2QueryExecutor(
            JsonContentFactory jsonContentFactory,
            ITweetsV2QueryGenerator tweetQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _jsonContentFactory = jsonContentFactory;
            _tweetQueryGenerator = tweetQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult<TweetResponseDTO>> GetTweetAsync(IGetTweetV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _tweetQueryGenerator.GetTweetQuery(parameters);
            return _twitterAccessor.ExecuteRequestAsync<TweetResponseDTO>(request);
        }

        public Task<ITwitterResult<TweetsResponseDTO>> GetTweetsAsync(IGetTweetsV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _tweetQueryGenerator.GetTweetsQuery(parameters);
            return _twitterAccessor.ExecuteRequestAsync<TweetsResponseDTO>(request);
        }

        public Task<ITwitterResult<TweetHideResponseDTO>> ChangeTweetReplyVisibilityAsync(IChangeTweetReplyVisibilityParameters parameters, ITwitterRequest request)
        {
            var content = new TweetHiddenStateDTO
            {
                hidden = parameters.Visibility == 0
            };

            request.Query.Url = _tweetQueryGenerator.GetChangeTweetReplyVisibilityQuery(parameters);
            request.Query.HttpMethod = HttpMethod.PUT;
            request.Query.HttpContent = _jsonContentFactory.Create(content);
            return _twitterAccessor.ExecuteRequestAsync<TweetHideResponseDTO>(request);
        }
    }
}