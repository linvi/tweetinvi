using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Public.Streaming.Webhooks;
using Tweetinvi.Models;
using Tweetinvi.Models.Webhooks;
using Tweetinvi.Core.Logic;
using Tweetinvi.WebhooksShared.Core.Logic;

namespace Tweetinvi.AspNet
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
            var matchingWebhooks = _webhooksHelper.GetWebhooksMatching(requestHandler, configuration).ToArray();
            var isCrcChallenge = _webhooksHelper.IsCrcChallenge(requestHandler);

            if (isCrcChallenge)
            {
                if (matchingWebhooks.Length > 1)
                {
                    requestHandler.SetResponseStatusCode(500);
                    return await Task.FromResult(true);
                }

                var matchingWebhook = matchingWebhooks[0];
                var matchingEnvironment = configuration.RegisteredWebhookEnvironments.Single(x => x.Webhooks.Contains(matchingWebhook));

                return await _webhooksRoutes.TryToReplyToCRCChallenge(requestHandler, matchingEnvironment.Credentials);
            }

            var jsonBody = await requestHandler.GetJsonFromBody();

            _webhookDispatcher.WebhookMessageReceived(new WebhookMessage(jsonBody));

            return await Task.FromResult(true);
        }
    }
}
