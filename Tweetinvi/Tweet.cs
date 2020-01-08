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