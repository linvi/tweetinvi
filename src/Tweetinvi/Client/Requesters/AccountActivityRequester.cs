using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
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

        public Task<ITwitterResult<IWebhookDTO>> CreateAccountActivityWebhookAsync(ICreateAccountActivityWebhookParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountActivityController.CreateAccountActivityWebhookAsync(parameters, request));
        }

        public Task<ITwitterResult<IGetAccountActivityWebhookEnvironmentsResultDTO>> GetAccountActivityWebhookEnvironmentsAsync(IGetAccountActivityWebhookEnvironmentsParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountActivityController.GetAccountActivityWebhookEnvironmentsAsync(parameters, request));
        }

        public Task<ITwitterResult<IWebhookDTO[]>> GetAccountActivityEnvironmentWebhooksAsync(IGetAccountActivityEnvironmentWebhooksParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountActivityController.GetAccountActivityEnvironmentWebhooksAsync(parameters, request));
        }

        public Task<ITwitterResult> DeleteAccountActivityWebhookAsync(IDeleteAccountActivityWebhookParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountActivityController.DeleteAccountActivityWebhookAsync(parameters, request));
        }

        public Task<ITwitterResult> TriggerAccountActivityWebhookCRCAsync(ITriggerAccountActivityWebhookCRCParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountActivityController.TriggerAccountActivityWebhookCRCAsync(parameters, request));
        }

        public Task<ITwitterResult> SubscribeToAccountActivityAsync(ISubscribeToAccountActivityParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountActivityController.SubscribeToAccountActivityAsync(parameters, request));
        }

        public Task<ITwitterResult<IWebhookSubscriptionsCount>> CountAccountActivitySubscriptionsAsync(ICountAccountActivitySubscriptionsParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountActivityController.CountAccountActivitySubscriptionsAsync(parameters, request));
        }

        public Task<ITwitterResult> IsAccountSubscribedToAccountActivityAsync(IIsAccountSubscribedToAccountActivityParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountActivityController.IsAccountSubscribedToAccountActivityAsync(parameters, request));
        }

        public Task<ITwitterResult<IWebhookEnvironmentSubscriptionsDTO>> GetAccountActivitySubscriptionsAsync(IGetAccountActivitySubscriptionsParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountActivityController.GetAccountActivitySubscriptionsAsync(parameters, request));
        }

        public Task<ITwitterResult> UnsubscribeFromAccountActivityAsync(IUnsubscribeFromAccountActivityParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _accountActivityController.UnsubscribeFromAccountActivityAsync(parameters, request));
        }
    }
}