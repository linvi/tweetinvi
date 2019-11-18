using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Models;

namespace Examplinvi.AccountActivityEvents.Controllers
{
    public class AccountActivityWebhooksController
    {
        private readonly IWebhookConfiguration _webhookConfiguration;

        public AccountActivityWebhooksController(IWebhookConfiguration webhookConfiguration)
        {
            _webhookConfiguration = webhookConfiguration;
        }

        public async Task<bool> ChallengeWebhook(string environment, string webhookId, long userId)
        {
            var userCredentials = await AccountActivityCredentialsRetriever.GetUserCredentials(userId);
            return await Webhooks.ChallengeWebhookAsync(environment, webhookId, userCredentials);
        }

        public async Task<bool> RegisterWebhook(string environment, string url, long userId)
        {
            var userCredentials = await AccountActivityCredentialsRetriever.GetUserCredentials(userId);
            var result = await Webhooks.RegisterWebhookAsync(environment, url, userCredentials);

            if (result == null)
            {
                return false;
            }

            // Register webhook in server
            var webhookEnvironment = _webhookConfiguration.RegisteredWebhookEnvironments.FirstOrDefault(x => x.Name == environment);

            webhookEnvironment?.AddWebhook(result);

            return true;
        }

        public async Task<bool> DeleteWebhook(string environment, string webhookId, long userId)
        {
            var userCredentials = await AccountActivityCredentialsRetriever.GetUserCredentials(userId);
            var result = await Webhooks.RemoveWebhookAsync(environment, webhookId, userCredentials);

            return result;
        }

        public async Task<IWebhookEnvironmentDTO[]> GetWebhookEnvironments()
        {
            var webhookEnvironments = await Webhooks.GetAllWebhookEnvironmentsAsync(_webhookConfiguration.ConsumerOnlyCredentials);
            return webhookEnvironments;
        }
    }
}
