namespace Tweetinvi.Models
{
    public interface IWebhookSubscriptionsCount
    {
        string AccountName { get; set; }
        string SubscriptionsCount { get; set; }
        string ProvisionedCount { get; set; }
    }
}
