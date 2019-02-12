using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Tweetinvi.Models;

namespace Tweetinvi.AspNet
{
    public static class WebhookMiddlewareExtensions
    {
        public static IApplicationBuilder UseTweetinviWebhooks(this IApplicationBuilder app, WebhookConfiguration configuration)
        {
            return app.UseMiddleware<WebhookMiddleware>(Options.Create(configuration));
        }
    }

    public class WebhookMiddleware
    {
        private readonly RequestDelegate _next;
        private IWebhookConfiguration _configuration;
        private IWebhookRouter _router;

        public WebhookMiddleware(RequestDelegate next, IOptions<WebhookConfiguration> options)
        {
            _next = next;
            _configuration = options.Value;

            _router = WebhooksPlugin.Container.Resolve<IWebhookRouter>();
        }

        public async Task Invoke(HttpContext context)
        {
            var requestHandler = new WebhooksRequestHandlerForAspNetCore(context);

            if (_router.IsRequestManagedByTweetinvi(requestHandler, _configuration))
            {
                var routeHandled = await _router.TryRouteRequest(requestHandler, _configuration);

                if (routeHandled)
                {
                    return;
                }
            }

            // Continue to any other request supported by the website.
            await _next(context);
        }
    }
}
