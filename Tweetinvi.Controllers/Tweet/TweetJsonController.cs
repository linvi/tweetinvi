using System;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetJsonController
    {
        // Get Tweet
        Task<string> GetTweet(long tweetId);


        // Publish Tweet
        Task<string> PublishTweet(IPublishTweetParameters parameters);

        // Publish Retweet
        Task<string> PublishRetweet(ITweet tweet);
        Task<string> PublishRetweet(ITweetDTO tweetDTO);
        Task<string> PublishRetweet(long tweetId);

        // Get Retweets
        Task<string> GetRetweets(ITweetIdentifier tweetIdentifier, int maxRetweetsToRetrieve = 100);
        Task<string> GetRetweets(long tweetId, int maxRetweetsToRetrieve = 100);

        // Destroy Tweet
        Task<string> DestroyTweet(ITweet tweet);
        Task<string> DestroyTweet(ITweetDTO tweetDTO);
        Task<string> DestroyTweet(long tweetId);

        // Favourite Tweet
        Task<string> FavoriteTweet(ITweet tweet);
        Task<string> FavoriteTweet(ITweetDTO tweetDTO);
        Task<string> FavoriteTweet(long tweetId);

        Task<string> UnFavoriteTweet(ITweet tweet);
        Task<string> UnFavoriteTweet(ITweetDTO tweetDTO);
        Task<string> UnFavoriteTweet(long tweetId);

        // Generate OembedTweet
        Task<string> GenerateOEmbedTweet(ITweet tweet);
        Task<string> GenerateOEmbedTweet(ITweetDTO tweetDTO);
        Task<string> GenerateOEmbedTweet(long tweetId);
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
        public Task<string> GetTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetTweetQuery(tweetId);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        public Task<string> PublishTweet(IPublishTweetParameters parameters)
        {
            // The exceptions have to be raised before the QueryGenerator as 
            // We do not want to wait for the media to be uploaded to throw the
            // Exception. And The logic of uploading the media should live in
            // the TweetController
            _tweetQueryValidator.ThrowIfTweetCannotBePublished(parameters);
            _tweetController.UploadMedias(parameters);

            var query = _tweetQueryGenerator.GetPublishTweetQuery(parameters);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        // Publish Retweet
        public Task<string> PublishRetweet(ITweet tweetToRetweet)
        {
            if (tweetToRetweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return PublishRetweet(tweetToRetweet.TweetDTO);
        }

        public Task<string> PublishRetweet(ITweetDTO tweetToRetweet)
        {
            string query = _tweetQueryGenerator.GetPublishRetweetQuery(tweetToRetweet);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public Task<string> PublishRetweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetPublishRetweetQuery(tweetId);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        // Get Retweets

        public Task<string> GetRetweets(ITweetIdentifier tweetIdentifier, int maxRetweetsToRetrieve = 100)
        {
            string query = _tweetQueryGenerator.GetRetweetsQuery(tweetIdentifier, maxRetweetsToRetrieve);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        public Task<string> GetRetweets(long tweetId, int maxRetweetsToRetrieve = 100)
        {
            return GetRetweets(new TweetIdentifier(tweetId), maxRetweetsToRetrieve);
        }

        public Task<string> DestroyTweet(ITweet tweetToDestroy)
        {
            if (tweetToDestroy == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return DestroyTweet(tweetToDestroy.TweetDTO);
        }

        // Destroy Tweet
        public Task<string> DestroyTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetDestroyTweetQuery(tweet);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public Task<string> DestroyTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetDestroyTweetQuery(tweetId);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        // Favourite Tweet
        public Task<string> FavoriteTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return FavoriteTweet(tweet.TweetDTO);
        }

        public Task<string> FavoriteTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetFavoriteTweetQuery(tweetId);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public Task<string> FavoriteTweet(ITweetDTO tweetDTO)
        {
            string query = _tweetQueryGenerator.GetFavoriteTweetQuery(tweetDTO);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public Task<string> UnFavoriteTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return UnFavoriteTweet(tweet.TweetDTO);
        }

        public Task<string> UnFavoriteTweet(ITweetDTO tweetDTO)
        {
            string query = _tweetQueryGenerator.GetUnFavoriteTweetQuery(tweetDTO);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public Task<string> UnFavoriteTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetUnFavoriteTweetQuery(tweetId);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        // Generate OEmbed Tweet
        public Task<string> GenerateOEmbedTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null");
            }

            return GenerateOEmbedTweet(tweet.TweetDTO);
        }

        public Task<string> GenerateOEmbedTweet(ITweetDTO tweet)
        {
            string query = _tweetQueryGenerator.GetGenerateOEmbedTweetQuery(tweet);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        public Task<string> GenerateOEmbedTweet(long tweetId)
        {
            string query = _tweetQueryGenerator.GetGenerateOEmbedTweetQuery(tweetId);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }
    }
}