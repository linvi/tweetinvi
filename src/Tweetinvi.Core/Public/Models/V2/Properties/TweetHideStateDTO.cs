using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetHideStateDTO
    {
        [JsonProperty("hidden")] public bool Hidden { get; set; }
    }
}