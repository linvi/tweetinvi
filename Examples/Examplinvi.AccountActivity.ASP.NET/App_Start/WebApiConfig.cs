using System.Threading.Tasks;
using System.Web.Http;
using Tweetinvi;
using Tweetinvi.AspNet;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Models;

namespace Examplinvi.WebhooksApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Plugins.Add<WebhooksPlugin>();
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            var consumerOnlyCredentials = new ConsumerOnlyCredentials("5EpUsp9mbMMRMJ0zqsug", "cau8CExOCUordXMJeoGfW0QoPTp6bUAOrqUELKk1CSM")
            {
                ApplicationOnlyBearerToken = "AAAAAAAAAAAAAAAAAAAAAFqqSQAAAAAABRtNASGJXtIVX1somRAmqhSj68o%3Dm3n0HLyG1OmZaFDsrLITnStpXHPU82RYr4HJAN1TdG9QpmEPky"
            };


            if (consumerOnlyCredentials.ApplicationOnlyBearerToken == null)
            {
                Auth.InitializeApplicationOnlyCredentials(consumerOnlyCredentials);
            }

            TweetinviWebhooksHost.Configuration = new WebhookConfiguration(consumerOnlyCredentials);

            var messageHandler = new WebhookMiddlewareMessageHandler(TweetinviWebhooksHost.Configuration);

            RegisterAccountActivities(consumerOnlyCredentials).Wait();

            //config.MessageHandlers.Add(messageHandler);

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

        private static async Task RegisterAccountActivities(IConsumerOnlyCredentials consumerOnlyCredentials)
        {
            var webhookEnvironments = await Webhooks.GetAllWebhookEnvironmentsAsync(consumerOnlyCredentials);

            webhookEnvironments.ForEach(async environment =>
            {
                var webhookEnvironment = new RegistrableWebhookEnvironment(environment)
                {
                    Credentials = consumerOnlyCredentials
                };

                TweetinviWebhooksHost.Configuration.AddWebhookEnvironment(webhookEnvironment);
            });
        }
    }
}
