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
            var consumerCredentials = new ConsumerCredentials("CONSUMER_TOKEN", "CONSUMER_SECRET")
            {
                ApplicationOnlyBearerToken = "BEARER_TOKEN"
            };

            Auth.SetApplicationOnlyCredentials(consumerCredentials.ConsumerKey, consumerCredentials.ConsumerSecret, consumerCredentials.ApplicationOnlyBearerToken);

            TweetinviWebhookConfiguration = new TweetinviWebhookConfiguration(consumerCredentials);

            var webhookEnvironments = Webhooks.GetAllWebhookEnvironments();
            webhookEnvironments.Environments.ForEach(environment =>
            {
                var webhookEnvironment = new RegistrableWebhookEnvironment(environment);
                TweetinviWebhookConfiguration.AddWebhookEnvironment(webhookEnvironment);

                var subscriptions = Webhooks.GetListOfSubscriptions(environment.Name);
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

            app.UseTweetinviWebhooks(TweetinviWebhookConfiguration);

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
