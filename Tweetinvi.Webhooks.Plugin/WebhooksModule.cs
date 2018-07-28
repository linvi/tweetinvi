using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi.Webhooks.Plugin
{
    public class WebhooksModule : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            container.RegisterType<ITweetinviWebhooksRoutes, TweetinviWebhooksRoutes>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITweetinviWebhookRouter, TweetinviWebhookRouter>(RegistrationLifetime.InstancePerApplication);
        }
    }
}
