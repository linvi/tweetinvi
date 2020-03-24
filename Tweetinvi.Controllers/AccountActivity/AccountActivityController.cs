using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers
{
    public class AccountActivityController : IAccountActivityController
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IAccountActivityQueryGenerator _accountActivityQueryGenerator;

        public AccountActivityController(ITwitterAccessor twitterAccessor, IAccountActivityQueryGenerator accountActivityQueryGenerator)
        {
            _twitterAccessor = twitterAccessor;
            _accountActivityQueryGenerator = accountActivityQueryGenerator;
        }

        public Task<ITwitterResult<IWebhookDTO>> CreateAccountActivityWebhook(ICreateAccountActivityWebhookParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _accountActivityQueryGenerator.GetCreateAccountActivityWebhookQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<IWebhookDTO>(request);
        }

        public Task<ITwitterResult<IGetAccountActivityWebhookEnvironmentsResultDTO>> GetAccountActivityWebhookEnvironments(IGetAccountActivityWebhookEnvironmentsParameters parameters, ITwitterRequest request)
        {
            var consumerCredentials = new ConsumerCredentials(request.Query.TwitterCredentials);

            request.Query.Url = _accountActivityQueryGenerator.GetAccountActivityWebhookEnvironmentsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            request.Query.TwitterCredentials = new TwitterCredentials(consumerCredentials);

            return _twitterAccessor.ExecuteRequest<IGetAccountActivityWebhookEnvironmentsResultDTO>(request);
        }

        public Task<ITwitterResult<IWebhookDTO[]>> GetAccountActivityEnvironmentWebhooks(IGetAccountActivityEnvironmentWebhooksParameters parameters, ITwitterRequest request)
        {
            var consumerCredentials = new ConsumerCredentials(request.Query.TwitterCredentials);

            request.Query.Url = _accountActivityQueryGenerator.GetAccountActivityEnvironmentWebhooksQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            request.Query.TwitterCredentials = new TwitterCredentials(consumerCredentials);

            return _twitterAccessor.ExecuteRequest<IWebhookDTO[]>(request);
        }

        public Task<ITwitterResult> DeleteAccountActivityWebhook(IDeleteAccountActivityWebhookParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _accountActivityQueryGenerator.GetDeleteAccountActivityWebhookQuery(parameters);
            request.Query.HttpMethod = HttpMethod.DELETE;
            return _twitterAccessor.ExecuteRequest(request);
        }

        public Task<ITwitterResult> TriggerAccountActivityWebhookCRC(ITriggerAccountActivityWebhookCRCParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _accountActivityQueryGenerator.GetTriggerAccountActivityWebhookCRCQuery(parameters);
            request.Query.HttpMethod = HttpMethod.PUT;
            return _twitterAccessor.ExecuteRequest(request);
        }

        public Task<ITwitterResult> SubscribeToAccountActivity(ISubscribeToAccountActivityParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _accountActivityQueryGenerator.GetSubscribeToAccountActivityQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest(request);
        }

        public Task<ITwitterResult> UnsubscribeFromAccountActivity(IUnsubscribeFromAccountActivityParameters parameters, ITwitterRequest request)
        {
            var consumerCredentials = new ConsumerCredentials(request.Query.TwitterCredentials);

            request.Query.Url = _accountActivityQueryGenerator.GetUnsubscribeToAccountActivityQuery(parameters);
            request.Query.HttpMethod = HttpMethod.DELETE;
            request.Query.TwitterCredentials = new TwitterCredentials(consumerCredentials);

            return _twitterAccessor.ExecuteRequest(request);
        }

        public Task<ITwitterResult<IWebhookSubscriptionsCount>> CountAccountActivitySubscriptions(ICountAccountActivitySubscriptionsParameters parameters, ITwitterRequest request)
        {
            var consumerCredentials = new ConsumerCredentials(request.Query.TwitterCredentials);

            request.Query.Url = _accountActivityQueryGenerator.GetCountAccountActivitySubscriptionsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            request.Query.TwitterCredentials = new TwitterCredentials(consumerCredentials);

            return _twitterAccessor.ExecuteRequest<IWebhookSubscriptionsCount>(request);
        }

        public Task<ITwitterResult> IsAccountSubscribedToAccountActivity(IIsAccountSubscribedToAccountActivityParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _accountActivityQueryGenerator.GetIsAccountSubscribedToAccountActivityQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest(request);
        }

        public Task<ITwitterResult<IWebhookEnvironmentSubscriptionsDTO>> GetAccountActivitySubscriptions(IGetAccountActivitySubscriptionsParameters parameters, ITwitterRequest request)
        {
            var consumerCredentials = new ConsumerCredentials(request.Query.TwitterCredentials);

            request.Query.Url = _accountActivityQueryGenerator.GetAccountActivitySubscriptionsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            request.Query.TwitterCredentials = new TwitterCredentials(consumerCredentials);

            return _twitterAccessor.ExecuteRequest<IWebhookEnvironmentSubscriptionsDTO>(request);
        }
    }
}