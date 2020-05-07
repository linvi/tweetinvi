using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class TwitterListsRequester : BaseRequester, ITwitterListsRequester
    {
        private readonly ITwitterResultFactory _twitterResultFactory;
        private readonly ITwitterClientFactories _factories;
        private readonly ITwitterListController _twitterListController;
        private readonly ITwitterListsClientRequiredParametersValidator _validator;

        public TwitterListsRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            ITwitterResultFactory twitterResultFactory,
            ITwitterClientFactories factories,
            ITwitterListController twitterListController,
            ITwitterListsClientRequiredParametersValidator validator)
            : base(client, clientEvents)
        {
            _twitterResultFactory = twitterResultFactory;
            _factories = factories;
            _twitterListController = twitterListController;
            _validator = validator;
        }

        public Task<ITwitterResult<ITwitterListDTO>> CreateListAsync(ICreateListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _twitterListController.CreateListAsync(parameters, request));
        }

        public Task<ITwitterResult<ITwitterListDTO>> GetListAsync(IGetListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _twitterListController.GetListAsync(parameters, request));
        }

        public Task<ITwitterResult<ITwitterListDTO[]>> GetListsSubscribedByUserAsync(IGetListsSubscribedByUserParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _twitterListController.GetListsSubscribedByUserAsync(parameters, request));
        }

        public Task<ITwitterResult<ITwitterListDTO>> UpdateListAsync(IUpdateListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _twitterListController.UpdateListAsync(parameters, request));
        }

        public Task<ITwitterResult<ITwitterListDTO>> DestroyListAsync(IDestroyListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _twitterListController.DestroyListAsync(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _twitterListController.GetListsOwnedByUserIterator(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> AddMemberToListAsync(IAddMemberToListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(async request =>
            {
                var twitterResult = await _twitterListController.AddMemberToListAsync(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO, ITwitterList>> AddMembersToListAsync(IAddMembersToListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(async request =>
            {
                var twitterResult = await _twitterListController.AddMembersToListAsync(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _factories.CreateTwitterList(dto));
            });
        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListMembershipsIterator(IGetUserListMembershipsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _twitterListController.GetUserListMembershipsIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfListIterator(IGetMembersOfListParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _twitterListController.GetMembersOfListIterator(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsAListMemberAsync(ICheckIfUserIsMemberOfListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _twitterListController.CheckIfUserIsAListMemberAsync(parameters, request));
        }

        public Task<ITwitterResult<ITwitterListDTO>> RemoveMemberFromListAsync(IRemoveMemberFromListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _twitterListController.RemoveMemberFromListAsync(parameters, request));
        }

        public Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromListAsync(IRemoveMembersFromListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _twitterListController.RemoveMembersFromListAsync(parameters, request));
        }

        public Task<ITwitterResult<ITwitterListDTO>> SubscribeToListAsync(ISubscribeToListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _twitterListController.SubscribeToListAsync(parameters, request));
        }

        public Task<ITwitterResult<ITwitterListDTO>> UnsubscribeFromListAsync(IUnsubscribeFromListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _twitterListController.UnsubscribeFromListAsync(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetListSubscribersIterator(IGetListSubscribersParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _twitterListController.GetListSubscribersIterator(parameters, request);

        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListSubscriptionsIterator(IGetUserListSubscriptionsParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _twitterListController.GetUserListSubscriptionsIterator(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsSubscriberOfListAsync(ICheckIfUserIsSubscriberOfListParameters parameters)
        {
            _validator.Validate(parameters);
            return ExecuteRequestAsync(request => _twitterListController.CheckIfUserIsSubscriberOfListAsync(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetTweetsFromListIterator(IGetTweetsFromListParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            return _twitterListController.GetTweetsFromListIterator(parameters, request);
        }
    }
}