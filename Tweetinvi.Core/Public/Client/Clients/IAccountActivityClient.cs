using System.Threading.Tasks;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    /// <summary>
    /// A client providing all the actions related with the account activity api
    /// </summary>
    public interface IAccountActivityClient
    {
        /// <inheritdoc cref="RegisterAccountActivityWebhook(IRegisterAccountActivityWebhookParameters)" />
        Task<IWebhookDTO> RegisterAccountActivityWebhook(string environment, string webhookUrl);

        /// <summary>
        /// Registers a webhook URL for all event types. The URL will be validated via CRC request before saving.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#post-account-activity-all-env-name-webhooks </para>
        /// </summary>
        /// <returns>The created webhook</returns>
        Task<IWebhookDTO> RegisterAccountActivityWebhook(IRegisterAccountActivityWebhookParameters parameters);

        /// <inheritdoc cref="GetAccountActivityWebhookEnvironments(IGetAccountActivityWebhookEnvironmentsParameters)" />
        Task<IWebhookEnvironmentDTO[]> GetAccountActivityWebhookEnvironments();

        /// <summary>
        /// Get the account activity webhook environments
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-webhooks </para>
        /// </summary>
        /// <returns>The account activity environments and their associated webhooks</returns>
        Task<IWebhookEnvironmentDTO[]> GetAccountActivityWebhookEnvironments(IGetAccountActivityWebhookEnvironmentsParameters parameters);

        /// <inheritdoc cref="RemoveAccountActivityWebhook(IRemoveAccountActivityWebhookParameters)" />
        Task RemoveAccountActivityWebhook(string environment, string webhookId);

        /// <summary>
        /// Remove the specified account activity webhook
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#delete-account-activity-all-env-name-webhooks-webhook-id </para>
        /// </summary>
        Task RemoveAccountActivityWebhook(IRemoveAccountActivityWebhookParameters parameters);
    }
}