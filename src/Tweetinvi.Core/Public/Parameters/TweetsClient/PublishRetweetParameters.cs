using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-retweet-id
    /// </summary>
    public interface IPublishRetweetParameters : ICustomRequestParameters, ITweetModeParameter
    {
        /// <summary>
        /// The tweet identifier you want to retweet
        /// </summary>
        ITweetIdentifier Tweet { get; set; }

        /// <summary>
        /// Tweets author object will not be populated when set to true
        /// </summary>
        bool? TrimUser { get; set; }
    }

    /// <inheritdoc/>
    public class PublishRetweetParameters : CustomRequestParameters, IPublishRetweetParameters
    {
        public PublishRetweetParameters(long tweetId)
        {
            Tweet = new TweetIdentifier(tweetId);
        }

        public PublishRetweetParameters(ITweetIdentifier tweet)
        {
            Tweet = tweet;
        }

        /// <inheritdoc/>
        public ITweetIdentifier Tweet { get; set; }
        /// <inheritdoc/>
        public bool? TrimUser { get; set; }
        /// <inheritdoc/>
        public TweetMode? TweetMode { get; set; }
    }
}