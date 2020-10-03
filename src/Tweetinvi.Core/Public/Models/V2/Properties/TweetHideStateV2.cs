using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetHideStateV2
    {
        [JsonProperty("hidden")] public bool Hidden { get; set; }
    }
}