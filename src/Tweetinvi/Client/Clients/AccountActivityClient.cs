using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
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

        public IAccountActivityRequestHandler CreateRequestHandler()
        {
            return _client.CreateTwitterExecutionContext().Container.Resolve<IAccountActivityRequestHandler>();
        }

        public Task<IWebhook> CreateAccountActivityWebhook(string environment, string webhookUrl)
        {
            return CreateAccountActivityWebhook(new CreateAccountActivityWebhookParameters(environment, webhookUrl));
        }

        public async Task<IWebhook> CreateAccountActivityWebhook(ICreateAccountActivityWebhookParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.CreateAccountActivityWebhook(parameters).ConfigureAwait(false);
            return _client.Factories.CreateWebhook(twitterResult?.DataTransferObject);
        }

        public Task<IWebhookEnvironment[]> GetAccountActivityWebhookEnvironments()
        {
            return GetAccountActivityWebhookEnvironments(new GetAccountActivityWebhookEnvironmentsParameters());
        }

        public async Task<IWebhookEnvironment[]> GetAccountActivityWebhookEnvironments(IGetAccountActivityWebhookEnvironmentsParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.GetAccountActivityWebhookEnvironments(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject?.Environments.Select(x => _client.Factories.CreateWebhookEnvironment(x)).ToArray();
        }

        public Task<IWebhook[]> GetAccountActivityEnvironmentWebhooks(string environment)
        {
            return GetAccountActivityEnvironmentWebhooks(new GetAccountActivityEnvironmentWebhooksParameters(environment));
        }

        public async Task<IWebhook[]> GetAccountActivityEnvironmentWebhooks(IGetAccountActivityEnvironmentWebhooksParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.GetAccountActivityEnvironmentWebhooks(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject?.Select(x => _client.Factories.CreateWebhook(x)).ToArray();
        }

        public Task DeleteAccountActivityWebhook(string environment, string webhookId)
        {
            return DeleteAccountActivityWebhook(new DeleteAccountActivityWebhookParameters(environment, webhookId));
        }

        public Task DeleteAccountActivityWebhook(IDeleteAccountActivityWebhookParameters parameters)
        {
            return _accountActivityRequester.DeleteAccountActivityWebhook(parameters);
        }

        public Task TriggerAccountActivityWebhookCRC(string environment, string webhookId)
        {
            return TriggerAccountActivityWebhookCRC(new TriggerAccountActivityWebhookCRCParameters(environment, webhookId));
        }

        public Task TriggerAccountActivityWebhookCRC(ITriggerAccountActivityWebhookCRCParameters parameters)
        {
            return _accountActivityRequester.TriggerAccountActivityWebhookCRC(parameters);
        }

        public Task SubscribeToAccountActivity(string environment)
        {
            return SubscribeToAccountActivity(new SubscribeToAccountActivityParameters(environment));
        }

        public Task SubscribeToAccountActivity(ISubscribeToAccountActivityParameters parameters)
        {
            return _accountActivityRequester.SubscribeToAccountActivity(parameters);
        }

        public Task<IWebhookSubscriptionsCount> CountAccountActivitySubscriptions()
        {
            return CountAccountActivitySubscriptions(new CountAccountActivitySubscriptionsParameters());
        }

        public async Task<IWebhookSubscriptionsCount> CountAccountActivitySubscriptions(ICountAccountActivitySubscriptionsParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.CountAccountActivitySubscriptions(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public Task<bool> IsAccountSubscribedToAccountActivity(string environment)
        {
            return IsAccountSubscribedToAccountActivity(new IsAccountSubscribedToAccountActivityParameters(environment));
        }

        public async Task<bool> IsAccountSubscribedToAccountActivity(IIsAccountSubscribedToAccountActivityParameters parameters)
        {
            try
            {
                var twitterResult = await _accountActivityRequester.IsAccountSubscribedToAccountActivity(parameters).ConfigureAwait(false);
                return twitterResult.Response.StatusCode == 204;
            }
            catch (TwitterException)
            {
                return false;
            }
        }

        public Task<IWebhookEnvironmentSubscriptions> GetAccountActivitySubscriptions(string environment)
        {
            return GetAccountActivitySubscriptions(new GetAccountActivitySubscriptionsParameters(environment));
        }

        public async Task<IWebhookEnvironmentSubscriptions> GetAccountActivitySubscriptions(IGetAccountActivitySubscriptionsParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.GetAccountActivitySubscriptions(parameters).ConfigureAwait(false);
            return _client.Factories.CreateWebhookEnvironmentSubscriptions(twitterResult?.DataTransferObject);
        }

        public Task UnsubscribeFromAccountActivity(string environment, long userId)
        {
            return UnsubscribeFromAccountActivity(new UnsubscribeFromAccountActivityParameters(environment, userId));
        }

        public Task UnsubscribeFromAccountActivity(IUnsubscribeFromAccountActivityParameters parameters)
        {
            return _accountActivityRequester.UnsubscribeFromAccountActivity(parameters);
        }
    }
}