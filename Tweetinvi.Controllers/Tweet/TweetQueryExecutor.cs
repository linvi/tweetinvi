using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetQueryExecutor
    {
        // Publish Tweet
        ITweetDTO PublishTweet(IPublishTweetParameters publishParameters);

        // Publish Retweet
        ITweetDTO PublishRetweet(ITweetDTO tweetToRetweet);
        ITweetDTO PublishRetweet(long tweetId);

        // UnRetweet
        ITweetDTO UnRetweet(ITweetIdentifier tweetToRetweet);
        ITweetDTO UnRetweet(long tweetId);

        // Get Retweets
        IEnumerable<ITweetDTO> GetRetweets(ITweetIdentifier tweetIdentifier, int maxRetweetsToRetrieve);

        //Get Retweeters Ids
        IEnumerable<long> GetRetweetersIds(ITweetIdentifier tweetIdentifier, int maxRetweetersToRetrieve);

        // Destroy Tweet
        bool DestroyTweet(ITweetDTO tweet);
        bool DestroyTweet(long tweetId);

        // Favorite Tweet
        bool FavoriteTweet(ITweetDTO tweet);
        bool FavoriteTweet(long tweetId);

        bool UnFavoriteTweet(ITweetDTO tweet);
        bool UnFavoriteTweet(long tweetId);

        // Generate OEmbedTweet
        IOEmbedTweetDTO GenerateOEmbedTweet(ITweetDTO tweetDTO);
        IOEmbedTweetDTO GenerateOEmbedTweet(long tweetId);
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

        public ITweetDTO PublishTweet(IPublishTweetParameters publishParameters)
        {
            string query = _tweetQueryGenerator.GetPublishTweetQuery(publishParameters);
            return _twitterAccessor.ExecutePOSTQuery<ITweetDTO>(query);
        }

        // Publish Retweet
        public ITweetDTO PublishRetweet(ITweetDTO tweetToRetweet)
        {
            string query = _tweetQueryGenerator.GetPublishRetweetQuery(tweetToRetweet);
            return _twitterAccessor.ExecutePOSTQuery<ITweetDTO>(query);
        }

        public ITweetDTO PublishRetweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetPublishRetweetQuery(tweetId);
            return _twitterAccessor.ExecutePOSTQuery<ITweetDTO>(query);
        }
        
        // Publish UnRetweet
        public ITweetDTO UnRetweet(ITweetIdentifier tweetToRetweet)
        {
            string query = _tweetQueryGenerator.GetUnRetweetQuery(tweetToRetweet);
            return _twitterAccessor.ExecutePOSTQuery<ITweetDTO>(query);
        }

        public ITweetDTO UnRetweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetUnRetweetQuery(tweetId);
            return _twitterAccessor.ExecutePOSTQuery<ITweetDTO>(query);
        }

        #region Get Retweets

        public IEnumerable<ITweetDTO> GetRetweets(ITweetIdentifier tweetIdentifier, int maxRetweetsToRetrieve)
        {
            var query = _tweetQueryGenerator.GetRetweetsQuery(tweetIdentifier, maxRetweetsToRetrieve);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        #endregion

        #region Get Retweeters IDs

        public IEnumerable<long> GetRetweetersIds(ITweetIdentifier tweetIdentifier, int maxRetweetersToRetrieve)
        {
            var query = _tweetQueryGenerator.GetRetweeterIdsQuery(tweetIdentifier, maxRetweetersToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query);
        }

        #endregion

        // Destroy Tweet
        public bool DestroyTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetDestroyTweetQuery(tweet);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool DestroyTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetDestroyTweetQuery(tweetId);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        // Favourite Tweet
        public bool FavoriteTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetFavoriteTweetQuery(tweet);
            return ExecuteFavoriteQuery(query);
        }

        public bool FavoriteTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetFavoriteTweetQuery(tweetId);
            return ExecuteFavoriteQuery(query);
        }

        private bool ExecuteFavoriteQuery(string query)
        {
            // We need the try catch here as we need to know whether if the operation has failed and why it did!
            try
            {
                return _twitterAccessor.ExecutePOSTQuery(query) != null;
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

        public bool UnFavoriteTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetUnFavoriteTweetQuery(tweet);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool UnFavoriteTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetUnFavoriteTweetQuery(tweetId);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        // Generate OEmbed Tweet
        public IOEmbedTweetDTO GenerateOEmbedTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetGenerateOEmbedTweetQuery(tweet);
            return _twitterAccessor.ExecuteGETQuery<IOEmbedTweetDTO>(query);
        }

        public IOEmbedTweetDTO GenerateOEmbedTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetGenerateOEmbedTweetQuery(tweetId);
            return _twitterAccessor.ExecuteGETQuery<IOEmbedTweetDTO>(query);
        }
    }
}
