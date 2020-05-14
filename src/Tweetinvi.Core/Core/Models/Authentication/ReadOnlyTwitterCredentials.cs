namespace Tweetinvi.Core.Models.Authentication
{
    public interface IReadOnlyTwitterCredentials : IReadOnlyConsumerCredentials
    {
        /// <summary>
        /// AccessToken granting access to user's specific account
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// AccessTokenSecret granting access to user's specific account
        /// </summary>
        string AccessTokenSecret { get; }
    }

    /// <summary>
    /// Readonly version of twitter credentials
    /// </summary>
    public class ReadOnlyTwitterCredentials : ReadOnlyConsumerCredentials, IReadOnlyTwitterCredentials
    {
        public ReadOnlyTwitterCredentials(string consumerKey, string consumerSecret) : base(new ReadOnlyConsumerCredentials(consumerKey, consumerSecret))
        {
        }

        public ReadOnlyTwitterCredentials(string consumerKey, string consumerSecret, string bearerToken) : base(new ReadOnlyConsumerCredentials(consumerKey, consumerSecret, bearerToken))
        {
        }

        public ReadOnlyTwitterCredentials(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret) : base(new ReadOnlyConsumerCredentials(consumerKey, consumerSecret))
        {
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
        }

        public ReadOnlyTwitterCredentials(IReadOnlyConsumerCredentials consumerCredentials, string accessToken, string accessTokenSecret) : base(consumerCredentials)
        {
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
        }

        public ReadOnlyTwitterCredentials(IReadOnlyConsumerCredentials source) : base(source)
        {
        }

        public ReadOnlyTwitterCredentials(IReadOnlyTwitterCredentials source) : base(source)
        {
            AccessToken = source?.AccessToken;
            AccessTokenSecret = source?.AccessTokenSecret;
        }

        /// <inheritdoc cref="IReadOnlyTwitterCredentials.AccessToken" />
        public string AccessToken { get; }

        /// <inheritdoc cref="IReadOnlyTwitterCredentials.AccessTokenSecret" />
        public string AccessTokenSecret { get; }
    }
}