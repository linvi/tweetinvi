namespace Tweetinvi.Models
{
    public interface IConsumerOnlyCredentials : IConsumerCredentials
    {

    }

    /// <summary>
    /// Authentication tokens of a specific app
    /// </summary>
    public class ConsumerOnlyCredentials : ConsumerCredentials, IConsumerOnlyCredentials
    {
        public ConsumerOnlyCredentials(string consumerKey, string consumerSecret) : base(consumerKey, consumerSecret)
        {
        }

        public ConsumerOnlyCredentials(IReadOnlyTwitterCredentials creds) : base(creds)
        {
        }
    }
}
