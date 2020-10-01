using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetHiddenStateDTO
    {
        [JsonProperty("hidden")] public bool hidden { get; set; }
    }
}