using Newtonsoft.Json;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.Model
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
