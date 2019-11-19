using Newtonsoft.Json;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;

namespace Tweetinvi.Logic.DTO
{
    public class CreateMessageDTO : ICreateMessageDTO
    {
        [JsonProperty("event")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IMessageEventDTO MessageEvent { get; set; }
    }
}
