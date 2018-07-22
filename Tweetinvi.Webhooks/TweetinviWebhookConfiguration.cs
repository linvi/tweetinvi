using Tweetinvi.Models;

namespace Examplinvi.ASP.NET.Core
{
    public class TweetinviWebhookConfiguration
    {
        private readonly IConsumerCredentials _consumerCredentials;

        public TweetinviWebhookConfiguration()
        {
            BasePath = "/tweetinvi-webhooks";
        }

        public TweetinviWebhookConfiguration(IConsumerCredentials consumerCredentials) : this()
        {
            _consumerCredentials = consumerCredentials;
        }

        public string AppUrl { get; set; }
        public string BasePath { get; set; }
        public string EnvName { get; set; }
        public IConsumerCredentials ConsumerCredentials { get; set; }
    }
}
