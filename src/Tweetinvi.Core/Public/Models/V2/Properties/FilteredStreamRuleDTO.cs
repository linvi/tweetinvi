using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class FilteredStreamRuleDTO
    {
        [JsonProperty("id")] public string id { get; set; }
        [JsonProperty("value")] public string value { get; set; }
        [JsonProperty("tag")] public string tag { get; set; }
    }
}