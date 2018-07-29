using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi.ASPNETPlugins
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
