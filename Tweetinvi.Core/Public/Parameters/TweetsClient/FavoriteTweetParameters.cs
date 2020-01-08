using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/post-favorites-create
    /// </summary>
    public interface IFavoriteTweetParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The identifier of the tweet you want to favorite
        /// </summary>
        ITweetIdentifier Tweet { get; set; }

        /// <summary>
        /// Include the tweet entities
        /// </summary>
        bool? IncludeEntities { get; set; }
    }

    /// <inheritdoc />
    public class FavoriteTweetParameters : CustomRequestParameters, IFavoriteTweetParameters
    {
        public FavoriteTweetParameters(long? tweetId) : this(new TweetIdentifier(tweetId))
        {
        }

        public FavoriteTweetParameters(ITweetIdentifier tweet)
        {
            Tweet = tweet;
        }

        /// <inheritdoc />
        public ITweetIdentifier Tweet { get; set; }
        /// <inheritdoc />
        public bool? IncludeEntities { get; set; }
    }
}