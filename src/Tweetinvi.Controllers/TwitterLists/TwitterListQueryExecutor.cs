using System.Threading.Tasks;
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
        Task<ITwitterResult<ITwitterListDTO>> CreateList(ICreateListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> GetList(IGetListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO[]>> GetListsSubscribedByUser(IGetListsSubscribedByUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> UpdateList(IUpdateListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> DestroyList(IDestroyListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsOwnedByUser(IGetListsOwnedByUserParameters parameters, ITwitterRequest request);

        // **************
        // MEMBERS
        // **************
        Task<ITwitterResult<ITwitterListDTO>> AddMemberToList(IAddMemberToListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> AddMembersToList(IAddMembersToListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListMemberships(IGetUserListMembershipsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfList(IGetMembersOfListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsAListMember(ICheckIfUserIsMemberOfListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> RemoveMemberFromList(IRemoveMemberFromListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromList(IRemoveMembersFromListParameters parameters, ITwitterRequest request);

        // **************
        // SUBSCRIPTIONS
        // **************
        Task<ITwitterResult<ITwitterListDTO>> SubscribeToList(ISubscribeToListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> UnsubscribeFromList(IUnsubscribeFromListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserCursorQueryResultDTO>> GetListSubscribers(IGetListSubscribersParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListSubscriptions(IGetUserListSubscriptionsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsSubscriberOfList(ICheckIfUserIsSubscriberOfListParameters parameters, ITwitterRequest request);

        // **************
        // TWEETS
        // **************
        Task<ITwitterResult<ITweetDTO[]>> GetTweetsFromList(IGetTweetsFromListParameters parameters, ITwitterRequest request);
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

        public Task<ITwitterResult<ITwitterListDTO>> CreateList(ICreateListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetCreateListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> GetList(IGetListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO[]>> GetListsSubscribedByUser(IGetListsSubscribedByUserParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetListsSubscribedByUserQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO[]>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> UpdateList(IUpdateListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetUpdateListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> DestroyList(IDestroyListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetDestroyListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsOwnedByUser(IGetListsOwnedByUserParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetListsOwnedByUserQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITwitterListCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> AddMemberToList(IAddMemberToListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetAddMemberToListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> AddMembersToList(IAddMembersToListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetAddMembersQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListMemberships(IGetUserListMembershipsParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetUserListMembershipsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITwitterListCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfList(IGetMembersOfListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetMembersOfListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<IUserCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsAListMember(ICheckIfUserIsMemberOfListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetCheckIfUserIsMemberOfListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> RemoveMemberFromList(IRemoveMemberFromListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetRemoveMemberFromListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromList(IRemoveMembersFromListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetRemoveMembersFromListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> SubscribeToList(ISubscribeToListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetSubscribeToListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> UnsubscribeFromList(IUnsubscribeFromListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetUnsubscribeFromListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<IUserCursorQueryResultDTO>> GetListSubscribers(IGetListSubscribersParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetListSubscribersQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<IUserCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetUserListSubscriptions(IGetUserListSubscriptionsParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetUserListSubscriptionsQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITwitterListCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsSubscriberOfList(ICheckIfUserIsSubscriberOfListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetCheckIfUserIsSubscriberOfListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITweetDTO[]>> GetTweetsFromList(IGetTweetsFromListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetTweetsFromListQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITweetDTO[]>(request);
        }
    }
}