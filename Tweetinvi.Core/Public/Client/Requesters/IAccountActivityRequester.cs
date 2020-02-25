using System.Threading.Tasks;
using Tweetinvi.Core.Web;
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
        Task<ITwitterResult<IWebhookDTO>> RegisterAccountActivityWebhook(IRegisterAccountActivityWebhookParameters parameters);

        /// <summary>
        /// Get the account activity webhook environments
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#get-account-activity-all-webhooks </para>
        /// </summary>
        /// <returns>Twitter result containing the account activity environments</returns>
        Task<ITwitterResult<IGetAccountActivityWebhookEnvironmentsResultDTO>> GetAccountActivityWebhookEnvironments(IGetAccountActivityWebhookEnvironmentsParameters parameters);

        /// <summary>
        /// Remove the specified account activity webhook
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#delete-account-activity-all-env-name-webhooks-webhook-id </para>
        /// </summary>
        /// <returns>Twitter Result</returns>
        Task<ITwitterResult> RemoveAccountActivityWebhook(IRemoveAccountActivityWebhookParameters parameters);

        /// <summary>
        /// Challenges a webhook and reenable it when it was disabled
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/api-reference/aaa-premium#put-account-activity-all-env-name-webhooks-webhook-id </para>
        /// </summary>
        /// <returns>Twitter Result</returns>
        Task<ITwitterResult> TriggerAccountActivityCRC(ITriggerAccountActivityCRCParameters parameters);
    }
}