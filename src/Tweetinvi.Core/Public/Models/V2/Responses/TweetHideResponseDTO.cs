using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class TweetHideResponseDTO
    {
        /// <summary>
        /// Tweet hidden state in a conversation
        /// </summary>
        [JsonProperty("data")] public TweetHideStateDTO TweetHideState { get; set; }
    }
}