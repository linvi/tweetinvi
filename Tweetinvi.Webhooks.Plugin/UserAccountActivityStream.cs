using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Webhooks.Plugin
{
    public interface IAccountActivityStream
    {
        long UserId { get; }

        void WebhookMessageReceived(IWebhookMessage message);
    }

    public class AccountActivityStream : IAccountActivityStream
    {
        private readonly ITwitterCredentials _credentials;

        public AccountActivityStream(long userId)
        {
            UserId = userId;
        }

        public long UserId { get; private set; }
        public void WebhookMessageReceived(IWebhookMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}
