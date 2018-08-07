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

        public static bool SubscribeToAllAuthenticatedUserEvents(string webhookEnvironmentName)
        {
            return WebhookController.SubscribeToAllAuthenticatedUserEvents(webhookEnvironmentName);
        }

        public static IGetWebhookSubscriptionsCountResultDTO CountNumberOfSubscriptions()
        {
            return WebhookController.CountNumberOfSubscriptions();
        }

        public static bool DoesAuthenticatedHaveASubscription(string webhookEnvironmentName)
        {
            return WebhookController.DoesAuthenticatedHaveASubscription(webhookEnvironmentName);
        }

        public static IWebhookSubcriptionListDTO GetListOfSubscriptions(string webhookEnvironmentName, IConsumerOnlyCredentials credentials)
        {
            return WebhookController.GetListOfSubscriptions(webhookEnvironmentName, credentials);
        }

        public static bool RemoveWebhook(string webhookEnvironmentName, string webhookId)
        {
            return WebhookController.RemoveWebhook(webhookEnvironmentName, webhookId);
        }

        public static bool RemoveAllAuthenticatedUserSubscriptions(string webhookEnvironmentName)
        {
            return WebhookController.RemoveAllAuthenticatedUserSubscriptions(webhookEnvironmentName);
        }
    }
}
