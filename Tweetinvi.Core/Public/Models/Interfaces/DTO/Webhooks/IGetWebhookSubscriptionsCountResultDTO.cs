namespace Tweetinvi.Models.DTO.Webhooks
{
    public interface IGetWebhookSubscriptionsCountResultDTO
    {
        string AccountName { get; set; }
        string SubscriptionsCountAll { get; set; }
        string SubscriptionsCountDirectMessages { get; set; }
    }
}
