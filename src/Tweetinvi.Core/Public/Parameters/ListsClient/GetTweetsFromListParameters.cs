using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-statuses
    /// </summary>
    /// <inheritdoc />
    public interface IGetTweetsFromListParameters : IListParameters, ITimelineRequestParameters
    {
        /// <summary>
        /// Include Retweets. When this parameter is set to false, Twitter will send you the same result set but without including the retweets.
        /// It means that if there are a total of 100 tweets, and the latest are 80 new tweets and 20 retweets.
        /// If the MaximumResultSet is set to 100, you will receive 80 tweets and not 100 even if there is more than 80 new tweets in the Timeline.
        /// </summary>
        bool? IncludeRetweets { get; set; }
    }

    /// <inheritdoc />
    public class GetTweetsFromListParameters : TimelineRequestParameters, IGetTweetsFromListParameters
    {
        public GetTweetsFromListParameters(long listId) : this(new TwitterListIdentifier(listId))
        {
        }

        public GetTweetsFromListParameters(ITwitterListIdentifier list)
        {
            List = list;
            PageSize = TwitterLimits.DEFAULTS.LISTS_GET_TWEETS_MAX_PAGE_SIZE;
        }

        public GetTweetsFromListParameters(IGetTweetsFromListParameters source) : base(source)
        {
            if (source == null)
            {
                PageSize = TwitterLimits.DEFAULTS.LISTS_GET_TWEETS_MAX_PAGE_SIZE;
                return;
            }

            List = source.List;
            IncludeRetweets = source.IncludeRetweets;
        }

        /// <inheritdoc />
        public ITwitterListIdentifier List { get; set; }


        /// <inheritdoc />
        public bool? IncludeRetweets { get; set; }
    }
}