using Tweetinvi.Models.V2;
using Tweetinvi.Models.V2.Responses;

namespace Tweetinvi.Events.V2
{
    public class FilteredStreamTweetV2EventArgs : TweetV2EventArgs
    {
        public FilteredStreamTweetV2EventArgs(FilteredStreamTweetResponseDTO response, string json) : base(response, json)
        {
            MatchingRules = response.MatchingRules;
        }

        public FilteredStreamTweetV2EventArgs(TweetDTO tweet, TweetIncludesDTO includes, FilteredStreamMatchingRuleDTO[] matchingRules, string json) : base(tweet, includes, json)
        {
            MatchingRules = matchingRules;
        }

        public FilteredStreamMatchingRuleDTO[] MatchingRules { get; }
    }
}