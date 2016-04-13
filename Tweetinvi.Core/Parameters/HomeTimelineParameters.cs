namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/statuses/home_timeline
    /// </summary>
    public interface IHomeTimelineParameters : ITimelineRequestParameters
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

    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/statuses/home_timeline
    /// </summary>
    public class HomeTimelineParameters : TimelineRequestParameters, IHomeTimelineParameters
    {
        public HomeTimelineParameters()
        {
            MaximumNumberOfTweetsToRetrieve = TweetinviConsts.TIMELINE_HOME_COUNT;
          
            IncludeContributorDetails = false;
            ExcludeReplies = false;
        }

        public bool IncludeContributorDetails { get; set; }
        public bool ExcludeReplies { get; set; }
    }
}