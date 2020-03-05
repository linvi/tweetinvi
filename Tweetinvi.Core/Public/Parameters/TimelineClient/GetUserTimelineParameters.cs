using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/tweets/timelines/api-reference/get-statuses-user_timeline
    /// </summary>
    /// <inheritdoc />
    public interface IGetUserTimelineParameters : ITimelineRequestParameters
    {
        /// <summary>
        /// User from who you want to get the timeline
        /// </summary>
        IUserIdentifier User { get; set; }

        /// <summary>
        /// Include Retweets. When this parameter is set to false, Twitter will send you the same result set but without including the retweets.
        /// It means that if there are a total of 100 tweets, and the latest are 80 new tweets and 20 retweets.
        /// If the MaximumResultSet is set to 100, you will receive 80 tweets and not 100 even if there is more than 80 new tweets in the Timeline.
        /// </summary>
        bool IncludeRetweets { get; set; }

        /// <summary>
        /// Exclude reply tweets from the result set.
        /// </summary>
        bool ExcludeReplies { get; set; }
    }

    /// <inheritdoc />
    public class GetUserTimelineParameters : TimelineRequestParameters, IGetUserTimelineParameters
    {
        public GetUserTimelineParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public GetUserTimelineParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public GetUserTimelineParameters(IUserIdentifier userIdentifier)
        {
            PageSize = TwitterLimits.DEFAULTS.TIMELINE_USER_PAGE_MAX_PAGE_SIZE;
            User = userIdentifier;
        }

        public GetUserTimelineParameters(IGetUserTimelineParameters source): base(source)
        {
            if (source == null)
            {
                PageSize = TwitterLimits.DEFAULTS.TIMELINE_USER_PAGE_MAX_PAGE_SIZE;
                return;
            }

            PageSize = source.PageSize;
            User = source.User;
            IncludeRetweets = source.IncludeRetweets;
            IncludeContributorDetails = source.ExcludeReplies;
            ExcludeReplies = source.ExcludeReplies;
        }

        public IUserIdentifier User { get; set; }
        public bool IncludeRetweets { get; set; }
        public bool IncludeContributorDetails { get; set; }
        public bool ExcludeReplies { get; set; }
    }
}