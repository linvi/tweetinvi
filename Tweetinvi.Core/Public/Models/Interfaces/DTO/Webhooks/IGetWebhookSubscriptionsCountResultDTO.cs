namespace Tweetinvi.Models.DTO.Webhooks
{
    public interface IGetWebhookSubscriptionsCountResultDTO
    {
        string AccountName { get; set; }
        string SubscriptionsCount { get; set; }
        string ProvisionedCount { get; set; }
    }
}
