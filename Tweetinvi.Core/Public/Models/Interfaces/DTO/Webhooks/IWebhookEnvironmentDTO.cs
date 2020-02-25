namespace Tweetinvi.Models.DTO.Webhooks
{
    public interface IWebhookEnvironmentDTO
    {
        string Name { get; set; }
        IWebhookDTO[] Webhooks { get; set; }
    }
}
