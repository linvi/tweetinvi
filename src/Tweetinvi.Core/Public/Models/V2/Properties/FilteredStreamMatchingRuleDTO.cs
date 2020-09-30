using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class FilteredStreamMatchingRuleDTO
    {
        [JsonProperty("id")] public string id { get; set; }
        [JsonProperty("tag")] public string tag { get; set; }
    }
}