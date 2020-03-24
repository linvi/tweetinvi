using Newtonsoft.Json;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Streams.Model
{
    public class WarningMessageTooManyFollowers : WarningMessage, IWarningMessageTooManyFollowers
    {
        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("timestamp_ms")]
        public string TimestampInMs { get; set; }
    }
}