namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#delete-account-activity-all-env-name-subscriptions-user-id-json
    /// </summary>
    public interface IUnsubscribeFromAccountActivityParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The environment in which the webhook is registered
        /// </summary>
        string Environment { get; set; }

        /// <summary>
        /// The user id that we want to unsubscribe
        /// </summary>
        long UserId { get; set; }
    }

    /// <inheritdoc/>
    public class UnsubscribeFromAccountActivityParameters : CustomRequestParameters, IUnsubscribeFromAccountActivityParameters
    {
        public UnsubscribeFromAccountActivityParameters(string environment, long userId)
        {
            Environment = environment;
            UserId = userId;
        }

        /// <inheritdoc/>
        public string Environment { get; set; }
        /// <inheritdoc/>
        public long UserId { get; set; }
    }
}