using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetAnnotationDTO
    {
        [JsonProperty("end")] public int end { get; set; }

        [JsonProperty("normalized_text")] public string normalized_text { get; set; }

        [JsonProperty("probability")] public int probability { get; set; }

        [JsonProperty("start")] public int start { get; set; }

        [JsonProperty("type")] public string type { get; set; }
    }
}