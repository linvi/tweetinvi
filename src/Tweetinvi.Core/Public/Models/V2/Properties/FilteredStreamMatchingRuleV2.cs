using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class FilteredStreamMatchingRuleV2
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("tag")] public string Tag { get; set; }
    }
}