using Newtonsoft.Json;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Streams.Model
{
    public class WarningMessageFallingBehind : WarningMessage, IWarningMessageFallingBehind
    {
        [JsonProperty("percent_full")]
        public int PercentFull { get; set; }
    }
}