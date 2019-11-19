using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Webhooks;

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

        public Task<IWebhookDTO> RegisterWebhookAsync(string webhookEnvironmentName, string url, ITwitterCredentials credentials)
        {
            var encodedUrl = Uri.EscapeDataString(url);

            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks.json?url={encodedUrl}";
            return _twitterAccessor.ExecuteQuery<IWebhookDTO>(query, HttpMethod.POST, credentials, null);
        }

        public async Task<IWebhookEnvironmentDTO[]> GetAllWebhooksAsync(IConsumerOnlyCredentials consumerCredentials)
        {
            var query = "https://api.twitter.com/1.1/account_activity/all/webhooks.json";
            var result = await _twitterAccessor.ExecuteQuery<IGetAllWebhooksResultDTO>(query, HttpMethod.GET, consumerCredentials, null);

            result?.Environments?.ForEach(environment =>
            {
                environment.ConsumerCredentials = consumerCredentials;
            });

            return result?.Environments;
        }

        public async Task<bool> ChallengeWebhookAsync(string webhookEnvironmentName, string webhookId, ITwitterCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks/{webhookId}.json";
            var result = await _twitterAccessor.ExecuteQuery(query, HttpMethod.PUT, credentials);

            return result.StatusCode != 214;
        }

        public async Task<bool> SubscribeToAllAuthenticatedUserEventsAsync(string webhookEnvironmentName, ITwitterCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = await _twitterAccessor.ExecuteQuery(query, HttpMethod.POST, credentials);

            return result.StatusCode != 348;
        }

        public async Task<IGetWebhookSubscriptionsCountResultDTO> CountNumberOfSubscriptionsAsync(IConsumerOnlyCredentials credentials)
        {
            var query = "https://api.twitter.com/1.1/account_activity/subscriptions/count.json";

            var result = await _twitterAccessor.ExecuteQuery(query, HttpMethod.GET, credentials, null);

            if (result.StatusCode == 32)
            {
                return null;
            }

            var subscriptionsCount = _jsonObjectConverter.DeserializeObject<IGetWebhookSubscriptionsCountResultDTO>(result.Text);

            return subscriptionsCount;

        }

        public async Task<bool> DoesAccountHaveASubscriptionAsync(string webhookEnvironmentName, ITwitterCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = await _twitterAccessor.ExecuteQuery(query, HttpMethod.POST, credentials);

            return result.StatusCode == 204;
        }

        public async Task<IWebhookSubscriptionListDTO> GetListOfSubscriptionsAsync(string webhookEnvironmentName, IConsumerOnlyCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions/list.json";

            var result = await _twitterAccessor.ExecuteQuery(query, HttpMethod.GET, credentials);

            var subscriptions = _jsonObjectConverter.DeserializeObject<IWebhookSubscriptionListDTO>(result.Text);

            return subscriptions;
        }

        public async Task<bool> RemoveWebhookAsync(string webhookEnvironmentName, string webhookId, ITwitterCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/webhooks/{webhookId}.json";

            var result = await _twitterAccessor.ExecuteQuery(query, HttpMethod.DELETE, credentials);

            return result.StatusCode == 204;
        }

        public async Task<bool> RemoveAllAccountSubscriptionsAsync(string webhookEnvironmentName, ITwitterCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = await _twitterAccessor.ExecuteQuery(query, HttpMethod.DELETE, credentials);

            return result.StatusCode == 204;
        }
    }
}
