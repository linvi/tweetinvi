using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Logic;

namespace Tweetinvi.Modules
{
    public class WebhooksSharedModule : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            container.RegisterType<IWebhooksHelper, WebhooksHelper>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IWebhooksRoutes, WebhooksRoutes>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<IWebhookRouter, WebhookRouter>(RegistrationLifetime.InstancePerApplication);
        }
    }
}
