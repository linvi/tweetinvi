using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;

namespace Tweetinvi.Logic.DTO
{
    public class GetMessageDTO : IGetMessageDTO
    {
        [JsonProperty("event")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IMessageEventDTO MessageEvent { get; set; }

        [JsonProperty("apps")]
        public Dictionary<long, IApp> Apps { get; set; }
    }
}
