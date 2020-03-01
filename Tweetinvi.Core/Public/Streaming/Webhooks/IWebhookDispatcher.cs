using Tweetinvi.Models;

namespace Tweetinvi.Streaming.Webhooks
{
    public interface IWebhookDispatcher
    {
        IAccountActivityStream[] SubscribedAccountActivityStreams { get; }

        void WebhookMessageReceived(IWebhookMessage message);
        void SubscribeAccountActivityStream(IAccountActivityStream accountActivityStream);
        void UnsubscribeAccountActivityStream(IAccountActivityStream accountActivityStream);
    }
}
