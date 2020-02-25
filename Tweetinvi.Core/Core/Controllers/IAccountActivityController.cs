using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IAccountActivityController
    {

        Task<ITwitterResult<IWebhookDTO>> RegisterAccountActivityWebhook(IRegisterAccountActivityWebhookParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IGetAccountActivityWebhookEnvironmentsResultDTO>> GetAccountActivityWebhookEnvironments(IGetAccountActivityWebhookEnvironmentsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult> RemoveAccountActivityWebhook(IRemoveAccountActivityWebhookParameters parameters, ITwitterRequest request);

        Task<bool> ChallengeWebhookAsync(string webhookEnvironmentName, string webhookId, ITwitterCredentials credentials);
        Task<bool> SubscribeToAllAuthenticatedUserEventsAsync(string webhookEnvironmentName, ITwitterCredentials credentials);
        Task<IGetWebhookSubscriptionsCountResultDTO> CountNumberOfSubscriptionsAsync(IConsumerOnlyCredentials credentials);
        Task<bool> DoesAccountHaveASubscriptionAsync(string webhookEnvironmentName, ITwitterCredentials credentials);
        Task<IWebhookSubscriptionListDTO> GetListOfSubscriptionsAsync(string webhookEnvironmentName, IConsumerOnlyCredentials credentials);
        Task<bool> RemoveAllAccountSubscriptionsAsync(string webhookEnvironmentName, ITwitterCredentials credentials);
    }
}
