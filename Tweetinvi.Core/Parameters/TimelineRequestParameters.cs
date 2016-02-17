namespace Tweetinvi.Core.Parameters
{
    public interface ITimelineRequestParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Maximum number of tweets to get from the timeline.
        /// </summary>
        int MaximumNumberOfTweetsToRetrieve { get; set; }

        /// <summary>
        /// Returns tweets with an ID greater than the specified value.
        /// </summary>
        long SinceId { get; set; }

        /// <summary>
        /// Returns tweets with an ID lower than the specified value.
        /// </summary>
        long MaxId { get; set; }

        /// <summary>
        /// If set to true, the creator property (IUser) will only contain the id.
        /// </summary>
        bool TrimUser { get; set; }

        /// <summary>
        /// Include tweet entities.
        /// </summary>
        bool IncludeEntities { get; set; }
    }

    public class TimelineRequestParameters : CustomRequestParameters, ITimelineRequestParameters
    {
        protected TimelineRequestParameters()
        {
            MaximumNumberOfTweetsToRetrieve = 40;

            SinceId = TweetinviSettings.DEFAULT_ID;
            MaxId = TweetinviSettings.DEFAULT_ID;

            TrimUser = false;
            IncludeEntities = true;
        }

        public int MaximumNumberOfTweetsToRetrieve { get; set; }
        public long SinceId { get; set; }
        public long MaxId { get; set; }
        public bool TrimUser { get; set; }
        public bool IncludeEntities { get; set; }
    }
}