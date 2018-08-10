using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

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
        private WebhookConfiguration _configuration;
        private IWebhookRouter _router;

        public WebhookMiddleware(RequestDelegate next, IOptions<WebhookConfiguration> options)
        {
            _next = next;
            _configuration = options.Value;

            _router = TweetinviContainer.Resolve<IWebhookRouter>();
        }

        public async Task Invoke(HttpContext context)
        {
            if (_router.IsRequestManagedByTweetinvi(context.Request, _configuration))
            {
                var routeHandled = await _router.Route(context, _configuration);

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
