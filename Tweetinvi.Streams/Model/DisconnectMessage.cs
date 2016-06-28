using Newtonsoft.Json;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Streams.Model
{
    public class DisconnectMessage : IDisconnectMessage
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("stream_name")]
        public string StreamName { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}