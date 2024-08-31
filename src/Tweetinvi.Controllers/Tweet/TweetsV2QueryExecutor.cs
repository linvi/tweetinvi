using System.Threading.Tasks;
using Tweetinvi.Core.QueryGenerators.V2;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetsV2QueryExecutor
    {
        Task<ITwitterResult<TweetV2Response>> GetTweetAsync(IGetTweetV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<TweetsV2Response>> GetTweetsAsync(IGetTweetsV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<TweetHideV2Response>> ChangeTweetReplyVisibilityAsync(IChangeTweetReplyVisibilityV2Parameters parameters, ITwitterRequest request);
        Task<ITwitterResult<TweetV2Response>> PublishTweetAsync(IPublishTweetV2Parameters parameters, ITwitterRequest request);
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

        public Task<ITwitterResult<TweetV2Response>> GetTweetAsync(IGetTweetV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _tweetQueryGenerator.GetTweetQuery(parameters);
            return _twitterAccessor.ExecuteRequestAsync<TweetV2Response>(request);
        }

        public Task<ITwitterResult<TweetsV2Response>> GetTweetsAsync(IGetTweetsV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _tweetQueryGenerator.GetTweetsQuery(parameters);
            return _twitterAccessor.ExecuteRequestAsync<TweetsV2Response>(request);
        }

        public Task<ITwitterResult<TweetHideV2Response>> ChangeTweetReplyVisibilityAsync(IChangeTweetReplyVisibilityV2Parameters parameters, ITwitterRequest request)
        {
            var content = new TweetHideStateV2
            {
                Hidden = parameters.Visibility == 0
            };

            request.Query.Url = _tweetQueryGenerator.GetChangeTweetReplyVisibilityQuery(parameters);
            request.Query.HttpMethod = HttpMethod.PUT;
            request.Query.HttpContent = _jsonContentFactory.Create(content);
            return _twitterAccessor.ExecuteRequestAsync<TweetHideV2Response>(request);
        }

        public Task<ITwitterResult<TweetV2Response>> PublishTweetAsync(IPublishTweetV2Parameters parameters, ITwitterRequest request)
        {
            request.Query.HttpMethod = HttpMethod.POST;
            request.Query.Url = _tweetQueryGenerator.GetTweetQuery(parameters);
            request.Query.HttpContent = _jsonContentFactory.Create(parameters);
            var stringContent = request.Query.HttpContent.ReadAsStringAsync().Result;
            return _twitterAccessor.ExecuteRequestAsync<TweetV2Response>(request);
        }
    }
}