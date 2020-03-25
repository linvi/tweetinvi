using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tweetinvi;
using Tweetinvi.AspNet;
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

            var credentials = new TwitterCredentials("S7zdhisQjfVeyxev4upGaDS6P",
                "hbgWl5XHWizuJbajBKq7xobhfW4aC3xAmz3xzaUL9NiBmrWG5t",
                "1577389800-JOJC1C4OvOq0ky13N8c5IelvfByB86uJelNmEuc", "DUM0gksYSe9ak1odfv2z2X0pWd0QsMV5ieivi6YOWrce8")
            {
                BearerToken = "AAAAAAAAAAAAAAAAAAAAAFqqSQAAAAAABRtNASGJXtIVX1somRAmqhSj68o%3Dm3n0HLyG1OmZaFDsrLITnStpXHPU82RYr4HJAN1TdG9QpmEPky"
            };

            WebhookClient = new TwitterClient(credentials);
            AccountActivityRequestHandler = WebhookClient.AccountActivity.CreateRequestHandler();
            var config = new WebhookMiddlewareConfiguration(AccountActivityRequestHandler);
            app.UseTweetinviWebhooks(config);
        }
    }
}
