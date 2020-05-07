using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;

namespace Tweetinvi.AspNet.Core.Logic
{
    public interface IWebhooksRoutes
    {
        Task<bool> TryToReplyToCrcChallengeAsync(IWebhooksRequest request, IConsumerCredentials credentials);
    }

    public class WebhooksRoutes : IWebhooksRoutes
    {
        private readonly IWebhooksHelper _webhooksHelper;

        public WebhooksRoutes(IWebhooksHelper webhooksHelper)
        {
            _webhooksHelper = webhooksHelper;
        }

        public async Task<bool> TryToReplyToCrcChallengeAsync(IWebhooksRequest request, IConsumerCredentials credentials)
        {
            var crcToken = request.GetQuery()["crc_token"];

            if (crcToken.IsNullOrEmpty())
            {
                return false;
            }

            await ReplyToCrcChallengeRequestAsync(crcToken[0], request, credentials).ConfigureAwait(false);

            return true;

        }

        private async Task ReplyToCrcChallengeRequestAsync(string crcToken, IWebhooksRequest request, IConsumerCredentials credentials)
        {
            var crcResponseInfo = _webhooksHelper.CreateCrcResponseToken(crcToken, credentials.ConsumerSecret);

            await request.WriteInResponseAsync(crcResponseInfo.Json, crcResponseInfo.ContentType).ConfigureAwait(false);
        }
    }
}
