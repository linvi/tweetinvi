using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Controllers.TwitterLists
{
    public interface ITwitterListQueryExecutor
    {
        IEnumerable<ITwitterListDTO> GetUserSubscribedLists(IUserIdentifier userIdentifier, bool getOwnedListsFirst);
        IEnumerable<ITwitterListDTO> GetUserSubscribedLists(long userId, bool getOwnedListsFirst);
        IEnumerable<ITwitterListDTO> GetUserSubscribedLists(string userScreenName, bool getOwnedListsFirst);

        ITwitterListDTO UpdateList(ITwitterListUpdateQueryParameters parameters);
        bool DestroyList(ITwitterListIdentifier identifier);
        IEnumerable<ITweetDTO> GetTweetsFromList(IGetTweetsFromListQueryParameters queryParameters);
        
        // Members
        IEnumerable<IUserDTO> GetMembersOfList(ITwitterListIdentifier identifier, int maxNumberOfUsersToRetrieve);
        bool AddMemberToList(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier);
        MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> userIdentifiers);
        bool RemoveMemberFromList(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier);
        MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> userIdentifiers);
        bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier);
        
        // Subscribers
        IEnumerable<ITwitterListDTO> GetUserSubscribedLists(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve);

        IEnumerable<IUserDTO> GetListSubscribers(ITwitterListIdentifier listIdentifier, int maximumNumberOfSubscribersToRetrieve);
        bool SubscribeAuthenticatedUserToList(ITwitterListIdentifier listIdentifier);
        bool UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier listIdentifier);
        bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier);
        IEnumerable<ITwitterListDTO> GetUserOwnedLists(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve);
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
        public IEnumerable<ITwitterListDTO> GetUserSubscribedLists(IUserIdentifier userIdentifier, bool getOwnedListsFirst)
        {
            var query = _listsQueryGenerator.GetUserSubscribedListsQuery(userIdentifier, getOwnedListsFirst);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITwitterListDTO>>(query);
        }

        public IEnumerable<ITwitterListDTO> GetUserSubscribedLists(long userId, bool getOwnedListsFirst)
        {
            var query = _listsQueryGenerator.GetUserSubscribedListsQuery(userId, getOwnedListsFirst);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITwitterListDTO>>(query);
        }

        public IEnumerable<ITwitterListDTO> GetUserSubscribedLists(string userScreenName, bool getOwnedListsFirst)
        {
            var query = _listsQueryGenerator.GetUserSubscribedListsQuery(userScreenName, getOwnedListsFirst);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITwitterListDTO>>(query);
        }

        // Owned Lists
        public IEnumerable<ITwitterListDTO> GetUserOwnedLists(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve)
        {
            var baseQuery = _listsQueryGenerator.GetUsersOwnedListQuery(userIdentifier, maximumNumberOfListsToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<ITwitterListDTO, ITwitterListCursorQueryResultDTO>(baseQuery, maximumNumberOfListsToRetrieve);
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

        public bool AddMemberToList(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            var query = _listsQueryGenerator.GetAddMemberToListQuery(listIdentifier, userIdentifier);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            var userIdentifiersArray = IEnumerableExtension.GetDistinctUserIdentifiers(userIdentifiers);

            for (int i = 0; i < userIdentifiersArray.Length; i += TweetinviConsts.LIST_ADD_OR_REMOVE_MULTIPLE_MEMBERS_MAX)
            {
                var userIdentifiersToAdd = userIdentifiersArray.Skip(i).Take(TweetinviConsts.LIST_ADD_OR_REMOVE_MULTIPLE_MEMBERS_MAX).ToArray();
                var query = _listsQueryGenerator.GetAddMultipleMembersToListQuery(listIdentifier, userIdentifiersToAdd);

                if (!_twitterAccessor.TryExecuteGETQuery(query))
                {
                    return i > 0 ? MultiRequestsResult.Partial : MultiRequestsResult.Failure;
                }
            }

            return MultiRequestsResult.Success;
        }

        public bool RemoveMemberFromList(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            var query = _listsQueryGenerator.GetRemoveMemberFromListQuery(listIdentifier, userIdentifier);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            var userIdentifiersArray = IEnumerableExtension.GetDistinctUserIdentifiers(userIdentifiers);

            for (int i = 0; i < userIdentifiersArray.Length; i += TweetinviConsts.LIST_ADD_OR_REMOVE_MULTIPLE_MEMBERS_MAX)
            {
                var userIdentifiersToAdd = userIdentifiersArray.Skip(i).Take(TweetinviConsts.LIST_ADD_OR_REMOVE_MULTIPLE_MEMBERS_MAX).ToArray();
                var query = _listsQueryGenerator.GetRemoveMultipleMembersFromListQuery(listIdentifier, userIdentifiersToAdd);

                if (!_twitterAccessor.TryExecuteGETQuery(query))
                {
                    return i > 0 ? MultiRequestsResult.Partial : MultiRequestsResult.Failure;
                }
            }

            return MultiRequestsResult.Success;
        }

        public bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            var query = _listsQueryGenerator.GetCheckIfUserIsAListMemberQuery(listIdentifier, userIdentifier);
            return _twitterAccessor.TryExecuteGETQuery(query);
        }

        // Subscribers
        public IEnumerable<ITwitterListDTO> GetUserSubscribedLists(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve)
        {
            var baseQuery = _listsQueryGenerator.GetUserSubscribedListsQuery(userIdentifier, maximumNumberOfListsToRetrieve);
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

        public bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            var query = _listsQueryGenerator.GetCheckIfUserIsAListSubscriberQuery(listIdentifier, userIdentifier);
            return _twitterAccessor.TryExecuteGETQuery(query);
        }
    }
}