using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters.ListsClient;

namespace Tweetinvi.Client.Requesters
{
    public class TwitterListsRequester : BaseRequester, ITwitterListsRequester
    {
        private readonly ITwitterResultFactory _twitterResultFactory;
        private readonly ITwitterClientFactories _factories;
        private readonly ITwitterListController _twitterListController;
        private readonly ITwitterListsClientRequiredParametersValidator _twitterListsClientRequiredParametersValidator;

        public TwitterListsRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            ITwitterResultFactory twitterResultFactory,
            ITwitterClientFactories factories,
            ITwitterListController twitterListController,
            ITwitterListsClientRequiredParametersValidator twitterListsClientRequiredParametersValidator)
            : base(client, clientEvents)
        {
            _twitterResultFactory = twitterResultFactory;
            _factories = factories;
            _twitterListController = twitterListController;
            _twitterListsClientRequiredParametersValidator = twitterListsClientRequiredParametersValidator;
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> CreateList(ICreateListParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.CreateList(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> GetList(IGetListParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(async request =>
            {
                var twitterResult = await _twitterListController.GetList(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO>> DestroyList(IDestroyListParameters parameters)
        {
            _twitterListsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _twitterListController.DestroyList(parameters, request));
        }
    }
}