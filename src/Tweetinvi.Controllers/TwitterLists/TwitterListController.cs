using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
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
        private readonly ITwitterListQueryExecutor _twitterListQueryExecutor;
        private readonly IPageCursorIteratorFactories _pageCursorIteratorFactories;

        public TwitterListController(
            ITwitterListQueryExecutor twitterListQueryExecutor,
            IPageCursorIteratorFactories pageCursorIteratorFactories)
        {
            _twitterListQueryExecutor = twitterListQueryExecutor;
            _pageCursorIteratorFactories = pageCursorIteratorFactories;
        }

        public Task<ITwitterResult<ITwitterListDTO>> CreateListAsync(ICreateListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.CreateListAsync(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> GetListAsync(IGetListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.GetListAsync(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO[]>> GetListsSubscribedByUserAsync(IGetListsSubscribedByUserParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.GetListsSubscribedByUserAsync(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> UpdateListAsync(IUpdateListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.UpdateListAsync(parameters, request);
        }

        Task<ITwitterResult<ITwitterListDTO>> ITwitterListController.DestroyListAsync(IDestroyListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.DestroyListAsync(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsOwnedByUserIterator(IGetListsOwnedByUserParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetListsOwnedByAccountByUserParameters(parameters)
                {
                    Cursor = cursor
                };

                return _twitterListQueryExecutor.GetListsOwnedByUserAsync(cursoredParameters, new TwitterRequest(request));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO>> AddMemberToListAsync(IAddMemberToListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.AddMemberToListAsync(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> AddMembersToListAsync(IAddMembersToListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.AddMembersToListAsync(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListMembershipsIterator(IGetUserListMembershipsParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetUserListMembershipsParameters(parameters)
                {
                    Cursor = cursor
                };

                return _twitterListQueryExecutor.GetUserListMembershipsAsync(cursoredParameters, new TwitterRequest(request));
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

                return _twitterListQueryExecutor.GetMembersOfListAsync(cursoredParameters, new TwitterRequest(request));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsAListMemberAsync(ICheckIfUserIsMemberOfListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.CheckIfUserIsAListMemberAsync(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> RemoveMemberFromListAsync(IRemoveMemberFromListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.RemoveMemberFromListAsync(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromListAsync(IRemoveMembersFromListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.RemoveMembersFromListAsync(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> SubscribeToListAsync(ISubscribeToListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.SubscribeToListAsync(parameters, request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> UnsubscribeFromListAsync(IUnsubscribeFromListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.UnsubscribeFromListAsync(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetListSubscribersIterator(IGetListSubscribersParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetListSubscribersParameters(parameters)
                {
                    Cursor = cursor
                };

                return _twitterListQueryExecutor.GetListSubscribersAsync(cursoredParameters, new TwitterRequest(request));
            });
        }

        public ITwitterPageIterator<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListSubscriptionsIterator(IGetUserListSubscriptionsParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetUserListSubscriptionsParameters(parameters)
                {
                    Cursor = cursor
                };

                return _twitterListQueryExecutor.GetUserListSubscriptionsAsync(cursoredParameters, new TwitterRequest(request));
            });
        }

        public Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsSubscriberOfListAsync(ICheckIfUserIsSubscriberOfListParameters parameters, ITwitterRequest request)
        {
            return _twitterListQueryExecutor.CheckIfUserIsSubscriberOfListAsync(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetTweetsFromListIterator(IGetTweetsFromListParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetTweetsFromListParameters(parameters)
                {
                    MaxId = cursor
                };

                return _twitterListQueryExecutor.GetTweetsFromListAsync(cursoredParameters, new TwitterRequest(request));
            });
        }
    }
}