using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class TweetWithheldInfoV2 : WithheldInfoV2
    {
        [JsonProperty("copyright")] public bool Copyright { get; set; }
    }
}