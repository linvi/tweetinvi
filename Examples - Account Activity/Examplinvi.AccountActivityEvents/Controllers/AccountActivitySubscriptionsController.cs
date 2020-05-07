using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

namespace Examplinvi.AccountActivityEvents.Controllers
{
    public class AccountActivitySubscriptionsController
    {
        private readonly ITwitterClient _accountActivityClient;

        public AccountActivitySubscriptionsController(ITwitterClient accountActivityClient)
        {
            _accountActivityClient = accountActivityClient;
        }

        public async Task<IWebhookSubscription[]> GetWebhookSubscriptionsAsync(string environment)
        {
            var webhookEnvironments = await _accountActivityClient.AccountActivity.GetAccountActivitySubscriptionsAsync(environment);
            return webhookEnvironments.Subscriptions;
        }

        public async Task<bool> SubscribeToAccountActivityAsync(string environment, long userId)
        {
            var userCredentials = await AccountActivityCredentialsRetriever.GetUserCredentialsAsync(userId);
            var client = new TwitterClient(userCredentials);

            try
            {
                await client.AccountActivity.SubscribeToAccountActivityAsync(environment);
                return true;
            }
            catch (TwitterException e)
            {
                if (e.TwitterExceptionInfos[0].Code == 355)
                {
                    // user already subscribed
                    return true;
                }

                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public async Task<bool> UnsubscribeFromAccountActivityAsync(string environment, long userId)
        {
            try
            {
                await _accountActivityClient.AccountActivity.UnsubscribeFromAccountActivityAsync(environment, userId);
                return true;
            }
            catch (TwitterException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
}
