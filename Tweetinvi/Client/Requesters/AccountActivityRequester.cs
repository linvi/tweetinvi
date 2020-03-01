using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO.Webhooks;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class AccountActivityRequester : BaseRequester, IAccountActivityRequester
    {
        private readonly IAccountActivityClientRequiredParametersValidator _validator;
        private readonly IAccountActivityController _accountActivityController;

        public AccountActivityRequester(
            ITwitterClient client,
            ITwitterClientEvents twitterClientEvents,
            IAccountActivityClientRequiredParametersValidator validator,
            IAccountActivityController accountActivityController)
            : base(client, twitterClientEvents)
        {
            _validator = validator;
            _accountActivityController = accountActivityController;
        }

        public Task<ITwitterResult<IWebhookDTO>> CreateAccountActivityWebhook(ICreateAccountActivityWebhookParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var result = await _accountActivityController.CreateAccountActivityWebhook(parameters, request).ConfigureAwait(false);
                return result;
            });
        }

        public Task<ITwitterResult<IGetAccountActivityWebhookEnvironmentsResultDTO>> GetAccountActivityWebhookEnvironments(IGetAccountActivityWebhookEnvironmentsParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request =>
            {
                return _accountActivityController.GetAccountActivityWebhookEnvironments(parameters, request);
            });
        }

        public Task<ITwitterResult<IWebhookDTO[]>> GetAccountActivityEnvironmentWebhooks(IGetAccountActivityEnvironmentWebhooksParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request =>
            {
                return _accountActivityController.GetAccountActivityEnvironmentWebhooks(parameters, request);
            });
        }

        public Task<ITwitterResult> DeleteAccountActivityWebhook(IDeleteAccountActivityWebhookParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountActivityController.DeleteAccountActivityWebhook(parameters, request));
        }

        public Task<ITwitterResult> TriggerAccountActivityWebhookCRC(ITriggerAccountActivityWebhookCRCParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountActivityController.TriggerAccountActivityWebhookCRC(parameters, request));
        }

        public Task<ITwitterResult> SubscribeToAccountActivity(ISubscribeToAccountActivityParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountActivityController.SubscribeToAccountActivity(parameters, request));
        }

        public Task<ITwitterResult<IGetWebhookSubscriptionsCountResultDTO>> CountAccountActivitySubscriptions(ICountAccountActivitySubscriptionsParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountActivityController.CountAccountActivitySubscriptions(parameters, request));
        }

        public Task<ITwitterResult> IsAccountSubscribedToAccountActivity(IIsAccountSubscribedToAccountActivityParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountActivityController.IsAccountSubscribedToAccountActivity(parameters, request));
        }

        public Task<ITwitterResult<IWebhookSubscriptionListDTO>> GetAccountActivitySubscriptions(IGetAccountActivitySubscriptionsParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountActivityController.GetAccountActivitySubscriptions(parameters, request));
        }

        public Task<ITwitterResult> UnsubscribeFromAccountActivity(IUnsubscribeFromAccountActivityParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequest(request => _accountActivityController.UnsubscribeFromAccountActivity(parameters, request));
        }
    }
}