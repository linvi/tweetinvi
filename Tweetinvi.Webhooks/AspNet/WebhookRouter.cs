using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Tweetinvi.Core.Public.Streaming.Webhooks;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Webhooks;

namespace Tweetinvi.AspNet
{
    public interface IWebhookRouter
    {
        bool IsRequestManagedByTweetinvi(HttpRequest request, IWebhookConfiguration configuration);
        Task<bool> Route(HttpContext context, IWebhookConfiguration configuration);
    }

    public class WebhookRouter : IWebhookRouter
    {
        private readonly IWebhookDispatcher _webhookDispatcher;
        private IWebhooksRoutes _webhooksRoutes;

        public WebhookRouter(
            IWebhookDispatcher webhookDispatcher,
            IWebhooksRoutes webhooksWebhooksRoutes)
        {
            _webhookDispatcher = webhookDispatcher;
            _webhooksRoutes = webhooksWebhooksRoutes;
        }

        private IEnumerable<IWebhookDTO> GetWebhooksMatching(HttpRequest request, IWebhookConfiguration configuration)
        {
            return configuration.RegisteredWebhookEnvironments.SelectMany(x => x.Webhooks).Where(webhook =>
                {
                    return webhook.Url.EndsWith(request.Path.ToString());
                });
        }

        public bool IsRequestManagedByTweetinvi(HttpRequest request, IWebhookConfiguration configuration)
        {
            var isRequestComingFromTwitter = IsRequestComingFromTwitter(request, configuration);

            if (!isRequestComingFromTwitter)
            {
                return false;
            }

            var webhooks = GetWebhooksMatching(request, configuration);
            var anyWebhookMatchingRequest = webhooks.Any();

            var isCrc = IsCrcChallenge(request);

            return anyWebhookMatchingRequest || isCrc;
        }

        public async Task<bool> Route(HttpContext context, IWebhookConfiguration configuration)
        {
            var matchingWebhooks = GetWebhooksMatching(context.Request, configuration).ToArray();
            var isCrcChallenge = context.Request.Query["crc_token"].Any();


            if (isCrcChallenge)
            {
                if (matchingWebhooks.Length > 1)
                {
                    context.Response.StatusCode = 500;
                    return await Task.FromResult(true);
                }

                var matchingEnvironment = configuration.RegisteredWebhookEnvironments[0];

                return await _webhooksRoutes.CRCChallenge(context, matchingEnvironment.Credentials);
            }

            context.Request.EnableRewind();
            var jsonBody = new StreamReader(context.Request.Body).ReadToEnd();

            _webhookDispatcher.WebhookMessageReceived(new WebhookMessage(jsonBody));

            return await Task.FromResult(true);
        }

        public bool IsCrcChallenge(HttpRequest request)
        {
            return request.Query["crc_token"].Count > 0;
        }

        public bool IsRequestComingFromTwitter(HttpRequest request, IWebhookConfiguration configuration)
        {
            if (!request.Headers.ContainsKey("x-twitter-webhooks-signature"))
            {
                return false;
            }

            // TODO Additional logic to ensure the request comes from Twitter
            // described here : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/guides/securing-webhooks

            return true;
        }
    }
}
