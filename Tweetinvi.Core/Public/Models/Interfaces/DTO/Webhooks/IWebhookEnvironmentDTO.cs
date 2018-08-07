using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks
{
    public interface IWebhookEnvironmentDTO
    {
        string Name { get; set; }
        IWebhookDTO[] Webhooks { get; set; }
        IConsumerCredentials ConsumerCredentials { get; set; }
    }
}
