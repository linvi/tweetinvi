namespace Tweetinvi.Core.Authentication
{
    /// <summary>
    /// Defines a contract of 4 information to connect to an OAuth service
    /// </summary>
    public interface ITwitterCredentials : IConsumerCredentials
    {
        /// <summary>
        /// Key provided to the consumer to provide an authentication of the client
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        /// Secret Key provided to the consumer to provide an authentication of the client
        /// </summary>
        string AccessTokenSecret { get; set; }

        /// <summary>
        /// Clone the current credentials.
        /// </summary>
        new ITwitterCredentials Clone();

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

        public TwitterCredentials(IConsumerCredentials credentials) : base("", "")
        {
            if (credentials != null)
            {
                ConsumerKey = credentials.ConsumerKey;
                ConsumerSecret = credentials.ConsumerSecret;

                ApplicationOnlyBearerToken = credentials.ApplicationOnlyBearerToken;
            }
        }

        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }

        public new ITwitterCredentials Clone()
        {
            var clone = new TwitterCredentials(ConsumerKey, ConsumerSecret, AccessToken, AccessTokenSecret);

            CopyPropertiesToClone(clone);

            return clone;
        }

        public bool AreSetupForUserAuthentication()
        {
            return AreSetupForApplicationAuthentication() &&
                   !string.IsNullOrEmpty(AccessToken) &&
                   !string.IsNullOrEmpty(AccessTokenSecret);
        }
    }
}