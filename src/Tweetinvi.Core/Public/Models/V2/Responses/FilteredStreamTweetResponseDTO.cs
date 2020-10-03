using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class FilteredStreamTweetResponseDTO : TweetResponseDTO
    {
        /// <summary>
        /// Rules that resulted in a tweet to be matched
        /// </summary>
        [JsonProperty("matching_rules")] public FilteredStreamMatchingRuleDTO[] MatchingRules { get; set; }
    }
}