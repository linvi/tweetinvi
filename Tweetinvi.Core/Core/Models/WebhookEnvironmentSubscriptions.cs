using System.Linq;
using Newtonsoft.Json;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi.Core.Models
{
    public class WebhookEnvironmentSubscriptions : IWebhookEnvironmentSubscriptions
    {
        public WebhookEnvironmentSubscriptions(IWebhookEnvironmentSubscriptionsDTO webhookEnvironmentSubscriptionsDTO, ITwitterClient client)
        {
            WebhookEnvironmentSubscriptionsDTO = webhookEnvironmentSubscriptionsDTO;
            Client = client;
        }

        public ITwitterClient Client { get; set; }

        public IWebhookEnvironmentSubscriptionsDTO WebhookEnvironmentSubscriptionsDTO { get; }

        public string EnvironmentName => WebhookEnvironmentSubscriptionsDTO.Environment;
        public string ApplicationId => WebhookEnvironmentSubscriptionsDTO.ApplicationId;
        public IWebhookSubscription[] Subscriptions => WebhookEnvironmentSubscriptionsDTO.Subscriptions.Select(x => new WebhookSubscription(x) as IWebhookSubscription).ToArray();
    }
}