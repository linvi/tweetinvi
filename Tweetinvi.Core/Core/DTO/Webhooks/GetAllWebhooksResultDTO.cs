using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi.Core.DTO.Webhooks
{
    public class GetAllWebhooksResultDTO : IGetAllWebhooksResultDTO
    {
        public IWebhookEnvironmentDTO[] Environments { get; set; }
    }
}
