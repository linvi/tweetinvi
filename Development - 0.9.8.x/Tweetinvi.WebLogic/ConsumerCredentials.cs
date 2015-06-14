using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.WebLogic
{
    public class ConsumerCredentials : IConsumerCredentials
    {
        public ConsumerCredentials(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
    }
}