namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweets_of_me
    /// </summary>
    public interface IGetRetweetsOfMeTimelineParameters : ITimelineRequestParameters
    {
        /// <summary>
        /// Include user entities.
        /// </summary>
        bool? IncludeUserEntities { get; set; }
    }

    /// <inheritdoc/>
    public class GetRetweetsOfMeTimelineParameters : TimelineRequestParameters, IGetRetweetsOfMeTimelineParameters
    {
        public GetRetweetsOfMeTimelineParameters()
        {
            PageSize = TwitterLimits.DEFAULTS.TIMELINE_RETWEETS_OF_ME_MAX_PAGE_SIZE;
        }

        public GetRetweetsOfMeTimelineParameters(IGetRetweetsOfMeTimelineParameters source) : base(source)
        {
            IncludeUserEntities = source?.IncludeUserEntities;
        }

        /// <inheritdoc/>
        public bool? IncludeUserEntities { get; set; }
    }
}