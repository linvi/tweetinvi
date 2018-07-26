using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Webhooks.Plugin;
using Tweetinvi.WebLogic.Webhooks;

namespace Examplinvi.ASP.NET.Core
{
    public static class TweetinviWebhookMiddlewareExtensions
    {
        public static IApplicationBuilder UseTweetinviWebhooks(this IApplicationBuilder app, TweetinviWebhookConfiguration configuration)
        {
            return app.UseMiddleware<TweetinviWebhookMiddleware>(Options.Create(configuration));
        }
    }

    public class TweetinviWebhookMiddleware
    {
        private readonly RequestDelegate _next;
        private TweetinviWebhookConfiguration _configuration;
        private ITweetinviWebhookRouter _router;

        public TweetinviWebhookMiddleware(RequestDelegate next, IOptions<TweetinviWebhookConfiguration> options)
        {
            _next = next;
            _configuration = options.Value;

            _router = TweetinviContainer.Resolve<ITweetinviWebhookRouter>();
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
