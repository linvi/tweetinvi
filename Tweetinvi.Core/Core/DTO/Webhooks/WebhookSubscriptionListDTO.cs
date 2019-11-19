using Newtonsoft.Json;
using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi.Logic.DTO
{
    public class WebhookSubscriptionDTO : IWebhookSubscriptionDTO
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    public class WebhookSubscriptionListDTO : IWebhookSubscriptionListDTO
    {
        public string Environment { get; set; }

        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        public IWebhookSubscriptionDTO[] Subscriptions { get; set; }
    }
}
