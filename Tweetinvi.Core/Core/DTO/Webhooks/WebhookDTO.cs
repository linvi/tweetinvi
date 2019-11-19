using System;
using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi.Logic.DTO
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
        [JsonConverter(typeof(JsonTwitterDateTimeConverter), "yyyy-MM-dd HH:mm:ss zzzz")]
        public DateTime CreatedAt { get; set; }
    }
}
