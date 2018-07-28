using System;
using Tweetinvi.Events;
using Tweetinvi.Models.Webhooks;

namespace Tweetinvi.Core.Public.Streaming
{
    public interface IAccountActivityStream
    {
        long UserId { get; }
        EventHandler<TweetReceivedEventArgs> TweetCreated { get; set; }

        void WebhookMessageReceived(IWebhookMessage message);
    }
}
