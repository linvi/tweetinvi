using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class AccountActivityClient : IAccountActivityClient
    {
        private readonly IAccountActivityRequester _accountActivityRequester;

        public AccountActivityClient(IAccountActivityRequester accountActivityRequester)
        {
            _accountActivityRequester = accountActivityRequester;
        }

        public Task<IWebhookDTO> RegisterAccountActivityWebhook(string environment, string webhookUrl)
        {
            return RegisterAccountActivityWebhook(new RegisterAccountActivityWebhookParameters(environment, webhookUrl));
        }

        public async Task<IWebhookDTO> RegisterAccountActivityWebhook(IRegisterAccountActivityWebhookParameters parameters)
        {
            var twitterResult = await _accountActivityRequester.RegisterAccountActivityWebhook(parameters).ConfigureAwait(false);
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

        public Task RemoveAccountActivityWebhook(string environment, string webhookId)
        {
            return RemoveAccountActivityWebhook(new RemoveAccountActivityWebhookParameters(environment, webhookId));
        }

        public async Task RemoveAccountActivityWebhook(IRemoveAccountActivityWebhookParameters parameters)
        {
            await _accountActivityRequester.RemoveAccountActivityWebhook(parameters).ConfigureAwait(false);
        }
    }
}