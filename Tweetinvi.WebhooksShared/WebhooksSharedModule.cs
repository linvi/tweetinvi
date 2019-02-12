using Tweetinvi.AspNet;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Logic;

namespace Tweetinvi.WebhooksShared
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
