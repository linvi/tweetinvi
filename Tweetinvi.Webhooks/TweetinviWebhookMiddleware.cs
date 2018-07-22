using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Webhooks.Controllers;

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
        private TwitterWebhookController _controller;

        public TweetinviWebhookMiddleware(RequestDelegate next, IOptions<TweetinviWebhookConfiguration> options)
        {
            _next = next;
            _configuration = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var requestLogMessage = $"REQUEST:\n{request.Method} - {request.Path.Value}{request.QueryString}";
            requestLogMessage += $"\nContentType: {request.ContentType ?? "Not specified"}";
            requestLogMessage += $"\nHost: {request.Host}";

            context.Items["consumer_secret"] = "CONSUMER_SECRET!";

            if (request.Path.ToString().StartsWith(_configuration.BasePath))
            {
                await TweetinviRouter.Route(context, _configuration);
            }
            else
            {
                Console.WriteLine(requestLogMessage);

                await _next(context);

                var response = context.Response;
                var responseLogMessage = $"\nRESPONSE:\nStatus Code: {response.StatusCode}";

                Console.WriteLine(responseLogMessage);
            }
        }
    }

    public class TweetinviRouter
    {
        public static async Task Route(HttpContext context, TweetinviWebhookConfiguration configuration)
        {
            if (!IsRequestComingFromTwitter(context, configuration))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Not authorized");
                return;
            }

            var path = context.Request.Path;

            if (path.StartsWithSegments(configuration.BasePath))
            {
                // Handle CRC Token
                await CRCTokenRequest(context, configuration);
            }
            else
            {
                Console.WriteLine("sa;it");
            }

        }

        private static bool IsRequestComingFromTwitter(HttpContext context, TweetinviWebhookConfiguration configuration)
        {
            if (!context.Request.Headers.ContainsKey("x-twitter-webhooks-signature"))
            {               
                return false;
            }

            // TODO Additional logic to ensure the request comes from Twitter
            // described here : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/guides/securing-webhooks

            return true;
        }

        private static async Task CRCTokenRequest(HttpContext context, TweetinviWebhookConfiguration configuration)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var crcToken = context.Request.Query["crc_token"];

            if (crcToken.IsNullOrEmpty())
            {
                await context.Response.WriteAsync("crc_token cannot be empty");
                return;
            }

            var response = new
            {
                response_token = CreateToken(crcToken, configuration.ConsumerCredentials.ConsumerSecret)
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private static string CreateToken(string message, string secret)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}
