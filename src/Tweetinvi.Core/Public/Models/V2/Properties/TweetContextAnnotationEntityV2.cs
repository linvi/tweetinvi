using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class TweetContextAnnotationEntityV2
    {
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
    }
}