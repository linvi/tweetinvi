using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;

namespace Tweetinvi.Logic.DTO
{
    public class GetAllWebhooksResultDTO : IGetAllWebhooksResultDTO
    {
        public IWebhookEnvironmentDTO[] Environments { get; set; }
    }
}
