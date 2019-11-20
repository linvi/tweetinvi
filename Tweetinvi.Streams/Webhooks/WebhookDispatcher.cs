using System.Collections.Generic;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Models.Webhooks;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.Webhooks;

namespace Tweetinvi.Streams.Webhooks
{
    public class WebhookDispatcher : IWebhookDispatcher
    {
        private readonly IJObjectStaticWrapper _jObjectStaticWrapper;
        private readonly List<IAccountActivityStream> _accountActivityStream;

        public WebhookDispatcher(IJObjectStaticWrapper jObjectStaticWrapper)
        {
            _jObjectStaticWrapper = jObjectStaticWrapper;
            _accountActivityStream = new List<IAccountActivityStream>();
        }

        public IAccountActivityStream[] SubscribedAccountActivityStreams
        {
            get { return _accountActivityStream.ToArray(); }
        }

        public void WebhookMessageReceived(IWebhookMessage message)
        {
            var jsonObjectEvent = _jObjectStaticWrapper.GetJobjectFromJson(message.Json);

            var userId = jsonObjectEvent["for_user_id"].ToString();

            _accountActivityStream.ForEach(activityStream =>
            {
                var isTargetingActivityStream = activityStream.AccountUserId.ToString() == userId;
                if (isTargetingActivityStream)
                {
                    activityStream.WebhookMessageReceived(message);
                }
            });
        }

        public void SubscribeAccountActivityStream(IAccountActivityStream accountActivityStream)
        {
            if (_accountActivityStream.Contains(accountActivityStream))
            {
                return;
            }

            _accountActivityStream.Add(accountActivityStream);
        }

        public void UnsubscribeAccountActivityStream(IAccountActivityStream accountActivityStream)
        {
            _accountActivityStream.Remove(accountActivityStream);
        }
    }
}
