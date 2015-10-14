using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
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

        // Get Retweets
        IEnumerable<ITweetDTO> GetRetweets(ITweetDTO tweet);
        IEnumerable<ITweetDTO> GetRetweets(long tweetId);

        // Destroy Tweet
        bool DestroyTweet(ITweetDTO tweet);
        bool DestroyTweet(long tweetId);

        // Favorite Tweet
        bool FavouriteTweet(ITweetDTO tweet);
        bool FavouriteTweet(long tweetId);

        bool UnFavouriteTweet(ITweetDTO tweet);
        bool UnFavouriteTweet(long tweetId);

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

        // Get Retweets
        public IEnumerable<ITweetDTO> GetRetweets(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetRetweetsQuery(tweet);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        public IEnumerable<ITweetDTO> GetRetweets(long tweetId)
        {
            string query = _tweetQueryGenerator.GetRetweetsQuery(tweetId);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

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
        public bool FavouriteTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetFavouriteTweetQuery(tweet);
            return ExecuteFavouriteQuery(query);
        }

        public bool FavouriteTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetFavouriteTweetQuery(tweetId);
            return ExecuteFavouriteQuery(query);
        }

        private bool ExecuteFavouriteQuery(string query)
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

        public bool UnFavouriteTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetUnFavouriteTweetQuery(tweet);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool UnFavouriteTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetUnFavouriteTweetQuery(tweetId);
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