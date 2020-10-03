using Newtonsoft.Json;

namespace Tweetinvi.Models
{
    public class TweetAttachmentsV2
    {
        [JsonProperty("media_keys")] public string[] MediaKeys { get; set; }
        [JsonProperty("poll_ids")] public string[] PollIds { get; set; }
    }
}