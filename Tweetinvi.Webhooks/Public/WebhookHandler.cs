namespace Tweetinvi.Webhooks.Public
{
    public interface IWebhookHandler
    {
        IWebhookReceiver AttachedTo { get; set; }
    }

    public class WebhookHandler : IWebhookHandler
    {
        public IWebhookReceiver AttachedTo { get; set; }
    }
}
