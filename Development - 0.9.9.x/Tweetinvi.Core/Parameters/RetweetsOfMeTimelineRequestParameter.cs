using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Parameters
{
    public class RetweetsOfMeTimelineRequestParameter : TimelineRequestParameters, IRetweetsOfMeTimelineRequestParameters
    {
        public RetweetsOfMeTimelineRequestParameter()
        {
            MaximumNumberOfTweetsToRetrieve = TweetinviConsts.TIMELINE_RETWEETS_OF_ME_COUNT;
            
            SinceId = TweetinviSettings.DEFAULT_ID;
            MaxId = TweetinviSettings.DEFAULT_ID;
            TrimUser = false;
            IncludeEntities = true;
            IncludeUserEntities = true;
        }

        public bool IncludeUserEntities { get; set; }
    }
}