using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Parameters
{
    public class UserTimelineParameters : TimelineRequestParameters, IUserTimelineParameters
    {
        public UserTimelineParameters()
        {
            MaximumNumberOfTweetsToRetrieve = TweetinviConsts.TIMELINE_USER_COUNT;

            IncludeRTS = true;
            IncludeContributorDetails = false;
        }

        public bool IncludeRTS { get; set; }
        public bool ExcludeReplies { get; set; }
        public bool IncludeContributorDetails { get; set; }
    }
}