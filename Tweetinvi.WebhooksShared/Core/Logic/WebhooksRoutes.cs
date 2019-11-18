using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Logic
{
    public interface IWebhooksRoutes
    {
        Task<bool> TryToReplyToCrcChallenge(IWebhooksRequestHandler requestHandler, IConsumerCredentials credentials);
    }

    public class WebhooksRoutes : IWebhooksRoutes
    {
        private readonly IWebhooksHelper _webhooksHelper;

        public WebhooksRoutes(IWebhooksHelper webhooksHelper)
        {
            _webhooksHelper = webhooksHelper;
        }

        public async Task<bool> TryToReplyToCrcChallenge(IWebhooksRequestHandler requestHandler, IConsumerCredentials credentials)
        {
            var crcToken = requestHandler.GetQuery()["crc_token"];

            if (crcToken.IsNullOrEmpty())
            {
                return false;
            }

            await ReplyToCrcChallengeRequest(crcToken[0], requestHandler, credentials).ConfigureAwait(false);

            return true;

        }

        private async Task ReplyToCrcChallengeRequest(string crcToken, IWebhooksRequestHandler requestHandler, IConsumerCredentials credentials)
        {
            var crcResponseInfo = _webhooksHelper.CreateCrcResponseToken(crcToken, credentials.ConsumerSecret);

            await requestHandler.WriteInResponseAsync(crcResponseInfo.Json, crcResponseInfo.ContentType).ConfigureAwait(false);
        }
    }
}
