using Newtonsoft.Json;
using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi.Models
{
    public interface IWebhookEnvironment
    {
        [JsonIgnore]
        IWebhookEnvironmentDTO WebhookEnvironmentDTO { get; }

        /// <summary>
        /// Name of the environment
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Webhooks registered in the environment
        /// </summary>
        IWebhook[] Webhooks { get; }
    }
}