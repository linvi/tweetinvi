using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetContextAnnotationEntityDTO
    {
        [JsonProperty("description")] public string description { get; set; }

        [JsonProperty("id")] public string id { get; set; }

        [JsonProperty("name")] public string name { get; set; }
    }
}