using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;
using Tweetinvi.Logic.QueryParameters;

namespace Tweetinvi.Controllers.TwitterLists
{
    public class TwitterListController : ITwitterListController
    {
        private readonly ITweetFactory _tweetFactory;
        private readonly IUserFactory _userFactory;
        private readonly ITwitterListQueryExecutor _twitterListQueryExecutor;
        private readonly ITwitterListFactory _twitterListsFactory;
        private readonly ITwitterListQueryParameterGenerator _twitterListQueryParameterGenerator;
        private readonly ITwitterListIdentifierFactory _twitterListIdentifierFactory;

        public TwitterListController(
            ITweetFactory tweetFactory,
            IUserFactory userFactory,
            ITwitterListQueryExecutor twitterListQueryExecutor,
            ITwitterListFactory twitterListsFactory,
            ITwitterListQueryParameterGenerator twitterListQueryParameterGenerator,
            ITwitterListIdentifierFactory twitterListIdentifierFactory)
        {
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;
            _twitterListQueryExecutor = twitterListQueryExecutor;
            _twitterListsFactory = twitterListsFactory;
            _twitterListQueryParameterGenerator = twitterListQueryParameterGenerator;
            _twitterListIdentifierFactory = twitterListIdentifierFactory;
        }

        // Get User Lists
        public IEnumerable<ITwitterList> GetUserSubscribedLists(IUserIdentifier user, bool getOwnedListsFirst)
        {
            var listDTOs = _twitterListQueryExecutor.GetUserSubscribedLists(user, getOwnedListsFirst);
            return _twitterListsFactory.CreateListsFromDTOs(listDTOs);
        }

        public IEnumerable<ITwitterList> GetUserSubscribedLists(long userId, bool getOwnedListsFirst)
        {
            var listDTOs = _twitterListQueryExecutor.GetUserSubscribedLists(userId, getOwnedListsFirst);
            return _twitterListsFactory.CreateListsFromDTOs(listDTOs);
        }

        public IEnumerable<ITwitterList> GetUserSubscribedLists(string userScreenName, bool getOwnedListsFirst)
        {
            var listDTOs = _twitterListQueryExecutor.GetUserSubscribedLists(userScreenName, getOwnedListsFirst);
            return _twitterListsFactory.CreateListsFromDTOs(listDTOs);
        }

        // Owned Lists
        public IEnumerable<ITwitterList> GetUserOwnedLists(long userId, int maximumNumberOfListsToRetrieve)
        {
            var userIdentifier = new UserIdentifier(userId);
            return GetUserOwnedLists(userIdentifier, maximumNumberOfListsToRetrieve);
        }

        public IEnumerable<ITwitterList> GetUserOwnedLists(string userScreenName, int maximumNumberOfListsToRetrieve)
        {
            var userIdentifier = new UserIdentifier(userScreenName);
            return GetUserOwnedLists(userIdentifier, maximumNumberOfListsToRetrieve);
        }

        public IEnumerable<ITwitterList> GetUserOwnedLists(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve)
        {
            var listDTOs = _twitterListQueryExecutor.GetUserOwnedLists(userIdentifier, maximumNumberOfListsToRetrieve);
            return _twitterListsFactory.CreateListsFromDTOs(listDTOs);
        }

        // Update List
        public ITwitterList UpdateList(long listId, ITwitterListUpdateParameters parameters)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return UpdateList(identifier, parameters);
        }

