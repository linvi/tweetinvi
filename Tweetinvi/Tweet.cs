using System;
using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

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

        // Length
        public static int Length(IPublishTweetParameters publishTweetParameters)
        {
            return TweetController.Length(publishTweetParameters);
        }

        public static int Length(string text, IPublishTweetOptionalParameters publishTweetOptionalParameters = null)
        {
            return TweetController.Length(text, publishTweetOptionalParameters);
        }

        // Can be published
        public static bool CanBePublished(IPublishTweetParameters publishTweetParameters)
        {
            return TweetController.CanBePublished(publishTweetParameters);
        }

        public static bool CanBePublished(string text, IPublishTweetOptionalParameters publishTweetOptionalParameters = null)
        {
            return TweetController.CanBePublished(text, publishTweetOptionalParameters);
        }

        // Publish Tweet
        public static ITweet PublishTweet(IPublishTweetParameters publishTweetParameters)
        {
            return TweetController.PublishTweet(publishTweetParameters);
        }

        public static ITweet PublishTweet(string text, IPublishTweetOptionalParameters publishTweetOptionalParameters = null)
        {
            return TweetController.PublishTweet(text, publishTweetOptionalParameters);
        }

        public static ITweet PublishTweetWithImage(string text, byte[] image)
        {
            return TweetController.PublishTweetWithMedia(text, image);
        }

        public static ITweet PublishTweetWithVideo(string text, byte[] video)
        {
            return TweetController.PublishTweetWithVideo(text, video);
        }
       
        public static ITweet PublishTweetInReplyTo(string text, long tweetToReplyToId)
        {
            return TweetController.PublishTweetInReplyTo(text, tweetToReplyToId);
        }

        public static ITweet PublishTweetInReplyTo(string text, ITweetIdentifier tweetToReplyTo)
        {
            return TweetController.PublishTweetInReplyTo(text, tweetToReplyTo);
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