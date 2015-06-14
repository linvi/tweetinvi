using System;
using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi
{
    public static class Tweet
    {
        [ThreadStatic]
        private static ITweetFactory _tweetFactory;
        public static ITweetFactory TweetFactory
        {
            get
            {
                if (_tweetFactory == null)
                {
                    Initialize();
                }

                return _tweetFactory;
            }
        }
        
        [ThreadStatic]
        private static ITweetController _tweetController;
        public static ITweetController TweetController
        {
            get
            {
                if (_tweetController == null)
                {
                    Initialize();
                }

                return _tweetController;
            }
        }

        static Tweet()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _tweetController = TweetinviContainer.Resolve<ITweetController>();
            _tweetFactory = TweetinviContainer.Resolve<ITweetFactory>();
        }

        #region Tweet Factory
        
        public static ITweet GetTweet(long tweetId)
        {
            return TweetFactory.GetTweet(tweetId);
        }

        public static IEnumerable<ITweet> GetTweets(params long[] tweetIds)
        {
            return TweetFactory.GetTweets(tweetIds);
        }

        public static ITweet CreateTweet(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("Text cannot be null");
            }

            return TweetFactory.CreateTweet(text);
        }

        public static ITweet CreateTweetWithMedia(string text, byte[] media)
        {
            var tweet = CreateTweet(text);
            tweet.AddMedia(media);
            return tweet;
        }

        public static ITweet GenerateTweetFromDTO(ITweetDTO tweetDTO)
        {
            return TweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        public static IEnumerable<ITweet> GenerateTweetsFromDTO(IEnumerable<ITweetDTO> tweetsDTO)
        {
            return TweetFactory.GenerateTweetsFromDTO(tweetsDTO);
        }

        #endregion

        #region Tweet Controller

        // Publish Tweet
        public static ITweet PublishTweet(string text)
        {
            var tweet = CreateTweet(text);
            if (TweetController.PublishTweet(tweet))
            {
                return tweet;
            }

            return null;
        }

        public static ITweet PublishTweetWithMedia(string text, byte[] media)
        {
            var tweetToPublish = CreateTweetWithMedia(text, media);
            tweetToPublish.Publish();
            return tweetToPublish;
        }

        public static bool PublishTweet(ITweet tweet)
        {
            return TweetController.PublishTweet(tweet);
        }

        // Publish TweetInReplyTo From Text
        public static ITweet PublishTweetInReplyTo(string text, ITweet tweetToReplyTo)
        {
            var tweet = TweetFactory.CreateTweet(text);
            return TweetController.PublishTweetInReplyTo(tweet.TweetDTO, tweetToReplyTo.TweetDTO);
        }

        public static ITweet PublishTweetInReplyTo(string text, long tweetToReplyToId)
        {
            var tweet = TweetFactory.CreateTweet(text);
            return TweetController.PublishTweetInReplyTo(tweet.TweetDTO, tweetToReplyToId);
        }

        // Publish TweetInReplyTo From Tweet object
        public static bool PublishTweetInReplyTo(ITweet tweet, ITweet tweetToReplyTo)
        {
            return TweetController.PublishTweetInReplyTo(tweet, tweetToReplyTo);
        }

        public static bool PublishTweetInReplyTo(ITweet tweet, long tweetIdToRespondTo)
        {
            return TweetController.PublishTweetInReplyTo(tweet, tweetIdToRespondTo);
        }

        // Publish TweetInReplyTo With Geo info
        public static ITweet PublishTweetWithGeoInReplyTo(string text, ICoordinates coordinates, ITweet tweetToReplyTo)
        {
            if (tweetToReplyTo == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            var tweet = TweetFactory.CreateTweet(text);
            if (TweetController.PublishTweetWithGeoInReplyTo(tweet, coordinates, tweetToReplyTo.Id))
            {
                return tweet;
            }

            return null;
        }

        public static ITweet PublishTweetWithGeoInReplyTo(string text, ICoordinates coordinates, long tweetIdToRespondTo)
        {
            var tweet = TweetFactory.CreateTweet(text);

            if (TweetController.PublishTweetWithGeoInReplyTo(tweet, coordinates, tweetIdToRespondTo))
            {
                return tweet;
            }

            return null;
        }

        public static ITweet PublishTweetWithGeoInReplyTo(string text, double longitude, double latitude, ITweet tweetToReplyTo)
        {
            if (tweetToReplyTo == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            var tweet = TweetFactory.CreateTweet(text);
            if (PublishTweetWithGeoInReplyTo(tweet, longitude, latitude, tweetToReplyTo.Id))
            {
                return tweet;
            }

            return null;
        }

        public static ITweet PublishTweetWithGeoInReplyTo(string text, double longitude, double latitude, long tweetIdToRespondTo)
        {
            var tweet = TweetFactory.CreateTweet(text);
            if (TweetController.PublishTweetWithGeoInReplyTo(tweet, longitude, latitude, tweetIdToRespondTo))
            {
                return tweet;
            }

            return null;
        }

        public static bool PublishTweetWithGeoInReplyTo(ITweet tweet, ICoordinates coordinates, ITweet tweetToReplyTo)
        {
            if (tweetToReplyTo == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            return TweetController.PublishTweetWithGeoInReplyTo(tweet, coordinates, tweetToReplyTo.Id);
        }

        public static bool PublishTweetWithGeoInReplyTo(ITweet tweet, ICoordinates coordinates, long tweetIdToRespondTo)
        {
            return TweetController.PublishTweetWithGeoInReplyTo(tweet, coordinates, tweetIdToRespondTo);
        }

        public static bool PublishTweetWithGeoInReplyTo(ITweet tweet, double longitude, double latitude, ITweet tweetToReplyTo)
        {
            if (tweetToReplyTo == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            return PublishTweetWithGeoInReplyTo(tweet, longitude, latitude, tweetToReplyTo.Id);
        }

        public static bool PublishTweetWithGeoInReplyTo(ITweet tweet, double longitude, double latitude, long tweetIdToRespondTo)
        {
            return TweetController.PublishTweetWithGeoInReplyTo(tweet, longitude, latitude, tweetIdToRespondTo);
        }

        // Publish Retweet
        public static ITweet PublishRetweet(ITweet tweet)
        {
            return TweetController.PublishRetweet(tweet);
        }

        public static ITweet PublishRetweet(long tweetId)
        {
            return TweetController.PublishRetweet(tweetId);
        }

        // Get Retweet
        public static IEnumerable<ITweet> GetRetweets(ITweet tweet)
        {
            return TweetController.GetRetweets(tweet);
        }

        public static IEnumerable<ITweet> GetRetweets(long tweetId)
        {
            return TweetController.GetRetweets(tweetId);
        }

        // Destroy Tweet
        public static bool DestroyTweet(ITweet tweet)
        {
            return TweetController.DestroyTweet(tweet);
        }

        public static bool DestroyTweet(long tweetId)
        {
            return TweetController.DestroyTweet(tweetId);
        }

        // Favorite Tweet
        public static bool FavoriteTweet(ITweet tweet)
        {
            return TweetController.FavoriteTweet(tweet);
        }

        public static bool FavoriteTweet(long tweetId)
        {
            return TweetController.FavoriteTweet(tweetId);
        }

        // Generate OEmbedTweet
        public static IOEmbedTweet GenerateOEmbedTweet(ITweet tweet)
        {
            return TweetController.GenerateOEmbedTweet(tweet);
        }

        public static IOEmbedTweet GenerateOEmbedTweet(long tweetId)
        {
            return TweetController.GenerateOEmbedTweet(tweetId);
        }

        #endregion
    }
}