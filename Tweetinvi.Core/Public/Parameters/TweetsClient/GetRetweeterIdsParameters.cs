using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweeters-ids
    /// </summary>
    public interface IGetRetweeterIdsParameters : ICursorQueryParameters
    {
        /// <summary>
        /// The identifier of the retweet
        /// </summary>
        ITweetIdentifier Tweet { get; set; }
    }

    /// <inheritdoc/>
    public class GetRetweeterIdsParameters : CursorQueryParameters, IGetRetweeterIdsParameters
    {
        public GetRetweeterIdsParameters()
        {
            PageSize = TwitterLimits.DEFAULTS.TWEETS_GET_RETWEETER_IDS_MAX_PAGE_SIZE;
        }

        public GetRetweeterIdsParameters(long? tweetId) : this()
        {
            Tweet = new TweetIdentifier(tweetId);
        }

        public GetRetweeterIdsParameters(ITweetIdentifier tweet) : this()
        {
            Tweet = tweet;
        }

        public GetRetweeterIdsParameters(IGetRetweeterIdsParameters source) : base(source)
        {
            if (source == null)
            {
                PageSize = TwitterLimits.DEFAULTS.TWEETS_GET_RETWEETER_IDS_MAX_PAGE_SIZE;
                return;
            }

            Tweet = source.Tweet;
        }

        /// <inheritdoc/>
        public ITweetIdentifier Tweet { get; set; }
    }
}