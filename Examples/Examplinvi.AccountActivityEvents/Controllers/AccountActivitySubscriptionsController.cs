using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

namespace Examplinvi.AccountActivityEvents.Controllers
{
    public class AccountActivitySubscriptionsController
    {
        private readonly IWebhookConfiguration _webhookConfiguration;

        public AccountActivitySubscriptionsController(IWebhookConfiguration webhookConfiguration)
        {
            _webhookConfiguration = webhookConfiguration;
        }

        public async Task<IWebhookSubscriptionDTO[]> GetWebhookSubscriptions(string environment)
        {
            var webhookEnvironments = await Webhooks.GetListOfSubscriptionsAsync(environment, _webhookConfiguration.ConsumerOnlyCredentials);
            return webhookEnvironments.Subscriptions;
        }

        public async Task<bool> SubscribeAccountToWebhook(string environment, long userId)
        {
            var userCredentials = await AccountActivityCredentialsRetriever.GetUserCredentials(userId);
            var success = await Webhooks.SubscribeToAccountActivityEventsAsync(environment, userCredentials);

            return success;
        }

        public async Task<bool> UnsubscribeAccountFromWebhooksEnvironment(string environment, long userId)
        {
            var userCredentials = await AccountActivityCredentialsRetriever.GetUserCredentials(userId);
            var result = await Webhooks.RemoveAllAccountSubscriptionsAsync(environment, userCredentials);

            return result;
        }

        public async Task<string> CountNumberOfWebhookSubscriptions(IConsumerOnlyCredentials credentials)
        {
            var result = await Webhooks.CountNumberOfSubscriptionsAsync(credentials);
            return result?.SubscriptionsCountAll;
        }


    }
}
