namespace Tweetinvi.Models
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

        /// <summary>
        /// Property used by Tweetinvi or yourself to track the IAuthenticationContext
        /// when the callback url is received.
        /// </summary>
        string AuthorizationUniqueIdentifier { get; set; }
    }
}