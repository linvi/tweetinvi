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

        private static async Task RegisterAccountActivities(ConsumerOnlyCredentials consumerOnlyCredentials)
        {
            var webhookEnvironments = await Webhooks.GetAllWebhookEnvironmentsAsync(consumerOnlyCredentials);

            webhookEnvironments.ForEach(environment =>
            {
                var webhookEnvironment = new RegistrableWebhookEnvironment(environment)
                {
                    Credentials = consumerOnlyCredentials
                };

                TweetinviWebhookConfiguration.AddWebhookEnvironment(webhookEnvironment);

                var subscriptions = Webhooks.GetListOfSubscriptions(environment.Name, consumerOnlyCredentials);
                subscriptions.Subscriptions.ForEach(subscription =>
                {
                    var activityStream = Stream.CreateAccountActivityStream(subscription.UserId);

                    activityStream.TweetDeleted += (sender, args) =>
                    {
                        Console.WriteLine($"Tweet {args.Timestamp} Favourited");
                    };

                    TweetinviWebhookConfiguration.AddActivityStream(activityStream);
                });
            });
        }
    }
}
