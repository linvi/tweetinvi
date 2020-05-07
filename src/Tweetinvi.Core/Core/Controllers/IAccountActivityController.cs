using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IAccountActivityController
    {
        Task<ITwitterResult<IWebhookDTO>> CreateAccountActivityWebhookAsync(ICreateAccountActivityWebhookParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IGetAccountActivityWebhookEnvironmentsResultDTO>> GetAccountActivityWebhookEnvironmentsAsync(IGetAccountActivityWebhookEnvironmentsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IWebhookDTO[]>> GetAccountActivityEnvironmentWebhooksAsync(IGetAccountActivityEnvironmentWebhooksParameters parameters, ITwitterRequest request);

        Task<ITwitterResult> DeleteAccountActivityWebhookAsync(IDeleteAccountActivityWebhookParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> TriggerAccountActivityWebhookCRCAsync(ITriggerAccountActivityWebhookCRCParameters parameters, ITwitterRequest request);

        Task<ITwitterResult<IWebhookEnvironmentSubscriptionsDTO>> GetAccountActivitySubscriptionsAsync(IGetAccountActivitySubscriptionsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IWebhookSubscriptionsCount>> CountAccountActivitySubscriptionsAsync(ICountAccountActivitySubscriptionsParameters parameters, ITwitterRequest request);

        Task<ITwitterResult> SubscribeToAccountActivityAsync(ISubscribeToAccountActivityParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> IsAccountSubscribedToAccountActivityAsync(IIsAccountSubscribedToAccountActivityParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> UnsubscribeFromAccountActivityAsync(IUnsubscribeFromAccountActivityParameters parameters, ITwitterRequest request);
    }
}
