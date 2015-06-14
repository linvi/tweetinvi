using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Parameters
{
    public class MentionsTimelineParameters : TimelineRequestParameters, IMentionsTimelineParameters
    {
        public MentionsTimelineParameters()
        {
            MaximumNumberOfTweetsToRetrieve = TweetinviConsts.TIMELINE_MENTIONS_COUNT;
        }

        public bool IncludeContributorDetails { get; set; }
    }
}