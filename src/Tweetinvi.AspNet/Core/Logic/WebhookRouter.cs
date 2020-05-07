using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Streaming.Webhooks;

namespace Tweetinvi.AspNet.Core.Logic
{
    public interface IWebhookRouter
    {
        Task<bool> IsRequestManagedByTweetinviAsync(IWebhooksRequest request);
        Task<bool> TryRouteRequestAsync(IWebhooksRequest request, IConsumerOnlyCredentials credentials);
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

        public Task<bool> IsRequestManagedByTweetinviAsync(IWebhooksRequest request)
        {
            return _webhooksHelper.IsRequestManagedByTweetinviAsync(request);
        }

        public async Task<bool> TryRouteRequestAsync(IWebhooksRequest request, IConsumerOnlyCredentials credentials)
        {
            var isCrcChallenge = _webhooksHelper.IsCrcChallenge(request);
            if (isCrcChallenge)
            {
                return await _webhooksRoutes.TryToReplyToCrcChallengeAsync(request, credentials).ConfigureAwait(false);
            }

            var jsonBody = await request.GetJsonFromBodyAsync().ConfigureAwait(false);

            _webhookDispatcher.WebhookMessageReceived(new WebhookMessage(jsonBody));

            return await Task.FromResult(true).ConfigureAwait(false);
        }
    }
}
