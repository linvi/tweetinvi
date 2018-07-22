using Newtonsoft.Json;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;

namespace Tweetinvi.Logic.DTO
{
    public class WebhookSubscriptionDTO : IWebhookSubscriptionDTO
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    public class WebhookSubcriptionListDTO : IWebhookSubcriptionListDTO
    {
        public string Environment { get; set; }

        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        public IWebhookSubscriptionDTO[] Subscriptions { get; set; }
    }
}
