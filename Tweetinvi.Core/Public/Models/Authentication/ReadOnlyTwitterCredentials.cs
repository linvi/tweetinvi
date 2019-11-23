namespace Tweetinvi.Models
{
    public interface IReadOnlyTwitterCredentials : IReadOnlyConsumerCredentials
    {
        /// <summary>
        /// Key provided to the consumer to provide an authentication of the client
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// Secret Key provided to the consumer to provide an authentication of the client
        /// </summary>
        string AccessTokenSecret { get; }
    }

    public class ReadOnlyTwitterCredentials : ReadOnlyConsumerCredentials, IReadOnlyTwitterCredentials
    {
        public ReadOnlyTwitterCredentials(string accessToken, string accessTokenSecret, IReadOnlyConsumerCredentials consumerCredentials) : base(consumerCredentials)
        {
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
        }

        public ReadOnlyTwitterCredentials(IReadOnlyTwitterCredentials source) : base(source)
        {
            AccessToken = source?.AccessToken;
            AccessTokenSecret = source?.AccessTokenSecret;
        }

        public string AccessToken { get; }
        public string AccessTokenSecret { get; }
    }
}