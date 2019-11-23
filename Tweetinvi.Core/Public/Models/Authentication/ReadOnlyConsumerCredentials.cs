namespace Tweetinvi.Models
{
    public interface IReadOnlyConsumerCredentials
    {
        string ConsumerKey { get; }
        string ConsumerSecret { get; }
        string BearerToken { get; }
    }

    public class ReadOnlyConsumerCredentials : IReadOnlyConsumerCredentials
    {
        public ReadOnlyConsumerCredentials(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public ReadOnlyConsumerCredentials(string consumerKey, string consumerSecret, string bearerToken)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            BearerToken = bearerToken;
        }

        public ReadOnlyConsumerCredentials(IReadOnlyConsumerCredentials source)
        {
            ConsumerKey = source?.ConsumerKey;
            ConsumerSecret = source?.ConsumerSecret;
            BearerToken = source?.BearerToken;
        }

        public string ConsumerKey { get; }
        public string ConsumerSecret { get; }
        public string BearerToken { get; }
    }
}