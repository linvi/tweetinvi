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

        public Task<ITwitterResult<IWebhookDTO>> CreateAccountActivityWebhookAsync(ICreateAccountActivityWebhookParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _accountActivityQueryGenerator.GetCreateAccountActivityWebhookQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<IWebhookDTO>(request);
        }

        public Task<ITwitterResult<IGetAccountActivityWebhookEnvironmentsResultDTO>> GetAccountActivityWebhookEnvironmentsAsync(IGetAccountActivityWebhookEnvironmentsParameters parameters, ITwitterRequest request)
        {
            var consumerCredentials = new ConsumerCredentials(request.Query.TwitterCredentials);

            request.Query.Url = _accountActivityQueryGenerator.GetAccountActivityWebhookEnvironmentsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            request.Query.TwitterCredentials = new TwitterCredentials(consumerCredentials);

            return _twitterAccessor.ExecuteRequestAsync<IGetAccountActivityWebhookEnvironmentsResultDTO>(request);
        }

        public Task<ITwitterResult<IWebhookDTO[]>> GetAccountActivityEnvironmentWebhooksAsync(IGetAccountActivityEnvironmentWebhooksParameters parameters, ITwitterRequest request)
        {
            var consumerCredentials = new ConsumerCredentials(request.Query.TwitterCredentials);

            request.Query.Url = _accountActivityQueryGenerator.GetAccountActivityEnvironmentWebhooksQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            request.Query.TwitterCredentials = new TwitterCredentials(consumerCredentials);

            return _twitterAccessor.ExecuteRequestAsync<IWebhookDTO[]>(request);
        }

        public Task<ITwitterResult> DeleteAccountActivityWebhookAsync(IDeleteAccountActivityWebhookParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _accountActivityQueryGenerator.GetDeleteAccountActivityWebhookQuery(parameters);
            request.Query.HttpMethod = HttpMethod.DELETE;
            return _twitterAccessor.ExecuteRequestAsync(request);
        }

        public Task<ITwitterResult> TriggerAccountActivityWebhookCRCAsync(ITriggerAccountActivityWebhookCRCParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _accountActivityQueryGenerator.GetTriggerAccountActivityWebhookCRCQuery(parameters);
            request.Query.HttpMethod = HttpMethod.PUT;
            return _twitterAccessor.ExecuteRequestAsync(request);
        }

        public Task<ITwitterResult> SubscribeToAccountActivityAsync(ISubscribeToAccountActivityParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _accountActivityQueryGenerator.GetSubscribeToAccountActivityQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync(request);
        }

        public Task<ITwitterResult> UnsubscribeFromAccountActivityAsync(IUnsubscribeFromAccountActivityParameters parameters, ITwitterRequest request)
        {
            var consumerCredentials = new ConsumerCredentials(request.Query.TwitterCredentials);

            request.Query.Url = _accountActivityQueryGenerator.GetUnsubscribeToAccountActivityQuery(parameters);
            request.Query.HttpMethod = HttpMethod.DELETE;
            request.Query.TwitterCredentials = new TwitterCredentials(consumerCredentials);

            return _twitterAccessor.ExecuteRequestAsync(request);
        }

        public Task<ITwitterResult<IWebhookSubscriptionsCount>> CountAccountActivitySubscriptionsAsync(ICountAccountActivitySubscriptionsParameters parameters, ITwitterRequest request)
        {
            var consumerCredentials = new ConsumerCredentials(request.Query.TwitterCredentials);

            request.Query.Url = _accountActivityQueryGenerator.GetCountAccountActivitySubscriptionsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            request.Query.TwitterCredentials = new TwitterCredentials(consumerCredentials);

            return _twitterAccessor.ExecuteRequestAsync<IWebhookSubscriptionsCount>(request);
        }

        public Task<ITwitterResult> IsAccountSubscribedToAccountActivityAsync(IIsAccountSubscribedToAccountActivityParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _accountActivityQueryGenerator.GetIsAccountSubscribedToAccountActivityQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync(request);
        }

        public Task<ITwitterResult<IWebhookEnvironmentSubscriptionsDTO>> GetAccountActivitySubscriptionsAsync(IGetAccountActivitySubscriptionsParameters parameters, ITwitterRequest request)
        {
            var consumerCredentials = new ConsumerCredentials(request.Query.TwitterCredentials);

            request.Query.Url = _accountActivityQueryGenerator.GetAccountActivitySubscriptionsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            request.Query.TwitterCredentials = new TwitterCredentials(consumerCredentials);

            return _twitterAccessor.ExecuteRequestAsync<IWebhookEnvironmentSubscriptionsDTO>(request);
        }
    }
}