namespace Tweetinvi.Core.Parameters
{
    public interface IRetweetsOfMeTimelineParameters : ITimelineRequestParameters
    {
        bool IncludeUserEntities { get; set; }
    }

    public class RetweetsOfMeTimelineParameter : TimelineRequestParameters, IRetweetsOfMeTimelineParameters
    {
        public RetweetsOfMeTimelineParameter()
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