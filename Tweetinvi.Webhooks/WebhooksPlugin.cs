using Tweetinvi.AspNet;
using Tweetinvi.Core.Injectinvi;

namespace Tweetinvi
{
    public class WebhooksPlugin : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            container.RegisterType<IWebhooksRoutes, WebhooksRoutes>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IWebhookRouter, WebhookRouter>(RegistrationLifetime.InstancePerApplication);
        }
    }
}
