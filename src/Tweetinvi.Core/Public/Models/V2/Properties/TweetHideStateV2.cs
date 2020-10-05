using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetHideStateV2
    {
        /// <summary>
        /// Visibility state of a reply tweets within the conversation in which it was published
        /// </summary>
        [JsonProperty("hidden")] public bool Hidden { get; set; }
    }
}