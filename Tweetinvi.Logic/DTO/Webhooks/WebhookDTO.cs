using System;
using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class WebhookDTO : IWebhookDTO
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public bool Valid { get; set; }

        [JsonProperty("created_timestamp")]
        [JsonConverter(typeof(JsonTwitterDateTimeConverter), "yyyy-MM-dd HH:mm:ss zzzz")]
        public DateTime CreatedAt { get; set; }
    }
}
