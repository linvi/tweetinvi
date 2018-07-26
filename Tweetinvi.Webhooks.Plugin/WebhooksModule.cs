using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi.Webhooks.Plugin
{
    public class WebhooksModule : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            container.RegisterType<IAccountActivityStream, AccountActivityStream>();
            container.RegisterType<IWebhookMessage, WebhookMessage>();

            container.RegisterType<ITweetinviWebhookController, TweetinviWebhookController>();

            container.RegisterType<ITweetinviWebhooksRoutes, TweetinviWebhooksRoutes>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITweetinviWebhookRouter, TweetinviWebhookRouter>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IWebhookDispatcher, WebhookDispatcher>(RegistrationLifetime.InstancePerApplication);
        }
    }
}
