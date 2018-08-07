using System.Threading.Tasks;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Controllers
{
    public interface IWebhookController
    {
        Task<IWebhookDTO> RegisterWebhookAsync(string webhookEnvironmentName, string url, ITwitterCredentials credentials);
        Task<IWebhookEnvironmentDTO[]> GetAllWebhooksAsync(IConsumerOnlyCredentials consumerCredentials);
        Task<bool> ChallengeWebhookAsync(string webhookEnvironmentName, string webhookId, ITwitterCredentials credentials);
        bool SubscribeToAllAuthenticatedUserEvents(string webhookEnvironmentName);
        IGetWebhookSubscriptionsCountResultDTO CountNumberOfSubscriptions();
        bool DoesAuthenticatedHaveASubscription(string webhookEnvironmentName);
        IWebhookSubcriptionListDTO GetListOfSubscriptions(string webhookEnvironmentName, IConsumerOnlyCredentials credentials);
        bool RemoveWebhook(string webhookEnvironmentName, string webhookId);
        bool RemoveAllAuthenticatedUserSubscriptions(string webhookEnvironmentName);
    }
}
