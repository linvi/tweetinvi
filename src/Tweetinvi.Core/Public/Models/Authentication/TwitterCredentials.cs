using Tweetinvi.Core.Helpers;

namespace Tweetinvi.Models
{
    /// <summary>
    /// Defines a contract of 4 information to connect to an OAuth service
    /// </summary>
    public interface ITwitterCredentials : IReadOnlyTwitterCredentials
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

        /// <summary>
        /// Key provided to the consumer to provide an authentication of the client
        /// </summary>
        new string AccessToken { get; set; }

        /// <summary>
        /// Secret Key provided to the consumer to provide an authentication of the client
        /// </summary>
        new string AccessTokenSecret { get; set; }
    }

    /// <summary>
    /// This class provides host basic information for authorizing a OAuth
    /// consumer to connect to a service. It does not contain any logic
    /// </summary>
    public class TwitterCredentials : ITwitterCredentials
    {
        public TwitterCredentials() { }

        /// <param name="consumerKey">Your application consumer key</param>
        /// <param name="consumerSecret">Your application consumer secret</param>
        public TwitterCredentials(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        /// <param name="consumerKey">Your application consumer key</param>
        /// <param name="consumerSecret">Your application consumer secret</param>
        /// <param name="bearerToken">Your application Bearer Token</param>
        public TwitterCredentials(string consumerKey, string consumerSecret, string bearerToken)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            BearerToken = bearerToken;
        }

        /// <param name="consumerKey">Your application consumer key</param>
        /// <param name="consumerSecret">Your application consumer secret</param>
        /// <param name="accessToken">The user token</param>
        /// <param name="accessTokenSecret">The user token secret</param>
        public TwitterCredentials(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
        }

        public TwitterCredentials(IReadOnlyTwitterCredentials source)
        {
            if (source == null)
            {
                return;
            }

            ConsumerKey = source.ConsumerKey;
            ConsumerSecret = source.ConsumerSecret;
            BearerToken = source.BearerToken;
            AccessToken = source.AccessToken;
            AccessTokenSecret = source.AccessTokenSecret;
            BearerToken = source.BearerToken;

        }

        public TwitterCredentials(IReadOnlyConsumerCredentials source)
        {
            if (source == null)
            {
                return;
            }

            ConsumerKey = source.ConsumerKey;
            ConsumerSecret = source.ConsumerSecret;
            BearerToken = source.BearerToken;
        }

        /// <inheritdoc cref="ITwitterCredentials.ConsumerKey" />
        public string ConsumerKey { get; set; }

        /// <inheritdoc cref="ITwitterCredentials.ConsumerSecret" />
        public string ConsumerSecret { get; set; }

        /// <inheritdoc cref="ITwitterCredentials.BearerToken" />
        public string BearerToken { get; set; }

        /// <inheritdoc cref="ITwitterCredentials.AccessToken" />
        public string AccessToken { get; set; }

        /// <inheritdoc cref="ITwitterCredentials.AccessTokenSecret" />
        public string AccessTokenSecret { get; set; }

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