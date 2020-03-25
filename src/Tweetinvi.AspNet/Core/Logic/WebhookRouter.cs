using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Streaming.Webhooks;

namespace Tweetinvi.AspNet.Core.Logic
{
    public interface IWebhookRouter
    {
        Task<bool> IsRequestManagedByTweetinvi(IWebhooksRequest request);
        Task<bool> TryRouteRequest(IWebhooksRequest request, IConsumerOnlyCredentials credentials);
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

        public Task<bool> IsRequestManagedByTweetinvi(IWebhooksRequest request)
        {
            return _webhooksHelper.IsRequestManagedByTweetinvi(request);
        }

        public async Task<bool> TryRouteRequest(IWebhooksRequest request, IConsumerOnlyCredentials credentials)
        {
            var isCrcChallenge = _webhooksHelper.IsCrcChallenge(request);
            if (isCrcChallenge)
            {
                return await _webhooksRoutes.TryToReplyToCrcChallenge(request, credentials);
            }

            var jsonBody = await request.GetJsonFromBody().ConfigureAwait(false);

            _webhookDispatcher.WebhookMessageReceived(new WebhookMessage(jsonBody));

            return await Task.FromResult(true);
        }
    }
}
