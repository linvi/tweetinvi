using System.Threading.Tasks;
using Examplinvi.AccountActivityEvents;
using Examplinvi.AccountActivityEvents.Controllers;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

namespace Examplinvi.AccountActivity.ASP.NETCore.Controllers
{
    [Route("tweetinvi/")]
    public class TweetinviWebhookController : Controller
    {
        private readonly AccountActivityWebhooksController _accountActivityWebhooksController;
        private readonly AccountActivitySubscriptionsController _accountActivitySubscriptionsController;
        private readonly AccountActivityEventsController _accountActivityEventsController;

        public TweetinviWebhookController()
        {
            _accountActivityWebhooksController = new AccountActivityWebhooksController(Startup.WebhookClient);
            _accountActivitySubscriptionsController = new AccountActivitySubscriptionsController(Startup.WebhookClient);
            _accountActivityEventsController = new AccountActivityEventsController(Startup.AccountActivityRequestHandler);
        }

        // WEBHOOK - Prepare and configure webhook

        [HttpPost("SetUserCredentials")]
        public async Task<string> SetUserCredentials(long userId, [FromBody]TwitterCredentials credentials)
        {
            await AccountActivityCredentialsRetriever.SetUserCredentials(userId, credentials);
            return "User registered!";
        }


        [HttpPost("TriggerAccountActivityWebhookCRC")]
        public async Task<bool> TriggerAccountActivityWebhookCRC(string environment, string webhookId, long userId)
        {
            return await _accountActivityWebhooksController.TriggerAccountActivityWebhookCRC(environment, webhookId, userId);
        }

        [HttpPost("RegisterWebhook")]
        public async Task<bool> RegisterWebhook(string environment, string url)
        {
            return await _accountActivityWebhooksController.CreateAccountActivityWebhook(environment, url);
        }

        [HttpDelete("DeleteWebhook")]
        public async Task<bool> DeleteWebhook(string environment, string webhookId)
        {
            return await _accountActivityWebhooksController.DeleteAccountActivityWebhook(environment, webhookId);
        }

        [HttpGet("GetWebhookEnvironments")]
        public async Task<IWebhookEnvironmentDTO[]> GetWebhookEnvironments()
        {
            return await _accountActivityWebhooksController.GetAccountActivityWebhookEnvironments();
        }

        [HttpGet("CountAccountActivitySubscriptions")]
        public async Task<string> CountAccountActivitySubscriptions()
        {
            return await _accountActivityWebhooksController.CountAccountActivitySubscriptions();
        }

        // SUBSCRIPTIONS - Subscribe / Unsubscribe user from webhook

        [HttpGet("GetWebhookSubscriptions")]
        public async Task<IWebhookSubscriptionDTO[]> GetWebhookSubscriptions(string environment)
        {
            return await _accountActivitySubscriptionsController.GetWebhookSubscriptions(environment);
        }

        [HttpPost("SubscribeToAccountActivity")]
        public async Task<bool> SubscribeToAccountActivity(string environment, long userId)
        {
            return await _accountActivitySubscriptionsController.SubscribeToAccountActivity(environment, userId);
        }

        [HttpPost("UnsubscribeFromAccountActivity")]
        public async Task<bool> UnsubscribeFromAccountActivity(string environment, long userId)
        {
            return await _accountActivitySubscriptionsController.UnsubscribeFromAccountActivity(environment, userId);
        }

        // ACCOUNT ACTIVITY EVENTS
        [HttpPost("SubscribeToEvents")]
        public async Task<string> SubscribeToEvents(string environment, long userId)
        {
            return await _accountActivityEventsController.SubscribeToEvents(environment, userId);
        }

        [HttpPost("UnsubscribeFromEvents")]
        public async Task<string> UnsubscribeFromEvents(string environment, long userId)
        {
            return await _accountActivityEventsController.UnsubscribeFromEvents(environment, userId);
        }
    }
}
