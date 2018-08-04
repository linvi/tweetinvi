using Newtonsoft.Json;

namespace Tweetinvi.Streams.Model
{
    public class AccountActivityTweetDeletedEventStatusDTO
    {
        [JsonProperty("id")]
        public long TweetId { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }
    }

    public class AccountActivityTweetDeletedEventDTO
    {
        [JsonProperty("status")]
        public AccountActivityTweetDeletedEventStatusDTO Status { get; set; }

        [JsonProperty("timestamp_ms")]
        public long Timestamp { get; set; }
    }
}
