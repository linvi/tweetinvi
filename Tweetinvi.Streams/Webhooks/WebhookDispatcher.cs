using System.Collections.Generic;
using Tweetinvi.Core.Public.Streaming;
using Tweetinvi.Core.Public.Streaming.Webhooks;
using Tweetinvi.Models.Webhooks;

namespace Tweetinvi.Streams.Webhooks
{
    public class WebhookDispatcher : IWebhookDispatcher
    {
        private List<IAccountActivityStream> _userAccountActivityStream;

        public WebhookDispatcher()
        {
            _userAccountActivityStream = new List<IAccountActivityStream>();
        }

        public IAccountActivityStream[] SubscribedAccountActivityStreams { get; set; }

        public void WebhookMessageReceived(IWebhookMessage message)
        {
            _userAccountActivityStream.ForEach(activityStream =>
            {
                var isTargetingActivityStream = true;
                if (isTargetingActivityStream)
                {
                    activityStream.WebhookMessageReceived(message);
                }
            });
        }

        public void SubscribeAccountActivityStream(IAccountActivityStream accountActivityStream)
        {
            if (_userAccountActivityStream.Contains(accountActivityStream))
            {
                return;
            }

            _userAccountActivityStream.Add(accountActivityStream);
        }

        public void UnsubscribeAccountActivityStream(IAccountActivityStream accountActivityStream)
        {
            _userAccountActivityStream.Remove(accountActivityStream);
        }
    }
}
