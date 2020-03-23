using System.Collections.ObjectModel;
using System.Net.Http;
using Tweetinvi.Models;

namespace Tweetinvi.AspNet
{
    public static class AspNetExtensions
    {
        public static WebhookMiddlewareMessageHandler UseTweetinviWebhooks(this Collection<DelegatingHandler> messageHandlers, IAccountActivityRequestHandler accountActivityRequestHandler)
        {
            var messageHandler = new WebhookMiddlewareMessageHandler(accountActivityRequestHandler);
            messageHandlers.Add(messageHandler);
            return messageHandler;
        }
    }
}