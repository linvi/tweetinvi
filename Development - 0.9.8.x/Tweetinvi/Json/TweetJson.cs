using System;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;

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
        public static string PublishTweet(ITweet tweet)
        {
            return TweetJsonController.PublishTweet(tweet);
        }

        public static string PublishTweet(ITweetDTO tweetDTO)
        {
            return TweetJsonController.PublishTweet(tweetDTO);
        }

        // Publish Tweet In Reply To
        public static string PublishTweetInReplyTo(ITweet tweetToPublish, ITweet tweetToReplyTo)
        {
            return TweetJsonController.PublishTweetInReplyTo(tweetToPublish, tweetToReplyTo);
        }

        public static string PublishTweetInReplyTo(ITweetDTO tweetToPublish, ITweetDTO tweetToReplyTo)
        {
            return TweetJsonController.PublishTweetInReplyTo(tweetToPublish, tweetToReplyTo);
        }

        public static string PublishTweetInReplyTo(ITweetDTO tweetToPublish, long tweetIdToReplyTo)
        {
            return TweetJsonController.PublishTweetInReplyTo(tweetToPublish, tweetIdToReplyTo);
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
        public static string FavouriteTweet(ITweet tweet)
        {
            return TweetJsonController.FavouriteTweet(tweet);
        }

        public static string FavouriteTweet(ITweetDTO tweetDTO)
        {
            return TweetJsonController.FavouriteTweet(tweetDTO);
        }

        public static string FavouriteTweet(long tweetId)
        {
            return TweetJsonController.FavouriteTweet(tweetId);
        }

        // UnFavourite
        public static string UnFavouriteTweet(ITweet tweet)
        {
            return TweetJsonController.UnFavouriteTweet(tweet);
        }

        public static string UnFavouriteTweet(ITweetDTO tweetDTO)
        {
            return TweetJsonController.UnFavouriteTweet(tweetDTO);
        }

        public static string UnFavouriteTweet(long tweetId)
        {
            return TweetJsonController.UnFavouriteTweet(tweetId);
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
