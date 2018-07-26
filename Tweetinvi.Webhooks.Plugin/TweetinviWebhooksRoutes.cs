using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Webhooks.Plugin.Models;

namespace Tweetinvi.Webhooks.Plugin
{
    public interface ITweetinviWebhooksRoutes
    {
        Task<bool> CRCChallenge(HttpContext context, IConsumerCredentials credentials);
    }

    public class TweetinviWebhooksRoutes : ITweetinviWebhooksRoutes
    {
        public async Task<bool> CRCChallenge(HttpContext context, IConsumerCredentials credentials)
        {
            var crcToken = context.Request.Query["crc_token"];

            if (!crcToken.IsNullOrEmpty())
            {
                await ReplyToCRCChallengeRequest(crcToken, context, credentials);

                return true;
            }

            return false;
        }

        private static async Task ReplyToCRCChallengeRequest(string crcToken, HttpContext context, IConsumerCredentials credentials)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var crcResponseToken = CreateToken(crcToken, credentials.ConsumerSecret);
            var response = new
            {
                response_token = $"sha256={crcResponseToken}"
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
