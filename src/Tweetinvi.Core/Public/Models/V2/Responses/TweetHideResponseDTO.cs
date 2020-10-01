using Newtonsoft.Json;

namespace Tweetinvi.Models.V2.Responses
{
    public class TweetHideResponseDTO
    {
        [JsonProperty("data")] public TweetHideStateDTO TweetHideState { get; set; }
    }
}