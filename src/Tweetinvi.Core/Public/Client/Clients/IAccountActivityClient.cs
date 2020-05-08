using System.Threading.Tasks;
using Tweetinvi.Models;
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

        /// <inheritdoc cref="IAccountActivityClient.CreateAccountActivityWebhookAsync(ICreateAccountActivityWebhookParameters)" />
        Task<IWebhook> CreateAccountActivityWebhookAsync(string environment, string webhookUrl);

        /// <summary>
        /// Registers a webhook URL for all event types. The URL will be validated via CRC request before saving.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#post-account-activity-all-env-name-webhooks </para>
        /// </summary>
        /// <returns>The created webhook</returns>
        Task<IWebhook> CreateAccountActivityWebhookAsync(ICreateAccountActivityWebhookParameters parameters);

        /// <inheritdoc cref="IAccountActivityClient.GetAccountActivityWebhookEnvironmentsAsync(IGetAccountActivityWebhookEnvironmentsParameters)" />
        Task<IWebhookEnvironment[]> GetAccountActivityWebhookEnvironmentsAsync();

        /// <summary>
        /// Get the account activity webhook environments
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-webhooks </para>
        /// </summary>
        /// <returns>The account activity environments and their associated webhooks</returns>
        Task<IWebhookEnvironment[]> GetAccountActivityWebhookEnvironmentsAsync(IGetAccountActivityWebhookEnvironmentsParameters parameters);

        /// <inheritdoc cref="IAccountActivityClient.GetAccountActivityEnvironmentWebhooksAsync(IGetAccountActivityEnvironmentWebhooksParameters)" />
        Task<IWebhook[]> GetAccountActivityEnvironmentWebhooksAsync(string environment);

        /// <summary>
        /// Returns the webhooks registered on a specific environment
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-webhooks </para>
        /// </summary>
        /// <returns>The account activity registered webhooks of a specific environment</returns>
        Task<IWebhook[]> GetAccountActivityEnvironmentWebhooksAsync(IGetAccountActivityEnvironmentWebhooksParameters parameters);

        /// <inheritdoc cref="IAccountActivityClient.DeleteAccountActivityWebhookAsync(IDeleteAccountActivityWebhookParameters)" />
        Task DeleteAccountActivityWebhookAsync(string environment, string webhookId);

        /// <summary>
        /// Remove the specified account activity webhook
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#delete-account-activity-all-env-name-webhooks-webhook-id </para>
        /// </summary>
        Task DeleteAccountActivityWebhookAsync(IDeleteAccountActivityWebhookParameters parameters);

        /// <inheritdoc cref="IAccountActivityClient.TriggerAccountActivityWebhookCRCAsync(ITriggerAccountActivityWebhookCRCParameters)" />
        Task TriggerAccountActivityWebhookCRCAsync(string environment, string webhookId);

        /// <summary>
        /// Challenges a webhook and reenable it when it was disabled
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#put-account-activity-all-env-name-webhooks-webhook-id </para>
        /// </summary>
        Task TriggerAccountActivityWebhookCRCAsync(ITriggerAccountActivityWebhookCRCParameters parameters);

        /// <inheritdoc cref="IAccountActivityClient.SubscribeToAccountActivityAsync(ISubscribeToAccountActivityParameters)" />
        Task SubscribeToAccountActivityAsync(string environment);

        /// <summary>
        /// Subscribes the provided application to all events for the provided environment for all message types. After activation, all events for the requesting user will be sent to the application’s webhook via POST request.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#post-account-activity-all-env-name-subscriptions </para>
        /// </summary>
        Task SubscribeToAccountActivityAsync(ISubscribeToAccountActivityParameters parameters);

        /// <inheritdoc cref="IAccountActivityClient.CountAccountActivitySubscriptionsAsync(ICountAccountActivitySubscriptionsParameters)" />
        Task<IWebhookSubscriptionsCount> CountAccountActivitySubscriptionsAsync();

        /// <summary>
        /// Returns the count of subscriptions that are currently active on your account for all activities.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-subscriptions-count </para>
        /// </summary>
        /// <returns>Count information</returns>
        Task<IWebhookSubscriptionsCount> CountAccountActivitySubscriptionsAsync(ICountAccountActivitySubscriptionsParameters parameters);

        /// <inheritdoc cref="IAccountActivityClient.IsAccountSubscribedToAccountActivityAsync(IIsAccountSubscribedToAccountActivityParameters)" />
        Task<bool> IsAccountSubscribedToAccountActivityAsync(string environment);

        /// <summary>
        /// Check if an account is subscribed to the webhooks
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-env-name-subscriptions </para>
        /// </summary>
        /// <returns>Whether the account is subscribed to the account activity environment</returns>
        Task<bool> IsAccountSubscribedToAccountActivityAsync(IIsAccountSubscribedToAccountActivityParameters parameters);

        /// <inheritdoc cref="IAccountActivityClient.GetAccountActivitySubscriptionsAsync(IGetAccountActivitySubscriptionsParameters)" />
        Task<IWebhookEnvironmentSubscriptions> GetAccountActivitySubscriptionsAsync(string environment);

        /// <summary>
        /// Get the account activity subscriptions
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-env-name-subscriptions-list </para>
        /// </summary>
        /// <returns>User subscriptions to account activities</returns>
        Task<IWebhookEnvironmentSubscriptions> GetAccountActivitySubscriptionsAsync(IGetAccountActivitySubscriptionsParameters parameters);

        /// <inheritdoc cref="IAccountActivityClient.UnsubscribeFromAccountActivityAsync(IUnsubscribeFromAccountActivityParameters)" />
        Task UnsubscribeFromAccountActivityAsync(string environment, long userId);

        /// <summary>
        /// Unsubscribe a user from account activity
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#delete-account-activity-all-env-name-subscriptions-user-id-json </para>
        /// </summary>
        Task UnsubscribeFromAccountActivityAsync(IUnsubscribeFromAccountActivityParameters parameters);
    }
}