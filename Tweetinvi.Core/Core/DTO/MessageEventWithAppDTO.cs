using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class MessageEventWithAppDTO : IMessageEventWithAppDTO
    {
        [JsonProperty("event")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IMessageEventDTO MessageEvent { get; set; }

        [JsonProperty("app")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IApp App { get; set; }
    }
}
