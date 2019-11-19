using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi.Logic.DTO
{
    public class GetAllWebhooksResultDTO : IGetAllWebhooksResultDTO
    {
        public IWebhookEnvironmentDTO[] Environments { get; set; }
    }
}
