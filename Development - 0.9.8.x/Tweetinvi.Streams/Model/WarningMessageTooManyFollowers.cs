using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models.StreamMessages;

namespace Tweetinvi.Streams.Model
{
    public class WarningMessageTooManyFollowers : WarningMessage, IWarningMessageTooManyFollowers
    {
        [JsonProperty("user_id")]
        public IEnumerable<long> UserIds { get; set; }
    }
}