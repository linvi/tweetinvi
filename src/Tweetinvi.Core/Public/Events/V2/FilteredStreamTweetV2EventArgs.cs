using Tweetinvi.Models;
using Tweetinvi.Models.V2;

namespace Tweetinvi.Events.V2
{
    public class FilteredStreamTweetV2EventArgs : TweetV2EventArgs
    {
        public FilteredStreamTweetV2EventArgs(FilteredStreamTweetV2Response response, string json) : base(response, json)
        {
            MatchingRules = response.MatchingRules;
        }

        public FilteredStreamTweetV2EventArgs(TweetV2 tweet, TweetIncludesV2 includes, FilteredStreamMatchingRuleV2[] matchingRules, string json) : base(tweet, includes, json)
        {
            MatchingRules = matchingRules;
        }

        public FilteredStreamMatchingRuleV2[] MatchingRules { get; }
    }
}