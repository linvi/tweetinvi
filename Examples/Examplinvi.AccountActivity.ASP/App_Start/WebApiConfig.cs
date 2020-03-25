using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Tweetinvi;
using Tweetinvi.AspNet;
using Tweetinvi.Models;

namespace Examplinvi.AccountActivity.ASP
{
    public static class WebApiConfig
    {
        public static IAccountActivityRequestHandler AccountActivityRequestHandler { get; set; }
        public static ITwitterClient WebhookClient { get; set; }

        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            Plugins.Add<WebhooksPlugin>();

            var credentials = new TwitterCredentials("CONSUMER_TOKEN", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET")
            {
                BearerToken = "BEARER_TOKEN"
            };

            WebhookClient = new TwitterClient(credentials);

            AccountActivityRequestHandler = WebhookClient.AccountActivity.CreateRequestHandler();

            // var messageHandler = new WebhookMiddlewareMessageHandler(WebhookClient);

            var messageHandler = config.MessageHandlers.UseTweetinviWebhooks(AccountActivityRequestHandler);
            // config.MessageHandlers.Add(messageHandler);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "TweetinviWebhooks",
                routeTemplate: "account_activity",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: messageHandler
            );
        }
    }
}
