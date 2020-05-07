using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examplinvi.AccountActivityEvents;
using Examplinvi.AccountActivityEvents.Controllers;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

namespace Examplinvi.AccountActivity.ASP.NETCore._3._0.Controllers
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
        public async Task<string> SetUserCredentialsAsync([FromBody]TwitterCredentials credentials)
        {
            var client = new TwitterClient(credentials);
            var user = await client.Users.GetAuthenticatedUserAsync();

            await AccountActivityCredentialsRetriever.SetUserCredentialsAsync(user.Id, credentials);
            return $"User {user.Id} registered!";
        }

        [HttpPost("TriggerAccountActivityWebhookCRC")]
        public async Task<bool> TriggerAccountActivityWebhookCRCAsync(string environment, string webhookId)
        {
            return await _accountActivityWebhooksController.TriggerAccountActivityWebhookCRCAsync(environment, webhookId);
        }

        [HttpPost("RegisterWebhook")]
        public async Task<bool> RegisterWebhookAsync(string environment, string url)
        {
            return await _accountActivityWebhooksController.CreateAccountActivityWebhookAsync(environment, url);
        }

        [HttpDelete("DeleteWebhook")]
        public async Task<bool> DeleteWebhookAsync(string environment, string webhookId)
        {
            return await _accountActivityWebhooksController.DeleteAccountActivityWebhookAsync(environment, webhookId);
        }

        [HttpGet("GetWebhookEnvironments")]
        public async Task<IEnumerable<IWebhookEnvironmentDTO>> GetWebhookEnvironmentsAsync()
        {
            return (await _accountActivityWebhooksController.GetAccountActivityWebhookEnvironmentsAsync()).Select(x => x.WebhookEnvironmentDTO);
        }

        [HttpGet("CountAccountActivitySubscriptions")]
        public async Task<string> CountAccountActivitySubscriptionsAsync()
        {
            return await _accountActivityWebhooksController.CountAccountActivitySubscriptionsAsync();
        }

        // SUBSCRIPTIONS - Subscribe / Unsubscribe user from webhook

        [HttpGet("GetWebhookSubscriptions")]
        public async Task<IWebhookSubscription[]> GetWebhookSubscriptionsAsync(string environment)
        {
            return await _accountActivitySubscriptionsController.GetWebhookSubscriptionsAsync(environment);
        }

        [HttpPost("SubscribeToAccountActivity")]
        public async Task<bool> SubscribeToAccountActivityAsync(string environment, long userId)
        {
            return await _accountActivitySubscriptionsController.SubscribeToAccountActivityAsync(environment, userId);
        }

        [HttpPost("UnsubscribeFromAccountActivity")]
        public async Task<bool> UnsubscribeFromAccountActivityAsync(string environment, long userId)
        {
            return await _accountActivitySubscriptionsController.UnsubscribeFromAccountActivityAsync(environment, userId);
        }

        // ACCOUNT ACTIVITY EVENTS
        [HttpPost("SubscribeToEvents")]
        public async Task<string> SubscribeToEventsAsync(string environment, long userId)
        {
            return await _accountActivityEventsController.SubscribeToEventsAsync(environment, userId);
        }

        [HttpPost("UnsubscribeFromEvents")]
        public async Task<string> UnsubscribeFromEventsAsync(string environment, long userId)
        {
            return await _accountActivityEventsController.UnsubscribeFromEventsAsync(environment, userId);
        }
    }
}
