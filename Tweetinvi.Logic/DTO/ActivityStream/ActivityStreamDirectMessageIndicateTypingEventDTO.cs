using System;
using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;

namespace Tweetinvi.Logic.DTO.ActivityStream
{
    public class ActivityStreamTargetRecipientDTO
    {
        [JsonProperty("recipient_id")]
        public long RecipientId { get; set; }
    }

    public class ActivityStreamDirectMessageIndicateTypingEventDTO
    {
        [JsonProperty("created_timestamp")]
        [JsonConverter(typeof(JsonPropertyConverterRepository))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("sender_id")]
        public long SenderId { get; set; }

        [JsonProperty("target")]
        public ActivityStreamTargetRecipientDTO Target { get; set; }
    }
}