        public ITwitterList UpdateList(string slug, IUserIdentifier owner, ITwitterListUpdateParameters parameters)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return UpdateList(identifier, parameters);
        }

        public ITwitterList UpdateList(string slug, long ownerId, ITwitterListUpdateParameters parameters)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return UpdateList(identifier, parameters);
        }

        public ITwitterList UpdateList(string slug, string ownerScreenName, ITwitterListUpdateParameters parameters)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return UpdateList(identifier, parameters);
        }

        public ITwitterList UpdateList(ITwitterListIdentifier list, ITwitterListUpdateParameters parameters)
        {
            var queryParameters = _twitterListQueryParameterGenerator.CreateTwitterListUpdateQueryParameters(list, parameters);
            return UpdateList(queryParameters);
        }

        private ITwitterList UpdateList(ITwitterListUpdateQueryParameters parameters)
        {
            var listDTO = _twitterListQueryExecutor.UpdateList(parameters);
            return _twitterListsFactory.CreateListFromDTO(listDTO);
        }

        // Destroy List
        public bool DestroyList(long listId)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return DestroyList(listIdentifier);
        }

        public bool DestroyList(string slug, IUserIdentifier owner)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return DestroyList(identifier);
        }

        public bool DestroyList(string slug, string ownerScreenName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return DestroyList(identifier);
        }

        public bool DestroyList(string slug, long ownerId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return DestroyList(identifier);
        }

        public bool DestroyList(ITwitterListIdentifier list)
        {
            return _twitterListQueryExecutor.DestroyList(list);
        }

        // Get Tweets from List
        public IEnumerable<ITweet> GetTweetsFromList(long listId)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return GetTweetsFromList(identifier);
        }

        public IEnumerable<ITweet> GetTweetsFromList(string slug, IUserIdentifier owner)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return GetTweetsFromList(identifier);
        }

        public IEnumerable<ITweet> GetTweetsFromList(string slug, string ownerScreenName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return GetTweetsFromList(identifier);
        }

        public IEnumerable<ITweet> GetTweetsFromList(string slug, long ownerId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return GetTweetsFromList(identifier);
        }

        public IEnumerable<ITweet> GetTweetsFromList(ITwitterListIdentifier list, IGetTweetsFromListParameters parameters = null)
        {
            var queryParameters = _twitterListQueryParameterGenerator.CreateTweetsFromListQueryParameters(list, parameters);
            return GetTweetsFromList(queryParameters);
        }

        private IEnumerable<ITweet> GetTweetsFromList(IGetTweetsFromListQueryParameters queryParameters)
        {
            var tweetsDTO = _twitterListQueryExecutor.GetTweetsFromList(queryParameters);
            return _tweetFactory.GenerateTweetsFromDTO(tweetsDTO);
        }

        #region Get List Members
        public IEnumerable<IUser> GetListMembers(long listId, int maximumNumberOfUsersToRetrieve = 100)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return GetListMembers(identifier, maximumNumberOfUsersToRetrieve);
        }

        public IEnumerable<IUser> GetListMembers(string slug, IUserIdentifier owner, int maximumNumberOfUsersToRetrieve = 100)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return GetListMembers(identifier, maximumNumberOfUsersToRetrieve);
        }

        public IEnumerable<IUser> GetListMembers(string slug, string ownerScreenName, int maximumNumberOfUsersToRetrieve = 100)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return GetListMembers(identifier, maximumNumberOfUsersToRetrieve);
        }

        public IEnumerable<IUser> GetListMembers(string slug, long ownerId, int maximumNumberOfUsersToRetrieve = 100)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return GetListMembers(identifier, maximumNumberOfUsersToRetrieve);
        }

        public IEnumerable<IUser> GetListMembers(ITwitterListIdentifier list, int maximumNumberOfUsersToRetrieve = 100)
        {
            var usersDTO = _twitterListQueryExecutor.GetMembersOfList(list, maximumNumberOfUsersToRetrieve);
            return _userFactory.GenerateUsersFromDTO(usersDTO);
        } 
        #endregion

        #region Add Member To List

        public bool AddMemberToList(long listId, long newUserId)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return AddMemberToList(identifier, newUserId);
        }

        public bool AddMemberToList(long listId, string newUserName)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return AddMemberToList(identifier, newUserName);
        }

        public bool AddMemberToList(long listId, IUserIdentifier newUser)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return AddMemberToList(identifier, newUser);
        }

        public bool AddMemberToList(string slug, long ownerId, long newUserId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return AddMemberToList(identifier, newUserId);
        }

        public bool AddMemberToList(string slug, long ownerId, string newUserName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return AddMemberToList(identifier, newUserName);
        }

        public bool AddMemberToList(string slug, long ownerId, IUserIdentifier newUser)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return AddMemberToList(identifier, newUser);
        }

        public bool AddMemberToList(string slug, string ownerScreenName, long newUserId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return AddMemberToList(identifier, newUserId);
        }

        public bool AddMemberToList(string slug, string ownerScreenName, string newUserName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return AddMemberToList(identifier, newUserName);
        }

        public bool AddMemberToList(string slug, string ownerScreenName, IUserIdentifier newUser)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return AddMemberToList(identifier, newUser);
        }

        public bool AddMemberToList(string slug, IUserIdentifier owner, long newUserId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return AddMemberToList(identifier, newUserId);
        }

        public bool AddMemberToList(string slug, IUserIdentifier owner, string newUserName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return AddMemberToList(identifier, newUserName);
        }

        public bool AddMemberToList(string slug, IUserIdentifier owner, IUserIdentifier newUser)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return AddMemberToList(identifier, newUser);
        }

        public bool AddMemberToList(ITwitterListIdentifier list, long newUserId)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromId(newUserId);
            return AddMemberToList(list, userIdentifier);
        }

        public bool AddMemberToList(ITwitterListIdentifier list, string newUserName)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromScreenName(newUserName);
            return AddMemberToList(list, userIdentifier);
        }

        public bool AddMemberToList(ITwitterListIdentifier list, IUserIdentifier newUser)
        {
            return _twitterListQueryExecutor.AddMemberToList(list, newUser);
        }

        // Add Multiple Members to List
        public MultiRequestsResult AddMultipleMembersToList(long listId, IEnumerable<long> userIds)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return AddMultipleMembersToList(listIdentifier, userIds);
        }

        public MultiRequestsResult AddMultipleMembersToList(long listId, IEnumerable<string> userScreenNames)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return AddMultipleMembersToList(listIdentifier, userScreenNames);
        }

        public MultiRequestsResult AddMultipleMembersToList(long listId, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return AddMultipleMembersToList(listIdentifier, userIdentifiers);
        }

        public MultiRequestsResult AddMultipleMembersToList(string slug, long ownerId, IEnumerable<long> userIds)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return AddMultipleMembersToList(listIdentifier, userIds);
        }

        public MultiRequestsResult AddMultipleMembersToList(string slug, long ownerId, IEnumerable<string> userScreenNames)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return AddMultipleMembersToList(listIdentifier, userScreenNames);
        }

        public MultiRequestsResult AddMultipleMembersToList(string slug, long ownerId, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return AddMultipleMembersToList(listIdentifier, userIdentifiers);
        }

        public MultiRequestsResult AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<long> userIds)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return AddMultipleMembersToList(listIdentifier, userIds);
        }

        public MultiRequestsResult AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<string> userScreenNames)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return AddMultipleMembersToList(listIdentifier, userScreenNames);
        }

        public MultiRequestsResult AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return AddMultipleMembersToList(listIdentifier, userIdentifiers);
        }

        public MultiRequestsResult AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<long> userIds)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return AddMultipleMembersToList(listIdentifier, userIds);
        }

        public MultiRequestsResult AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<string> userScreenNames)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return AddMultipleMembersToList(listIdentifier, userScreenNames);
        }

        public MultiRequestsResult AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return AddMultipleMembersToList(listIdentifier, userIdentifiers);
        }

        public MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<long> newUserIds)
        {
            var userIdentifiers = newUserIds.Select(userId => _userFactory.GenerateUserIdentifierFromId(userId));
            return AddMultipleMembersToList(list, userIdentifiers);
        }

        public MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<string> newUserScreenNames)
        {
            var userIdentifiers = newUserScreenNames.Select(screenName => _userFactory.GenerateUserIdentifierFromScreenName(screenName));
            return AddMultipleMembersToList(list, userIdentifiers);
        }

        public MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> newUserIdentifiers)
        {
            return _twitterListQueryExecutor.AddMultipleMembersToList(list, newUserIdentifiers);
        }

        #endregion

        #region Remove Member From List

        public bool RemoveMemberFromList(long listId, long newUserId)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return RemoveMemberFromList(identifier, newUserId);
        }

        public bool RemoveMemberFromList(long listId, string newUserName)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return RemoveMemberFromList(identifier, newUserName);
        }

        public bool RemoveMemberFromList(long listId, IUserIdentifier newUser)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return RemoveMemberFromList(identifier, newUser);
        }

        public bool RemoveMemberFromList(string slug, long ownerId, long newUserId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return RemoveMemberFromList(identifier, newUserId);
        }

        public bool RemoveMemberFromList(string slug, long ownerId, string newUserName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return RemoveMemberFromList(identifier, newUserName);
        }

        public bool RemoveMemberFromList(string slug, long ownerId, IUserIdentifier newUser)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return RemoveMemberFromList(identifier, newUser);
        }

        public bool RemoveMemberFromList(string slug, string ownerScreenName, long newUserId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return RemoveMemberFromList(identifier, newUserId);
        }

        public bool RemoveMemberFromList(string slug, string ownerScreenName, string newUserName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return RemoveMemberFromList(identifier, newUserName);
        }

        public bool RemoveMemberFromList(string slug, string ownerScreenName, IUserIdentifier newUser)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return RemoveMemberFromList(identifier, newUser);
        }

        public bool RemoveMemberFromList(string slug, IUserIdentifier owner, long newUserId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return RemoveMemberFromList(identifier, newUserId);
        }

        public bool RemoveMemberFromList(string slug, IUserIdentifier owner, string newUserName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return RemoveMemberFromList(identifier, newUserName);
        }

        public bool RemoveMemberFromList(string slug, IUserIdentifier owner, IUserIdentifier newUser)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return RemoveMemberFromList(identifier, newUser);
        }

        public bool RemoveMemberFromList(ITwitterListIdentifier list, long newUserId)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromId(newUserId);
            return RemoveMemberFromList(list, userIdentifier);
        }

        public bool RemoveMemberFromList(ITwitterListIdentifier list, string newUserName)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromScreenName(newUserName);
            return RemoveMemberFromList(list, userIdentifier);
        }
        
        public bool RemoveMemberFromList(ITwitterListIdentifier list, IUserIdentifier newUser)
        {
            return _twitterListQueryExecutor.RemoveMemberFromList(list, newUser);
        }

        // Multiple

        public MultiRequestsResult RemoveMultipleMembersFromList(long listId, IEnumerable<long> userIds)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return RemoveMultipleMembersFromList(listIdentifier, userIds);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(long listId, IEnumerable<string> userScreenNames)
        {
            throw new NotImplementedException();
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(long listId, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            throw new NotImplementedException();
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<long> userIds)
        {
            throw new NotImplementedException();
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<string> userScreenNames)
        {
            throw new NotImplementedException();
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            throw new NotImplementedException();
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<long> userIds)
        {
            throw new NotImplementedException();
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<string> userScreenNames)
        {
            throw new NotImplementedException();
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            throw new NotImplementedException();
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<long> userIds)
        {
            throw new NotImplementedException();
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<string> userScreenNames)
        {
            throw new NotImplementedException();
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            throw new NotImplementedException();
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<long> userIds)
        {
            var userIdentifiers = userIds.Select(userId => _userFactory.GenerateUserIdentifierFromId(userId));
            return RemoveMultipleMembersFromList(list, userIdentifiers);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<string> userScreenNames)
        {
            var userIdentifiers = userScreenNames.Select(screenName => _userFactory.GenerateUserIdentifierFromScreenName(screenName));
            return RemoveMultipleMembersFromList(list, userIdentifiers);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            return _twitterListQueryExecutor.RemoveMultipleMembersFromList(list, userIdentifiers);
        }


        #endregion

        // User List Subscriptions
        public IEnumerable<ITwitterList> GetUserSubscribedLists(long userId, int maxNumberOfListsToRetrieve)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromId(userId);
            return GetUserSubscribedLists(userIdentifier, maxNumberOfListsToRetrieve);
        }

        public IEnumerable<ITwitterList> GetUserSubscribedLists(string userName, int maxNumberOfListsToRetrieve)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromScreenName(userName);
            return GetUserSubscribedLists(userIdentifier, maxNumberOfListsToRetrieve);
        }

        public IEnumerable<ITwitterList> GetUserSubscribedLists(IUserIdentifier user, int maxNumberOfListsToRetrieve)
        {
            var listDTOs = _twitterListQueryExecutor.GetUserSubscribedLists(user, maxNumberOfListsToRetrieve);
            return _twitterListsFactory.CreateListsFromDTOs(listDTOs);
        }
        
        // Check Membership
        public bool CheckIfUserIsAListMember(long listId, long userId)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return CheckIfUserIsAListMember(listIdentifier, userId);
        }

        public bool CheckIfUserIsAListMember(long listId, string userScreenName)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return CheckIfUserIsAListMember(listIdentifier, userScreenName);
        }

        public bool CheckIfUserIsAListMember(long listId, IUserIdentifier userIdentifier)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return CheckIfUserIsAListMember(listIdentifier, userIdentifier);
        }

        public bool CheckIfUserIsAListMember(string slug, long ownerId, long userId)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return CheckIfUserIsAListMember(listIdentifier, userId);
        }

        public bool CheckIfUserIsAListMember(string slug, long ownerId, string userScreenName)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return CheckIfUserIsAListMember(listIdentifier, userScreenName);
        }

        public bool CheckIfUserIsAListMember(string slug, long ownerId, IUserIdentifier userIdentifier)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return CheckIfUserIsAListMember(listIdentifier, userIdentifier);
        }

        public bool CheckIfUserIsAListMember(string slug, string ownerScreenName, long userId)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return CheckIfUserIsAListMember(listIdentifier, userId);
        }

        public bool CheckIfUserIsAListMember(string slug, string ownerScreenName, string userScreenName)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return CheckIfUserIsAListMember(listIdentifier, userScreenName);
        }

        public bool CheckIfUserIsAListMember(string slug, string ownerScreenName, IUserIdentifier userIdentifier)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return CheckIfUserIsAListMember(listIdentifier, userIdentifier);
        }

        public bool CheckIfUserIsAListMember(string slug, IUserIdentifier owner, long userId)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return CheckIfUserIsAListMember(listIdentifier, userId);
        }

        public bool CheckIfUserIsAListMember(string slug, IUserIdentifier owner, string userScreenName)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return CheckIfUserIsAListMember(listIdentifier, userScreenName);
        }

        public bool CheckIfUserIsAListMember(string slug, IUserIdentifier owner, IUserIdentifier userIdentifier)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return CheckIfUserIsAListMember(listIdentifier, userIdentifier);
        }

        public bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, long userId)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromId(userId);
            return CheckIfUserIsAListMember(listIdentifier, userIdentifier);
        }

        public bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, string userScreenName)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromScreenName(userScreenName);
            return CheckIfUserIsAListMember(listIdentifier, userIdentifier);
        }

        public bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            return _twitterListQueryExecutor.CheckIfUserIsAListMember(listIdentifier, userIdentifier);
        }

        // Get list subscribers
        public IEnumerable<IUser> GetListSubscribers(long listId, int maximumNumberOfUsersToRetrieve = 100)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return GetListSubscribers(identifier, maximumNumberOfUsersToRetrieve);
        }

        public IEnumerable<IUser> GetListSubscribers(string slug, IUserIdentifier owner, int maximumNumberOfUsersToRetrieve = 100)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return GetListSubscribers(identifier, maximumNumberOfUsersToRetrieve);
        }

        public IEnumerable<IUser> GetListSubscribers(string slug, string ownerScreenName, int maximumNumberOfUsersToRetrieve = 100)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return GetListSubscribers(identifier, maximumNumberOfUsersToRetrieve);
        }

        public IEnumerable<IUser> GetListSubscribers(string slug, long ownerId, int maximumNumberOfUsersToRetrieve = 100)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return GetListSubscribers(identifier, maximumNumberOfUsersToRetrieve);
        }

        public IEnumerable<IUser> GetListSubscribers(ITwitterListIdentifier list, int maximumNumberOfUsersToRetrieve = 100)
        {
            var usersDTO = _twitterListQueryExecutor.GetListSubscribers(list, maximumNumberOfUsersToRetrieve);
            return _userFactory.GenerateUsersFromDTO(usersDTO);
        } 

        // Add subscriber to List
        public bool SubscribeAuthenticatedUserToList(long listId)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return SubscribeAuthenticatedUserToList(identifier);
        }

        public bool SubscribeAuthenticatedUserToList(string slug, long ownerId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return SubscribeAuthenticatedUserToList(identifier);
        }

        public bool SubscribeAuthenticatedUserToList(string slug, string ownerScreenName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return SubscribeAuthenticatedUserToList(identifier);
        }

        public bool SubscribeAuthenticatedUserToList(string slug, IUserIdentifier owner)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return SubscribeAuthenticatedUserToList(identifier);
        }

        public bool SubscribeAuthenticatedUserToList(ITwitterListIdentifier list)
        {
            return _twitterListQueryExecutor.SubscribeAuthenticatedUserToList(list);
        }

        // Remove subscriber from list
        public bool UnSubscribeAuthenticatedUserFromList(long listId)
        {
            var identifier = _twitterListIdentifierFactory.Create(listId);
            return UnSubscribeAuthenticatedUserFromList(identifier);
        }

        public bool UnSubscribeAuthenticatedUserFromList(string slug, long ownerId)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return UnSubscribeAuthenticatedUserFromList(identifier);
        }

        public bool UnSubscribeAuthenticatedUserFromList(string slug, string ownerScreenName)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return UnSubscribeAuthenticatedUserFromList(identifier);
        }

        public bool UnSubscribeAuthenticatedUserFromList(string slug, IUserIdentifier owner)
        {
            var identifier = _twitterListIdentifierFactory.Create(slug, owner);
            return UnSubscribeAuthenticatedUserFromList(identifier);
        }

        public bool UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier list)
        {
            return _twitterListQueryExecutor.UnSubscribeAuthenticatedUserFromList(list);
        }

        // Check Subscriptions
        public bool CheckIfUserIsAListSubscriber(long listId, long userId)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return CheckIfUserIsAListSubscriber(listIdentifier, userId);
        }

        public bool CheckIfUserIsAListSubscriber(long listId, string userScreenName)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return CheckIfUserIsAListSubscriber(listIdentifier, userScreenName);
        }

        public bool CheckIfUserIsAListSubscriber(long listId, IUserIdentifier userIdentifier)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return CheckIfUserIsAListSubscriber(listIdentifier, userIdentifier);
        }

        public bool CheckIfUserIsAListSubscriber(string slug, long ownerId, long userId)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return CheckIfUserIsAListSubscriber(listIdentifier, userId);
        }

        public bool CheckIfUserIsAListSubscriber(string slug, long ownerId, string userScreenName)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return CheckIfUserIsAListSubscriber(listIdentifier, userScreenName);
        }

        public bool CheckIfUserIsAListSubscriber(string slug, long ownerId, IUserIdentifier userIdentifier)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return CheckIfUserIsAListSubscriber(listIdentifier, userIdentifier);
        }

        public bool CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, long userId)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return CheckIfUserIsAListSubscriber(listIdentifier, userId);
        }

        public bool CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, string userScreenName)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return CheckIfUserIsAListSubscriber(listIdentifier, userScreenName);
        }

        public bool CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, IUserIdentifier userIdentifier)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return CheckIfUserIsAListSubscriber(listIdentifier, userIdentifier);
        }

        public bool CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, long userId)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return CheckIfUserIsAListSubscriber(listIdentifier, userId);
        }

        public bool CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, string userScreenName)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return CheckIfUserIsAListSubscriber(listIdentifier, userScreenName);
        }

        public bool CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, IUserIdentifier userIdentifier)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return CheckIfUserIsAListSubscriber(listIdentifier, userIdentifier);
        }

        public bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, long userId)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromId(userId);
            return CheckIfUserIsAListSubscriber(listIdentifier, userIdentifier);
        }

        public bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, string userScreenName)
        {
            var userIdentifier = _userFactory.GenerateUserIdentifierFromScreenName(userScreenName);
            return CheckIfUserIsAListSubscriber(listIdentifier, userIdentifier);
        }

        public bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            return _twitterListQueryExecutor.CheckIfUserIsAListSubscriber(listIdentifier, userIdentifier);
        }
    }
}