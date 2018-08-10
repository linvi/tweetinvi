using Newtonsoft.Json;

namespace Tweetinvi.Streams.Model.AccountActivity
{

    public class AccountActivityTweetDeletedEventDTO
    {
        [JsonProperty("status")]
        public AccountActivityTweetDeletedEventStatusDTO Status { get; set; }

        [JsonProperty("timestamp_ms")]
        public long Timestamp { get; set; }
    }
}
