using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class AccountActivityRequester : BaseRequester, IAccountActivityRequester
    {
        private readonly IAccountActivityController _accountActivityController;

        public AccountActivityRequester(
            ITwitterClient client,
            ITwitterClientEvents twitterClientEvents,
            IAccountActivityController accountActivityController)
            : base(client, twitterClientEvents)
        {
            _accountActivityController = accountActivityController;
        }

        public Task<ITwitterResult<IWebhookDTO>> CreateAccountActivityWebhook(ICreateAccountActivityWebhookParameters parameters)
        {
            return ExecuteRequest(async request =>
            {
                var result = await _accountActivityController.CreateAccountActivityWebhook(parameters, request).ConfigureAwait(false);
                return result;
            });
        }

        public Task<ITwitterResult<IGetAccountActivityWebhookEnvironmentsResultDTO>> GetAccountActivityWebhookEnvironments(IGetAccountActivityWebhookEnvironmentsParameters parameters)
        {
            return ExecuteRequest(request =>
            {
                return _accountActivityController.GetAccountActivityWebhookEnvironments(parameters, request);
            });
        }

        public Task<ITwitterResult<IWebhookDTO[]>> GetAccountActivityEnvironmentWebhooks(IGetAccountActivityEnvironmentWebhooksParameters parameters)
        {
            return ExecuteRequest(request =>
            {
                return _accountActivityController.GetAccountActivityEnvironmentWebhooks(parameters, request);
            });
        }

        public Task<ITwitterResult> DeleteAccountActivityWebhook(IDeleteAccountActivityWebhookParameters parameters)
        {
            return ExecuteRequest(request => _accountActivityController.DeleteAccountActivityWebhook(parameters, request));
        }

        public Task<ITwitterResult> TriggerAccountActivityWebhookCRC(ITriggerAccountActivityWebhookCRCParameters parameters)
        {
            return ExecuteRequest(request => _accountActivityController.TriggerAccountActivityWebhookCRC(parameters, request));
        }

        public Task<ITwitterResult> SubscribeToAccountActivity(ISubscribeToAccountActivityParameters parameters)
        {
            return ExecuteRequest(request => _accountActivityController.SubscribeToAccountActivity(parameters, request));
        }

        public Task<ITwitterResult<IGetWebhookSubscriptionsCountResultDTO>> CountAccountActivitySubscriptions(ICountAccountActivitySubscriptionsParameters parameters)
        {
            return ExecuteRequest(request => _accountActivityController.CountAccountActivitySubscriptions(parameters, request));
        }

        public Task<ITwitterResult> IsAccountSubscribedToAccountActivity(IIsAccountSubscribedToAccountActivityParameters parameters)
        {
            return ExecuteRequest(request => _accountActivityController.IsAccountSubscribedToAccountActivity(parameters, request));
        }

        public Task<ITwitterResult<IWebhookSubscriptionListDTO>> GetAccountActivitySubscriptions(IGetAccountActivitySubscriptionsParameters parameters)
        {
            return ExecuteRequest(request => _accountActivityController.GetAccountActivitySubscriptions(parameters, request));
        }

        public Task<ITwitterResult> UnsubscribeFromAccountActivity(IUnsubscribeFromAccountActivityParameters parameters)
        {
            return ExecuteRequest(request => _accountActivityController.UnsubscribeFromAccountActivity(parameters, request));
        }
    }
}