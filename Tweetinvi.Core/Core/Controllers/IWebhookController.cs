using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Core.Controllers
{
    public interface IWebhookController
    {
        IWebhookDTO RegisterWebhook(string webhookEnvironmentName, string url);
        IGetAllWebhooksResultDTO GetAllWebhooks();
        bool ChallengeWebhook(string webhookEnvironmentName, string webhookId);
        bool SubscribeToAllAuthenticatedUserEvents(string webhookEnvironmentName);
        IGetWebhookSubscriptionsCountResultDTO CountNumberOfSubscriptions();
        bool DoesAuthenticatedHaveASubscription(string webhookEnvironmentName);
        IWebhookSubcriptionListDTO GetListOfSubscriptions(string webhookEnvironmentName);
        bool RemoveWebhook(string webhookEnvironmentName, string webhookId);
        bool RemoveAllAuthenticatedUserSubscriptions(string webhookEnvironmentName);
    }
}
