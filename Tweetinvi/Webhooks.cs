using System;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi
{
    public static class Webhooks
    {
        [ThreadStatic]
        private static IWebhookController _webhookController;

        /// <summary>
        /// Factory creating Users
        /// </summary>
        public static IWebhookController WebhookController
        {
            get
            {
                if (_webhookController == null)
                {
                    Initialize();
                }

                return _webhookController;
            }
        }

        static Webhooks()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _webhookController = TweetinviContainer.Resolve<IWebhookController>();
        }

        public static Task<IWebhookDTO> RegisterWebhookAsync(string webhookEnvironmentName, string url, ITwitterCredentials credentials)
        {
            Sync.PrepareForAsync();
            return WebhookController.RegisterWebhookAsync(webhookEnvironmentName, url, credentials);
        }

        private static AsyncLocal<string> _myAsync = new AsyncLocal<string>();

        public static Task<IWebhookEnvironmentDTO[]> GetAllWebhookEnvironmentsAsync(IConsumerOnlyCredentials consumerCredentials)
        {
            return WebhookController.GetAllWebhooksAsync(consumerCredentials);
        }

        public static Task<bool> ChallengeWebhookAsync(string webhookEnvironmentName, string webhookId, ITwitterCredentials credentials)
        {
            return WebhookController.ChallengeWebhookAsync(webhookEnvironmentName, webhookId, credentials);
        }

        public static Task<bool> SubscribeToAccountActivityEventsAsync(string webhookEnvironmentName, ITwitterCredentials credentials)
        {
            return WebhookController.SubscribeToAllAuthenticatedUserEventsAsync(webhookEnvironmentName, credentials);
        }

        public static Task<IGetWebhookSubscriptionsCountResultDTO> CountNumberOfSubscriptionsAsync(IConsumerOnlyCredentials consumerCredentials)
        {
            return WebhookController.CountNumberOfSubscriptionsAsync(consumerCredentials);
        }

        public static Task<bool> DoesAccountHaveASubscriptionAsync(string webhookEnvironmentName, ITwitterCredentials credentials)
        {
            return WebhookController.DoesAccountHaveASubscriptionAsync(webhookEnvironmentName, credentials);
        }

        public static Task<IWebhookSubcriptionListDTO> GetListOfSubscriptionsAsync(string webhookEnvironmentName, IConsumerOnlyCredentials credentials)
        {
            return WebhookController.GetListOfSubscriptionsAsync(webhookEnvironmentName, credentials);
        }

        public static Task<bool> RemoveWebhookAsync(string webhookEnvironmentName, string webhookId, ITwitterCredentials credentials)
        {
            return WebhookController.RemoveWebhookAsync(webhookEnvironmentName, webhookId, credentials);
        }

        public static Task<bool> RemoveAllAccountSubscriptionsAsync(string webhookEnvironmentName, ITwitterCredentials credentials)
        {
            return WebhookController.RemoveAllAccountSubscriptionsAsync(webhookEnvironmentName, credentials);
        }
    }
}
