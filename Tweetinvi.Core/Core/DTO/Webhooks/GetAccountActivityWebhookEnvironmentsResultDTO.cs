using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi.Core.DTO.Webhooks
{
    public class GetAccountActivityWebhookEnvironmentsResultDTO : IGetAccountActivityWebhookEnvironmentsResultDTO
    {
        public IWebhookEnvironmentDTO[] Environments { get; set; }
    }
}
