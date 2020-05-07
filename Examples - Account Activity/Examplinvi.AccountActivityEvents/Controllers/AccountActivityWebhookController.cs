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

        public async Task<bool> TriggerAccountActivityWebhookCRCAsync(string environment, string webhookId)
        {
            try
            {
                await _accountActivityClient.AccountActivity.TriggerAccountActivityWebhookCRCAsync(environment, webhookId);
                return true;
            }
            catch (TwitterException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public async Task<bool> CreateAccountActivityWebhookAsync(string environment, string url)
        {
            try
            {
                await _accountActivityClient.AccountActivity.CreateAccountActivityWebhookAsync(environment, url);
                return true;
            }
            catch (TwitterException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteAccountActivityWebhookAsync(string environment, string webhookId)
        {
            try
            {
                await _accountActivityClient.AccountActivity.DeleteAccountActivityWebhookAsync(environment, webhookId);
                return true;
            }
            catch (TwitterException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public async Task<IWebhookEnvironment[]> GetAccountActivityWebhookEnvironmentsAsync()
        {
            var webhookEnvironments = await _accountActivityClient.AccountActivity.GetAccountActivityWebhookEnvironmentsAsync();
            return webhookEnvironments;
        }

        public async Task<string> CountAccountActivitySubscriptionsAsync()
        {
            var subscriptionsCount = await _accountActivityClient.AccountActivity.CountAccountActivitySubscriptionsAsync();
            return $"{subscriptionsCount?.SubscriptionsCount}/{subscriptionsCount?.ProvisionedCount}";
        }
    }
}
