using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tweetinvi;
using Tweetinvi.AspNet;
using Tweetinvi.Models;

namespace Examplinvi.AccountActivity.ASP.NETCore._3._0
{
    public class Startup
    {
        public static IAccountActivityRequestHandler AccountActivityRequestHandler { get; set; }
        public static ITwitterClient WebhookClient { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            WebhookServerInitialization(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            var r= Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(System.Runtime.Versioning.TargetFrameworkAttribute), false)
                .SingleOrDefault();
                Console.WriteLine(r);
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