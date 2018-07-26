using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Webhooks
{
    public interface ITweetinviWebhookController
    {
        IWebhookDTO RegisterWebhook(string webhookEnvironmentName, string url);
        IGetAllWebhooksResultDTO GetAllWebhooks();
        bool ChallengeWebhook(string webhookEnvironmentName, string webhookId);
        bool SubscribeToAllAuthenticatedUserEvents(string webhookEnvironmentName);
        IGetWebhookSubscriptionsCountResultDTO CountNumberOfSubscriptions();
        bool DoesAuthenticatedHaveASubscription(string webhookEnvironmentName);
        IWebhookSubcriptionListDTO GetListOfSubscriptions(string webhookEnvironmentName);
        bool RemoveWebhook(string webhookEnvironmentName, string webhookId);
        bool RemoveAllAuthenticatedUserSubscriptions(string webhookEnvironmentName);
    }

    public class TweetinviWebhookController : ITweetinviWebhookController
    {
        public IWebhookDTO RegisterWebhook(string webhookEnvironmentName, string url)
        {
            var result = TwitterAccessor.ExecutePOSTQuery<IWebhookDTO>(
                $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks.json?url={url}");


            return result;
        }

        public IGetAllWebhooksResultDTO GetAllWebhooks()
        {
            var result = TwitterAccessor.ExecuteGETQuery<IGetAllWebhooksResultDTO>(
                $"https://api.twitter.com/1.1/account_activity/all/webhooks.json");

            return result;
        }

        public bool ChallengeWebhook(string webhookEnvironmentName, string webhookId)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks/{webhookId}.json";
            var result = TwitterAccessor.ExecuteQueryWithDetails(query, HttpMethod.PUT);

            return result.StatusCode != 214;
        }

        public bool SubscribeToAllAuthenticatedUserEvents(string webhookEnvironmentName)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = TwitterAccessor.ExecuteQueryWithDetails(query, HttpMethod.POST);

            return result.StatusCode != 348;
        }

        public IGetWebhookSubscriptionsCountResultDTO CountNumberOfSubscriptions()
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

        public bool DoesAuthenticatedHaveASubscription(string webhookEnvironmentName)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = TwitterAccessor.ExecuteQueryWithDetails(query, HttpMethod.POST);

            return result.StatusCode == 204;
        }

        public IWebhookSubcriptionListDTO GetListOfSubscriptions(string webhookEnvironmentName)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions/list.json";

            var result = TwitterAccessor.ExecuteConsumerQuery(query, HttpMethod.POST, Auth.Credentials);

            return TweetinviContainer.Resolve<IJsonObjectConverter>()
                .DeserializeObject<IWebhookSubcriptionListDTO>(result.Text);
        }

        public bool RemoveWebhook(string webhookEnvironmentName, string webhookId)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks/{webhookId}.json";

            var result = TwitterAccessor.ExecuteQueryWithDetails(query, HttpMethod.DELETE);

            return result.StatusCode == 204;
        }

        public bool RemoveAllAuthenticatedUserSubscriptions(string webhookEnvironmentName)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = TwitterAccessor.ExecuteQueryWithDetails(query, HttpMethod.DELETE);

            return result.StatusCode == 204;
        }
    }
}
