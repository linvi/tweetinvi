using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class TweetHideStateV2
    {
        [JsonProperty("hidden")] public bool Hidden { get; set; }
    }
}