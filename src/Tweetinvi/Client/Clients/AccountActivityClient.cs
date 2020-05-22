using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class AccountActivityClient : IAccountActivityClient
    {
        private readonly IAccountActivityRequester _accountActivityRequester;
        private readonly ITwitterClient _client;

        public AccountActivityClient(
            IAccountActivityRequester accountActivityRequester,
            ITwitterClient client)
        {
            _accountActivityRequester = accountActivityRequester;
            _client = client;
        }

        public IAccountActivityClientParametersValidator ParametersValidator => _client.ParametersValidator;

        public IAccountActivityRequestHandler CreateRequestHandler()
        {
            return _client.CreateTwitterExecutionContext().Container.Resolve<IAccountActivityRequestHandler>();
        }

        public Task<IWebhook> CreateAccountActivityWebhookAsync(string environment, string webhookUrl)
        {
            return CreateAccountActivityWebhookAsync(new CreateAccountActivityWebhookParameters(environment, webhookUrl));
        }

        public async Task<IWebhook> CreateAccountActivityWebhookAsync(ICreateAccountActivityWebhookParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.CreateAccountActivityWebhookAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateWebhook(twitterResult?.Model);
        }

        public Task<IWebhookEnvironment[]> GetAccountActivityWebhookEnvironmentsAsync()
        {
            return GetAccountActivityWebhookEnvironmentsAsync(new GetAccountActivityWebhookEnvironmentsParameters());
        }

        public async Task<IWebhookEnvironment[]> GetAccountActivityWebhookEnvironmentsAsync(IGetAccountActivityWebhookEnvironmentsParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.GetAccountActivityWebhookEnvironmentsAsync(parameters).ConfigureAwait(false);
            return twitterResult?.Model?.Environments.Select(x => _client.Factories.CreateWebhookEnvironment(x)).ToArray();
        }

        public Task<IWebhook[]> GetAccountActivityEnvironmentWebhooksAsync(string environment)
        {
            return GetAccountActivityEnvironmentWebhooksAsync(new GetAccountActivityEnvironmentWebhooksParameters(environment));
        }

        public async Task<IWebhook[]> GetAccountActivityEnvironmentWebhooksAsync(IGetAccountActivityEnvironmentWebhooksParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.GetAccountActivityEnvironmentWebhooksAsync(parameters).ConfigureAwait(false);
            return twitterResult?.Model?.Select(x => _client.Factories.CreateWebhook(x)).ToArray();
        }

        public Task DeleteAccountActivityWebhookAsync(string environment, string webhookId)
        {
            return DeleteAccountActivityWebhookAsync(new DeleteAccountActivityWebhookParameters(environment, webhookId));
        }

        public Task DeleteAccountActivityWebhookAsync(string environment, IWebhook webhook)
        {
            return DeleteAccountActivityWebhookAsync(new DeleteAccountActivityWebhookParameters(environment, webhook.Id));
        }

        public Task DeleteAccountActivityWebhookAsync(IDeleteAccountActivityWebhookParameters parameters)
        {
            return _accountActivityRequester.DeleteAccountActivityWebhookAsync(parameters);
        }

        public Task TriggerAccountActivityWebhookCRCAsync(string environment, string webhookId)
        {
            return TriggerAccountActivityWebhookCRCAsync(new TriggerAccountActivityWebhookCRCParameters(environment, webhookId));
        }

        public Task TriggerAccountActivityWebhookCRCAsync(ITriggerAccountActivityWebhookCRCParameters parameters)
        {
            return _accountActivityRequester.TriggerAccountActivityWebhookCRCAsync(parameters);
        }

        public Task SubscribeToAccountActivityAsync(string environment)
        {
            return SubscribeToAccountActivityAsync(new SubscribeToAccountActivityParameters(environment));
        }

        public Task SubscribeToAccountActivityAsync(ISubscribeToAccountActivityParameters parameters)
        {
            return _accountActivityRequester.SubscribeToAccountActivityAsync(parameters);
        }

        public Task<IWebhookSubscriptionsCount> CountAccountActivitySubscriptionsAsync()
        {
            return CountAccountActivitySubscriptionsAsync(new CountAccountActivitySubscriptionsParameters());
        }

        public async Task<IWebhookSubscriptionsCount> CountAccountActivitySubscriptionsAsync(ICountAccountActivitySubscriptionsParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.CountAccountActivitySubscriptionsAsync(parameters).ConfigureAwait(false);
            return twitterResult?.Model;
        }

        public Task<bool> IsAccountSubscribedToAccountActivityAsync(string environment)
        {
            return IsAccountSubscribedToAccountActivityAsync(new IsAccountSubscribedToAccountActivityParameters(environment));
        }

        public async Task<bool> IsAccountSubscribedToAccountActivityAsync(IIsAccountSubscribedToAccountActivityParameters parameters)
        {
            try
            {
                var twitterResult = await _accountActivityRequester.IsAccountSubscribedToAccountActivityAsync(parameters).ConfigureAwait(false);
                return twitterResult.Response.StatusCode == 204;
            }
            catch (TwitterException)
            {
                return false;
            }
        }

        public Task<IWebhookEnvironmentSubscriptions> GetAccountActivitySubscriptionsAsync(string environment)
        {
            return GetAccountActivitySubscriptionsAsync(new GetAccountActivitySubscriptionsParameters(environment));
        }

        public async Task<IWebhookEnvironmentSubscriptions> GetAccountActivitySubscriptionsAsync(IGetAccountActivitySubscriptionsParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.GetAccountActivitySubscriptionsAsync(parameters).ConfigureAwait(false);
            return _client.Factories.CreateWebhookEnvironmentSubscriptions(twitterResult?.Model);
        }

        public Task UnsubscribeFromAccountActivityAsync(string environment, long userId)
        {
            return UnsubscribeFromAccountActivityAsync(new UnsubscribeFromAccountActivityParameters(environment, userId));
        }

        public Task UnsubscribeFromAccountActivityAsync(IUnsubscribeFromAccountActivityParameters parameters)
        {
            return _accountActivityRequester.UnsubscribeFromAccountActivityAsync(parameters);
        }
    }
}