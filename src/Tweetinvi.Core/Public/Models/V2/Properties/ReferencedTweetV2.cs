using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class ReferencedTweetV2
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
    }
}