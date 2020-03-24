using Newtonsoft.Json;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Models.Properties
{
    public class QuickReplyResponse : IQuickReplyResponse
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public QuickReplyType Type { get; set; }

        [JsonProperty("metadata")]
        public string Metadata { get; set; }
    }
}
