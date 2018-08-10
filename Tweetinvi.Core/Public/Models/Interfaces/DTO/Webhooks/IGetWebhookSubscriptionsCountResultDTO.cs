namespace Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks
{
    public interface IGetWebhookSubscriptionsCountResultDTO
    {
        string AccountName { get; set; }
        string SubscriptionsCountAll { get; set; }
        string SubscriptionsCountDirectMessages { get; set; }
    }
}
