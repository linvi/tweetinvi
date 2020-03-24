namespace Tweetinvi.Models
{
    public interface IWebhookSubscription
    {
        /// <summary>
        /// User identifier of the subscription
        /// </summary>
        string UserId { get; }
    }
}