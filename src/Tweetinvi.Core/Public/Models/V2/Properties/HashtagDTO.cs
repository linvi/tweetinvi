using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class HashtagDTO
    {
        [JsonProperty("start")] public int start { get; set; }

        [JsonProperty("end")] public int end { get; set; }

        [JsonProperty("hashtag")] public string hashtag
        {
            set => tag = value;
        }

        [JsonProperty("tag")] public string tag { get; set; }
    }
}