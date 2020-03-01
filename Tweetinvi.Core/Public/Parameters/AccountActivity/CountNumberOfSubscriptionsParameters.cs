namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-subscriptions-count
    /// </summary>
    public interface ICountAccountActivitySubscriptionsParameters : ICustomRequestParameters
    {
    }

    /// <inheritdoc/>
    public class CountAccountActivitySubscriptionsParameters : CustomRequestParameters, ICountAccountActivitySubscriptionsParameters
    {
    }
}