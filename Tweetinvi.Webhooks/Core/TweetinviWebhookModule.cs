using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Webhooks;

namespace Tweetinvi.Webhooks.Core
{
    public class TweetinviWebhookModule : ITweetinviModule
    {
        public void Initialize(ITweetinviContainer container)
        {
            container.RegisterType<IWebhookProtocolProcessClient, WebhookProtocolProcessClient>();
        }
    }
}
