using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Parameters
{
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