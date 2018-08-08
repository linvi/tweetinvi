using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examplinvi.WebhooksServer;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Models;

namespace WebApplication1.Controllers
{
    [Route("tweetinvi/")]
    public class TweetinviWebhookController : Controller
    {
        // WEBHOOK

        [HttpPost("ChallengeWebhook")]
        public async Task<bool> ChallengeWebhook(string environment, string webhookId, string userId)
        {
            var userCredentials = await CredentialsRetriever.GetUserCredentials(userId);
            return await Webhooks.ChallengeWebhookAsync(environment, webhookId, userCredentials);
        }

        [HttpPost("RegisterWebhook")]
        public async Task<bool> RegisterWebhook(string environment, string url, string userId)
        {
            var userCredentials = await CredentialsRetriever.GetUserCredentials(userId);
            var result = await Webhooks.RegisterWebhookAsync(environment, url, userCredentials);

            if (result == null)
            {
                return false;
            }

            // Register webhook in server
            var webhookEnvironment = Startup.TweetinviWebhookConfiguration.RegisteredWebhookEnvironments.FirstOrDefault(x =>
                x.Name == environment);

            webhookEnvironment?.AddWebhook(result);

            return true;
        }

        [HttpGet("GetWebhookEnvironments")]
        public async Task<IWebhookEnvironmentDTO[]> GetWebhookEnvironments()
        {
            var webhookEnvironments = await Webhooks.GetAllWebhookEnvironmentsAsync(Startup.TweetinviWebhookConfiguration.ConsumerOnlyCredentials);
            return webhookEnvironments;
        }

        // SUBSCRIPTIONS

        [HttpGet("GetWebhookSubscriptions")]
        public async Task<IWebhookSubscriptionDTO[]> GetWebhookSubscriptions(string environment)
        {
            var webhookEnvironments = await Webhooks.GetListOfSubscriptionsAsync(environment, Startup.TweetinviWebhookConfiguration.ConsumerOnlyCredentials);  
            return webhookEnvironments.Subscriptions;
        }

        [HttpPost("SubscribeAccountToWebhook")]
        public async Task<bool> SubscribeAccountToWebhook(string environment, string userId)
        {
            var userCredentials = await CredentialsRetriever.GetUserCredentials(userId);
            var success = await Webhooks.SubscribeToAccountActivityEventsAsync(environment, userCredentials);

            return success;
        }

        [HttpPost("UnsubscribeAccountFromWebhooks")]
        public async Task<bool> UnsubscribeAccountFromWebhooks(string environment, string userId)
        {
            var userCredentials = await CredentialsRetriever.GetUserCredentials(userId);
            var result = await Webhooks.RemoveAllAccountSubscriptionsAsync(environment, userCredentials);

            return result;
        }

        [HttpGet("CountSubscriptions")]
        public async Task<string> CountSubscriptions(string userId)
        {
            var credentials = Startup.TweetinviWebhookConfiguration.ConsumerOnlyCredentials;
            var result = await Webhooks.CountNumberOfSubscriptionsAsync(credentials);
            return result?.SubscriptionsCountAll;
        }

        // Account Activity

        [HttpPost("SubscribeToAccountActivities")]
        public async Task<string> SubscribeToAccountActivities(string environment, string userId)
        {
            var userCredentials = await CredentialsRetriever.GetUserCredentials(userId);
            
            var webhook = Startup.TweetinviWebhookConfiguration.RegisteredWebhookEnvironments.FirstOrDefault(x => x.Name == environment);

            if (webhook == null)
            {
                return "ENVIRONMENT_NOT_REGISTERED";
            }

            var activityStream = Stream.CreateAccountActivityStream(userId);
            Startup.TweetinviWebhookConfiguration.AddActivityStream(activityStream);

            activityStream.TweetFavourited += (sender, args) =>
            {
                Console.WriteLine($"{userId} favourited tweet!");
            };

            return "SUBSCRIBED_ON_SERVER";
        }

        [HttpPost("UnsubscribeFromAccountActivities")]
        public string UnsubscribeFromAccountActivities(string environment, string userId)
        {
            var webhook = Startup.TweetinviWebhookConfiguration.RegisteredWebhookEnvironments.FirstOrDefault(x => x.Name == environment);

            if (webhook == null)
            {
                return "ENVIRONMENT_NOT_MATCHED";
            }

            var streams = Startup.TweetinviWebhookConfiguration.RegisteredActivityStreams.Where(x => x.UserId.ToString() == userId);

            streams.ForEach(stream =>
            {
                Startup.TweetinviWebhookConfiguration.RemoveActivityStream(stream);
            });

            return "UNSUBSCRIBED";
        }
    }
}
