using System;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Json
{
    public static class TweetJson
    {
        [ThreadStatic]
        private static ITweetJsonController _tweetJsonController;
        public static ITweetJsonController TweetJsonController
        {
            get
            {
                if (_tweetJsonController == null)
                {
                    Initialize();
                }

                return _tweetJsonController;
            }
        }

        static TweetJson()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _tweetJsonController = TweetinviContainer.Resolve<ITweetJsonController>();
        }

        // Get Tweet
        public static Task<string> GetTweet(long tweetId)
        {
            return TweetJsonController.GetTweet(tweetId);
        }

        // Publish Tweet
        public static Task<string> PublishTweet(string text)
        {
            var parameters = new PublishTweetParameters(text);
            return PublishTweet(parameters);
        }

        public static Task<string> PublishTweet(IPublishTweetParameters parameters)
        {
            return TweetJsonController.PublishTweet(parameters);
        }

        // Publish Retweet
        public static Task<string> PublishRetweet(ITweet tweet)
        {
            return TweetJsonController.PublishRetweet(tweet);
        }

        public static Task<string> PublishRetweet(ITweetDTO tweetDTO)
        {
            return TweetJsonController.PublishRetweet(tweetDTO);
        }

        public static Task<string> PublishRetweet(long tweetId)
        {
            return TweetJsonController.PublishRetweet(tweetId);
        }

        // Get Retweets
        public static Task<string> GetRetweets(ITweet tweet)
        {
            return TweetJsonController.GetRetweets(tweet);
        }

        public static Task<string> GetRetweets(ITweetDTO tweetDTO)
        {
            return TweetJsonController.GetRetweets(tweetDTO);
        }

        public static Task<string> GetRetweets(long tweetId)
        {
            return TweetJsonController.GetRetweets(tweetId);
        }

        // Destroy Tweet
        public static Task<string> DestroyTweet(ITweet tweet)
        {
            return TweetJsonController.DestroyTweet(tweet);
        }

        public static Task<string> DestroyTweet(ITweetDTO tweetDTO)
        {
            return TweetJsonController.DestroyTweet(tweetDTO);
        }

        public static Task<string> DestroyTweet(long tweetId)
        {
            return TweetJsonController.DestroyTweet(tweetId);
        }

        // Favorite Tweet
        public static Task<string> FavoriteTweet(ITweet tweet)
        {
            return TweetJsonController.FavoriteTweet(tweet);
        }

        public static Task<string> FavoriteTweet(ITweetDTO tweetDTO)
        {
            return TweetJsonController.FavoriteTweet(tweetDTO);
        }

        public static Task<string> FavoriteTweet(long tweetId)
        {
            return TweetJsonController.FavoriteTweet(tweetId);
        }

        // UnFavourite
        public static Task<string> UnFavoriteTweet(ITweet tweet)
        {
            return TweetJsonController.UnFavoriteTweet(tweet);
        }

        public static Task<string> UnFavoriteTweet(ITweetDTO tweetDTO)
        {
            return TweetJsonController.UnFavoriteTweet(tweetDTO);
        }

        public static Task<string> UnFavoriteTweet(long tweetId)
        {
            return TweetJsonController.UnFavoriteTweet(tweetId);
        }

        // Generate OEmbedTweet
        public static Task<string> GenerateOEmbedTweet(ITweet tweet)
        {
            return TweetJsonController.GenerateOEmbedTweet(tweet);
        }

        public static Task<string> GenerateOEmbedTweet(ITweetDTO tweetDTO)
        {
            return TweetJsonController.GenerateOEmbedTweet(tweetDTO);
        }

        public static Task<string> GenerateOEmbedTweet(long tweetId)
        {
            return TweetJsonController.GenerateOEmbedTweet(tweetId);
        }
    }
}
