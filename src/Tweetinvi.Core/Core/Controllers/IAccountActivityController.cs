using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IAccountActivityController
    {
        Task<ITwitterResult<IWebhookDTO>> CreateAccountActivityWebhook(ICreateAccountActivityWebhookParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IGetAccountActivityWebhookEnvironmentsResultDTO>> GetAccountActivityWebhookEnvironments(IGetAccountActivityWebhookEnvironmentsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IWebhookDTO[]>> GetAccountActivityEnvironmentWebhooks(IGetAccountActivityEnvironmentWebhooksParameters parameters, ITwitterRequest request);

        Task<ITwitterResult> DeleteAccountActivityWebhook(IDeleteAccountActivityWebhookParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> TriggerAccountActivityWebhookCRC(ITriggerAccountActivityWebhookCRCParameters parameters, ITwitterRequest request);

        Task<ITwitterResult<IWebhookEnvironmentSubscriptionsDTO>> GetAccountActivitySubscriptions(IGetAccountActivitySubscriptionsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IWebhookSubscriptionsCount>> CountAccountActivitySubscriptions(ICountAccountActivitySubscriptionsParameters parameters, ITwitterRequest request);

        Task<ITwitterResult> SubscribeToAccountActivity(ISubscribeToAccountActivityParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> IsAccountSubscribedToAccountActivity(IIsAccountSubscribedToAccountActivityParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> UnsubscribeFromAccountActivity(IUnsubscribeFromAccountActivityParameters parameters, ITwitterRequest request);
    }
}
