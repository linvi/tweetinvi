using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-statuses-destroy-id
    /// </summary>
    public interface IDestroyTweetParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The identifier of the tweet you want to destroy
        /// </summary>
        ITweetIdentifier Tweet { get; set; }

        /// <summary>
        /// If set to true, the creator property (IUser) will only contain the id.
        /// </summary>
        bool? TrimUser { get; set; }

        /// <summary>
        /// Decide whether to use Extended or Compat mode
        /// </summary>
        TweetMode? TweetMode { get; set; }
    }

    /// <inheritdoc/>
    public class DestroyTweetParameters : CustomRequestParameters, IDestroyTweetParameters
    {
        public DestroyTweetParameters(long tweetId) : this(new TweetIdentifier(tweetId))
        {
        }

        public DestroyTweetParameters(ITweetIdentifier tweet)
        {
            Tweet = tweet;
        }

        /// <inheritdoc/>
        public ITweetIdentifier Tweet { get; set; }
        /// <inheritdoc/>
        public bool? TrimUser { get; set; }

        public TweetMode? TweetMode { get; set; }
    }
}