namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/statuses/user_timeline
    /// </summary>
    public interface IUserTimelineParameters : ITimelineRequestParameters
    {
        /// <summary>
        /// Include Retweets. When this parameter is set to false, Twitter will send you the same result set but without including the retweets.
        /// It means that if there are a total of 100 tweets, and the latest are 80 new tweets and 20 retweets. 
        /// If the MaximumResultSet is set to 100, you will receive 80 tweets and not 100 even if there is more than 80 new tweets in the Timeline.
        /// </summary>
        bool IncludeRTS { get; set; }

        /// <summary>
        /// Exclude replies.
        /// </summary>
        bool ExcludeReplies { get; set; }

        /// <summary>
        /// Include contributors details.
        /// </summary>
        bool IncludeContributorDetails { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/statuses/user_timeline
    /// </summary>
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