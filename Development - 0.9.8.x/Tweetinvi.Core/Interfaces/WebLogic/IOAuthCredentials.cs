namespace Tweetinvi.Core.Interfaces.WebLogic
{
    /// <summary>
    /// Defines a contract of 4 information to connect to an OAuth service
    /// </summary>
    public interface IOAuthCredentials : IConsumerCredentials
    {
        /// <summary>
        /// Key provided to the consumer to provide an authentication of the client
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        /// Secret Key provided to the consumer to provide an authentication of the client
        /// </summary>
        string AccessTokenSecret { get; set; }
    }
}