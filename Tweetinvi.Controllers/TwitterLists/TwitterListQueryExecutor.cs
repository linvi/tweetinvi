using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters.ListsClient;

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
        Task<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfList(IGetMembersOfListParameters parameters, ITwitterRequest request);








        // OLD

        Task<IEnumerable<ITwitterListDTO>> GetUserSubscribedLists(IUserIdentifier user, bool getOwnedListsFirst);

        Task<IEnumerable<ITweetDTO>> GetTweetsFromList(IGetTweetsFromListQueryParameters queryParameters);

        // Members
        Task<MultiRequestsResult> AddMultipleMembersToList(ITwitterListIdentifier listIdentifier,
            IEnumerable<IUserIdentifier> users);
        Task<bool> RemoveMemberFromList(ITwitterListIdentifier listIdentifier, IUserIdentifier user);
        Task<MultiRequestsResult> RemoveMultipleMembersFromList(ITwitterListIdentifier listIdentifier,
            IEnumerable<IUserIdentifier> users);
        Task<bool> CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier user);

        // Subscribers
        Task<IEnumerable<ITwitterListDTO>> GetUserSubscribedLists(IUserIdentifier user,
            int maximumNumberOfListsToRetrieve);

        Task<IEnumerable<IUserDTO>> GetListSubscribers(ITwitterListIdentifier listIdentifier,
            int maximumNumberOfSubscribersToRetrieve);
        Task<bool> SubscribeAuthenticatedUserToList(ITwitterListIdentifier listIdentifier);
        Task<bool> UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier listIdentifier);
        Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier user);

        // User memberships
        Task<IEnumerable<ITwitterListDTO>> GetUserListMemberships(
            IGetUserListMembershipsQueryParameters queryParameters);

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

        public Task<ITwitterResult<IUserCursorQueryResultDTO>> GetMembersOfList(IGetMembersOfListParameters parameters, ITwitterRequest request)
        {
            request.Query.Url = _listsQueryGenerator.GetMembersOfListQuery(parameters);
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<IUserCursorQueryResultDTO>(request);
        }


        // User

        public Task<IEnumerable<ITwitterListDTO>> GetUserSubscribedLists(IUserIdentifier user, bool getOwnedListsFirst)
        {
            var query = _listsQueryGenerator.GetUserSubscribedListsQuery(user, getOwnedListsFirst);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITwitterListDTO>>(query);
        }


        // Owned Lists

        // Memberships
        public Task<IEnumerable<ITwitterListDTO>> GetUserOwnedLists(IUserIdentifier user, int maximumNumberOfListsToRetrieve)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ITwitterListDTO>> GetUserListMemberships(IGetUserListMembershipsQueryParameters queryParameters)
        {
            throw new NotImplementedException();
            // var parameters = queryParameters.Parameters;
            // var query = _listsQueryGenerator.GetUserListMembershipsQuery(queryParameters);
            // return _twitterAccessor.ExecuteCursorGETQuery<ITwitterListDTO, ITwitterListCursorQueryResultDTO>(query, parameters);
        }

        // Get Tweets from list
        public Task<IEnumerable<ITweetDTO>> GetTweetsFromList(IGetTweetsFromListQueryParameters queryParameters)
        {
            string query = _listsQueryGenerator.GetTweetsFromListQuery(queryParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        public Task<MultiRequestsResult> AddMultipleMembersToList(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> users)
        {
            throw new NotImplementedException();
        }

        // Members




        // Remove Members
        public async Task<bool> RemoveMemberFromList(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            throw new NotImplementedException();

            // var query = _listsQueryGenerator.GetRemoveMemberFromListQuery(listIdentifier, user);
            // var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery(query);
            //
            // return asyncOperation.Success;
        }

        public async Task<MultiRequestsResult> RemoveMultipleMembersFromList(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> users)
        {
            throw new NotImplementedException();

            // var usersArray = IEnumerableExtension.GetDistinctUserIdentifiers(users);
            //
            // for (int i = 0; i < usersArray.Length; i += TweetinviConsts.LIST_ADD_OR_REMOVE_MULTIPLE_MEMBERS_MAX)
            // {
            //     var usersToAdd = usersArray.Skip(i).Take(TweetinviConsts.LIST_ADD_OR_REMOVE_MULTIPLE_MEMBERS_MAX).ToArray();
            //     var query = _listsQueryGenerator.GetRemoveMultipleMembersFromListQuery(listIdentifier, usersToAdd);
            //
            //     var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery(query);
            //
            //     if (!asyncOperation.Success)
            //     {
            //         return i > 0 ? MultiRequestsResult.Partial : MultiRequestsResult.Failure;
            //     }
            // }
            //
            // return MultiRequestsResult.Success;
        }

        public async Task<bool> CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            throw new NotImplementedException();
            // var query = _listsQueryGenerator.GetCheckIfUserIsAListMemberQuery(listIdentifier, user);
            //
            // var asyncOperation = await _twitterAccessor.TryExecuteGETQuery(query);
            //
            // return asyncOperation.Success;
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