using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class MessageCreateDTO : IMessageCreateDTO
    {
        // Tweetinvi fields
        public bool IsDestroyed { get; set; }

        // Twitter fields
        [JsonProperty("target")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IMessageCreateTargetDTO Target { get; set; }

        [JsonProperty("sender_id")]
        public long SenderId { get; set; }

        [JsonProperty("source_app_id")]
        public long? SourceAppId { get; set; }

        [JsonProperty("message_data")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public IMessageDataDTO MessageData { get; set; }
    }
}
