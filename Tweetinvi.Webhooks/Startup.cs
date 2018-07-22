using Examplinvi.ASP.NET.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tweetinvi.Models;

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
            app.UseTweetinviWebhooks(new TweetinviWebhookConfiguration()
            {
                AppUrl = "http://192.168.0.13",
                BasePath = "/tweetinvi-webhooks",
                EnvName = "sandbox",
                ConsumerCredentials = new ConsumerCredentials("CONSUMER_KEY", "CONSUMER_SECRET")
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
;        }
    }
}
