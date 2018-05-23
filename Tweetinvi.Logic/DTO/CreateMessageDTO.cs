using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class CreateMessageDTO : ICreateMessageDTO
    {
        [JsonProperty("event")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IEventDTO Event { get; set; }
    }
}
