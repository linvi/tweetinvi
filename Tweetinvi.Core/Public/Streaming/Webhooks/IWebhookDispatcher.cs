using Tweetinvi.Models.Webhooks;
using Tweetinvi.Streaming;

namespace Tweetinvi.Core.Public.Streaming.Webhooks
{
    public interface IWebhookDispatcher
    {
        IAccountActivityStream[] SubscribedAccountActivityStreams { get; }

        void WebhookMessageReceived(IWebhookMessage message);
        void SubscribeAccountActivityStream(IAccountActivityStream accountActivityStream);
        void UnsubscribeAccountActivityStream(IAccountActivityStream accountActivityStream);
    }
}
