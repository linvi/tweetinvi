using Examplinvi.ASP.NET.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Models;
using Tweetinvi.Webhooks.Plugin.Models;
using Tweetinvi.WebLogic.Webhooks;

namespace Tweetinvi.Webhooks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var sandboxEnvironment = new RegistrableWebhookEnvironment
            {
                Name = "sandbox",
                Credentials = new ConsumerCredentials("5EpUsp9mbMMRMJ0zqsug", "cau8CExOCUordXMJeoGfW0QoPTp6bUAOrqUELKk1CSM")
                {
                    ApplicationOnlyBearerToken = "AAAAAAAAAAAAAAAAAAAAAFqqSQAAAAAABRtNASGJXtIVX1somRAmqhSj68o%3Dm3n0HLyG1OmZaFDsrLITnStpXHPU82RYr4HJAN1TdG9QpmEPky"
                }
            };

            sandboxEnvironment.AddWebhook(new WebhookDTO()
            {
                Id = "1021811834721067013",
                Url = "https://ce8e844f.ngrok.io/tweetinvi-webhooks"
            });

            var webhooksConfig = new TweetinviWebhookConfiguration();
            webhooksConfig.AddWebhookEnvironment(sandboxEnvironment);

            app.UseTweetinviWebhooks(webhooksConfig);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            ;
        }
    }
}
