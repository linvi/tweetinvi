using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.TwitterLists
{
    public class TwitterListController : ITwitterListController
    {
        private readonly IUserFactory _userFactory;
        private readonly ITwitterListQueryExecutor _twitterListQueryExecutor;
        private readonly ITwitterListIdentifierFactory _twitterListIdentifierFactory;
        private readonly IPageCursorIteratorFactories _pageCursorIteratorFactories;

        public TwitterListController(
            IUserFactory userFactory,
            ITwitterListQueryExecutor twitterListQueryExecutor,
            ITwitterListIdentifierFactory twitterListIdentifierFactory,
            IPageCursorIteratorFactories pageCursorIteratorFactories)
        {
            _userFactory = userFactory;
            _twitterListQueryExecutor = twitterListQueryExecutor;
            _twitterListIdentifierFactory = twitterListIdentifierFactory;
            _pageCursorIteratorFactories = pageCursorIteratorFactories;
        }

        public Task<ITwitterResult<ITwitterListDTO>> CreateList(ICreateListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.CreateList(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> GetList(IGetListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.GetList(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO[]>> GetListsSubscribedByUser(IGetListsSubscribedByUserParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.GetListsSubscribedByUser(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> UpdateList(IUpdateListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.UpdateList(parameters, request);
        }

        Task<ITwitterResult<ITwitterListDTO>> ITwitterListController.DestroyList(IDestroyListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.DestroyList(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetListsOwnedByAccountByUserParameters(parameters)
                {
                    Cursor = cursor
                };

                return _twitterListQueryExecutor.GetListsOwnedByUser(cursoredParameters, new TwitterRequest(request));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO>> AddMemberToList(IAddMemberToListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.AddMemberToList(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> AddMembersToList(IAddMembersToListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.AddMembersToList(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListMembershipsIterator(IGetUserListMembershipsParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetUserListMembershipsParameters(parameters)
                {
                    Cursor = cursor
                };

                return _twitterListQueryExecutor.GetUserListMemberships(cursoredParameters, new TwitterRequest(request));
            });
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfListIterator(IGetMembersOfListParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetMembersOfListParameters(parameters)
                {
                    Cursor = cursor
                };

                return _twitterListQueryExecutor.GetMembersOfList(cursoredParameters, new TwitterRequest(request));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsAListMember(ICheckIfUserIsMemberOfListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.CheckIfUserIsAListMember(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> RemoveMemberFromList(IRemoveMemberFromListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.RemoveMemberFromList(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromList(IRemoveMembersFromListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.RemoveMembersFromList(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> SubscribeToList(ISubscribeToListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.SubscribeToList(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> UnsubscribeFromList(IUnsubscribeFromListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.UnsubscribeFromList(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetListSubscribers(IGetListSubscribersParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetListSubscribersParameters(parameters)
                {
                    Cursor = cursor
                };

                return _twitterListQueryExecutor.GetListSubscribers(cursoredParameters, new TwitterRequest(request));
            });
        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListSubscriptions(IGetUserListSubscriptionsParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetUserListSubscriptionsParameters(parameters)
                {
                    Cursor = cursor
                };

                return _twitterListQueryExecutor.GetUserListSubscriptions(cursoredParameters, new TwitterRequest(request));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsSubscriberOfList(ICheckIfUserIsSubscriberOfListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.CheckIfUserIsSubscriberOfList(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetTweetsFromListIterator(IGetTweetsFromListParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetTweetsFromListParameters(parameters)
                {
                    MaxId = cursor
                };

                return _twitterListQueryExecutor.GetTweetsFromList(cursoredParameters, new TwitterRequest(request));
            });
        }
    }
}