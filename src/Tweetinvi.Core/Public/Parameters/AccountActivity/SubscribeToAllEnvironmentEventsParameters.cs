namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#post-account-activity-all-env-name-subscriptions
    /// </summary>
    public interface ISubscribeToAccountActivityParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The environment in which the webhook is registered
        /// </summary>
        string Environment { get; set; }
    }

    /// <inheritdoc/>
    public class SubscribeToAccountActivityParameters : CustomRequestParameters, ISubscribeToAccountActivityParameters
    {
        public SubscribeToAccountActivityParameters(string environment)
        {
            Environment = environment;
        }

        /// <inheritdoc/>
        public string Environment { get; set; }
    }
}