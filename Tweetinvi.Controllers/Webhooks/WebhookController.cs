using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Public.Models.Authentication;
using Tweetinvi.Core.Public.Models.Interfaces.DTO.Webhooks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Controllers.Webhooks
{
    public class WebhookController : IWebhookController
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IJsonObjectConverter _jsonObjectConverter;

        public WebhookController(
            ITwitterAccessor twitterAccessor,
            IJsonObjectConverter jsonObjectConverter)
        {
            _twitterAccessor = twitterAccessor;
            _jsonObjectConverter = jsonObjectConverter;
        }

        public async Task<IWebhookDTO> RegisterWebhookAsync(string webhookEnvironmentName, string url, ITwitterCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks.json?url={url}";
            var result = _twitterAccessor.ExecuteQuery<IWebhookDTO>(query, HttpMethod.POST, credentials, null);

            return await Task.FromResult(result);
        }

        public async Task<IWebhookEnvironmentDTO[]> GetAllWebhooksAsync(IConsumerOnlyCredentials consumerCredentials)
        {
            var query = "https://api.twitter.com/1.1/account_activity/all/webhooks.json";
            var result = _twitterAccessor.ExecuteQuery<IGetAllWebhooksResultDTO>(query, HttpMethod.GET, consumerCredentials, null);

            result?.Environments?.ForEach(environment =>
            {
                environment.ConsumerCredentials = consumerCredentials;
            });

            return await Task.FromResult(result?.Environments);
        }

        public async Task<bool> ChallengeWebhookAsync(string webhookEnvironmentName, string webhookId, ITwitterCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks/{webhookId}.json";
            var result = _twitterAccessor.ExecuteQuery(query, HttpMethod.PUT, credentials);

            return await Task.FromResult(result.StatusCode != 214);
        }

        public async Task<bool> SubscribeToAllAuthenticatedUserEventsAsync(
            string webhookEnvironmentName,
            ITwitterCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = _twitterAccessor.ExecuteQuery(query, HttpMethod.POST, credentials);

            return await Task.FromResult(result.StatusCode != 348);
        }

        public async Task<IGetWebhookSubscriptionsCountResultDTO> CountNumberOfSubscriptionsAsync(
            IConsumerOnlyCredentials credentials)
        {
            var query = "https://api.twitter.com/1.1/account_activity/subscriptions/count.json";

            var result = _twitterAccessor.ExecuteQuery(query, HttpMethod.GET, credentials, null);

            if (result.StatusCode == 32)
            {
                return null;
            }

            var subscriptionsCount = _jsonObjectConverter.DeserializeObject<IGetWebhookSubscriptionsCountResultDTO>(result.Text);

            return await Task.FromResult(subscriptionsCount);
        }

        public async Task<bool> DoesAccountHaveASubscriptionAsync(
            string webhookEnvironmentName,
            ITwitterCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = _twitterAccessor.ExecuteQuery(query, HttpMethod.POST, credentials);

            return await Task.FromResult(result.StatusCode == 204);
        }

        public async Task<IWebhookSubcriptionListDTO> GetListOfSubscriptionsAsync(
            string webhookEnvironmentName,
            IConsumerOnlyCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions/list.json";

            var result = _twitterAccessor.ExecuteQuery(query, HttpMethod.GET, credentials);

            var subscriptions = _jsonObjectConverter.DeserializeObject<IWebhookSubcriptionListDTO>(result.Text);

            return await Task.FromResult(subscriptions);
        }

        public async Task<bool> RemoveWebhookAsync(string webhookEnvironmentName, string webhookId, ITwitterCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks/{webhookId}.json";

            var result = _twitterAccessor.ExecuteQuery(query, HttpMethod.DELETE, credentials);

            return await Task.FromResult(result.StatusCode == 204);
        }

        public async Task<bool> RemoveAllAccountSubscriptionsAsync(
            string webhookEnvironmentName,
            ITwitterCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = _twitterAccessor.ExecuteQuery(query, HttpMethod.DELETE, credentials);

            return await Task.FromResult(result.StatusCode == 204);
        }
    }
}
