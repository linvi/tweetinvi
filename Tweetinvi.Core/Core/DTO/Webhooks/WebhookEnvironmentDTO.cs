using Newtonsoft.Json;
using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi.Core.DTO.Webhooks
{
    public class WebhookEnvironmentDTO : IWebhookEnvironmentDTO
    {
        [JsonProperty("environment_name")]
        public string Name { get; set; }

        public IWebhookDTO[] Webhooks { get; set; }
    }
}
