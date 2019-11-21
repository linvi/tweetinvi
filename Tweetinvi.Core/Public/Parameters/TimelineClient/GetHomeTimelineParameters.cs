namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/tweets/timelines/api-reference/get-statuses-home_timeline
    /// </summary>
    /// <inheritdoc />
    public interface IGetHomeTimelineParameters : ITimelineRequestParameters
    {
        /// <summary>
        /// Add details to the contributors who participated to the tweets.
        /// </summary>
        bool IncludeContributorDetails { get; set; }

        /// <summary>
        /// Exclude reply tweets from the result set.
        /// </summary>
        bool ExcludeReplies { get; set; }
    }

    /// <inheritdoc />
    public class GetHomeTimelineParameters : TimelineRequestParameters, IGetHomeTimelineParameters
    {
        public GetHomeTimelineParameters()
        {
            PageSize = TwitterLimits.DEFAULTS.TIMELINE_HOME_PAGE_MAX_PAGE_SIZE;
        }

        public GetHomeTimelineParameters(IGetHomeTimelineParameters source): base(source)
        {
            if (source == null)
            {
                PageSize = TwitterLimits.DEFAULTS.TIMELINE_HOME_PAGE_MAX_PAGE_SIZE;
                return;
            }

            PageSize = source.PageSize;
            IncludeContributorDetails = source.ExcludeReplies;
            ExcludeReplies = source.ExcludeReplies;
        }

        public bool IncludeContributorDetails { get; set; }
        public bool ExcludeReplies { get; set; }
    }
}