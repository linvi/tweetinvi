using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetQueryExecutor
    {
        Task<ITwitterResult<ITweetDTO>> GetTweet(IGetTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO>> PublishTweet(IPublishTweetParameters parameters, ITwitterRequest request);
        
        
        // Publish Retweet
        Task<ITwitterResult<ITweetDTO>> PublishRetweet(ITweetIdentifier tweetId, ITwitterRequest request);

        // UnRetweet
        Task<ITwitterResult> DestroyRetweet(ITweetIdentifier retweet, ITwitterRequest request);

        // Get Retweets
        Task<ITwitterResult<ITweetDTO[]>> GetRetweets(ITweetIdentifier tweetIdentifier, int? maxRetweetsToRetrieve, ITwitterRequest request);

        //Get Retweeters Ids
        Task<IEnumerable<long>> GetRetweetersIds(ITweetIdentifier tweetIdentifier, int maxRetweetersToRetrieve);

        // Destroy Tweet
        Task<ITwitterResult<ITweetDTO>> DestroyTweet(IDestroyTweetParameters parameters, ITwitterRequest request);

        // Favorite Tweet
        Task<bool> FavoriteTweet(ITweetDTO tweet);
        Task<bool> FavoriteTweet(long? tweetId);

        Task<bool> UnFavoriteTweet(ITweetDTO tweet);
        Task<bool> UnFavoriteTweet(long? tweetId);

        // Generate OEmbedTweet
        Task<IOEmbedTweetDTO> GenerateOEmbedTweet(ITweetDTO tweetDTO);
        Task<IOEmbedTweetDTO> GenerateOEmbedTweet(long tweetId);
        Task<ITwitterResult<ITweetDTO[]>> GetFavoriteTweets(IGetFavoriteTweetsParameters parameters, ITwitterRequest request);
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

        public Task<ITwitterResult<ITweetDTO>> PublishTweet(IPublishTweetParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetPublishTweetQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITweetDTO>(request);
        }

        // Publish Retweet
        public Task<ITwitterResult<ITweetDTO>> PublishRetweet(ITweetIdentifier tweetId, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetPublishRetweetQuery(tweetId, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITweetDTO>(request);
        }
        
        // Publish UnRetweet
        public Task<ITwitterResult> DestroyRetweet(ITweetIdentifier retweet, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetUnRetweetQuery(retweet);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest(request);
        }

        #region Get Retweets

        public Task<ITwitterResult<ITweetDTO[]>> GetRetweets(ITweetIdentifier tweetIdentifier, int? maxRetweetsToRetrieve, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetRetweetsQuery(tweetIdentifier, maxRetweetsToRetrieve, request.ExecutionContext);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<ITweetDTO[]>(request);
        }

        #endregion

        #region Get Retweeters IDs

        public Task<IEnumerable<long>> GetRetweetersIds(ITweetIdentifier tweetIdentifier, int maxRetweetersToRetrieve)
        {
            var query = _tweetQueryGenerator.GetRetweeterIdsQuery(tweetIdentifier, maxRetweetersToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query);
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
        public Task<ITwitterResult<ITweetDTO[]>> GetFavoriteTweets(IGetFavoriteTweetsParameters parameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetFavoriteTweetsQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITweetDTO[]>(request);
        }
        
        public Task<bool> FavoriteTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetFavoriteTweetQuery(tweet);
            return ExecuteFavoriteQuery(query);
        }

        public Task<bool> FavoriteTweet(long? tweetId)
        {
            string query = _tweetQueryGenerator.GetFavoriteTweetQuery(tweetId);
            return ExecuteFavoriteQuery(query);
        }

        private async Task<bool> ExecuteFavoriteQuery(string query)
        {
            // We need the try catch here as we need to know whether if the operation has failed and why it did!
            try
            {
                var jsonObject = await _twitterAccessor.ExecutePOSTQuery(query);
                return jsonObject != null;
            }
            catch (TwitterException ex)
            {
                // In this case the tweet has already been favourited.
                // Therefore we consider that the operation has been successfull.
                if (ex.TwitterExceptionInfos != null && ex.TwitterExceptionInfos.Any() && ex.TwitterExceptionInfos.First().Code == 139)
                {
                    return false;
                }

                // If the failure was caused by an error that is different from the one informing
                // that a tweet has already been favourited, then throw the exception because
                // we are currently not swallowing exceptions as it would have been done in the TwitterAccessor.
                throw;
            }
        }

        public async Task<bool> UnFavoriteTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetUnFavoriteTweetQuery(tweet);
            var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery(query);

            return asyncOperation.Success;
        }

        public async Task<bool> UnFavoriteTweet(long? tweetId)
        {
            string query = _tweetQueryGenerator.GetUnFavoriteTweetQuery(tweetId);
            var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery(query);

            return asyncOperation.Success;
        }

        // Generate OEmbed Tweet
        public Task<IOEmbedTweetDTO> GenerateOEmbedTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetGenerateOEmbedTweetQuery(tweet);
            return _twitterAccessor.ExecuteGETQuery<IOEmbedTweetDTO>(query);
        }

        public Task<IOEmbedTweetDTO> GenerateOEmbedTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetGenerateOEmbedTweetQuery(tweetId);
            return _twitterAccessor.ExecuteGETQuery<IOEmbedTweetDTO>(query);
        }
    }
}
