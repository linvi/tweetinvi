namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/statuses/retweets_of_me
    /// </summary>
    public interface IRetweetsOfMeTimelineParameters : ITimelineRequestParameters
    {
        /// <summary>
        /// Include user entities.
        /// </summary>
        bool IncludeUserEntities { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/statuses/retweets_of_me
    /// </summary>
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