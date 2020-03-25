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
        Task<ITwitterResult<ITweetDTO>> GetTweet(IGetTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO[]>> GetTweets(IGetTweetsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO>> PublishTweet(IPublishTweetParameters parameters, ITwitterRequest request);


        // Publish Retweet
        Task<ITwitterResult<ITweetDTO>> PublishRetweet(IPublishRetweetParameters parameters, ITwitterRequest request);

        // UnRetweet
        Task<ITwitterResult<ITweetDTO>> DestroyRetweet(IDestroyRetweetParameters parameters, ITwitterRequest request);

        // Get Retweets
        Task<ITwitterResult<ITweetDTO[]>> GetRetweets(IGetRetweetsParameters parameters, ITwitterRequest request);

        //Get Retweeters Ids
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetRetweeterIds(IGetRetweeterIdsParameters parameters, ITwitterRequest request);

        // Destroy Tweet
        Task<ITwitterResult<ITweetDTO>> DestroyTweet(IDestroyTweetParameters parameters, ITwitterRequest request);

        // Favorite Tweet
        Task<ITwitterResult<ITweetDTO>> FavoriteTweet(IFavoriteTweetParameters parameters, ITwitterRequest request);

        Task<ITwitterResult<ITweetDTO>> UnfavoriteTweet(IUnfavoriteTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO[]>> GetFavoriteTweets(IGetUserFavoriteTweetsParameters parameters, ITwitterRequest request);

        Task<ITwitterResult<IOEmbedTweetDTO>> GetOEmbedTweet(IGetOEmbedTweetParameters parameters, ITwitterRequest request);
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

        public Task<ITwitterResult<ITweetDTO>> GetTweet(IGetTweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetTweetQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITweetDTO>(request);
        }

        public Task<ITwitterResult<ITweetDTO[]>> GetTweets(IGetTweetsParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetTweetsQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITweetDTO[]>(request);
        }

        public Task<ITwitterResult<ITweetDTO>> PublishTweet(IPublishTweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetPublishTweetQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITweetDTO>(request);
        }

        // Publish Retweet
        public Task<ITwitterResult<ITweetDTO[]>> GetRetweets(IGetRetweetsParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetRetweetsQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITweetDTO[]>(request);
        }

        public Task<ITwitterResult<ITweetDTO>> PublishRetweet(IPublishRetweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetPublishRetweetQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITweetDTO>(request);
        }

        // Publish UnRetweet
        public Task<ITwitterResult<ITweetDTO>> DestroyRetweet(IDestroyRetweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetDestroyRetweetQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITweetDTO>(request);
        }

        #region Get Retweeters IDs

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetRetweeterIds(IGetRetweeterIdsParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetRetweeterIdsQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<IIdsCursorQueryResultDTO>(request);
        }

        #endregion

        // Destroy Tweet
        public Task<ITwitterResult<ITweetDTO>> DestroyTweet(IDestroyTweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetDestroyTweetQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITweetDTO>(request);
        }

        // Favourite Tweet
        public Task<ITwitterResult<ITweetDTO[]>> GetFavoriteTweets(IGetUserFavoriteTweetsParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetFavoriteTweetsQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITweetDTO[]>(request);
        }

        public Task<ITwitterResult<IOEmbedTweetDTO>> GetOEmbedTweet(IGetOEmbedTweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetOEmbedTweetQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<IOEmbedTweetDTO>(request);
        }

        public Task<ITwitterResult<ITweetDTO>> FavoriteTweet(IFavoriteTweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetCreateFavoriteTweetQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITweetDTO>(request);
        }

        public Task<ITwitterResult<ITweetDTO>> UnfavoriteTweet(IUnfavoriteTweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetUnfavoriteTweetQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITweetDTO>(request);
        }
    }
}
