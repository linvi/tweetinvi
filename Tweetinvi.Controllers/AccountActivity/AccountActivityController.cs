using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers
{
    public class AccountActivityController : IAccountActivityController
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IWebhooksQueryGenerator _webhooksQueryGenerator;
        private readonly IJsonObjectConverter _jsonObjectConverter;

        public AccountActivityController(
            ITwitterAccessor twitterAccessor,
            IWebhooksQueryGenerator webhooksQueryGenerator,
            IJsonObjectConverter jsonObjectConverter)
        {
            _twitterAccessor = twitterAccessor;
            _webhooksQueryGenerator = webhooksQueryGenerator;
            _jsonObjectConverter = jsonObjectConverter;
        }

        public Task<ITwitterResult<IWebhookDTO>> RegisterAccountActivityWebhook(IRegisterAccountActivityWebhookParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _webhooksQueryGenerator.GetRegisterAccountActivityWebhookQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<IWebhookDTO>(request);
        }

        public Task<ITwitterResult<IGetAccountActivityWebhookEnvironmentsResultDTO>> GetAccountActivityWebhookEnvironments(IGetAccountActivityWebhookEnvironmentsParameters parameters, ITwitterRequest request)
        {
            var consumerCredentials = new ConsumerCredentials(request.Query.TwitterCredentials);

            request.Query.Url = _webhooksQueryGenerator.GetAccountActivityWebhookEnvironmentsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            request.Query.TwitterCredentials = new TwitterCredentials(consumerCredentials);

            return _twitterAccessor.ExecuteRequest<IGetAccountActivityWebhookEnvironmentsResultDTO>(request);
        }

        public Task<ITwitterResult> RemoveAccountActivityWebhook(IRemoveAccountActivityWebhookParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _webhooksQueryGenerator.GetRemoveAccountActivityWebhookQuery(parameters);
            request.Query.HttpMethod = HttpMethod.DELETE;
            return _twitterAccessor.ExecuteRequest(request);
        }

        public Task<ITwitterResult> TriggerAccountActivityCRC(ITriggerAccountActivityCRCParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _webhooksQueryGenerator.GetTriggerAccountActivityCRCQuery(parameters);
            request.Query.HttpMethod = HttpMethod.PUT;
            return _twitterAccessor.ExecuteRequest(request);
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

        public async Task<bool> RemoveAllAccountSubscriptionsAsync(string webhookEnvironmentName, ITwitterCredentials credentials)
        {
            var query = $"https://api.twitter.com/1.1/account_activity/all/{webhookEnvironmentName}/subscriptions.json";

            var result = await _twitterAccessor.ExecuteQuery(query, HttpMethod.DELETE, credentials);

            return result.StatusCode == 204;
        }
    }
}
