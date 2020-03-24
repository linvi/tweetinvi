using Tweetinvi.Core.Helpers;

namespace Tweetinvi.Models
{
    public interface IReadOnlyConsumerCredentialsWithoutBearer
    {
        /// <summary>
        /// ConsumerKey identifying a unique application
        /// </summary>
        string ConsumerKey { get; }

        /// <summary>
        /// ConsumerSecret identifying a unique application
        /// </summary>
        string ConsumerSecret { get; }
    }

    public interface IReadOnlyConsumerCredentials : IReadOnlyConsumerCredentialsWithoutBearer
    {
        /// <summary>
        /// Bearer token used to make API requests on an application's own behalf.
        /// </summary>
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

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj?.GetHashCode();
        }

        public override int GetHashCode()
        {
            return CredentialsHashCodeGenerator.CreateHash(this).GetHashCode();
        }
    }
}