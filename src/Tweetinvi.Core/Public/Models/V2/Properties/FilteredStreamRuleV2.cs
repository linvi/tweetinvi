using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class FilteredStreamRuleV2
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("value")] public string Value { get; set; }
        [JsonProperty("tag")] public string Tag { get; set; }
    }
}