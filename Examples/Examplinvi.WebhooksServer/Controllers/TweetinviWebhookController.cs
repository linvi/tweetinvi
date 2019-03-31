using System.Threading.Tasks;
using Examplinvi.AccountActivityEvents;
using Examplinvi.AccountActivityEvents.Controllers;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Models;

namespace Examplinvi.WebhooksServer.Controllers
{
    [Route("tweetinvi/")]
    public class TweetinviWebhookController : Controller
    {
        private readonly AccountActivityWebhooksController _accountActivityWebhooksController;
        private readonly AccountActivitySubscriptionsController _accountActivitySubscriptionsController;
        private readonly AccountActivityEventsController _accountActivityEventsController;

        public TweetinviWebhookController()
        {
            _accountActivityWebhooksController = new AccountActivityWebhooksController(Startup.WebhookConfiguration);
            _accountActivitySubscriptionsController = new AccountActivitySubscriptionsController(Startup.WebhookConfiguration);
            _accountActivityEventsController = new AccountActivityEventsController(Startup.WebhookConfiguration);
        }

        // WEBHOOK - Prepare and configure webhook

        [HttpPost("SetUserCredentials")]
        public async Task SetUserCredentials(long userId, [FromBody]TwitterCredentials credentials)
        {
            await AccountActivityCredentialsRetriever.SetUserCredentials(userId, credentials);
        }


        [HttpPost("ChallengeWebhook")]
        public async Task<bool> ChallengeWebhook(string environment, string webhookId, long userId)
        {
            return await _accountActivityWebhooksController.ChallengeWebhook(environment, webhookId, userId);
        }

        [HttpPost("RegisterWebhook")]
        public async Task<bool> RegisterWebhook(string environment, string url, long userId)
        {
            return await _accountActivityWebhooksController.RegisterWebhook(environment, url, userId);
        }

        [HttpDelete("DeleteWebhook")]
        public async Task<bool> DeleteWebhook(string environment, string webhookId, long userId)
        {
            return await _accountActivityWebhooksController.DeleteWebhook(environment, webhookId, userId);
        }

        [HttpGet("GetWebhookEnvironments")]
        public async Task<IWebhookEnvironmentDTO[]> GetWebhookEnvironments()
        {
            return await _accountActivityWebhooksController.GetWebhookEnvironments();
        }

        // SUBSCRIPTIONS - Subscribe / Unsubscribe user from webhook

        [HttpGet("GetWebhookSubscriptions")]
        public async Task<IWebhookSubscriptionDTO[]> GetWebhookSubscriptions(string environment)
        {
            return await _accountActivitySubscriptionsController.GetWebhookSubscriptions(environment);
        }

        [HttpPost("SubscribeAccountToWebhook")]
        public async Task<bool> SubscribeAccountToWebhook(string environment, long userId)
        {
            return await _accountActivitySubscriptionsController.SubscribeAccountToWebhook(environment, userId);
        }

        [HttpPost("UnsubscribeAccountFromWebhooksEnvironment")]
        public async Task<bool> UnsubscribeAccountFromWebhooksEnvironment(string environment, long userId)
        {
            return await _accountActivitySubscriptionsController.UnsubscribeAccountFromWebhooksEnvironment(environment, userId);
        }

        [HttpGet("CountNumberOfWebhookSubscriptions")]
        public async Task<string> CountNumberOfWebhookSubscriptions()
        {
            var consumerCredentials = Startup.WebhookConfiguration.ConsumerOnlyCredentials;
            return await _accountActivitySubscriptionsController.CountNumberOfWebhookSubscriptions(consumerCredentials);
        }

        // ACCOUNT ACTIVITY EVENTS

        [HttpPost("StartListeningToEventsForAllSubscribedAccounts")]
        public async Task<string> StartListeningToEventsForAllSubscribedAccounts(string environment)
        {
            return await _accountActivityEventsController.StartListeningToEventsForAllSubscribedAccounts(environment);
        }

        [HttpPost("StopListeningToEventsForAllSubscribedAccounts")]
        public async Task<string> StopListeningToEventsForAllSubscribedAccounts(string environment)
        {
            return await _accountActivityEventsController.StopAllAccountActivityStreams(environment);
        }

        [HttpPost("SubscribeToAccountActivitiesEvents")]
        public async Task<string> SubscribeToAccountActivitiesEvents(string environment, long userId)
        {
            return await _accountActivityEventsController.SubscribeToAccountActivitiesEvents(environment, userId);
        }

        [HttpPost("UnsubscribeFromAccountActivitiesEvents")]
        public async Task<string> UnsubscribeFromAccountActivitiesEvents(string environment, string userId)
        {
            return await _accountActivityEventsController.UnsubscribeFromAccountActivitiesEvents(environment, userId);
        }
    }
}
