namespace Tweetinvi.Models.DTO.Webhooks
{
    public interface IGetAllWebhooksResultDTO
    {
        IWebhookEnvironmentDTO[] Environments { get; set; }
    }
}
