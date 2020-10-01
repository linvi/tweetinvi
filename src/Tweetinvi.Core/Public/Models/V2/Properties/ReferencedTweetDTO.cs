using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class ReferencedTweetDTO
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
    }
}