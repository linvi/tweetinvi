namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/tweets/timelines/api-reference/get-statuses-home_timeline
    /// </summary>
    /// <inheritdoc />
    public interface IGetHomeTimelineParameters : ITimelineRequestParameters
    {
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
            ExcludeReplies = source.ExcludeReplies;
        }

        public bool ExcludeReplies { get; set; }
    }
}