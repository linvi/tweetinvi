using System.Net;
using Microsoft.AspNetCore.Http;
using Tweetinvi.Core.Logic;
using Tweetinvi.Models;

namespace Tweetinvi.AspNet
{
    public class WebhookRequestFactory
    {
        public static IWebhooksRequest Create(HttpContext context)
        {
            return new WebhooksRequestForAspNetCore(context);
        }

        public static IWebhooksRequest Create(HttpListenerContext context)
        {
            return new WebhooksRequestForHttpServer(context);
        }
    }
}