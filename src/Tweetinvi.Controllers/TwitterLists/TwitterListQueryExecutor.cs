using System.Threading.Tasks;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.TwitterLists
{
    public interface ITwitterListQueryExecutor
    {
        // list
        Task<ITwitterResult<ITwitterListDTO>> CreateListAsync(ICreateListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> GetListAsync(IGetListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO[]>> GetListsSubscribedByUserAsync(IGetListsSubscribedByUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> UpdateListAsync(IUpdateListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> DestroyListAsync(IDestroyListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsOwnedByUserAsync(IGetListsOwnedByUserParameters parameters, ITwitterRequest request);

        // **************
        // MEMBERS
        // **************
        Task<ITwitterResult<ITwitterListDTO>> AddMemberToListAsync(IAddMemberToListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> AddMembersToListAsync(IAddMembersToListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListMembershipsAsync(IGetUserListMembershipsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfListAsync(IGetMembersOfListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsAListMemberAsync(ICheckIfUserIsMemberOfListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> RemoveMemberFromListAsync(IRemoveMemberFromListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromListAsync(IRemoveMembersFromListParameters parameters, ITwitterRequest request);

        // **************
        // SUBSCRIPTIONS
        // **************
        Task<ITwitterResult<ITwitterListDTO>> SubscribeToListAsync(ISubscribeToListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> UnsubscribeFromListAsync(IUnsubscribeFromListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserCursorQueryResultDTO>> GetListSubscribersAsync(IGetListSubscribersParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListSubscriptionsAsync(IGetUserListSubscriptionsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsSubscriberOfListAsync(ICheckIfUserIsSubscriberOfListParameters parameters, ITwitterRequest request);

        // **************
        // TWEETS
        // **************
        Task<ITwitterResult<ITweetDTO[]>> GetTweetsFromListAsync(IGetTweetsFromListParameters parameters, ITwitterRequest request);
    }

    public class TwitterListQueryExecutor : ITwitterListQueryExecutor
    {
        private readonly ITwitterListQueryGenerator _listsQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public TwitterListQueryExecutor(ITwitterListQueryGenerator listsQueryGenerator, ITwitterAccessor twitterAccessor)
        {
            _listsQueryGenerator = listsQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult<ITwitterListDTO>> CreateListAsync(ICreateListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetCreateListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> GetListAsync(IGetListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO[]>> GetListsSubscribedByUserAsync(IGetListsSubscribedByUserParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetListsSubscribedByUserQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListDTO[]>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> UpdateListAsync(IUpdateListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetUpdateListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> DestroyListAsync(IDestroyListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetDestroyListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsOwnedByUserAsync(IGetListsOwnedByUserParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetListsOwnedByUserQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> AddMemberToListAsync(IAddMemberToListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetAddMemberToListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> AddMembersToListAsync(IAddMembersToListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetAddMembersQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListMembershipsAsync(IGetUserListMembershipsParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetUserListMembershipsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfListAsync(IGetMembersOfListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetMembersOfListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<IUserCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsAListMemberAsync(ICheckIfUserIsMemberOfListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetCheckIfUserIsMemberOfListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> RemoveMemberFromListAsync(IRemoveMemberFromListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetRemoveMemberFromListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromListAsync(IRemoveMembersFromListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetRemoveMembersFromListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> SubscribeToListAsync(ISubscribeToListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetSubscribeToListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> UnsubscribeFromListAsync(IUnsubscribeFromListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetUnsubscribeFromListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<IUserCursorQueryResultDTO>> GetListSubscribersAsync(IGetListSubscribersParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetListSubscribersQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<IUserCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListSubscriptionsAsync(IGetUserListSubscriptionsParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetUserListSubscriptionsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsSubscriberOfListAsync(ICheckIfUserIsSubscriberOfListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetCheckIfUserIsSubscriberOfListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITweetDTO[]>> GetTweetsFromListAsync(IGetTweetsFromListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetTweetsFromListQuery(parameters, new ComputedTweetMode(parameters, request));
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<ITweetDTO[]>(request);
        }
    }
}