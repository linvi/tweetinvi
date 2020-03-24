namespace Tweetinvi.Models
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