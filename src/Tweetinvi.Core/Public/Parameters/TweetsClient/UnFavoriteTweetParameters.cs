using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-favorites-destroy
    /// </summary>
    /// <inheritdoc />
    public interface IUnfavoriteTweetParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The identifier of the tweet you no longer want to be a favorite
        /// </summary>
        ITweetIdentifier Tweet { get; set; }

        /// <summary>
        /// Include the tweet entities
        /// </summary>
        bool? IncludeEntities { get; set; }
    }

    /// <inheritdoc cref="IUnfavoriteTweetParameters" />
    public class UnfavoriteTweetParameters : CustomRequestParameters, IUnfavoriteTweetParameters
    {
        public UnfavoriteTweetParameters(long tweetId) : this(new TweetIdentifier(tweetId))
        {
        }

        public UnfavoriteTweetParameters(ITweetIdentifier tweet)
        {
            Tweet = tweet;
        }

        /// <inheritdoc />
        public ITweetIdentifier Tweet { get; set; }
        /// <inheritdoc />
        public bool? IncludeEntities { get; set; }
    }
}