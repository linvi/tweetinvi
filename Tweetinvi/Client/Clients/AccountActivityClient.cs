using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.Webhooks;
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

        public Task<IWebhookDTO> CreateAccountActivityWebhook(string environment, string webhookUrl)
        {
            return CreateAccountActivityWebhook(new CreateAccountActivityWebhookParameters(environment, webhookUrl));
        }

        public async Task<IWebhookDTO> CreateAccountActivityWebhook(ICreateAccountActivityWebhookParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.CreateAccountActivityWebhook(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public Task<IWebhookEnvironmentDTO[]> GetAccountActivityWebhookEnvironments()
        {
            return GetAccountActivityWebhookEnvironments(new GetAccountActivityWebhookEnvironmentsParameters());
        }

        public async Task<IWebhookEnvironmentDTO[]> GetAccountActivityWebhookEnvironments(IGetAccountActivityWebhookEnvironmentsParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.GetAccountActivityWebhookEnvironments(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject?.Environments;
        }

        public Task<IWebhookDTO[]> GetAccountActivityEnvironmentWebhooks(string environment)
        {
            return GetAccountActivityEnvironmentWebhooks(new GetAccountActivityEnvironmentWebhooksParameters(environment));
        }

        public async Task<IWebhookDTO[]> GetAccountActivityEnvironmentWebhooks(IGetAccountActivityEnvironmentWebhooksParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.GetAccountActivityEnvironmentWebhooks(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public Task DeleteAccountActivityWebhook(string environment, string webhookId)
        {
            return DeleteAccountActivityWebhook(new DeleteAccountActivityWebhookParameters(environment, webhookId));
        }

        public async Task DeleteAccountActivityWebhook(IDeleteAccountActivityWebhookParameters parameters)
        {
            await _accountActivityRequester.DeleteAccountActivityWebhook(parameters).ConfigureAwait(false);
        }

        public Task TriggerAccountActivityWebhookCRC(string environment, string webhookId)
        {
            return TriggerAccountActivityWebhookCRC(new TriggerAccountActivityWebhookCRCParameters(environment, webhookId));
        }

        public async Task TriggerAccountActivityWebhookCRC(ITriggerAccountActivityWebhookCRCParameters parameters)
        {
            await _accountActivityRequester.TriggerAccountActivityWebhookCRC(parameters).ConfigureAwait(false);
        }

        public Task SubscribeToAccountActivity(string environment)
        {
            return SubscribeToAccountActivity(new SubscribeToAccountActivityParameters(environment));
        }

        public async Task SubscribeToAccountActivity(ISubscribeToAccountActivityParameters parameters)
        {
            await _accountActivityRequester.SubscribeToAccountActivity(parameters).ConfigureAwait(false);
        }

        public Task<IGetWebhookSubscriptionsCountResultDTO> CountAccountActivitySubscriptions()
        {
            return CountAccountActivitySubscriptions(new CountAccountActivitySubscriptionsParameters());
        }

        public async Task<IGetWebhookSubscriptionsCountResultDTO> CountAccountActivitySubscriptions(ICountAccountActivitySubscriptionsParameters parameters)
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
                var twitterResult = await _accountActivityRequester.IsAccountSubscribedToAccountActivity(parameters);
                return twitterResult.Response.StatusCode == 204;
            }
            catch (TwitterException)
            {
                return false;
            }
        }

        public Task<IWebhookSubscriptionListDTO> GetAccountActivitySubscriptions(string environment)
        {
            return GetAccountActivitySubscriptions(new GetAccountActivitySubscriptionsParameters(environment));
        }

        public async Task<IWebhookSubscriptionListDTO> GetAccountActivitySubscriptions(IGetAccountActivitySubscriptionsParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.GetAccountActivitySubscriptions(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }

        public Task UnsubscribeFromAccountActivity(string environment, long? userId)
        {
            return UnsubscribeFromAccountActivity(new UnsubscribeFromAccountActivityParameters(environment, userId));
        }

        public async Task UnsubscribeFromAccountActivity(IUnsubscribeFromAccountActivityParameters parameters)
        {
            await _accountActivityRequester.UnsubscribeFromAccountActivity(parameters).ConfigureAwait(false);
        }
    }
}