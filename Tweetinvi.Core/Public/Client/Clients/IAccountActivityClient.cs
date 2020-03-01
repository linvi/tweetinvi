using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    /// <summary>
    /// A client providing all the actions related with the account activity api
    /// </summary>
    public interface IAccountActivityClient
    {
        /// <summary>
        /// Creates an AccountActivity request handler that will properly route requests
        /// </summary>
        /// <returns>AccountActivity Request Handler</returns>
        IAccountActivityRequestHandler CreateRequestHandler();

        /// <inheritdoc cref="CreateAccountActivityWebhook(ICreateAccountActivityWebhookParameters)" />
        Task<IWebhookDTO> CreateAccountActivityWebhook(string environment, string webhookUrl);

        /// <summary>
        /// Registers a webhook URL for all event types. The URL will be validated via CRC request before saving.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#post-account-activity-all-env-name-webhooks </para>
        /// </summary>
        /// <returns>The created webhook</returns>
        Task<IWebhookDTO> CreateAccountActivityWebhook(ICreateAccountActivityWebhookParameters parameters);

        /// <inheritdoc cref="GetAccountActivityWebhookEnvironments(IGetAccountActivityWebhookEnvironmentsParameters)" />
        Task<IWebhookEnvironmentDTO[]> GetAccountActivityWebhookEnvironments();

        /// <summary>
        /// Get the account activity webhook environments
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-webhooks </para>
        /// </summary>
        /// <returns>The account activity environments and their associated webhooks</returns>
        Task<IWebhookEnvironmentDTO[]> GetAccountActivityWebhookEnvironments(IGetAccountActivityWebhookEnvironmentsParameters parameters);

        /// <inheritdoc cref="GetAccountActivityEnvironmentWebhooks(IGetAccountActivityEnvironmentWebhooksParameters)" />
        Task<IWebhookDTO[]> GetAccountActivityEnvironmentWebhooks(string environment);

        /// <summary>
        /// Returns the webhooks registered on a specific environment
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-webhooks </para>
        /// </summary>
        /// <returns>The account activity registered webhooks of a specific environment</returns>
        Task<IWebhookDTO[]> GetAccountActivityEnvironmentWebhooks(IGetAccountActivityEnvironmentWebhooksParameters parameters);

        /// <inheritdoc cref="DeleteAccountActivityWebhook(IDeleteAccountActivityWebhookParameters)" />
        Task DeleteAccountActivityWebhook(string environment, string webhookId);

        /// <summary>
        /// Remove the specified account activity webhook
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#delete-account-activity-all-env-name-webhooks-webhook-id </para>
        /// </summary>
        Task DeleteAccountActivityWebhook(IDeleteAccountActivityWebhookParameters parameters);

        /// <inheritdoc cref="TriggerAccountActivityWebhookCRC(ITriggerAccountActivityWebhookCRCParameters)" />
        Task TriggerAccountActivityWebhookCRC(string environment, string webhookId);

        /// <summary>
        /// Challenges a webhook and reenable it when it was disabled
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#put-account-activity-all-env-name-webhooks-webhook-id </para>
        /// </summary>
        Task TriggerAccountActivityWebhookCRC(ITriggerAccountActivityWebhookCRCParameters parameters);

        /// <inheritdoc cref="SubscribeToAccountActivity(ISubscribeToAccountActivityParameters)" />
        Task SubscribeToAccountActivity(string environment);

        /// <summary>
        /// Subscribes the provided application to all events for the provided environment for all message types. After activation, all events for the requesting user will be sent to the application’s webhook via POST request.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#post-account-activity-all-env-name-subscriptions </para>
        /// </summary>
        Task SubscribeToAccountActivity(ISubscribeToAccountActivityParameters parameters);

        /// <inheritdoc cref="CountAccountActivitySubscriptions(ICountAccountActivitySubscriptionsParameters)" />
        Task<IGetWebhookSubscriptionsCountResultDTO> CountAccountActivitySubscriptions();

        /// <summary>
        /// Returns the count of subscriptions that are currently active on your account for all activities.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-subscriptions-count </para>
        /// </summary>
        /// <returns>Count information</returns>
        Task<IGetWebhookSubscriptionsCountResultDTO> CountAccountActivitySubscriptions(ICountAccountActivitySubscriptionsParameters parameters);

        /// <inheritdoc cref="IsAccountSubscribedToAccountActivity(IIsAccountSubscribedToAccountActivityParameters)" />
        Task<bool> IsAccountSubscribedToAccountActivity(string environment);

        /// <summary>
        /// Check if an account is subscribed to the webhooks
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-env-name-subscriptions </para>
        /// </summary>
        /// <returns>Whether the account is subscribed to the account activity environment</returns>
        Task<bool> IsAccountSubscribedToAccountActivity(IIsAccountSubscribedToAccountActivityParameters parameters);

        /// <inheritdoc cref="GetAccountActivitySubscriptions(IGetAccountActivitySubscriptionsParameters)" />
        Task<IWebhookSubscriptionListDTO> GetAccountActivitySubscriptions(string environment);

        /// <summary>
        /// Get the account activity subscriptions
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-env-name-subscriptions-list </para>
        /// </summary>
        /// <returns>User subscriptions to account activities</returns>
        Task<IWebhookSubscriptionListDTO> GetAccountActivitySubscriptions(IGetAccountActivitySubscriptionsParameters parameters);

        /// <inheritdoc cref="UnsubscribeFromAccountActivity(IUnsubscribeFromAccountActivityParameters)" />
        Task UnsubscribeFromAccountActivity(string environment, long? userId);

        /// <summary>
        /// Unsubscribe a user from account activity
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#delete-account-activity-all-env-name-subscriptions-user-id-json </para>
        /// </summary>
        Task UnsubscribeFromAccountActivity(IUnsubscribeFromAccountActivityParameters parameters);
    }
}