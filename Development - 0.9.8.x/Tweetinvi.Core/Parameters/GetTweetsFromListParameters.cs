using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Parameters
{
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