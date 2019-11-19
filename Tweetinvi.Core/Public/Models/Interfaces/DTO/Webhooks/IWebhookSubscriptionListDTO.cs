namespace Tweetinvi.Models.DTO.Webhooks
{
    public interface IWebhookSubscriptionDTO
    {
        string UserId { get; set; }
    }


    public interface IWebhookSubscriptionListDTO
    {
        string Environment { get; set; }
        string ApplicationId { get; set; }
        IWebhookSubscriptionDTO[] Subscriptions { get; set; }
    }

}
