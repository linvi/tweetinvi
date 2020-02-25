using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

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
            var client = new TwitterClient(userCredentials);

            try
            {
                await client.AccountActivity.TriggerAccountActivityCRC(environment, webhookId);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> RegisterWebhook(string environment, string url, long userId)
        {
            var userCredentials = await AccountActivityCredentialsRetriever.GetUserCredentials(userId);
            var client = new TwitterClient(userCredentials);
            var result = await client.AccountActivity.RegisterAccountActivityWebhook(environment, url).ConfigureAwait(false);

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
            var client = new TwitterClient(userCredentials);

            try
            {
                await client.AccountActivity.RemoveAccountActivityWebhook(environment, webhookId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IWebhookEnvironmentDTO[]> GetWebhookEnvironments()
        {
            var client = new TwitterClient(_webhookConfiguration.ConsumerOnlyCredentials);
            var webhookEnvironments = await client.AccountActivity.GetAccountActivityWebhookEnvironments();
            return webhookEnvironments;
        }
    }
}
