using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Core.Logic;
using Tweetinvi.WebhooksShared.Core.Logic;

namespace Tweetinvi.AspNet
{
    public interface IWebhooksRoutes
    {
        Task<bool> TryToReplyToCRCChallenge(IWebhooksRequestHandler requestHandler, IConsumerCredentials credentials);
    }

    public class WebhooksRoutes : IWebhooksRoutes
    {
        private readonly IWebhooksHelper _webhooksHelper;

        public WebhooksRoutes(IWebhooksHelper webhooksHelper)
        {
            _webhooksHelper = webhooksHelper;
        }

        public async Task<bool> TryToReplyToCRCChallenge(IWebhooksRequestHandler requestHandler, IConsumerCredentials credentials)
        {
            var crcToken = requestHandler.GetQuery()["crc_token"];

            if (!crcToken.IsNullOrEmpty())
            {
                await ReplyToCRCChallengeRequest(crcToken[0], requestHandler, credentials);

                return true;
            }

            return false;
        }

        private async Task ReplyToCRCChallengeRequest(string crcToken, IWebhooksRequestHandler requestHandler, IConsumerCredentials credentials)
        {
            var crcResponseInfo = _webhooksHelper.CreateCRCResponseToken(crcToken, credentials.ConsumerSecret);

            await requestHandler.WriteInResponseAsync(crcResponseInfo.Json, crcResponseInfo.ContenType);
        } 
    }
}
