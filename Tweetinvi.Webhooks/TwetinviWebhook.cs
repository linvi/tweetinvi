using System.Threading.Tasks;
using Examplinvi.ASP.NET.Core;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Webhooks
{
    public static class TwetinviWebhook
    {
        public static IWebhookDTO RegisterWebhook(string webhookEnvironmentName)
        {
            //var result = TwitterAccessor.ExecutePOSTQuery<IWebhookDTO>(
            //    $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks.json");

            var webookRegistrationResult = TweetinviContainer.Resolve<IWebhookDTO>();

            return webookRegistrationResult;
        }

        public static IGetAllWebhooksResultDTO GetAllWebhooks(IConsumerCredentials credentials)
        {
            var result = TwitterAccessor.ExecuteGETQuery<IGetAllWebhooksResultDTO>(
                $"https://api.twitter.com/1.1/account_activity/all/webhooks.json");

            return result;
        }

        public static bool ChallengeWebhook(string webhookEnvironmentName, string webhookId)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks/{webhookId}.json";
            var result = TwitterAccessor.ExecuteQueryWithDetails(query, HttpMethod.PUT);

            return result.StatusCode != 214;
        }

        public static bool SubscribeToAllAuthenticatedUserEvents(string webhookEnvironmentName)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = TwitterAccessor.ExecuteQueryWithDetails(query, HttpMethod.POST);

            return result.StatusCode != 348;
        }

        public static IGetWebhookSubscriptionsCountResultDTO CountNumberOfSubscriptions()
        {
            var query = "https://api.twitter.com/1.1/account_activity/subscriptions/count.json";

            var result = TwitterAccessor.ExecuteQueryWithDetails(query, HttpMethod.GET);

            if (result.StatusCode == 32)
            {
                return null;
            }

            return TweetinviContainer.Resolve<IJsonObjectConverter>()
                .DeserializeObject<IGetWebhookSubscriptionsCountResultDTO>(result.Text);
        }

        public static bool DoesAuthenticatedHaveASubscription(string webhookEnvironmentName)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = TwitterAccessor.ExecuteQueryWithDetails(query, HttpMethod.POST);

            return result.StatusCode == 204;
        }

        public static IWebhookSubcriptionListDTO GetListOfSubscriptions(string webhookEnvironmentName)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions/list.json";

            var result = TwitterAccessor.ExecuteConsumerQuery(query, HttpMethod.POST, Auth.Credentials);

            return TweetinviContainer.Resolve<IJsonObjectConverter>()
                .DeserializeObject<IWebhookSubcriptionListDTO>(result.Text);
        }

        public static bool RemoveWebhook(string webhookEnvironmentName, string webhookId)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks/{webhookId}.json";

            var result = TwitterAccessor.ExecuteQueryWithDetails(query, HttpMethod.DELETE);

            return result.StatusCode == 204;
        }

        public static bool RemoveAllAuthenticatedUserSubscriptions(string webhookEnvironmentName)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = TwitterAccessor.ExecuteQueryWithDetails(query, HttpMethod.DELETE);

            return result.StatusCode == 204;
        }
    }
}
