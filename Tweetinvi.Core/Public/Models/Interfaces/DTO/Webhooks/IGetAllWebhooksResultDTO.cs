namespace Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks
{
    public interface IGetAllWebhooksResultDTO
    {
        IWebhookEnvironmentDTO[] Environments { get; set; }
    }
}
