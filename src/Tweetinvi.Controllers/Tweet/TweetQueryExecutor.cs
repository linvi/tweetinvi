using System.Threading.Tasks;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetQueryExecutor
    {
        Task<ITwitterResult<ITweetDTO>> GetTweetAsync(IGetTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO[]>> GetTweetsAsync(IGetTweetsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO>> PublishTweetAsync(IPublishTweetParameters parameters, ITwitterRequest request);


        // Publish Retweet
        Task<ITwitterResult<ITweetDTO>> PublishRetweetAsync(IPublishRetweetParameters parameters, ITwitterRequest request);

        // UnRetweet
        Task<ITwitterResult<ITweetDTO>> DestroyRetweetAsync(IDestroyRetweetParameters parameters, ITwitterRequest request);

        // Get Retweets
        Task<ITwitterResult<ITweetDTO[]>> GetRetweetsAsync(IGetRetweetsParameters parameters, ITwitterRequest request);

        //Get Retweeters Ids
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetRetweeterIdsAsync(IGetRetweeterIdsParameters parameters, ITwitterRequest request);

        // Destroy Tweet
        Task<ITwitterResult<ITweetDTO>> DestroyTweetAsync(IDestroyTweetParameters parameters, ITwitterRequest request);

        // Favorite Tweet
        Task<ITwitterResult<ITweetDTO>> FavoriteTweetAsync(IFavoriteTweetParameters parameters, ITwitterRequest request);

        Task<ITwitterResult<ITweetDTO>> UnfavoriteTweetAsync(IUnfavoriteTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO[]>> GetFavoriteTweetsAsync(IGetUserFavoriteTweetsParameters parameters, ITwitterRequest request);

        Task<ITwitterResult<IOEmbedTweetDTO>> GetOEmbedTweetAsync(IGetOEmbedTweetParameters parameters, ITwitterRequest request);
    }

    public class TweetQueryExecutor : ITweetQueryExecutor
    {
        private readonly ITweetQueryGenerator _tweetQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public TweetQueryExecutor(
            ITweetQueryGenerator tweetQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _tweetQueryGenerator = tweetQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult<ITweetDTO>> GetTweetAsync(IGetTweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetTweetQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITweetDTO>(request);
        }

        public Task<ITwitterResult<ITweetDTO[]>> GetTweetsAsync(IGetTweetsParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetTweetsQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITweetDTO[]>(request);
        }

        public Task<ITwitterResult<ITweetDTO>> PublishTweetAsync(IPublishTweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetPublishTweetQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITweetDTO>(request);
        }

        // Publish Retweet
        public Task<ITwitterResult<ITweetDTO[]>> GetRetweetsAsync(IGetRetweetsParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetRetweetsQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITweetDTO[]>(request);
        }

        public Task<ITwitterResult<ITweetDTO>> PublishRetweetAsync(IPublishRetweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetPublishRetweetQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITweetDTO>(request);
        }

        // Publish UnRetweet
        public Task<ITwitterResult<ITweetDTO>> DestroyRetweetAsync(IDestroyRetweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetDestroyRetweetQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITweetDTO>(request);
        }

        #region Get Retweeters IDs

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetRetweeterIdsAsync(IGetRetweeterIdsParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetRetweeterIdsQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request);
        }

        #endregion

        // Destroy Tweet
        public Task<ITwitterResult<ITweetDTO>> DestroyTweetAsync(IDestroyTweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetDestroyTweetQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITweetDTO>(request);
        }

        // Favourite Tweet
        public Task<ITwitterResult<ITweetDTO[]>> GetFavoriteTweetsAsync(IGetUserFavoriteTweetsParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetFavoriteTweetsQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITweetDTO[]>(request);
        }

        public Task<ITwitterResult<IOEmbedTweetDTO>> GetOEmbedTweetAsync(IGetOEmbedTweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetOEmbedTweetQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<IOEmbedTweetDTO>(request);
        }

        public Task<ITwitterResult<ITweetDTO>> FavoriteTweetAsync(IFavoriteTweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetCreateFavoriteTweetQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITweetDTO>(request);
        }

        public Task<ITwitterResult<ITweetDTO>> UnfavoriteTweetAsync(IUnfavoriteTweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetUnfavoriteTweetQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITweetDTO>(request);
        }
    }
}
