using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Webhooks.Plugin
{
    public interface IWebhookMessage
    {
        string Json { get; set; }
    }

    public class WebhookMessage : IWebhookMessage
    {
        public string Json { get; set; }
    }
}
