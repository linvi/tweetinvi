using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Core.Parameters;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Controllers.TwitterLists
{
    public interface ITwitterListQueryExecutor
    {
        IEnumerable<ITwitterListDTO> GetUserSubscribedLists(IUserIdentifier user, bool getOwnedListsFirst);

        ITwitterListDTO UpdateList(ITwitterListUpdateQueryParameters parameters);
        bool DestroyList(ITwitterListIdentifier identifier);
        IEnumerable<ITweetDTO> GetTweetsFromList(IGetTweetsFromListQueryParameters queryParameters);
        
        // Members
        IEnumerable<IUserDTO> GetMembersOfList(ITwitterListIdentifier identifier, int maxNumberOfUsersToRetrieve);
        bool AddMemberToList(ITwitterListIdentifier listIdentifier, IUserIdentifier user);
        MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> users);
        bool RemoveMemberFromList(ITwitterListIdentifier listIdentifier, IUserIdentifier user);
        MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> users);
        bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier user);
        
        // Subscribers
        IEnumerable<ITwitterListDTO> GetUserSubscribedLists(IUserIdentifier user, int maximumNumberOfListsToRetrieve);

        IEnumerable<IUserDTO> GetListSubscribers(ITwitterListIdentifier listIdentifier, int maximumNumberOfSubscribersToRetrieve);
        bool SubscribeAuthenticatedUserToList(ITwitterListIdentifier listIdentifier);
        bool UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier listIdentifier);
        bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier user);
        IEnumerable<ITwitterListDTO> GetUserOwnedLists(IUserIdentifier user, int maximumNumberOfListsToRetrieve);

        // User memberships
        IEnumerable<ITwitterListDTO> GetUserListMemberships(IGetUserListMembershipsQueryParameters queryParameters);
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

        // User
        public IEnumerable<ITwitterListDTO> GetUserSubscribedLists(IUserIdentifier user, bool getOwnedListsFirst)
        {
            var query = _listsQueryGenerator.GetUserSubscribedListsQuery(user, getOwnedListsFirst);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITwitterListDTO>>(query);
        }

        // Owned Lists
        public IEnumerable<ITwitterListDTO> GetUserOwnedLists(IUserIdentifier user, int maximumNumberOfListsToRetrieve)
        {
            var baseQuery = _listsQueryGenerator.GetUsersOwnedListQuery(user, maximumNumberOfListsToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<ITwitterListDTO, ITwitterListCursorQueryResultDTO>(baseQuery, maximumNumberOfListsToRetrieve);
        }

        // Memberships
        public IEnumerable<ITwitterListDTO> GetUserListMemberships(IGetUserListMembershipsQueryParameters queryParameters)
        {
            var parameters = queryParameters.Parameters;
            var query = _listsQueryGenerator.GetUserListMembershipsQuery(queryParameters);
            return _twitterAccessor.ExecuteCursorGETQuery<ITwitterListDTO, ITwitterListCursorQueryResultDTO>(query, parameters);
        }

       // Update List
        public ITwitterListDTO UpdateList(ITwitterListUpdateQueryParameters parameters)
        {
            string query = _listsQueryGenerator.GetUpdateListQuery(parameters);
            return _twitterAccessor.ExecutePOSTQuery<ITwitterListDTO>(query);
        }

        // Destroy List
        public bool DestroyList(ITwitterListIdentifier identifier)
        {
            string query = _listsQueryGenerator.GetDestroyListQuery(identifier);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        // Get Tweets from list
        public IEnumerable<ITweetDTO> GetTweetsFromList(IGetTweetsFromListQueryParameters queryParameters)
        {
            string query = _listsQueryGenerator.GetTweetsFromListQuery(queryParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        // Members
        public IEnumerable<IUserDTO> GetMembersOfList(ITwitterListIdentifier identifier, int maxNumberOfUsersToRetrieve)
        {
            string baseQuery = _listsQueryGenerator.GetMembersFromListQuery(identifier, Math.Min(maxNumberOfUsersToRetrieve, 5000));
            return _twitterAccessor.ExecuteCursorGETQuery<IUserDTO, IUserCursorQueryResultDTO>(baseQuery, maxNumberOfUsersToRetrieve);
        }

        // Add Member
        public bool AddMemberToList(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            var query = _listsQueryGenerator.GetAddMemberToListQuery(listIdentifier, user);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> users)
        {
            var usersArray = IEnumerableExtension.GetDistinctUserIdentifiers(users);

            for (int i = 0; i < usersArray.Length; i += TweetinviConsts.LIST_ADD_OR_REMOVE_MULTIPLE_MEMBERS_MAX)
            {
                var usersToAdd = usersArray.Skip(i).Take(TweetinviConsts.LIST_ADD_OR_REMOVE_MULTIPLE_MEMBERS_MAX).ToArray();
                var query = _listsQueryGenerator.GetAddMultipleMembersToListQuery(listIdentifier, usersToAdd);

                if (!_twitterAccessor.TryExecutePOSTQuery(query))
                {
                    return i > 0 ? MultiRequestsResult.Partial : MultiRequestsResult.Failure;
                }
            }

            return MultiRequestsResult.Success;
        }

        // Remove Members
        public bool RemoveMemberFromList(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            var query = _listsQueryGenerator.GetRemoveMemberFromListQuery(listIdentifier, user);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> users)
        {
            var usersArray = IEnumerableExtension.GetDistinctUserIdentifiers(users);

            for (int i = 0; i < usersArray.Length; i += TweetinviConsts.LIST_ADD_OR_REMOVE_MULTIPLE_MEMBERS_MAX)
            {
                var usersToAdd = usersArray.Skip(i).Take(TweetinviConsts.LIST_ADD_OR_REMOVE_MULTIPLE_MEMBERS_MAX).ToArray();
                var query = _listsQueryGenerator.GetRemoveMultipleMembersFromListQuery(listIdentifier, usersToAdd);

                if (!_twitterAccessor.TryExecutePOSTQuery(query))
                {
                    return i > 0 ? MultiRequestsResult.Partial : MultiRequestsResult.Failure;
                }
            }

            return MultiRequestsResult.Success;
        }

        public bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            var query = _listsQueryGenerator.GetCheckIfUserIsAListMemberQuery(listIdentifier, user);
            return _twitterAccessor.TryExecuteGETQuery(query);
        }

        // Subscribers
        public IEnumerable<ITwitterListDTO> GetUserSubscribedLists(IUserIdentifier user, int maximumNumberOfListsToRetrieve)
        {
            var baseQuery = _listsQueryGenerator.GetUserSubscribedListsQuery(user, maximumNumberOfListsToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<ITwitterListDTO, ITwitterListCursorQueryResultDTO>(baseQuery, maximumNumberOfListsToRetrieve);
        }

        public IEnumerable<IUserDTO> GetListSubscribers(ITwitterListIdentifier listIdentifier, int maximumNumberOfSubscribersToRetrieve)
        {
            string baseQuery = _listsQueryGenerator.GetListSubscribersQuery(listIdentifier, Math.Min(maximumNumberOfSubscribersToRetrieve, 5000));
            return _twitterAccessor.ExecuteCursorGETQuery<IUserDTO, IUserCursorQueryResultDTO>(baseQuery, maximumNumberOfSubscribersToRetrieve);
        }

        public bool SubscribeAuthenticatedUserToList(ITwitterListIdentifier listIdentifier)
        {
            var query = _listsQueryGenerator.GetSubscribeUserToListQuery(listIdentifier);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier listIdentifier)
        {
            var query = _listsQueryGenerator.GetUnSubscribeUserFromListQuery(listIdentifier);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            var query = _listsQueryGenerator.GetCheckIfUserIsAListSubscriberQuery(listIdentifier, user);
            return _twitterAccessor.TryExecuteGETQuery(query);
        }
    }
}