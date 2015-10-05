namespace Tweetinvi.Core.Parameters
{
    public interface IHomeTimelineParameters : ITimelineRequestParameters
    {
        bool IncludeContributorDetails { get; set; }
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