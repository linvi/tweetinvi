namespace Tweetinvi.Models
{
    /// <summary>
    /// Defines a contract of 4 information to connect to an OAuth service
    /// </summary>
    public interface ITwitterCredentials : IReadOnlyTwitterCredentials
    {
        /// <summary>
        /// Key provided to the consumer to provide an authentication of the client
        /// </summary>
        new string AccessToken { get; set; }

        /// <summary>
        /// Secret Key provided to the consumer to provide an authentication of the client
        /// </summary>
        new string AccessTokenSecret { get; set; }

        /// <summary>
        /// Are credentials correctly set up for user authentication.
        /// </summary>
        bool AreSetupForUserAuthentication();
    }

    /// <summary>
    /// This class provides host basic information for authorizing a OAuth
    /// consumer to connect to a service. It does not contain any logic
    /// </summary>
    public class TwitterCredentials : ConsumerCredentials, ITwitterCredentials
    {
        public TwitterCredentials() : base(null, null) { }

        public TwitterCredentials(string consumerKey, string consumerSecret)
            : base(consumerKey, consumerSecret)
        {
        }

        public TwitterCredentials(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
            : this(consumerKey, consumerSecret)
        {
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
        }

        public TwitterCredentials(IReadOnlyTwitterCredentials source) : base(source)
        {
            if (source == null)
            {
                return;
            }

            AccessToken = source.AccessToken;
            AccessTokenSecret = source.AccessTokenSecret;
            BearerToken = source.BearerToken;
        }

        public TwitterCredentials(IReadOnlyConsumerCredentials credentials) : base(credentials)
        {
        }

        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }

        public bool AreSetupForUserAuthentication()
        {
            return AreSetupForApplicationAuthentication() &&
                   !string.IsNullOrEmpty(AccessToken) &&
                   !string.IsNullOrEmpty(AccessTokenSecret);
        }
    }
}