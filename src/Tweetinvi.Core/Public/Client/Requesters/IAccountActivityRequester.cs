using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IAccountActivityRequester
    {
        /// <summary>
        /// Registers a webhook URL for all event types. The URL will be validated via CRC request before saving.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#post-account-activity-all-env-name-webhooks </para>
        /// </summary>
        /// <returns>Twitter Result containing the created webhook</returns>
        Task<ITwitterResult<IWebhookDTO>> CreateAccountActivityWebhookAsync(ICreateAccountActivityWebhookParameters parameters);

        /// <summary>
        /// Get the account activity webhook environments
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-webhooks </para>
        /// </summary>
        /// <returns>Twitter result containing the account activity environments</returns>
        Task<ITwitterResult<IGetAccountActivityWebhookEnvironmentsResultDTO>> GetAccountActivityWebhookEnvironmentsAsync(IGetAccountActivityWebhookEnvironmentsParameters parameters);

        /// <summary>
        /// Returns the webhooks registered on a specific environments
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-webhooks </para>
        /// </summary>
        /// <returns>Twitter result containing the account activity registered webhooks</returns>
        Task<ITwitterResult<IWebhookDTO[]>> GetAccountActivityEnvironmentWebhooksAsync(IGetAccountActivityEnvironmentWebhooksParameters parameters);

        /// <summary>
        /// Remove the specified account activity webhook
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#delete-account-activity-all-env-name-webhooks-webhook-id </para>
        /// </summary>
        Task<ITwitterResult> DeleteAccountActivityWebhookAsync(IDeleteAccountActivityWebhookParameters parameters);

        /// <summary>
        /// Challenges a webhook and reenable it when it was disabled
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#put-account-activity-all-env-name-webhooks-webhook-id </para>
        /// </summary>
        Task<ITwitterResult> TriggerAccountActivityWebhookCRCAsync(ITriggerAccountActivityWebhookCRCParameters parameters);

        /// <summary>
        /// Subscribes the provided application to all events for the provided environment for all message types. After activation, all events for the requesting user will be sent to the application’s webhook via POST request.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#post-account-activity-all-env-name-subscriptions </para>
        /// </summary>
        Task<ITwitterResult> SubscribeToAccountActivityAsync(ISubscribeToAccountActivityParameters parameters);

        /// <summary>
        /// Returns the count of subscriptions that are currently active on your account for all activities.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-subscriptions-count </para>
        /// </summary>
        /// <returns>Twitter result containing the count information</returns>
        Task<ITwitterResult<IWebhookSubscriptionsCount>> CountAccountActivitySubscriptionsAsync(ICountAccountActivitySubscriptionsParameters parameters);

        /// <summary>
        /// Check if an account is subscribed to the webhooks
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-env-name-subscriptions </para>
        /// </summary>
        Task<ITwitterResult> IsAccountSubscribedToAccountActivityAsync(IIsAccountSubscribedToAccountActivityParameters parameters);

        /// <summary>
        /// Get the account activity subscriptions
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-env-name-subscriptions-list </para>
        /// </summary>
        /// <returns>Twitter result containing the user subscriptions to account activities</returns>
        Task<ITwitterResult<IWebhookEnvironmentSubscriptionsDTO>> GetAccountActivitySubscriptionsAsync(IGetAccountActivitySubscriptionsParameters parameters);

        /// <summary>
        /// Unsubscribe a user from account activity
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#delete-account-activity-all-env-name-subscriptions-user-id-json </para>
        /// </summary>
        Task<ITwitterResult> UnsubscribeFromAccountActivityAsync(IUnsubscribeFromAccountActivityParameters parameters);
    }
}