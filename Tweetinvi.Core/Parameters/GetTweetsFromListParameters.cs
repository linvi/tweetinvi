namespace Tweetinvi.Core.Parameters
{
    public interface IGetTweetsFromListParameters : ICustomRequestParameters
    {
        int MaximumNumberOfTweetsToRetrieve { get; set; }

        long? SinceId { get; set; }
        long? MaxId { get; set; }
        bool IncludeEntities { get; set; }
        bool IncludeRetweets { get; set; }
    }

    public class GetTweetsFromListParameters : CustomRequestParameters, IGetTweetsFromListParameters
    {
        public GetTweetsFromListParameters()
        {
            MaximumNumberOfTweetsToRetrieve = TweetinviConsts.TWITTER_LIST_GET_TWEETS_COUNT;

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