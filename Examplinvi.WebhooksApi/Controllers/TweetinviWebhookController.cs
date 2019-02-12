using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Examplinvi.WebhooksApi;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Events;
using Tweetinvi.Models;

namespace WebApplication1.Controllers
{
    [RoutePrefix("tweetinvi")]
    public class TweetinviWebhookController : ApiController
    {
        // WEBHOOK
        [HttpPost]
        [Route("SetUserCredentials")]
        public async Task SetUserCredentials(long userId, [System.Web.Http.FromBody]TwitterCredentials credentials)
        {
            await CredentialsRetriever.SetUserCredentials(userId, credentials);
        }


        [HttpPost]
        [Route("ChallengeWebhook")]
        public async Task<bool> ChallengeWebhook(string environment, string webhookId, long userId)
        {
            var userCredentials = await CredentialsRetriever.GetUserCredentials(userId);
            return await Webhooks.ChallengeWebhookAsync(environment, webhookId, userCredentials);
        }

        [HttpPost]
        [Route("RegisterWebhook")]
        public async Task<bool> RegisterWebhook(string environment, string url, long userId)
        {
            var userCredentials = await CredentialsRetriever.GetUserCredentials(userId);
            var result = await Webhooks.RegisterWebhookAsync(environment, url, userCredentials);

            if (result == null)
            {
                return false;
            }

            // Register webhook in server
            var webhookEnvironment = TweetinviWebhooksHost.Configuration.RegisteredWebhookEnvironments.FirstOrDefault(x => x.Name == environment);

            webhookEnvironment?.AddWebhook(result);

            return true;
        }

        [HttpDelete]
        [Route("DeleteWebhook")]
        public async Task<bool> DeleteWebhook(string environment, string webhookId, long userId)
        {
            var userCredentials = await CredentialsRetriever.GetUserCredentials(userId);
            var result = await Webhooks.RemoveWebhookAsync(environment, webhookId, userCredentials);

            return result;
        }

        [HttpGet]
        [Route("GetWebhookEnvironments")]
        public async Task<IWebhookEnvironmentDTO[]> GetWebhookEnvironments()
        {
            var webhookEnvironments = await Webhooks.GetAllWebhookEnvironmentsAsync(TweetinviWebhooksHost.Configuration.ConsumerOnlyCredentials);
            return webhookEnvironments;
        }

        [HttpPost]
        [Route("StopAllAccountActivityStreams")]
        public void StopAllAccountActivityStreams()
        {
            TweetinviWebhooksHost.Configuration.RegisteredActivityStreams.ToArray().ForEach(accountActivityStream =>
            {
                TweetinviWebhooksHost.Configuration.RemoveActivityStream(accountActivityStream);
            });
        }

        [HttpPost]
        [Route("StartAllAccountActivityStreams")]
        public async Task StartAllAccountActivityStreams(string environment)
        {
            if (!TweetinviWebhooksHost.Configuration.RegisteredWebhookEnvironments.Any(x => x.Name == environment))
            {
                throw new InvalidOperationException("You attempted to listen to streams for an environment that was not registered");
            }

            var webhooksSubscriptions = await GetWebhookSubscriptions(environment);

            webhooksSubscriptions.ForEach(subscription =>
            {
                var accountActivityStream = TweetinviWebhooksHost.Configuration.RegisteredActivityStreams.SingleOrDefault(x => x.UserId.ToString() == subscription.UserId);

                if (accountActivityStream == null)
                {
                    accountActivityStream = Stream.CreateAccountActivityStream(subscription.UserId);
                    TweetinviWebhooksHost.Configuration.AddActivityStream(accountActivityStream);
                }

                accountActivityStream.JsonObjectReceived += JsonObjectReceived;
                accountActivityStream.MessageReceived += MessageReceived;
                accountActivityStream.MessageSent += MessageSent;
            });
        }

        private void MessageSent(object sender, MessageEventArgs args)
        {
            Console.WriteLine(args.Message.App);
        }

        private void MessageReceived(object sender, MessageEventArgs args)
        {
            Console.WriteLine(args.Message.App);
        }

        private void JsonObjectReceived(object sender, JsonObjectEventArgs args)
        {
            Console.WriteLine(args.Json);
        }


        // SUBSCRIPTIONS

        [HttpGet]
        public async Task<IWebhookSubscriptionDTO[]> GetWebhookSubscriptions(string environment)
        {
            var webhookEnvironments = await Webhooks.GetListOfSubscriptionsAsync(environment, TweetinviWebhooksHost.Configuration.ConsumerOnlyCredentials);
            return webhookEnvironments.Subscriptions;
        }

        [HttpPost]
        public async Task<bool> SubscribeAccountToWebhook(string environment, long userId)
        {
            var userCredentials = await CredentialsRetriever.GetUserCredentials(userId);
            var success = await Webhooks.SubscribeToAccountActivityEventsAsync(environment, userCredentials);

            return success;
        }

        [HttpPost]
        public async Task<bool> UnsubscribeAccountFromWebhooks(string environment, long userId)
        {
            var userCredentials = await CredentialsRetriever.GetUserCredentials(userId);
            var result = await Webhooks.RemoveAllAccountSubscriptionsAsync(environment, userCredentials);

            return result;
        }

        [HttpGet]
        public async Task<string> CountSubscriptions(string userId)
        {
            var credentials = TweetinviWebhooksHost.Configuration.ConsumerOnlyCredentials;
            var result = await Webhooks.CountNumberOfSubscriptionsAsync(credentials);
            return result?.SubscriptionsCountAll;
        }

        // Account Activity

        [HttpPost]
        public async Task<string> SubscribeToAccountActivities(string environment, long userId)
        {
            var userCredentials = await CredentialsRetriever.GetUserCredentials(userId);

            var webhook = TweetinviWebhooksHost.Configuration.RegisteredWebhookEnvironments.FirstOrDefault(x => x.Name == environment);

            if (webhook == null)
            {
                return "ENVIRONMENT_NOT_REGISTERED";
            }

            var activityStream = Stream.CreateAccountActivityStream(userId);
            TweetinviWebhooksHost.Configuration.AddActivityStream(activityStream);

            activityStream.TweetFavourited += (sender, args) =>
            {
                Console.WriteLine($"{userId} favourited tweet!");
            };

            return "SUBSCRIBED_ON_SERVER";
        }

        [HttpPost]
        public string UnsubscribeFromAccountActivities(string environment, string userId)
        {
            var webhook = TweetinviWebhooksHost.Configuration.RegisteredWebhookEnvironments.FirstOrDefault(x => x.Name == environment);

            if (webhook == null)
            {
                return "ENVIRONMENT_NOT_MATCHED";
            }

            var streams = TweetinviWebhooksHost.Configuration.RegisteredActivityStreams.Where(x => x.UserId.ToString() == userId);

            streams.ForEach(stream =>
            {
                TweetinviWebhooksHost.Configuration.RemoveActivityStream(stream);
            });

            return "UNSUBSCRIBED";
        }
    }
}
