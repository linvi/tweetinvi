using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
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

        public Task<ITwitterResult<IWebhookDTO>> RegisterAccountActivityWebhook(IRegisterAccountActivityWebhookParameters parameters)
        {
            return ExecuteRequest(async request =>
            {
                var result = await _accountActivityController.RegisterAccountActivityWebhook(parameters, request).ConfigureAwait(false);
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

        public Task<ITwitterResult> RemoveAccountActivityWebhook(IRemoveAccountActivityWebhookParameters parameters)
        {
            return ExecuteRequest(request => _accountActivityController.RemoveAccountActivityWebhook(parameters, request));
        }
    }
}