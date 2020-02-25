namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-webhooks
    /// </summary>
    public interface IGetAccountActivityWebhookEnvironmentsParameters : ICustomRequestParameters
    {
    }

    /// <inheritdoc/>
    public class GetAccountActivityWebhookEnvironmentsParameters : CustomRequestParameters, IGetAccountActivityWebhookEnvironmentsParameters
    {
    }
}