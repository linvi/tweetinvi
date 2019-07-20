using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    /// <summary>
    /// Publish, access, delete... Everything you need to do on tweets is here.
    /// </summary>
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

        public static ITweet GenerateTweetFromDTO(ITweetDTO tweetDTO)
        {
            return TweetFactory.GenerateTweetFromDTO(tweetDTO, null, null);
        }

        public static IEnumerable<ITweet> GenerateTweetsFromDTO(IEnumerable<ITweetDTO> tweetsDTO)
        {
            return TweetFactory.GenerateTweetsFromDTO(tweetsDTO, null, null);
        }

        #endregion

        #region Tweet Controller

        // Can be published

        /// <summary>
        /// Verify that a tweet can be published
        /// </summary>
        [Obsolete("This method currently only returns true as a refactoring of length calculation is required to get the correct results out of this.")]
        public static bool CanBePublished(IPublishTweetParameters publishTweetParameters)
        {
            return TweetController.CanBePublished(publishTweetParameters);
        }

        /// <summary>
        /// Verify that a tweet can be published
        /// </summary>
        [Obsolete("This method currently only returns true as a refactoring of length calculation is required to get the correct results out of this.")]
        public static bool CanBePublished(string text)
        {
            IPublishTweetParameters parameters = new PublishTweetParameters(text);
            return TweetController.CanBePublished(parameters);
        }

        // UnRetweet

        /// <summary>
        /// Publish a unretweet tweet
        /// </summary>
        public static async Task<bool> UnRetweet(ITweetIdentifier tweet)
        {
            var retweet = await TweetController.UnRetweet(tweet);
            return retweet != null;
        }

        /// <summary>
        /// Publish a unretweet tweet
        /// </summary>
        public static async Task<bool> UnRetweet(long tweetId)
        {
            var retweet = await TweetController.UnRetweet(tweetId);
            return retweet != null;
        }

        // Get Retweet

        /// <summary>
        /// Get the retweets of a specific tweet
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetRetweets(ITweetIdentifier tweet)
        {
            return TweetController.GetRetweets(tweet);
        }

        /// <summary>
        /// Get the retweets of a specific tweet
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetRetweets(long tweetId)
        {
            return TweetController.GetRetweets(tweetId);
        }

        // Get Retweeters Ids

        /// <summary>
        /// Get the retweeter Ids who tweeted a specific tweet
        /// </summary>
        public static Task<IEnumerable<long>> GetRetweetersIds(long tweetId, int maxRetweetersToRetrieve = 100)
        {
            return TweetController.GetRetweetersIds(tweetId, maxRetweetersToRetrieve);
        }

        /// <summary>
        /// Get the retweeter Ids who tweeted a specific tweet
        /// </summary>
        public static Task<IEnumerable<long>> GetRetweetersIds(ITweetIdentifier tweetIdentifier, int maxRetweetersToRetrieve = 100)
        {
            return TweetController.GetRetweetersIds(tweetIdentifier, maxRetweetersToRetrieve);
        }

        // Favorite Tweet

        /// <summary>
        /// Favorite a specific tweet
        /// </summary>
        public static Task<bool> FavoriteTweet(ITweet tweet)
        {
            return TweetController.FavoriteTweet(tweet);
        }

        /// <summary>
        /// Favorite a specific tweet
        /// </summary>
        public static Task<bool> FavoriteTweet(long tweetId)
        {
            return TweetController.FavoriteTweet(tweetId);
        }

        // UnFavorite Tweet

        /// <summary>
        /// UnFavorite a specific tweet
        /// </summary>
        public static Task<bool> UnFavoriteTweet(ITweet tweet)
        {
            return TweetController.UnFavoriteTweet(tweet);
        }

        /// <summary>
        /// UnFavorite a specific tweet
        /// </summary>
        public static Task<bool> UnFavoriteTweet(long tweetId)
        {
            return TweetController.UnFavoriteTweet(tweetId);
        }

        // Generate OEmbedTweet

        /// <summary>
        /// Generate an OEmbed Tweet
        /// </summary>
        public static Task<IOEmbedTweet> GetOEmbedTweet(ITweet tweet)
        {
            return TweetController.GenerateOEmbedTweet(tweet);
        }

        /// <summary>
        /// Generate an OEmbed Tweet
        /// </summary>
        public static Task<IOEmbedTweet> GetOEmbedTweet(long tweetId)
        {
            return TweetController.GenerateOEmbedTweet(tweetId);
        }

        #endregion
    }
}