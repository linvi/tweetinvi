namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#put-account-activity-all-env-name-webhooks-webhook-id
    /// </summary>
    public interface ITriggerAccountActivityWebhookCRCParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The environment in which the webhook is registered
        /// </summary>
        string Environment { get; set; }

        /// <summary>
        /// The webhook identifier
        /// </summary>
        string WebhookId { get; set; }
    }

    /// <inheritdoc/>
    public class TriggerAccountActivityWebhookCRCParameters : CustomRequestParameters, ITriggerAccountActivityWebhookCRCParameters
    {
        public TriggerAccountActivityWebhookCRCParameters(string environment, string webhookId)
        {
            Environment = environment;
            WebhookId = webhookId;
        }

        /// <inheritdoc/>
        public string Environment { get; set; }
        /// <inheritdoc/>
        public string WebhookId { get; set; }
    }
}