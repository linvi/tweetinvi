using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tweetinvi;
using Tweetinvi.AspNet;
using Tweetinvi.AspNet.Public;
using Tweetinvi.Models;

namespace Examplinvi.AccountActivity.ASP.NETCore
{
    public class Startup
    {
        public static IAccountActivityRequestHandler AccountActivityRequestHandler { get; set; }
        public static ITwitterClient WebhookClient { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            WebhookServerInitialization(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private static void WebhookServerInitialization(IApplicationBuilder app)
        {
            Plugins.Add<WebhooksPlugin>();

            var credentials = new TwitterCredentials("CONSUMER_TOKEN", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET")
            {
                BearerToken = "BEARER_TOKEN"
            };

            WebhookClient = new TwitterClient(credentials);
            AccountActivityRequestHandler = WebhookClient.AccountActivity.CreateRequestHandler();
            var config = new WebhookMiddlewareConfiguration(AccountActivityRequestHandler);
            app.UseTweetinviWebhooks(config);
        }
    }
}
