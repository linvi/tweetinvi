using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class FilteredStreamRuleV2
    {
        /// <summary>
        /// Unique identifier of this rule.
        /// </summary>
        [JsonProperty("id")] public string Id { get; set; }

        /// <summary>
        /// The filter rule as submitted when creating the rule.
        /// </summary>
        [JsonProperty("value")] public string Value { get; set; }

        /// <summary>
        /// The tag label as defined when creating the rule.
        /// </summary>
        [JsonProperty("tag")] public string Tag { get; set; }
    }
}