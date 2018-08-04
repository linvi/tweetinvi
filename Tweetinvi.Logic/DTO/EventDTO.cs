using System;
using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class EventDTO : IEventDTO
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public EventType Type { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("created_timestamp")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("initiated_via")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IEventInitiatedViaDTO InitiatedVia { get; set; }

        [JsonProperty("message_create")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IMessageCreateDTO MessageCreate { get; set; }
    }
}
