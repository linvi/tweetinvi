using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class FilteredStreamMatchingRuleV2
    {
        /// <summary>
        /// Id of the rule that resulted in the tweet to be returned by the filtered stream
        /// </summary>
        [JsonProperty("id")] public string Id { get; set; }

        /// <summary>
        /// Tag of the rule that resulted in the tweet to be returned by the filtered stream
        /// </summary>
        [JsonProperty("tag")] public string Tag { get; set; }
    }
}