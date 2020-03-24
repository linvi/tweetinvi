using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;

namespace Examplinvi.AccountActivityEvents.Controllers
{
    public class AccountActivityWebhooksController
    {
        private readonly ITwitterClient _accountActivityClient;

        public AccountActivityWebhooksController(ITwitterClient accountActivityClient)
        {
            _accountActivityClient = accountActivityClient;
        }

        public async Task<bool> TriggerAccountActivityWebhookCRC(string environment, string webhookId)
        {
            try
            {
                await _accountActivityClient.AccountActivity.TriggerAccountActivityWebhookCRC(environment, webhookId);
                return true;
            }
            catch (TwitterException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public async Task<bool> CreateAccountActivityWebhook(string environment, string url)
        {
            try
            {
                await _accountActivityClient.AccountActivity.CreateAccountActivityWebhook(environment, url);
                return true;
            }
            catch (TwitterException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteAccountActivityWebhook(string environment, string webhookId)
        {
            try
            {
                await _accountActivityClient.AccountActivity.DeleteAccountActivityWebhook(environment, webhookId);
                return true;
            }
            catch (TwitterException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public async Task<IWebhookEnvironment[]> GetAccountActivityWebhookEnvironments()
        {
            var webhookEnvironments = await _accountActivityClient.AccountActivity.GetAccountActivityWebhookEnvironments();
            return webhookEnvironments;
        }

        public async Task<string> CountAccountActivitySubscriptions()
        {
            var subscriptionsCount = await _accountActivityClient.AccountActivity.CountAccountActivitySubscriptions();
            return $"{subscriptionsCount?.SubscriptionsCount}/{subscriptionsCount?.ProvisionedCount}";
        }
    }
}
