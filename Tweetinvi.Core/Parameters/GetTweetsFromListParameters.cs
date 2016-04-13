namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/lists/statuses
    /// </summary>
    public interface IGetTweetsFromListParameters : ICustomRequestParameters
    {
        int MaximumNumberOfTweetsToRetrieve { get; set; }

        /// <summary>
        /// Returns tweets with an ID greater than the specified value.
        /// </summary>
        long? SinceId { get; set; }

        /// <summary>
        /// Returns tweets with an ID lower than the specified value.
        /// </summary>
        long? MaxId { get; set; }

        /// <summary>
        /// Include tweet entities.
        /// </summary>
        bool IncludeEntities { get; set; }

        /// <summary>
        /// Include Retweets. When this parameter is set to false, Twitter will send you the same result set but without including the retweets.
        /// It means that if there are a total of 100 tweets, and the latest are 80 new tweets and 20 retweets. 
        /// If the MaximumResultSet is set to 100, you will receive 80 tweets and not 100 even if there is more than 80 new tweets in the Timeline.
        /// </summary>
        bool IncludeRetweets { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/lists/statuses
    /// </summary>
    public class GetTweetsFromListParameters : CustomRequestParameters, IGetTweetsFromListParameters
    {
        public GetTweetsFromListParameters()
        {
            MaximumNumberOfTweetsToRetrieve = TweetinviConsts.LIST_GET_TWEETS_COUNT;

            IncludeEntities = true;
            IncludeRetweets = true;
        }

        public int MaximumNumberOfTweetsToRetrieve { get; set; }
        public long? SinceId { get; set; }
        public long? MaxId { get; set; }
        public bool IncludeEntities { get; set; }
        public bool IncludeRetweets { get; set; }
    }
}