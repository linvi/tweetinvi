using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Controllers
{
    public interface IWebhookController
    {
        IWebhookDTO RegisterWebhook(string webhookEnvironmentName, string url);
        IWebhookEnvironmentDTO[] GetAllWebhooks(IConsumerCredentials consumerCredentials);
        bool ChallengeWebhook(string webhookEnvironmentName, string webhookId);
        bool SubscribeToAllAuthenticatedUserEvents(string webhookEnvironmentName);
        IGetWebhookSubscriptionsCountResultDTO CountNumberOfSubscriptions();
        bool DoesAuthenticatedHaveASubscription(string webhookEnvironmentName);
        IWebhookSubcriptionListDTO GetListOfSubscriptions(string webhookEnvironmentName, IConsumerOnlyCredentials credentials);
        bool RemoveWebhook(string webhookEnvironmentName, string webhookId);
        bool RemoveAllAuthenticatedUserSubscriptions(string webhookEnvironmentName);
    }
}
