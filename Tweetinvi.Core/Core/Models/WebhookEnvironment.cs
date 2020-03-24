using System.Linq;
using Newtonsoft.Json;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi.Core.Models
{
    public class WebhookEnvironment : IWebhookEnvironment
    {
        private readonly ITwitterClient _client;

        public WebhookEnvironment(IWebhookEnvironmentDTO webhookEnvironmentDTO, ITwitterClient client)
        {
            _client = client;
            WebhookEnvironmentDTO = webhookEnvironmentDTO;
        }

        [JsonIgnore]
        public IWebhookEnvironmentDTO WebhookEnvironmentDTO { get; }

        public string Name => WebhookEnvironmentDTO.Name;
        public IWebhook[] Webhooks => WebhookEnvironmentDTO.Webhooks.Select(x => _client.Factories.CreateWebhook(x)).ToArray();
    }
}