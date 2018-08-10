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
        Task<bool> SubscribeToAllAuthenticatedUserEventsAsync(string webhookEnvironmentName, ITwitterCredentials credentials);
        Task<IGetWebhookSubscriptionsCountResultDTO> CountNumberOfSubscriptionsAsync(IConsumerOnlyCredentials credentials);
        Task<bool> DoesAccountHaveASubscriptionAsync(string webhookEnvironmentName, ITwitterCredentials credentials);
        Task<IWebhookSubcriptionListDTO> GetListOfSubscriptionsAsync(string webhookEnvironmentName, IConsumerOnlyCredentials credentials);
        Task<bool> RemoveWebhookAsync(string webhookEnvironmentName, string webhookId, ITwitterCredentials credentials);
        Task<bool> RemoveAllAccountSubscriptionsAsync(string webhookEnvironmentName, ITwitterCredentials credentials);
    }
}
