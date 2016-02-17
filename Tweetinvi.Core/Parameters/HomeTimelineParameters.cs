namespace Tweetinvi.Core.Parameters
{
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