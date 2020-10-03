using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class TweetAnnotationV2
    {
        [JsonProperty("end")] public int End { get; set; }
        [JsonProperty("normalized_text")] public string NormalizedText { get; set; }
        [JsonProperty("probability")] public int Probability { get; set; }
        [JsonProperty("start")] public int Start { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
    }
}