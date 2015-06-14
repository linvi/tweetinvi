using System;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.QueryGenerators;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetJsonController
    {
        // Get Tweet
        string GetTweet(long tweetId);


        // Publish Tweet
        string PublishTweet(ITweet tweet);
        string PublishTweet(ITweetDTO tweetDTO);

        // Publish Tweet in reply to
        string PublishTweetInReplyTo(ITweet tweetToPublish, ITweet tweetToReplyTo);
        string PublishTweetInReplyTo(ITweetDTO tweetToPublish, ITweetDTO tweetToReplyTo);
        string PublishTweetInReplyTo(ITweetDTO tweetToPublish, long tweetIdToReplyTo);

        // Publish Retweet
        string PublishRetweet(ITweet tweet);
        string PublishRetweet(ITweetDTO tweetDTO);
        string PublishRetweet(long tweetId);

        // Get Retweets
        string GetRetweets(ITweet tweet);
        string GetRetweets(ITweetDTO tweetDTO);
        string GetRetweets(long tweetId);

        // Destroy Tweet
        string DestroyTweet(ITweet tweet);
        string DestroyTweet(ITweetDTO tweetDTO);
        string DestroyTweet(long tweetId);

        // Favourite Tweet
        string FavouriteTweet(ITweet tweet);
        string FavouriteTweet(ITweetDTO tweetDTO);
        string FavouriteTweet(long tweetId);

        string UnFavouriteTweet(ITweet tweet);
        string UnFavouriteTweet(ITweetDTO tweetDTO);
        string UnFavouriteTweet(long tweetId);

        // Generate OembedTweet
        string GenerateOEmbedTweet(ITweet tweet);
        string GenerateOEmbedTweet(ITweetDTO tweetDTO);
        string GenerateOEmbedTweet(long tweetId);
    }

    public class TweetJsonController : ITweetJsonController
    {
        private readonly IUploadQueryExecutor _uploadQueryExecutor;
        private readonly ITweetQueryGenerator _tweetQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public TweetJsonController(
            IUploadQueryExecutor uploadQueryExecutor,
            ITweetQueryGenerator tweetQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _uploadQueryExecutor = uploadQueryExecutor;
            _tweetQueryGenerator = tweetQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        // Get Tweet
        public string GetTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetTweetQuery(tweetId);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        // Publish Tweet
        public string PublishTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return PublishTweet(tweet.TweetDTO);
        }

        public string PublishTweet(ITweetDTO tweetToPublish)
        {
            _uploadQueryExecutor.UploadTweetMediasBeforePublish(tweetToPublish);

            string query = _tweetQueryGenerator.GetPublishTweetQuery(tweetToPublish);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        // Publish In Reply To
        public string PublishTweetInReplyTo(ITweet tweetToPublish, ITweet tweetToReplyTo)
        {
            if (tweetToPublish == null || tweetToReplyTo == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return PublishTweetInReplyTo(tweetToPublish.TweetDTO, tweetToReplyTo.TweetDTO);
        }

        public string PublishTweetInReplyTo(ITweetDTO tweetToPublish, ITweetDTO tweetToReplyTo)
        {
            _uploadQueryExecutor.UploadTweetMediasBeforePublish(tweetToPublish);

            string query = _tweetQueryGenerator.GetPublishTweetInReplyToQuery(tweetToPublish, tweetToReplyTo);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string PublishTweetInReplyTo(ITweetDTO tweetToPublish, long tweetIdToReplyTo)
        {
            _uploadQueryExecutor.UploadTweetMediasBeforePublish(tweetToPublish);

            string query = _tweetQueryGenerator.GetPublishTweetInReplyToQuery(tweetToPublish, tweetIdToReplyTo);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        // Publish Retweet
        public string PublishRetweet(ITweet tweetToRetweet)
        {
            if (tweetToRetweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return PublishRetweet(tweetToRetweet.TweetDTO);
        }

        public string PublishRetweet(ITweetDTO tweetToRetweet)
        {
            string query = _tweetQueryGenerator.GetPublishRetweetQuery(tweetToRetweet);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string PublishRetweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetPublishRetweetQuery(tweetId);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        // Get Retweets
        public string GetRetweets(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return GetRetweets(tweet.TweetDTO);
        }

        public string GetRetweets(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetRetweetsQuery(tweet);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        public string GetRetweets(long tweetId)
        {
            string query = _tweetQueryGenerator.GetRetweetsQuery(tweetId);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        public string DestroyTweet(ITweet tweetToDestroy)
        {
            if (tweetToDestroy == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return DestroyTweet(tweetToDestroy.TweetDTO);
        }

        // Destroy Tweet
        public string DestroyTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetDestroyTweetQuery(tweet);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string DestroyTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetDestroyTweetQuery(tweetId);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        // Favourite Tweet
        public string FavouriteTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return FavouriteTweet(tweet.TweetDTO);
        }

        public string FavouriteTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetFavouriteTweetQuery(tweetId);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string FavouriteTweet(ITweetDTO tweetDTO)
        {
            string query = _tweetQueryGenerator.GetFavouriteTweetQuery(tweetDTO);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string UnFavouriteTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return UnFavouriteTweet(tweet.TweetDTO);
        }

        public string UnFavouriteTweet(ITweetDTO tweetDTO)
        {
            string query = _tweetQueryGenerator.GetUnFavouriteTweetQuery(tweetDTO);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string UnFavouriteTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetUnFavouriteTweetQuery(tweetId);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        // Generate OEmbed Tweet
        public string GenerateOEmbedTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return GenerateOEmbedTweet(tweet.TweetDTO);
        }

        public string GenerateOEmbedTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetGenerateOEmbedTweetQuery(tweet);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        public string GenerateOEmbedTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetGenerateOEmbedTweetQuery(tweetId);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }
    }
}