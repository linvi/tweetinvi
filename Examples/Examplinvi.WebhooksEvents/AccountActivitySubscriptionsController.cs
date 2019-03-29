using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Models;

namespace Examplinvi.WebhooksEvents
{
    public class AccountActivitySubscriptionsController
    {
        private readonly IWebhookConfiguration _webhookConfiguration;
        private readonly AccountActivityEventsManager _accountActivityEventsManager;

        public AccountActivitySubscriptionsController(IWebhookConfiguration webhookConfiguration)
        {
            _webhookConfiguration = webhookConfiguration;
            _accountActivityEventsManager = new AccountActivityEventsManager();
        }

        public async Task StartAllAccountActivityStreams(string environment)
        {
            // ReSharper disable once SimplifyLinqExpression
            if (!_webhookConfiguration.RegisteredWebhookEnvironments.Any(x => x.Name == environment))
            {
                throw new InvalidOperationException("You attempted to listen to streams for an environment that was not registered");
            }

            var webhooksSubscriptions = await GetWebhookSubscriptions(environment);

            webhooksSubscriptions.ForEach(subscription =>
            {
                var accountActivityStream = _webhookConfiguration.RegisteredActivityStreams.SingleOrDefault(x => x.AccountUserId.ToString() == subscription.UserId);

                if (accountActivityStream == null)
                {
                    accountActivityStream = Stream.CreateAccountActivityStream(subscription.UserId);
                    _webhookConfiguration.AddActivityStream(accountActivityStream);
                }

                _accountActivityEventsManager.RegisterAccountActivityStream(accountActivityStream);
            });
        }

        public async Task<IWebhookSubscriptionDTO[]> GetWebhookSubscriptions(string environment)
        {
            var webhookEnvironments = await Webhooks.GetListOfSubscriptionsAsync(environment, _webhookConfiguration.ConsumerOnlyCredentials);
            return webhookEnvironments.Subscriptions;
        }
    }
}
