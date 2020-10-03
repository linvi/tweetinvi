using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class FilteredStreamMatchingRuleV2
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("tag")] public string Tag { get; set; }
    }
}