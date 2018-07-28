using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Tweetinvi.Core.Public.Streaming.Webhooks;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Webhooks;
using Tweetinvi.WebLogic.Webhooks;

namespace Tweetinvi.Webhooks.Plugin
{
    public interface ITweetinviWebhookRouter
    {
        bool IsRequestManagedByTweetinvi(HttpRequest request, ITweetinviWebhookConfiguration configuration);
        Task<bool> Route(HttpContext context, ITweetinviWebhookConfiguration configuration);
    }

    public class TweetinviWebhookRouter : ITweetinviWebhookRouter
    {
        private readonly IWebhookDispatcher _webhookDispatcher;
        private ITweetinviWebhooksRoutes _webhooksRoutes;

        public TweetinviWebhookRouter(
            IWebhookDispatcher webhookDispatcher,
            ITweetinviWebhooksRoutes webhooksWebhooksRoutes)
        {
            _webhookDispatcher = webhookDispatcher;
            _webhooksRoutes = webhooksWebhooksRoutes;
        }

        private IEnumerable<IWebhookDTO> GetWebhooksMatching(HttpRequest request, ITweetinviWebhookConfiguration configuration)
        {
            return configuration.RegisteredWebhookEnvironments.SelectMany(x => x.Webhooks).Where(webhook =>
                {
                    var path = new Uri(webhook.Url);
                    return request.Path.ToString().StartsWith(webhook.Url);
                });
        }

        public bool IsRequestManagedByTweetinvi(HttpRequest request, ITweetinviWebhookConfiguration configuration)
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

        public async Task<bool> Route(HttpContext context, ITweetinviWebhookConfiguration configuration)
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

        public bool IsRequestComingFromTwitter(HttpRequest request, ITweetinviWebhookConfiguration configuration)
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
