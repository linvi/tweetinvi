using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;

namespace Examplinvi.AccountActivityEvents.Controllers
{
    public class AccountActivityEventsController
    {
        private readonly IWebhookConfiguration _webhookConfiguration;
        private readonly AccountActivityEventsManager _accountActivityEventsManager;

        public AccountActivityEventsController(IWebhookConfiguration webhookConfiguration)
        {
            _webhookConfiguration = webhookConfiguration;
            _accountActivityEventsManager = new AccountActivityEventsManager();
        }

        // EVENT

        public async Task<string> StartListeningToEventsForAllSubscribedAccounts(string environment)
        {
            // ReSharper disable once SimplifyLinqExpression
            if (!_webhookConfiguration.RegisteredWebhookEnvironments.Any(x => x.Name == environment))
            {
                throw new InvalidOperationException("You attempted to listen to streams for an environment that was not registered");
            }

            var subscriptionInfos = await Webhooks.GetListOfSubscriptionsAsync(environment, _webhookConfiguration.ConsumerOnlyCredentials);
            var subscriptions = subscriptionInfos.Subscriptions;

            subscriptions.ForEach(subscription =>
            {
                var accountActivityStream = _webhookConfiguration.RegisteredActivityStreams.SingleOrDefault(x => x.AccountUserId.ToString() == subscription.UserId);
                var isSubscriptionAlreadyWatched = accountActivityStream != null;

                if (isSubscriptionAlreadyWatched)
                {
                    accountActivityStream = Stream.CreateAccountActivityStream(subscription.UserId);
                    _webhookConfiguration.AddActivityStream(accountActivityStream);
                }

                _accountActivityEventsManager.RegisterAccountActivityStream(accountActivityStream);
            });


            return $"SERVER IS NOW WATCHING EVENTS FOR ALL SUBSCRIPTIONS ON ENVIRONMENT : {environment}";
        }

        public async Task<string> StopAllAccountActivityStreams(string environment)
        {
            var subscriptionInfos = await Webhooks.GetListOfSubscriptionsAsync(environment, _webhookConfiguration.ConsumerOnlyCredentials);
            var subscriptions = subscriptionInfos.Subscriptions;

            var subscribedAccountUserIds = subscriptions.Select(x => x.UserId);

            // BUG #842: a problem here is that if an account is subscribed to 2 different environments, then we will delete the listening of both subscriptions
            // BUG #842: as we base the deletion on only the AccountUserId.
            var accountActivityStreamsToStop = _webhookConfiguration.RegisteredActivityStreams.Where(x => subscribedAccountUserIds.Contains(x.AccountUserId.ToString()));

            accountActivityStreamsToStop.ForEach(accountActivityStream =>
            {
                _accountActivityEventsManager.UnregisterAccountActivityStream(accountActivityStream);
                _webhookConfiguration.RemoveActivityStream(accountActivityStream);
            });

            return $"SERVER HAS STOPPED WATCHING EVENTS FOR ALL SUBSCRIPTIONS ON ENVIRONMENT : {environment}";
        }

        public async Task<string> SubscribeToAccountActivities(string environment, long userId)
        {
            var webhookEnvironment = _webhookConfiguration.RegisteredWebhookEnvironments.FirstOrDefault(x => x.Name == environment);
            var doesWebhookEnvironmentExist = webhookEnvironment != null;

            if (doesWebhookEnvironmentExist)
            {
                return "ENVIRONMENT_NOT_REGISTERED";
            }

            var activityStream = Stream.CreateAccountActivityStream(userId);
            _webhookConfiguration.AddActivityStream(activityStream);

            _accountActivityEventsManager.RegisterAccountActivityStream(activityStream);

            return "SUBSCRIBED_ON_SERVER";
        }

        public async Task<string> UnsubscribeFromAccountActivities(string environment, string userId)
        {
            var webhookEnvironment = _webhookConfiguration.RegisteredWebhookEnvironments.FirstOrDefault(x => x.Name == environment);
            var doesWebhookEnvironmentExist = webhookEnvironment != null;

            if (doesWebhookEnvironmentExist)
            {
                return "ENVIRONMENT_NOT_REGISTERED";
            }

            var accountActivityWebhook = _webhookConfiguration.RegisteredActivityStreams.Where(x => x.AccountUserId.ToString() == userId);

            accountActivityWebhook.ForEach(stream =>
            {
                _webhookConfiguration.RemoveActivityStream(stream);

                _accountActivityEventsManager.UnregisterAccountActivityStream(stream);
            });

            return "UNSUBSCRIBED";
        }
    }
}
