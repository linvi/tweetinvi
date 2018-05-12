namespace Tweetinvi.Core.Webhooks
{
    public interface IWebhookProtocolProcessClient : IWebhookProtocolClient
    {
        void Start();
    }
}
