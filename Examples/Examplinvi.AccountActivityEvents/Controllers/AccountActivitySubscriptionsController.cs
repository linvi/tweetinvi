using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Exceptions;
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

        public async Task<IWebhookSubscriptionDTO[]> GetWebhookSubscriptions(string environment)
        {
            var webhookEnvironments = await _accountActivityClient.AccountActivity.GetAccountActivitySubscriptions(environment);
            return webhookEnvironments.Subscriptions;
        }

        public async Task<bool> SubscribeToAccountActivity(string environment, long userId)
        {
            var userCredentials = await AccountActivityCredentialsRetriever.GetUserCredentials(userId);
            var client = new TwitterClient(userCredentials);

            try
            {
                await client.AccountActivity.SubscribeToAccountActivity(environment);
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

        public async Task<bool> UnsubscribeFromAccountActivity(string environment, long userId)
        {
            try
            {
                await _accountActivityClient.AccountActivity.UnsubscribeFromAccountActivity(environment, userId);
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
