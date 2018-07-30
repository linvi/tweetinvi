using System;
using Examplinvi.ASP.NET.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Tweetinvi;
using Tweetinvi.ASPNETPlugins;
using Tweetinvi.ASPNETPlugins.Models;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Models;

namespace Examplinvi.WebhooksServer
{
    public class Startup
    {
        public static TweetinviWebhookConfiguration TweetinviWebhookConfiguration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Plugins.Add<WebhooksModule>();

            var consumerOnlyCredentials = new ConsumerOnlyCredentials("CONSUMER_TOKEN", "CONSUMER_SECRET")
            {
                ApplicationOnlyBearerToken = "BEARER_TOKEN"
            };

            Auth.SetApplicationOnlyCredentials(consumerOnlyCredentials.ConsumerKey, consumerOnlyCredentials.ConsumerSecret, consumerOnlyCredentials.ApplicationOnlyBearerToken);

            TweetinviWebhookConfiguration = new TweetinviWebhookConfiguration(consumerOnlyCredentials);

            app.UseTweetinviWebhooks(TweetinviWebhookConfiguration);

            var webhookEnvironments = Webhooks.GetAllWebhookEnvironments(consumerOnlyCredentials);
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

                    activityStream.TweetFavourited += (sender, args) =>
                    {
                        Console.WriteLine($"Tweet {args.Tweet.Id} Favourited");
                    };

                    TweetinviWebhookConfiguration.AddActivityStream(activityStream);
                });

            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
