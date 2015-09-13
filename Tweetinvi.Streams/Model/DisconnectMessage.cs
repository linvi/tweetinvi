using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models.StreamMessages;

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