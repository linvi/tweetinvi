namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-env-name-subscriptions
    /// </summary>
    public interface IIsAccountSubscribedToAccountActivityParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The environment for which we want to test the subscription
        /// </summary>
        string Environment { get; set; }
    }

    /// <inheritdoc/>
    public class IsAccountSubscribedToAccountActivityParameters : CustomRequestParameters, IIsAccountSubscribedToAccountActivityParameters
    {
        public IsAccountSubscribedToAccountActivityParameters(string environment)
        {
            Environment = environment;
        }

        /// <inheritdoc/>
        public string Environment { get; set; }
    }
}