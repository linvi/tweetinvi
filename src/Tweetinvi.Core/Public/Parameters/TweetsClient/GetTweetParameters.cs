using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-show-id
    /// </summary>
    public interface IGetTweetParameters : ICustomRequestParameters, ITweetModeParameter
    {
        /// <summary>
        /// The identifier of the tweet you want to retrieve
        /// </summary>
        ITweetIdentifier Tweet { get; set; }

        /// <summary>
        /// Tweet's author object will not be populated when set to true
        /// </summary>
        bool? TrimUser { get; set; }

        /// <summary>
        /// Tweet's `current_user_retweet` field will be populated when set to true
        /// </summary>
        bool? IncludeMyRetweet { get; set; }

        /// <summary>
        /// Tweet's entities will not be included if set to false
        /// </summary>
        bool? IncludeEntities { get; set; }

        /// <summary>
        /// Tweet's alt text attached to media will be included when set to true
        /// </summary>
        bool? IncludeExtAltText { get; set; }

        /// <summary>
        /// Tweet's card uri will be included when set to true
        /// </summary>
        bool? IncludeCardUri { get; set; }
    }

    /// <inheritdoc/>
    public class GetTweetParameters : CustomRequestParameters, IGetTweetParameters
    {
        public GetTweetParameters(long tweetId) : this(new TweetIdentifier(tweetId))
        {
        }

        public GetTweetParameters(ITweetIdentifier tweet)
        {
            Tweet = tweet;
        }

        /// <inheritdoc/>
        public ITweetIdentifier Tweet { get; set; }

        /// <inheritdoc/>
        public bool? TrimUser { get; set; }
        /// <inheritdoc/>
        public bool? IncludeMyRetweet { get; set; }
        /// <inheritdoc/>
        public bool? IncludeEntities { get; set; }
        /// <inheritdoc/>
        public bool? IncludeExtAltText { get; set; }
        /// <inheritdoc/>
        public bool? IncludeCardUri { get; set; }
        /// <inheritdoc/>
        public TweetMode? TweetMode { get; set; }
    }
}