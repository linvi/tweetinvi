using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetAnnotationDTO
    {
        [JsonProperty("end")] public int End { get; set; }
        [JsonProperty("normalized_text")] public string NormalizedText { get; set; }
        [JsonProperty("probability")] public int Probability { get; set; }
        [JsonProperty("start")] public int Start { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
    }
}