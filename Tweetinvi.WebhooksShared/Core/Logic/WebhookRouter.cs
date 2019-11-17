using System.Threading.Tasks;
using Tweetinvi.Core.Public.Streaming.Webhooks;
using Tweetinvi.Models;
using Tweetinvi.Models.Webhooks;

namespace Tweetinvi.Core.Logic
{
    public interface IWebhookRouter
    {
        bool IsRequestManagedByTweetinvi(IWebhooksRequestHandler request, IWebhookConfiguration configuration);
        Task<bool> TryRouteRequest(IWebhooksRequestHandler requestHandler, IWebhookConfiguration configuration);
    }

    public class WebhookRouter : IWebhookRouter
    {
        private readonly IWebhookDispatcher _webhookDispatcher;
        private readonly IWebhooksRoutes _webhooksRoutes;
        private readonly IWebhooksHelper _webhooksHelper;

        public WebhookRouter(
            IWebhookDispatcher webhookDispatcher,
            IWebhooksRoutes webhooksWebhooksRoutes,
            IWebhooksHelper webhooksHelper)
        {
            _webhookDispatcher = webhookDispatcher;
            _webhooksRoutes = webhooksWebhooksRoutes;
            _webhooksHelper = webhooksHelper;
        }

        public bool IsRequestManagedByTweetinvi(IWebhooksRequestHandler request, IWebhookConfiguration configuration)
        {
            return _webhooksHelper.IsRequestManagedByTweetinvi(request, configuration);
        }

        public async Task<bool> TryRouteRequest(IWebhooksRequestHandler requestHandler, IWebhookConfiguration configuration)
        {
            var isCrcChallenge = _webhooksHelper.IsCrcChallenge(requestHandler);

            if (isCrcChallenge)
            {
                return await _webhooksRoutes.TryToReplyToCRCChallenge(requestHandler, configuration.ConsumerOnlyCredentials);
            }

            var jsonBody = await requestHandler.GetJsonFromBody();

            _webhookDispatcher.WebhookMessageReceived(new WebhookMessage(jsonBody));

            return await Task.FromResult(true);
        }
    }
}
