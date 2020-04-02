using Tweetinvi.Models;

namespace Tweetinvi.AspNet
{
    public class InternalWebhookMiddlewareConfiguration
    {
        public InternalWebhookMiddlewareConfiguration()
        {
        }

        public InternalWebhookMiddlewareConfiguration(IAccountActivityRequestHandler requestHandler)
        {
            RequestHandler = requestHandler;
        }

        public IAccountActivityRequestHandler RequestHandler { get; set; }
    }

    public class WebhookMiddlewareConfiguration : InternalWebhookMiddlewareConfiguration
    {
        public WebhookMiddlewareConfiguration(IAccountActivityRequestHandler requestHandler) : base(requestHandler)
        {
        }
    }
}