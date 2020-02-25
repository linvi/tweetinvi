namespace Tweetinvi.Models.DTO.Webhooks
{
    public interface IGetAccountActivityWebhookEnvironmentsResultDTO
    {
        IWebhookEnvironmentDTO[] Environments { get; set; }
    }
}
