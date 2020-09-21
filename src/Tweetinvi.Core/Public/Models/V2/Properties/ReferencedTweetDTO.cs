using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class ReferencedTweetDTO
    {
        [JsonProperty("id")] public string id { get; set; }

        [JsonProperty("type")] public string type { get; set; }
    }
}