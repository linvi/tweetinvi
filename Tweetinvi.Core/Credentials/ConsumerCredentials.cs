namespace Tweetinvi.Core.Credentials
{
    public interface IConsumerCredentials
    {
        /// <summary>
        /// Key identifying a specific consumer application
        /// </summary>
        string ConsumerKey { get; set; }

        /// <summary>
        /// Secret Key identifying a specific consumer application
        /// </summary>
        string ConsumerSecret { get; set; }

        /// <summary>
        /// Token required for Application Only Authentication
        /// </summary>
        string ApplicationOnlyBearerToken { get; set; }

        /// <summary>
        /// Key required for user authentication.
        /// This key needs to be saved when using url redirect authenthentication
        /// </summary>
        string AuthorizationKey { get; set; }

        /// <summary>
        /// Key required for user authentication
        /// This key needs to be saved when using url redirect authenthentication
        /// </summary>
        string AuthorizationSecret { get; set; }

        /// <summary>
        /// Verification information received when a user accepts an application to use its account.
        /// If this value is changed manually it will overridden by Tweetinvi.
        /// </summary>
        string VerifierCode { get; set; }

        IConsumerCredentials Clone();

        bool AreSetupForApplicationAuthentication();
    }

    public class ConsumerCredentials : IConsumerCredentials
    {
        public ConsumerCredentials(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }

        public string ApplicationOnlyBearerToken { get; set; }
        
        public string AuthorizationKey { get; set; }
        public string AuthorizationSecret { get; set; }
        public string VerifierCode { get; set; }

        public IConsumerCredentials Clone()
        {
            var clone = new ConsumerCredentials(ConsumerKey, ConsumerSecret);

            CopyPropertiesToClone(clone);

            return clone;
        }

        public bool AreSetupForApplicationAuthentication()
        {
            return !string.IsNullOrEmpty(ConsumerKey) &&
                   !string.IsNullOrEmpty(ConsumerSecret);
        }

        protected void CopyPropertiesToClone(IConsumerCredentials clone)
        {
            clone.ApplicationOnlyBearerToken = ApplicationOnlyBearerToken;
            clone.AuthorizationKey = AuthorizationKey;
            clone.AuthorizationSecret = AuthorizationSecret;
            clone.VerifierCode = VerifierCode;
        }
    }
}