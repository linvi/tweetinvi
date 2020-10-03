using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class HashtagV2
    {
        [JsonProperty("start")] public int Start { get; set; }
        [JsonProperty("end")] public int End { get; set; }
        [JsonProperty("tag")] public string Tag { get; set; }

        [JsonProperty("hashtag")] public string Hashtag
        {
            set => Tag = value;
        }
    }
}