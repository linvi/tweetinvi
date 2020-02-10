using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Parameters;
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

        // list members
        Task<ITwitterResult<ITwitterListDTO>> AddMemberToList(IAddMemberToListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> AddMembersToList(IAddMembersToListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsAUserIsMemberOf(IGetListsAUserIsMemberOfParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfList(IGetMembersOfListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> CheckIfUserIsAListMember(ICheckIfUserIsMemberOfListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> RemoveMemberFromList(IRemoveMemberFromListParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromList(IRemoveMembersFromListParameters parameters, ITwitterRequest request);







        // OLD

        Task<IEnumerable<ITweetDTO>> GetTweetsFromList(IGetTweetsFromListQueryParameters queryParameters);

        // Subscribers
        Task<IEnumerable<ITwitterListDTO>> GetUserSubscribedLists(IUserIdentifier user,
            int maximumNumberOfListsToRetrieve);

        Task<IEnumerable<IUserDTO>> GetListSubscribers(ITwitterListIdentifier listIdentifier,
            int maximumNumberOfSubscribersToRetrieve);
        Task<bool> SubscribeAuthenticatedUserToList(ITwitterListIdentifier listIdentifier);
        Task<bool> UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier listIdentifier);
        Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier user);

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

        public Task<ITwitterResult<ITwitterListCursorQueryResultDTO>> GetListsAUserIsMemberOf(IGetListsAUserIsMemberOfParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetListsAUserIsMemberOfQuery(parameters);
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
            request.Query.Url = _listsQueryGenerator.GetRemoveMemberFromListParameter(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }

        public Task<ITwitterResult<ITwitterListDTO>> RemoveMembersFromList(IRemoveMembersFromListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetRemoveMembersFromListParameters(parameters);
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequest<ITwitterListDTO>(request);
        }


        // User

        public Task<IEnumerable<ITwitterListDTO>> GetUserSubscribedLists(IUserIdentifier user, bool getOwnedListsFirst)
        {
            var query = _listsQueryGenerator.GetUserSubscribedListsQuery(user, getOwnedListsFirst);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITwitterListDTO>>(query);
        }


        // Get Tweets from list
        public Task<IEnumerable<ITweetDTO>> GetTweetsFromList(IGetTweetsFromListQueryParameters queryParameters)
        {
            string query = _listsQueryGenerator.GetTweetsFromListQuery(queryParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        // Subscribers
        public Task<IEnumerable<ITwitterListDTO>> GetUserSubscribedLists(IUserIdentifier user, int maximumNumberOfListsToRetrieve)
        {
            var baseQuery = _listsQueryGenerator.GetUserSubscribedListsQuery(user, maximumNumberOfListsToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<ITwitterListDTO, ITwitterListCursorQueryResultDTO>(baseQuery, maximumNumberOfListsToRetrieve);
        }

        public Task<IEnumerable<IUserDTO>> GetListSubscribers(ITwitterListIdentifier listIdentifier, int maximumNumberOfSubscribersToRetrieve)
        {
            string baseQuery = _listsQueryGenerator.GetListSubscribersQuery(listIdentifier, Math.Min(maximumNumberOfSubscribersToRetrieve, 5000));
            return _twitterAccessor.ExecuteCursorGETQuery<IUserDTO, IUserCursorQueryResultDTO>(baseQuery, maximumNumberOfSubscribersToRetrieve);
        }

        public async Task<bool> SubscribeAuthenticatedUserToList(ITwitterListIdentifier listIdentifier)
        {
            var query = _listsQueryGenerator.GetSubscribeUserToListQuery(listIdentifier);
            var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery(query);

            return asyncOperation.Success;
        }

        public async Task<bool> UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier listIdentifier)
        {
            var query = _listsQueryGenerator.GetUnSubscribeUserFromListQuery(listIdentifier);
            var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery(query);

            return asyncOperation.Success;
        }

        public async Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            var query = _listsQueryGenerator.GetCheckIfUserIsAListSubscriberQuery(listIdentifier, user);
            var asyncOperation = await _twitterAccessor.TryExecuteGETQuery(query);

            return asyncOperation.Success;
        }
    }
}