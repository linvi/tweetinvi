namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-webhooks
    /// </summary>
    public interface IGetAccountActivityEnvironmentWebhooksParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The environment for which we want to get the list of webhooks
        /// </summary>
        string Environment { get; set; }
    }

    /// <inheritdoc/>
    public class GetAccountActivityEnvironmentWebhooksParameters : CustomRequestParameters, IGetAccountActivityEnvironmentWebhooksParameters
    {
        public GetAccountActivityEnvironmentWebhooksParameters(string environment)
        {
            Environment = environment;
        }

        /// <inheritdoc/>
        public string Environment { get; set; }
    }
}