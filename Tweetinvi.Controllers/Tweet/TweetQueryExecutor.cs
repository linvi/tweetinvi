using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Models.Interfaces;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetQueryExecutor
    {
        // Publish Tweet
        Task<ITwitterResult<ITweetDTO>> PublishTweet(IPublishTweetParameters publishParameters, ITwitterRequest request);

        // Publish Retweet
        Task<ITwitterResult<ITweetDTO>> PublishRetweet(long tweetId, ITwitterRequest request);

        // UnRetweet
        Task<ITweetDTO> UnRetweet(ITweetIdentifier tweetToRetweet);
        Task<ITweetDTO> UnRetweet(long tweetId);

        // Get Retweets
        Task<IEnumerable<ITweetDTO>> GetRetweets(ITweetIdentifier tweetIdentifier, int maxRetweetsToRetrieve);

        //Get Retweeters Ids
        Task<IEnumerable<long>> GetRetweetersIds(ITweetIdentifier tweetIdentifier, int maxRetweetersToRetrieve);

        // Destroy Tweet
        Task<ITwitterResult> DestroyTweet(long tweetId, ITwitterRequest request);

        // Favorite Tweet
        Task<bool> FavoriteTweet(ITweetDTO tweet);
        Task<bool> FavoriteTweet(long tweetId);

        Task<bool> UnFavoriteTweet(ITweetDTO tweet);
        Task<bool> UnFavoriteTweet(long tweetId);

        // Generate OEmbedTweet
        Task<IOEmbedTweetDTO> GenerateOEmbedTweet(ITweetDTO tweetDTO);
        Task<IOEmbedTweetDTO> GenerateOEmbedTweet(long tweetId);
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

        // Publish Tweet

        public Task<ITwitterResult<ITweetDTO>> PublishTweet(IPublishTweetParameters publishParameters, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetPublishTweetQuery(publishParameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequest<ITweetDTO>(request);
        }

        // Publish Retweet
        public Task<ITwitterResult<ITweetDTO>> PublishRetweet(long tweetId, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetPublishRetweetQuery(tweetId);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequest<ITweetDTO>(request);
        }
        
        // Publish UnRetweet
        public Task<ITweetDTO> UnRetweet(ITweetIdentifier tweetToRetweet)
        {
            string query = _tweetQueryGenerator.GetUnRetweetQuery(tweetToRetweet);
            return _twitterAccessor.ExecutePOSTQuery<ITweetDTO>(query);
        }

        public Task<ITweetDTO> UnRetweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetUnRetweetQuery(tweetId);
            return _twitterAccessor.ExecutePOSTQuery<ITweetDTO>(query);
        }

        #region Get Retweets

        public Task<IEnumerable<ITweetDTO>> GetRetweets(ITweetIdentifier tweetIdentifier, int maxRetweetsToRetrieve)
        {
            var query = _tweetQueryGenerator.GetRetweetsQuery(tweetIdentifier, maxRetweetsToRetrieve);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
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
        public Task<ITwitterResult> DestroyTweet(long tweetId, ITwitterRequest request)
        {
            var query = _tweetQueryGenerator.GetDestroyTweetQuery(tweetId);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequest(request);
        }

        // Favourite Tweet
        public Task<bool> FavoriteTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetFavoriteTweetQuery(tweet);
            return ExecuteFavoriteQuery(query);
        }

        public Task<bool> FavoriteTweet(long tweetId)
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

        public async Task<bool> UnFavoriteTweet(long tweetId)
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
