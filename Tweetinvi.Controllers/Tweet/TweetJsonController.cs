using System;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetJsonController
    {
        // Get Tweet
        string GetTweet(long tweetId);


        // Publish Tweet
        string PublishTweet(string text, IPublishTweetOptionalParameters optionalParameters = null);

        // Publish Retweet
        string PublishRetweet(ITweet tweet);
        string PublishRetweet(ITweetDTO tweetDTO);
        string PublishRetweet(long tweetId);

        // Get Retweets
        string GetRetweets(ITweetIdentifier tweetDTO, int maxRetweetsToRetrieve = 100);
        string GetRetweets(long tweetId, int maxRetweetsToRetrieve = 100);

        // Destroy Tweet
        string DestroyTweet(ITweet tweet);
        string DestroyTweet(ITweetDTO tweetDTO);
        string DestroyTweet(long tweetId);

        // Favourite Tweet
        string FavoriteTweet(ITweet tweet);
        string FavoriteTweet(ITweetDTO tweetDTO);
        string FavoriteTweet(long tweetId);

        string UnFavoriteTweet(ITweet tweet);
        string UnFavoriteTweet(ITweetDTO tweetDTO);
        string UnFavoriteTweet(long tweetId);

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
        private readonly ITweetQueryValidator _tweetQueryValidator;
        private readonly ITweetController _tweetController;

        public TweetJsonController(
            IUploadQueryExecutor uploadQueryExecutor,
            ITweetQueryGenerator tweetQueryGenerator,
            ITwitterAccessor twitterAccessor,
            ITweetQueryValidator tweetQueryValidator,
            ITweetController tweetController)
        {
            _uploadQueryExecutor = uploadQueryExecutor;
            _tweetQueryGenerator = tweetQueryGenerator;
            _twitterAccessor = twitterAccessor;
            _tweetQueryValidator = tweetQueryValidator;
            _tweetController = tweetController;
        }

        // Get Tweet
        public string GetTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetTweetQuery(tweetId);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        public string PublishTweet(string text, IPublishTweetOptionalParameters optionalParameters = null)
        {
            // The exceptions have to be raised before the QueryGenerator as 
            // We do not want to wait for the media to be uploaded to throw the
            // Exception. And The logic of uploading the media should live in
            // the TweetController

            var publishParameter = new PublishTweetParameters(text, optionalParameters);

            _tweetQueryValidator.ThrowIfTweetCannotBePublished(publishParameter);
            _tweetController.UploadMedias(publishParameter);

            var query = _tweetQueryGenerator.GetPublishTweetQuery(publishParameter);
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

        public string GetRetweets(ITweetIdentifier tweetIdentifier, int maxRetweetsToRetrieve = 100)
        {
            string query = _tweetQueryGenerator.GetRetweetsQuery(tweetIdentifier, maxRetweetsToRetrieve);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        public string GetRetweets(long tweetId, int maxRetweetsToRetrieve = 100)
        {
            return GetRetweets(new TweetIdentifier(tweetId), maxRetweetsToRetrieve);
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
        public string FavoriteTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return FavoriteTweet(tweet.TweetDTO);
        }

        public string FavoriteTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetFavoriteTweetQuery(tweetId);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string FavoriteTweet(ITweetDTO tweetDTO)
        {
            string query = _tweetQueryGenerator.GetFavoriteTweetQuery(tweetDTO);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string UnFavoriteTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return UnFavoriteTweet(tweet.TweetDTO);
        }

        public string UnFavoriteTweet(ITweetDTO tweetDTO)
        {
            string query = _tweetQueryGenerator.GetUnFavoriteTweetQuery(tweetDTO);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string UnFavoriteTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetUnFavoriteTweetQuery(tweetId);
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