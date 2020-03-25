using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Tweetinvi.Models;

namespace Tweetinvi.AspNet
{
    public static class WebhookMiddlewareExtensions
    {
        public static IApplicationBuilder UseTweetinviWebhooks(this IApplicationBuilder app, WebhookMiddlewareConfiguration configuration)
        {
            var config = configuration as InternalWebhookMiddlewareConfiguration;
            return app.UseMiddleware<WebhookMiddleware>(Options.Create(config));
        }
    }

    public class WebhookMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAccountActivityRequestHandler _accountActivityHandler;

        public WebhookMiddleware(RequestDelegate next, IOptions<InternalWebhookMiddlewareConfiguration> options)
        {
            _next = next;

            _accountActivityHandler = options.Value.RequestHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = new WebhooksRequestForAspNetCore(context);
            var isRequestManagedByTweetinvi = await _accountActivityHandler.IsRequestManagedByTweetinvi(request).ConfigureAwait(false);

            if (isRequestManagedByTweetinvi)
            {
                var routeHandled = await _accountActivityHandler.TryRouteRequest(request).ConfigureAwait(false);
                if (routeHandled)
                {
                    return;
                }
            }

            // Continue to any other request supported by the website.
            await _next(context).ConfigureAwait(false);
        }
    }
}
