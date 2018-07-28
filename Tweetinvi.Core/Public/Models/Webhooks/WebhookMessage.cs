namespace Tweetinvi.Models.Webhooks
{
    public interface IWebhookMessage
    {
        string Json { get; set; }
    }

    public class WebhookMessage : IWebhookMessage
    {
        public WebhookMessage(string json)
        {
            Json = json;
        }

        public string Json { get; set; }
    }
}
