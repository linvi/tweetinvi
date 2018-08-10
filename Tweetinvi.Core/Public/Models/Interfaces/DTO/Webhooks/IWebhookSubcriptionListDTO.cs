namespace Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks
{
    public interface IWebhookSubscriptionDTO
    {
        string UserId { get; set; }
    }


    public interface IWebhookSubcriptionListDTO
    {
        string Environment { get; set; }
        string ApplicationId { get; set; }
        IWebhookSubscriptionDTO[] Subscriptions { get; set; }
    }

}
