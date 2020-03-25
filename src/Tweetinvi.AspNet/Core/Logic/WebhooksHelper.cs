using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tweetinvi.AspNet.Core.Models;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Models;

namespace Tweetinvi.AspNet.Core.Logic
{
    public interface IWebhooksHelper
    {
        CrcResponseTokenInfo CreateCrcResponseToken(string message, string secret);
        bool IsCrcChallenge(IWebhooksRequestInfoRetriever request);
        Task<bool> IsRequestManagedByTweetinvi(IWebhooksRequest request);
    }

    public class WebhooksHelper : IWebhooksHelper
    {
        private readonly IJObjectStaticWrapper _jObjectStaticWrapper;

        public WebhooksHelper(IJObjectStaticWrapper jObjectStaticWrapper)
        {
            _jObjectStaticWrapper = jObjectStaticWrapper;
        }

        public async Task<bool> IsRequestManagedByTweetinvi(IWebhooksRequest request)
        {
            var isRequestComingFromTwitter = IsRequestComingFromTwitter(request);
            if (!isRequestComingFromTwitter)
            {
                return false;
            }

            var body = await request.GetJsonFromBody().ConfigureAwait(false);

            if (body != null)
            {
                var jsonObjectEvent = _jObjectStaticWrapper.GetJobjectFromJson(body);
                var isAccountActivityRequest = jsonObjectEvent?.ContainsKey("for_user_id");

                if (isAccountActivityRequest == true)
                {
                    return true;
                }
            }

            return IsCrcChallenge(request);
        }

        private static bool IsRequestComingFromTwitter(IWebhooksRequestInfoRetriever request)
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

        public CrcResponseTokenInfo CreateCrcResponseToken(string message, string secret)
        {
            var encoding = new System.Text.ASCIIEncoding();
            var keyBytes = encoding.GetBytes(secret);
            var messageBytes = encoding.GetBytes(message);

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                var hashmessage = hmacsha256.ComputeHash(messageBytes);
                var crcResponseToken = Convert.ToBase64String(hashmessage);

                var response = new
                {
                    response_token = $"sha256={crcResponseToken}"
                };

                var crcResponseJson = JsonConvert.SerializeObject(response);

                return new CrcResponseTokenInfo
                {
                   Json = crcResponseJson,
                   CrcResponseToken = crcResponseToken
                };
            }
        }
    }
}
