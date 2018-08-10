using Newtonsoft.Json;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class WebhookEnvironmentDTO : IWebhookEnvironmentDTO
    {
        [JsonProperty("environment_name")]
        public string Name { get; set; }

        public IWebhookDTO[] Webhooks { get; set; }

        [JsonIgnore]
        public IConsumerCredentials ConsumerCredentials { get; set; }
    }
}
