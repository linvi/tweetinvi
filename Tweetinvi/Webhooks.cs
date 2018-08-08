using System;
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

        public static async Task<IWebhookDTO> RegisterWebhookAsync(string webhookEnvironmentName, string url, ITwitterCredentials credentials)
        {
            return await WebhookController.RegisterWebhookAsync(webhookEnvironmentName, url, credentials);
        }

        public static async Task<IWebhookEnvironmentDTO[]> GetAllWebhookEnvironmentsAsync(IConsumerOnlyCredentials consumerCredentials)
        {
            return await WebhookController.GetAllWebhooksAsync(consumerCredentials);
        }

        public static async Task<bool> ChallengeWebhookAsync(string webhookEnvironmentName, string webhookId, ITwitterCredentials credentials)
        {
            return await WebhookController.ChallengeWebhookAsync(webhookEnvironmentName, webhookId, credentials);
        }

        public static async Task<bool> SubscribeToAllAuthenticatedUserEventsAsync(string webhookEnvironmentName)
        {
            return await WebhookController.SubscribeToAllAuthenticatedUserEventsAsync(webhookEnvironmentName);
        }

        public static async Task<IGetWebhookSubscriptionsCountResultDTO> CountNumberOfSubscriptionsAsync()
        {
            return await WebhookController.CountNumberOfSubscriptionsAsync();
        }

        public static async Task<bool> DoesAuthenticatedHaveASubscription(string webhookEnvironmentName)
        {
            return await WebhookController.DoesAuthenticatedHaveASubscriptionAsync(webhookEnvironmentName);
        }

        public static async Task<IWebhookSubcriptionListDTO> GetListOfSubscriptionsAsync(string webhookEnvironmentName, IConsumerOnlyCredentials credentials)
        {
            return await WebhookController.GetListOfSubscriptionsAsync(webhookEnvironmentName, credentials);
        }

        public static async Task<bool> RemoveWebhook(string webhookEnvironmentName, string webhookId)
        {
            return await WebhookController.RemoveWebhookAsync(webhookEnvironmentName, webhookId);
        }

        public static async Task<bool> RemoveAllAuthenticatedUserSubscriptions(string webhookEnvironmentName)
        {
            return await WebhookController.RemoveAllAuthenticatedUserSubscriptionsAsync(webhookEnvironmentName);
        }
    }
}
