using System;
using System.Threading.Tasks;
using Examplinvi.ASP.NET.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tweetinvi;
using Tweetinvi.ASPNETPlugins;
using Tweetinvi.ASPNETPlugins.Models;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;

namespace Examplinvi.WebhooksServer
{
    public class Startup
    {
        public static TweetinviWebhookConfiguration TweetinviWebhookConfiguration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var consumerOnlyCredentials = new ConsumerOnlyCredentials("CONSUMER_TOKEN", "CONSUMER_SECRET")
            {
                ApplicationOnlyBearerToken = "BEARER_TOKEN"
            };

            WebhookServerInitialization(app, consumerOnlyCredentials);
            RegisterAccountActivities(consumerOnlyCredentials).Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private static void WebhookServerInitialization(IApplicationBuilder app, IConsumerOnlyCredentials consumerOnlyCredentials)
        {
            Plugins.Add<WebhooksModule>();

            Auth.SetApplicationOnlyCredentials(
                consumerOnlyCredentials.ConsumerKey, 
                consumerOnlyCredentials.ConsumerSecret,
                consumerOnlyCredentials.ApplicationOnlyBearerToken);

            TweetinviWebhookConfiguration = new TweetinviWebhookConfiguration(consumerOnlyCredentials);

            app.UseTweetinviWebhooks(TweetinviWebhookConfiguration);
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

                TweetinviWebhookConfiguration.AddWebhookEnvironment(webhookEnvironment);

                await SubscribeToAllAccountActivities(consumerOnlyCredentials, environment);
            });
        }

        private static async Task SubscribeToAllAccountActivities(
            IConsumerOnlyCredentials consumerOnlyCredentials,
            IWebhookEnvironmentDTO environment)
        {
            // If you wish to subscribe to the different account activity events you can do the following
            var subscriptions = await Webhooks.GetListOfSubscriptionsAsync(environment.Name, consumerOnlyCredentials);

            subscriptions.Subscriptions.ForEach(subscription =>
            {
                var activityStream = Stream.CreateAccountActivityStream(subscription.UserId);

                activityStream.JsonObjectReceived += (sender, args) => { Console.WriteLine("json received : " + args.Json); };

                TweetinviWebhookConfiguration.AddActivityStream(activityStream);
            });
        }
    }
}
