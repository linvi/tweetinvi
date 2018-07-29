using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Webhooks
{
    public class WebhookController : IWebhookController
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly ICredentialsAccessor _credentialsAccessor;
        private readonly IJsonObjectConverter _jsonObjectConverter;

        public WebhookController(
            ITwitterAccessor twitterAccessor,
            ICredentialsAccessor credentialsAccessor,
            IJsonObjectConverter jsonObjectConverter)
        {
            _twitterAccessor = twitterAccessor;
            _credentialsAccessor = credentialsAccessor;
            _jsonObjectConverter = jsonObjectConverter;
        }

        public IWebhookDTO RegisterWebhook(string webhookEnvironmentName, string url)
        {
            var result = _twitterAccessor.ExecutePOSTQuery<IWebhookDTO>(
                $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks.json?url={url}");


            return result;
        }

        public IGetAllWebhooksResultDTO GetAllWebhooks()
        {
            var result = _twitterAccessor.ExecuteGETQuery<IGetAllWebhooksResultDTO>(
                $"https://api.twitter.com/1.1/account_activity/all/webhooks.json");

            return result;
        }

        public bool ChallengeWebhook(string webhookEnvironmentName, string webhookId)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks/{webhookId}.json";
            var result = _twitterAccessor.ExecuteQuery(query, HttpMethod.PUT);

            return result.StatusCode != 214;
        }

        public bool SubscribeToAllAuthenticatedUserEvents(string webhookEnvironmentName)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = _twitterAccessor.ExecuteQuery(query, HttpMethod.POST);

            return result.StatusCode != 348;
        }

        public IGetWebhookSubscriptionsCountResultDTO CountNumberOfSubscriptions()
        {
            var query = "https://api.twitter.com/1.1/account_activity/subscriptions/count.json";

            var result = _twitterAccessor.ExecuteQuery(query, HttpMethod.GET);

            if (result.StatusCode == 32)
            {
                return null;
            }

            return _jsonObjectConverter.DeserializeObject<IGetWebhookSubscriptionsCountResultDTO>(result.Text);
        }

        public bool DoesAuthenticatedHaveASubscription(string webhookEnvironmentName)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = _twitterAccessor.ExecuteQuery(query, HttpMethod.POST);

            return result.StatusCode == 204;
        }

        public IWebhookSubcriptionListDTO GetListOfSubscriptions(string webhookEnvironmentName)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions/list.json";

            var result = _twitterAccessor.ExecuteConsumerQuery(query, HttpMethod.POST, null, _credentialsAccessor.CurrentThreadCredentials);

            return _jsonObjectConverter.DeserializeObject<IWebhookSubcriptionListDTO>(result.Text);
        }

        public bool RemoveWebhook(string webhookEnvironmentName, string webhookId)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks/{webhookId}.json";

            var result = _twitterAccessor.ExecuteQuery(query, HttpMethod.DELETE);

            return result.StatusCode == 204;
        }

        public bool RemoveAllAuthenticatedUserSubscriptions(string webhookEnvironmentName)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = _twitterAccessor.ExecuteQuery(query, HttpMethod.DELETE);

            return result.StatusCode == 204;
        }
    }
}
