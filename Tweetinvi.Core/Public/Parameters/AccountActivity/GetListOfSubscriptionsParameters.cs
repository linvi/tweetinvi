namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-env-name-subscriptions-list
    /// </summary>
    public interface IGetAccountActivitySubscriptionsParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The environment for which we want to test the subscription
        /// </summary>
        string Environment { get; set; }
    }

    public class GetAccountActivitySubscriptionsParameters : CustomRequestParameters, IGetAccountActivitySubscriptionsParameters
    {
        public GetAccountActivitySubscriptionsParameters(string environment)
        {
            Environment = environment;
        }

        public string Environment { get; set; }
    }
}