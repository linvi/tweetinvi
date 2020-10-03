using Newtonsoft.Json;

namespace Tweetinvi.Models.Responses
{
    public class FilteredStreamTweetV2Response : TweetV2Response
    {
        /// <summary>
        /// Rules that resulted in a tweet to be matched
        /// </summary>
        [JsonProperty("matching_rules")] public FilteredStreamMatchingRuleV2[] MatchingRules { get; set; }
    }
}