using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi.Core.Models
{
    public class WebhookSubscription : IWebhookSubscription
    {
        private readonly IWebhookSubscriptionDTO _subscriptionDTO;

        public WebhookSubscription(IWebhookSubscriptionDTO subscriptionDTO)
        {
            _subscriptionDTO = subscriptionDTO;
        }

        public string UserId => _subscriptionDTO.UserId;
    }
}