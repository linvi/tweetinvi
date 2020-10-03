using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetHideV2Response
    {
        /// <summary>
        /// Tweet hidden state in a conversation
        /// </summary>
        [JsonProperty("data")] public TweetHideStateV2 TweetHideState { get; set; }
    }
}