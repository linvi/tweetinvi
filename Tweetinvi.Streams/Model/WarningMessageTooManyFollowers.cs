using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Streams.Model
{
    public class WarningMessageTooManyFollowers : WarningMessage, IWarningMessageTooManyFollowers
    {
        [JsonProperty("user_id")]
        public IEnumerable<long> UserIds { get; set; }
    }
}