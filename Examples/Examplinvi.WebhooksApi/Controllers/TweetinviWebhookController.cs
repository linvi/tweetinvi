using System.Threading.Tasks;
using System.Web.Http;
using Examplinvi.AccountActivityEvents;
using Examplinvi.AccountActivityEvents.Controllers;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Models;

namespace Examplinvi.WebhooksApi.Controllers
{
    [RoutePrefix("tweetinvi")]
    public class TweetinviWebhookController : ApiController
    {
        private readonly AccountActivityWebhooksController _accountActivityWebhooksController;
        private readonly AccountActivitySubscriptionsController _accountActivitySubscriptionsController;
        private readonly AccountActivityEventsController _accountActivityEventsController;

        public TweetinviWebhookController()
        {
            _accountActivityWebhooksController = new AccountActivityWebhooksController(TweetinviWebhooksHost.Configuration);
            _accountActivitySubscriptionsController = new AccountActivitySubscriptionsController(TweetinviWebhooksHost.Configuration);
            _accountActivityEventsController = new AccountActivityEventsController(TweetinviWebhooksHost.Configuration);
        }

        // WEBHOOK
        [HttpPost]
        [Route("SetUserCredentials")]
        public async Task SetUserCredentials(long userId, [System.Web.Http.FromBody]TwitterCredentials credentials)
        {
            await AccountActivityCredentialsRetriever.SetUserCredentials(userId, credentials);
        }


        [HttpPost]
        [Route("ChallengeWebhook")]
        public async Task<bool> ChallengeWebhook(string environment, string webhookId, long userId)
        {
            return await _accountActivityWebhooksController.ChallengeWebhook(environment, webhookId, userId);
        }

        [HttpPost]
        [Route("RegisterWebhook")]
        public async Task<bool> RegisterWebhook(string environment, string url, long userId)
        {
            return await _accountActivityWebhooksController.RegisterWebhook(environment, url, userId);
        }

        [HttpDelete]
        [Route("DeleteWebhook")]
        public async Task<bool> DeleteWebhook(string environment, string webhookId, long userId)
        {
            return await _accountActivityWebhooksController.DeleteWebhook(environment, webhookId, userId);
        }

        [HttpGet]
        [Route("GetWebhookEnvironments")]
        public async Task<IWebhookEnvironmentDTO[]> GetWebhookEnvironments()
        {
            return await _accountActivityWebhooksController.GetWebhookEnvironments();
        }

        // SUBSCRIPTIONS - Subscribe / Unsubscribe user from webhook

        [HttpGet]
        [Route("GetWebhookSubscriptions")]
        public async Task<IWebhookSubscriptionDTO[]> GetWebhookSubscriptions(string environment)
        {
            return await _accountActivitySubscriptionsController.GetWebhookSubscriptions(environment);
        }

        [HttpPost]
        [Route("SubscribeAccountToWebhook")]
        public async Task<bool> SubscribeAccountToWebhook(string environment, long userId)
        {
            return await _accountActivitySubscriptionsController.SubscribeAccountToWebhook(environment, userId);
        }

        [HttpPost]
        [Route("UnsubscribeAccountFromWebhooksEnvironment")]
        public async Task<bool> UnsubscribeAccountFromWebhooksEnvironment(string environment, long userId)
        {
            return await _accountActivitySubscriptionsController.UnsubscribeAccountFromWebhooksEnvironment(environment, userId);
        }

        [HttpGet]
        [Route("CountNumberOfWebhookSubscriptions")]
        public async Task<string> CountNumberOfWebhookSubscriptions(string userId)
        {
            var consumerCredentials = TweetinviWebhooksHost.Configuration.ConsumerOnlyCredentials;
            return await _accountActivitySubscriptionsController.CountNumberOfWebhookSubscriptions(consumerCredentials);
        }

        // ACCOUNT ACTIVITY EVENTS

        [HttpPost]
        [Route("StartListeningToEventsForAllSubscribedAccounts")]
        public async Task<string> StartListeningToEventsForAllSubscribedAccounts(string environment)
        {
            return await _accountActivityEventsController.StartListeningToEventsForAllSubscribedAccounts(environment);
        }

        [HttpPost]
        [Route("StopListeningToEventsForAllSubscribedAccounts")]
        public async Task<string> StopListeningToEventsForAllSubscribedAccounts(string environment)
        {
            return await _accountActivityEventsController.StopAllAccountActivityStreams(environment);
        }

        [HttpPost]
        [Route("SubscribeToAccountActivitiesEvents")]
        public async Task<string> SubscribeToAccountActivitiesEvents(string environment, long userId)
        {
            return await _accountActivityEventsController.SubscribeToAccountActivitiesEvents(environment, userId);
        }

        [HttpPost]
        [Route("UnsubscribeFromAccountActivitiesEvents")]
        public async Task<string> UnsubscribeFromAccountActivitiesEvents(string environment, string userId)
        {
            return await _accountActivityEventsController.UnsubscribeFromAccountActivitiesEvents(environment, userId);
        }
    }
}
