using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.WebhooksShared.Core.Logic;

namespace Tweetinvi.Core.Logic
{
    public interface IWebhooksHelper
    {
        CRCResponseTokenInfo CreateCRCResponseToken(string message, string secret);
        IEnumerable<IWebhookDTO> GetWebhooksMatching(IWebhooksRequestInfoRetriever request, IWebhookConfiguration configuration);
        bool IsCrcChallenge(IWebhooksRequestInfoRetriever request);
        bool IsRequestManagedByTweetinvi(IWebhooksRequestInfoRetriever request, IWebhookConfiguration configuration);
    }

    public class WebhooksHelper : IWebhooksHelper
    {
        public bool IsRequestManagedByTweetinvi(IWebhooksRequestInfoRetriever request, IWebhookConfiguration configuration)
        {
            var isRequestComingFromTwitter = IsRequestComingFromTwitter(request);

            if (!isRequestComingFromTwitter)
            {
                return false;
            }

            var webhooks = GetWebhooksMatching(request, configuration);
            var anyWebhookMatchingRequest = webhooks.Any();

            var isCrc = IsCrcChallenge(request);

            return anyWebhookMatchingRequest || isCrc;
        }

        public IEnumerable<IWebhookDTO> GetWebhooksMatching(IWebhooksRequestInfoRetriever request, IWebhookConfiguration configuration)
        {
            return configuration.RegisteredWebhookEnvironments.SelectMany(x => x.Webhooks).Where(webhook =>
            {
                var path = request.GetPath();
                return webhook.Url.EndsWith(path);
            });
        }

        public bool IsRequestComingFromTwitter(IWebhooksRequestInfoRetriever request)
        {
            if (!request.GetHeaders().ContainsKey("x-twitter-webhooks-signature"))
            {
                return false;
            }

            // TODO Additional logic to ensure the request comes from Twitter
            // described here : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/guides/securing-webhooks

            return true;
        }

        public bool IsCrcChallenge(IWebhooksRequestInfoRetriever request)
        {
            if (!request.GetQuery().TryGetValue("crc_token", out var crcToken))
            {
                return false;
            }

            return crcToken.Any();
        }

        public CRCResponseTokenInfo CreateCRCResponseToken(string message, string secret)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                var crcResponseToken = Convert.ToBase64String(hashmessage);

                var response = new
                {
                    response_token = $"sha256={crcResponseToken}"
                };

                var crcResponseJson = JsonConvert.SerializeObject(response);

                return new CRCResponseTokenInfo()
                {
                   Json = crcResponseJson,
                   CrcResponseToken = crcResponseToken
                };
            }
        }
    }
}
