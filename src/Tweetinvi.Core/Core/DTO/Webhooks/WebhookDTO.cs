using System;
using Newtonsoft.Json;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi.Core.DTO.Webhooks
{
    public class WebhookDTO : IWebhookDTO
    {
        private Uri _uri;

        public string Id { get; set; }
        public string Url { get; set; }

        [JsonIgnore]
        public Uri Uri
        {
            get
            {
                if (_uri == null)
                {
                    _uri = new Uri(Url);
                }

                return _uri;
            }
        }

        public bool Valid { get; set; }

        [JsonProperty("created_timestamp")]
        [JsonConverter(typeof(JsonTwitterDateTimeOffsetConverter), "yyyy-MM-dd HH:mm:ss zzzz")]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
