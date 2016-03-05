using System;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Parameters;

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
        public static string GetTweet(long tweetId)
        {
            return TweetJsonController.GetTweet(tweetId);
        }

        // Publish Tweet
        public static string PublishTweet(string text, IPublishTweetOptionalParameters parameters = null)
        {
            return TweetJsonController.PublishTweet(text, parameters);
        }

        // Publish Retweet
        public static string PublishRetweet(ITweet tweet)
        {
            return TweetJsonController.PublishRetweet(tweet);
        }

        public static string PublishRetweet(ITweetDTO tweetDTO)
        {
            return TweetJsonController.PublishRetweet(tweetDTO);
        }

        public static string PublishRetweet(long tweetId)
        {
            return TweetJsonController.PublishRetweet(tweetId);
        }

        // Get Retweets
        public static string GetRetweets(ITweet tweet)
        {
            return TweetJsonController.GetRetweets(tweet);
        }

        public static string GetRetweets(ITweetDTO tweetDTO)
        {
            return TweetJsonController.GetRetweets(tweetDTO);
        }

        public static string GetRetweets(long tweetId)
        {
            return TweetJsonController.GetRetweets(tweetId);
        }

        // Destroy Tweet
        public static string DestroyTweet(ITweet tweet)
        {
            return TweetJsonController.DestroyTweet(tweet);
        }

        public static string DestroyTweet(ITweetDTO tweetDTO)
        {
            return TweetJsonController.DestroyTweet(tweetDTO);
        }

        public static string DestroyTweet(long tweetId)
        {
            return TweetJsonController.DestroyTweet(tweetId);
        }

        // Favorite Tweet
        public static string FavoriteTweet(ITweet tweet)
        {
            return TweetJsonController.FavoriteTweet(tweet);
        }

        public static string FavoriteTweet(ITweetDTO tweetDTO)
        {
            return TweetJsonController.FavoriteTweet(tweetDTO);
        }

        public static string FavoriteTweet(long tweetId)
        {
            return TweetJsonController.FavoriteTweet(tweetId);
        }

        // UnFavourite
        public static string UnFavoriteTweet(ITweet tweet)
        {
            return TweetJsonController.UnFavoriteTweet(tweet);
        }

        public static string UnFavoriteTweet(ITweetDTO tweetDTO)
        {
            return TweetJsonController.UnFavoriteTweet(tweetDTO);
        }

        public static string UnFavoriteTweet(long tweetId)
        {
            return TweetJsonController.UnFavoriteTweet(tweetId);
        }

        // Generate OEmbedTweet
        public static string GenerateOEmbedTweet(ITweet tweet)
        {
            return TweetJsonController.GenerateOEmbedTweet(tweet);
        }

        public static string GenerateOEmbedTweet(ITweetDTO tweetDTO)
        {
            return TweetJsonController.GenerateOEmbedTweet(tweetDTO);
        }

        public static string GenerateOEmbedTweet(long tweetId)
        {
            return TweetJsonController.GenerateOEmbedTweet(tweetId);
        }
    }
}
