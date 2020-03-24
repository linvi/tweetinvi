namespace Tweetinvi.Models
{
    public interface IWebhookMessage
    {
        string Json { get; }
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
