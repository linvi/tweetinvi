namespace Tweetinvi.Core.Credentials
{
    public interface IAuthenticationToken
    {
        IConsumerCredentials ConsumerCredentials { get; set; }

        /// <summary>
        /// Key identifying a specific consumer application
        /// </summary>
        string ConsumerKey { get; }

        /// <summary>
        /// Secret Key identifying a specific consumer application
        /// </summary>
        string ConsumerSecret { get; }

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
    }

    public class AuthenticationToken : IAuthenticationToken
    {
        public IConsumerCredentials ConsumerCredentials { get; set; }
        public string ConsumerKey { get { return ConsumerCredentials.ConsumerKey; } }
        public string ConsumerSecret { get { return ConsumerCredentials.ConsumerSecret; } }
        public string AuthorizationKey { get; set; }
        public string AuthorizationSecret { get; set; }
        
        public string VerifierCode { get; set; }
    }
}