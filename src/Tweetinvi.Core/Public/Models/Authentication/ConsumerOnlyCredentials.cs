using Tweetinvi.Core.Models.Authentication;

namespace Tweetinvi.Models
{
    public interface IConsumerOnlyCredentials : IReadOnlyConsumerCredentials
    {
        /// <summary>
        /// Key identifying a specific consumer application
        /// </summary>
        new string ConsumerKey { get; set; }

        /// <summary>
        /// Secret Key identifying a specific consumer application
        /// </summary>
        new string ConsumerSecret { get; set; }

        /// <summary>
        /// Token required for Application Only Authentication
        /// </summary>
        new string BearerToken { get; set; }
    }

    /// <summary>
    /// Authentication tokens of a specific app
    /// </summary>
    public class ConsumerOnlyCredentials : IConsumerOnlyCredentials
    {
        public ConsumerOnlyCredentials()
        {
        }

        public ConsumerOnlyCredentials(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public ConsumerOnlyCredentials(string consumerKey, string consumerSecret, string bearerToken)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            BearerToken = bearerToken;
        }

        public ConsumerOnlyCredentials(IReadOnlyConsumerCredentials creds)
        {
            ConsumerKey = creds?.ConsumerKey;
            ConsumerSecret = creds?.ConsumerSecret;
            BearerToken = creds?.BearerToken;
        }

        /// <inheritdoc cref="ConsumerOnlyCredentials.ConsumerKey" />
        public string ConsumerKey { get; set; }
        /// <inheritdoc cref="ConsumerOnlyCredentials.ConsumerSecret" />
        public string ConsumerSecret { get; set; }
        /// <inheritdoc cref="ConsumerOnlyCredentials.BearerToken" />
        public string BearerToken { get; set; }
    }
}
