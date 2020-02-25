using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;

namespace Tweetinvi
{
    public static class Webhooks
    {
        [ThreadStatic]
        private static IAccountActivityController _accountActivityController;

        /// <summary>
        /// Factory creating Users
        /// </summary>
        public static IAccountActivityController AccountActivityController
        {
            get
            {
                if (_accountActivityController == null)
                {
                    Initialize();
                }

                return _accountActivityController;
            }
        }

        static Webhooks()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _accountActivityController = TweetinviContainer.Resolve<IAccountActivityController>();
        }

        public static async Task<bool> SubscribeToAccountActivityEventsAsync(string webhookEnvironmentName, ITwitterCredentials credentials)
        {
            return await AccountActivityController.SubscribeToAllAuthenticatedUserEventsAsync(webhookEnvironmentName, credentials);
        }

        public static async Task<IGetWebhookSubscriptionsCountResultDTO> CountNumberOfSubscriptionsAsync(IConsumerOnlyCredentials consumerCredentials)
        {
            return await AccountActivityController.CountNumberOfSubscriptionsAsync(consumerCredentials);
        }

        public static async Task<bool> DoesAccountHaveASubscriptionAsync(string webhookEnvironmentName, ITwitterCredentials credentials)
        {
            return await AccountActivityController.DoesAccountHaveASubscriptionAsync(webhookEnvironmentName, credentials);
        }

        public static async Task<IWebhookSubscriptionListDTO> GetListOfSubscriptionsAsync(string webhookEnvironmentName, IConsumerOnlyCredentials credentials)
        {
            return await AccountActivityController.GetListOfSubscriptionsAsync(webhookEnvironmentName, credentials);
        }

        public static async Task<bool> RemoveAllAccountSubscriptionsAsync(string webhookEnvironmentName, ITwitterCredentials credentials)
        {
            return await AccountActivityController.RemoveAllAccountSubscriptionsAsync(webhookEnvironmentName, credentials);
        }
    }
}
