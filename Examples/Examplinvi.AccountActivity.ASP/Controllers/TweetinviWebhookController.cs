using System.Threading.Tasks;
using System.Web.Http;
using Examplinvi.AccountActivityEvents;
using Examplinvi.AccountActivityEvents.Controllers;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

namespace Examplinvi.AccountActivity.ASP.Controllers
{
    [RoutePrefix("tweetinvi")]
    public class TweetinviWebhookController : ApiController
    {
        private static AccountActivityWebhooksController _accountActivityWebhooksController;
        private static AccountActivitySubscriptionsController _accountActivitySubscriptionsController;
        private static AccountActivityEventsController _accountActivityEventsController;

        static TweetinviWebhookController()
        {
            _accountActivityWebhooksController = new AccountActivityWebhooksController(WebApiConfig.WebhookClient);
            _accountActivitySubscriptionsController = new AccountActivitySubscriptionsController(WebApiConfig.WebhookClient);
            _accountActivityEventsController = new AccountActivityEventsController(WebApiConfig.AccountActivityRequestHandler);
        }

        // WEBHOOK
        [HttpPost]
        [Route("SetUserCredentials")]
        public async Task<string> SetUserCredentials(long userId, [FromBody]TwitterCredentials credentials)
        {
            var client = new TwitterClient(credentials);
            var user = await client.Users.GetAuthenticatedUser();

            await AccountActivityCredentialsRetriever.SetUserCredentials(user.Id, credentials);
            return $"User {user.Id} registered!";
        }


        [HttpPost]
        [Route("TriggerAccountActivityWebhookCRC")]
        public Task<bool> TriggerAccountActivityWebhookCRC(string environment, string webhookId)
        {
            return _accountActivityWebhooksController.TriggerAccountActivityWebhookCRC(environment, webhookId);
        }

        [HttpPost]
        [Route("RegisterWebhook")]
        public Task<bool> RegisterWebhook(string environment, string url, long userId)
        {
            return _accountActivityWebhooksController.CreateAccountActivityWebhook(environment, url);
        }

        [HttpDelete]
        [Route("DeleteWebhook")]
        public Task<bool> DeleteWebhook(string environment, string webhookId, long userId)
        {
            return _accountActivityWebhooksController.DeleteAccountActivityWebhook(environment, webhookId);
        }

        [HttpGet]
        [Route("GetWebhookEnvironments")]
        public async Task<IWebhookEnvironmentDTO[]> GetWebhookEnvironments()
        {
            return await _accountActivityWebhooksController.GetAccountActivityWebhookEnvironments();
        }

        // SUBSCRIPTIONS - Subscribe / Unsubscribe user from webhook

        [HttpGet]
        [Route("CountAccountActivitySubscriptions")]
        public async Task<string> CountAccountActivitySubscriptions()
        {
            return await _accountActivityWebhooksController.CountAccountActivitySubscriptions();
        }
        
        [HttpGet]
        [Route("GetWebhookSubscriptions")]
        public async Task<IWebhookSubscriptionDTO[]> GetWebhookSubscriptions(string environment)
        {
            return await _accountActivitySubscriptionsController.GetWebhookSubscriptions(environment);
        }
        
        [HttpPost]
        [Route("SubscribeToAccountActivity")]
        public async Task<bool> SubscribeToAccountActivity(string environment, long userId)
        {
            return await _accountActivitySubscriptionsController.SubscribeToAccountActivity(environment, userId);
        }
        
        [HttpPost]
        public async Task<bool> UnsubscribeFromAccountActivity(string environment, long userId)
        {
            return await _accountActivitySubscriptionsController.UnsubscribeFromAccountActivity(environment, userId);
        }

        // ACCOUNT ACTIVITY EVENTS
        [HttpPost]
        [Route("SubscribeToEvents")]
        public async Task<string> SubscribeToEvents(string environment, long userId)
        {
            return await _accountActivityEventsController.SubscribeToEvents(environment, userId);
        }

        [HttpPost]
        public async Task<string> UnsubscribeFromEvents(string environment, long userId)
        {
            return await _accountActivityEventsController.UnsubscribeFromEvents(environment, userId);
        }
    }
}
