using System;
using System.Collections.Generic;
using Tweetinvi.Core.Injectinvi;
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

        public static IPublishTweetParameters CreatePublishTweetParameters(string text, IPublishTweetOptionalParameters optionalParameters = null)
        {
            return new PublishTweetParameters(text, optionalParameters);
        }

        public static IPublishTweetOptionalParameters CreatePublishTweetOptionalParameters()
        {
            return new PublishTweetOptionalParameters();
        }

        #endregion

        #region Tweet Controller

        // Length

        /// <summary>
        /// Get the length of a tweet as calculated by Twitter
        /// </summary>
        public static int Length(IPublishTweetParameters publishTweetParameters)
        {
            return TweetController.Length(publishTweetParameters);
        }

        /// <summary>
        /// Get the length of a tweet as calculated by Twitter
        /// </summary>
        public static int Length(string text, IPublishTweetOptionalParameters publishTweetOptionalParameters = null)
        {
            return TweetController.Length(text, publishTweetOptionalParameters);
        }

        // Can be published

        /// <summary>
        /// Verify that a tweet can be published
        /// </summary>
        public static bool CanBePublished(IPublishTweetParameters publishTweetParameters)
        {
            return TweetController.CanBePublished(publishTweetParameters);
        }

        /// <summary>
        /// Verify that a tweet can be published
        /// </summary>
        public static bool CanBePublished(string text, IPublishTweetOptionalParameters publishTweetOptionalParameters = null)
        {
            return TweetController.CanBePublished(text, publishTweetOptionalParameters);
        }

        // Publish Tweet

        /// <summary>
        /// Publish a tweet
        /// </summary>
        public static ITweet PublishTweet(IPublishTweetParameters publishTweetParameters)
        {
            return TweetController.PublishTweet(publishTweetParameters);
        }

        /// <summary>
        /// Publish a tweet
        /// </summary>
        public static ITweet PublishTweet(string text, IPublishTweetOptionalParameters publishTweetOptionalParameters = null)
        {
            return TweetController.PublishTweet(text, publishTweetOptionalParameters);
        }

        /// <summary>
        /// Publish a tweet with an image
        /// </summary>
        public static ITweet PublishTweetWithImage(string text, byte[] image)
        {
            return TweetController.PublishTweetWithMedia(text, image);
        }

        /// <summary>
        /// Publish a tweet with a video
        /// </summary>
        public static ITweet PublishTweetWithVideo(string text, byte[] video)
        {
            return TweetController.PublishTweetWithVideo(text, video);
        }

        /// <summary>
        /// Publish a tweet in reply to another one
        /// </summary>
        public static ITweet PublishTweetInReplyTo(string text, long tweetToReplyToId)
        {
            return TweetController.PublishTweetInReplyTo(text, tweetToReplyToId);
        }

        /// <summary>
        /// Publish a tweet in reply to another one
        /// </summary>
        public static ITweet PublishTweetInReplyTo(string text, ITweetIdentifier tweetToReplyTo)
        {
            return TweetController.PublishTweetInReplyTo(text, tweetToReplyTo);
        }

        // Publish Retweet

        /// <summary>
        /// Publish a retweet tweet
        /// </summary>
        public static ITweet PublishRetweet(ITweet tweet)
        {
            return TweetController.PublishRetweet(tweet);
        }

        /// <summary>
        /// Publish a retweet tweet
        /// </summary>
        public static ITweet PublishRetweet(long tweetId)
        {
            return TweetController.PublishRetweet(tweetId);
        }

        // UnRetweet

        /// <summary>
        /// Publish a unretweet tweet
        /// </summary>
        public static bool UnRetweet(ITweetIdentifier tweet)
        {
            return TweetController.UnRetweet(tweet) != null;
        }

        /// <summary>
        /// Publish a unretweet tweet
        /// </summary>
        public static bool UnRetweet(long tweetId)
        {
            return TweetController.UnRetweet(tweetId) != null;
        }

        // Get Retweet

        /// <summary>
        /// Get the retweets of a specific tweet
        /// </summary>
        public static IEnumerable<ITweet> GetRetweets(ITweetIdentifier tweet)
        {
            return TweetController.GetRetweets(tweet);
        }

        /// <summary>
        /// Get the retweets of a specific tweet
        /// </summary>
        public static IEnumerable<ITweet> GetRetweets(long tweetId)
        {
            return TweetController.GetRetweets(tweetId);
        }

        // Get Retweeters Ids

        /// <summary>
        /// Get the retweeter Ids who tweeted a specific tweet
        /// </summary>
        public static IEnumerable<long> GetRetweetersIds(long tweetId, int maxRetweetersToRetrieve = 100)
        {
            return TweetController.GetRetweetersIds(tweetId, maxRetweetersToRetrieve);
        }

        /// <summary>
        /// Get the retweeter Ids who tweeted a specific tweet
        /// </summary>
        public static IEnumerable<long> GetRetweetersIds(ITweetIdentifier tweetIdentifier, int maxRetweetersToRetrieve = 100)
        {
            return TweetController.GetRetweetersIds(tweetIdentifier, maxRetweetersToRetrieve);
        }

        // Destroy Tweet

        /// <summary>
        /// Destroy a specific tweet
        /// </summary>
        public static bool DestroyTweet(ITweet tweet)
        {
            return TweetController.DestroyTweet(tweet);
        }

        /// <summary>
        /// Destroy a specific tweet
        /// </summary>
        public static bool DestroyTweet(long tweetId)
        {
            return TweetController.DestroyTweet(tweetId);
        }

        // Favorite Tweet

        /// <summary>
        /// Favorite a specific tweet
        /// </summary>
        public static bool FavoriteTweet(ITweet tweet)
        {
            return TweetController.FavoriteTweet(tweet);
        }

        /// <summary>
        /// Favorite a specific tweet
        /// </summary>
        public static bool FavoriteTweet(long tweetId)
        {
            return TweetController.FavoriteTweet(tweetId);
        }

        // UnFavorite Tweet

        /// <summary>
        /// UnFavorite a specific tweet
        /// </summary>
        public static bool UnFavoriteTweet(ITweet tweet)
        {
            return TweetController.UnFavoriteTweet(tweet);
        }

        /// <summary>
        /// UnFavorite a specific tweet
        /// </summary>
        public static bool UnFavoriteTweet(long tweetId)
        {
            return TweetController.UnFavoriteTweet(tweetId);
        }

        // Generate OEmbedTweet

        /// <summary>
        /// Generate an OEmbed Tweet
        /// </summary>
        public static IOEmbedTweet GenerateOEmbedTweet(ITweet tweet)
        {
            return TweetController.GenerateOEmbedTweet(tweet);
        }

        /// <summary>
        /// Generate an OEmbed Tweet
        /// </summary>
        public static IOEmbedTweet GenerateOEmbedTweet(long tweetId)
        {
            return TweetController.GenerateOEmbedTweet(tweetId);
        }

        #endregion
    }
}