using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Core.Parameters;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Public.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Logic.QueryParameters;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

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

        #region Get User Lists

        public IEnumerable<ITwitterList> GetUserSubscribedLists(IUserIdentifier user, bool getOwnedListsFirst)
        {
            var listDTOs = _twitterListQueryExecutor.GetUserSubscribedLists(user, getOwnedListsFirst);
            return _twitterListsFactory.CreateListsFromDTOs(listDTOs);
        }

        public IEnumerable<ITwitterList> GetUserSubscribedLists(long userId, bool getOwnedListsFirst)
        {
            var listDTOs = _twitterListQueryExecutor.GetUserSubscribedLists(new UserIdentifier(userId), getOwnedListsFirst);
            return _twitterListsFactory.CreateListsFromDTOs(listDTOs);
        }

        public IEnumerable<ITwitterList> GetUserSubscribedLists(string userScreenName, bool getOwnedListsFirst)
        {
            var listDTOs = _twitterListQueryExecutor.GetUserSubscribedLists(new UserIdentifier(userScreenName), getOwnedListsFirst);
            return _twitterListsFactory.CreateListsFromDTOs(listDTOs);
        }

        #endregion

        #region Owned Lists
        public IEnumerable<ITwitterList> GetUserOwnedLists(long userId, int maximumNumberOfListsToRetrieve)
        {
            var user = new UserIdentifier(userId);
            return GetUserOwnedLists(user, maximumNumberOfListsToRetrieve);
        }

        public IEnumerable<ITwitterList> GetUserOwnedLists(string userScreenName, int maximumNumberOfListsToRetrieve)
        {
            var user = new UserIdentifier(userScreenName);
            return GetUserOwnedLists(user, maximumNumberOfListsToRetrieve);
        }

        public IEnumerable<ITwitterList> GetUserOwnedLists(IUserIdentifier user, int maximumNumberOfListsToRetrieve)
        {
            var listDTOs = _twitterListQueryExecutor.GetUserOwnedLists(user, maximumNumberOfListsToRetrieve);
            return _twitterListsFactory.CreateListsFromDTOs(listDTOs);
        } 
        #endregion

        #region Update List
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
        #endregion

        #region Destroy List
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
        #endregion

        #region Get Tweets from List
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
        #endregion

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
            var user = _userFactory.GenerateUserIdentifierFromId(newUserId);
            return AddMemberToList(list, user);
        }

        public bool AddMemberToList(ITwitterListIdentifier list, string newUserName)
        {
            var user = _userFactory.GenerateUserIdentifierFromScreenName(newUserName);
            return AddMemberToList(list, user);
        }

        public bool AddMemberToList(ITwitterListIdentifier list, IUserIdentifier newUser)
        {
            return _twitterListQueryExecutor.AddMemberToList(list, newUser);
        }

        #endregion

        #region Add Multiple Members to List

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

        public MultiRequestsResult AddMultipleMembersToList(long listId, IEnumerable<IUserIdentifier> users)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return AddMultipleMembersToList(listIdentifier, users);
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

        public MultiRequestsResult AddMultipleMembersToList(string slug, long ownerId, IEnumerable<IUserIdentifier> users)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return AddMultipleMembersToList(listIdentifier, users);
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

        public MultiRequestsResult AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<IUserIdentifier> users)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return AddMultipleMembersToList(listIdentifier, users);
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

        public MultiRequestsResult AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<IUserIdentifier> users)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return AddMultipleMembersToList(listIdentifier, users);
        }

        public MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<long> newUserIds)
        {
            var users = newUserIds.Select(userId => _userFactory.GenerateUserIdentifierFromId(userId));
            return AddMultipleMembersToList(list, users);
        }

        public MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<string> newUserScreenNames)
        {
            var users = newUserScreenNames.Select(screenName => _userFactory.GenerateUserIdentifierFromScreenName(screenName));
            return AddMultipleMembersToList(list, users);
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
            var user = _userFactory.GenerateUserIdentifierFromId(newUserId);
            return RemoveMemberFromList(list, user);
        }

        public bool RemoveMemberFromList(ITwitterListIdentifier list, string newUserName)
        {
            var user = _userFactory.GenerateUserIdentifierFromScreenName(newUserName);
            return RemoveMemberFromList(list, user);
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
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return RemoveMultipleMembersFromList(listIdentifier, userScreenNames);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(long listId, IEnumerable<IUserIdentifier> users)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return RemoveMultipleMembersFromList(listIdentifier, users);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<long> userIds)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return RemoveMultipleMembersFromList(listIdentifier, userIds);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<string> userScreenNames)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return RemoveMultipleMembersFromList(listIdentifier, userScreenNames);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<IUserIdentifier> users)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return RemoveMultipleMembersFromList(listIdentifier, users);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<long> userIds)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return RemoveMultipleMembersFromList(listIdentifier, userIds);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<string> userScreenNames)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return RemoveMultipleMembersFromList(listIdentifier, userScreenNames);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<IUserIdentifier> users)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return RemoveMultipleMembersFromList(listIdentifier, users);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<long> userIds)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return RemoveMultipleMembersFromList(listIdentifier, userIds);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<string> userScreenNames)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return RemoveMultipleMembersFromList(listIdentifier, userScreenNames);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<IUserIdentifier> users)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return RemoveMultipleMembersFromList(listIdentifier, users);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<long> userIds)
        {
            var users = userIds.Select(userId => _userFactory.GenerateUserIdentifierFromId(userId));
            return RemoveMultipleMembersFromList(list, users);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<string> userScreenNames)
        {
            var users = userScreenNames.Select(screenName => _userFactory.GenerateUserIdentifierFromScreenName(screenName));
            return RemoveMultipleMembersFromList(list, users);
        }

        public MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> users)
        {
            return _twitterListQueryExecutor.RemoveMultipleMembersFromList(list, users);
        }

        #endregion

        #region GetUserSubscribedLists
        public IEnumerable<ITwitterList> GetUserSubscribedLists(long userId, int maxNumberOfListsToRetrieve)
        {
            var user = _userFactory.GenerateUserIdentifierFromId(userId);
            return GetUserSubscribedLists(user, maxNumberOfListsToRetrieve);
        }

        public IEnumerable<ITwitterList> GetUserSubscribedLists(string userName, int maxNumberOfListsToRetrieve)
        {
            var user = _userFactory.GenerateUserIdentifierFromScreenName(userName);
            return GetUserSubscribedLists(user, maxNumberOfListsToRetrieve);
        }

        public IEnumerable<ITwitterList> GetUserSubscribedLists(IUserIdentifier user, int maxNumberOfListsToRetrieve)
        {
            var listDTOs = _twitterListQueryExecutor.GetUserSubscribedLists(user, maxNumberOfListsToRetrieve);
            return _twitterListsFactory.CreateListsFromDTOs(listDTOs);
        } 
        #endregion

        #region Check Membership
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

        public bool CheckIfUserIsAListMember(long listId, IUserIdentifier user)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return CheckIfUserIsAListMember(listIdentifier, user);
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

        public bool CheckIfUserIsAListMember(string slug, long ownerId, IUserIdentifier user)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return CheckIfUserIsAListMember(listIdentifier, user);
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

        public bool CheckIfUserIsAListMember(string slug, string ownerScreenName, IUserIdentifier user)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return CheckIfUserIsAListMember(listIdentifier, user);
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

        public bool CheckIfUserIsAListMember(string slug, IUserIdentifier owner, IUserIdentifier user)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return CheckIfUserIsAListMember(listIdentifier, user);
        }

        public bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, long userId)
        {
            var user = _userFactory.GenerateUserIdentifierFromId(userId);
            return CheckIfUserIsAListMember(listIdentifier, user);
        }

        public bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, string userScreenName)
        {
            var user = _userFactory.GenerateUserIdentifierFromScreenName(userScreenName);
            return CheckIfUserIsAListMember(listIdentifier, user);
        }

        public bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            return _twitterListQueryExecutor.CheckIfUserIsAListMember(listIdentifier, user);
        }
        #endregion

        #region Get list subscribers
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
        #endregion

        #region Add subscriber to List

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

        #endregion

        #region UnSubscribeAuthenticatedUserFromList
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
        #endregion

        #region CheckIfUserIsAListSubscriber
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

        public bool CheckIfUserIsAListSubscriber(long listId, IUserIdentifier user)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(listId);
            return CheckIfUserIsAListSubscriber(listIdentifier, user);
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

        public bool CheckIfUserIsAListSubscriber(string slug, long ownerId, IUserIdentifier user)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerId);
            return CheckIfUserIsAListSubscriber(listIdentifier, user);
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

        public bool CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, IUserIdentifier user)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, ownerScreenName);
            return CheckIfUserIsAListSubscriber(listIdentifier, user);
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

        public bool CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, IUserIdentifier user)
        {
            var listIdentifier = _twitterListIdentifierFactory.Create(slug, owner);
            return CheckIfUserIsAListSubscriber(listIdentifier, user);
        }

        public bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, long userId)
        {
            var user = _userFactory.GenerateUserIdentifierFromId(userId);
            return CheckIfUserIsAListSubscriber(listIdentifier, user);
        }

        public bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, string userScreenName)
        {
            var user = _userFactory.GenerateUserIdentifierFromScreenName(userScreenName);
            return CheckIfUserIsAListSubscriber(listIdentifier, user);
        }

        public bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            return _twitterListQueryExecutor.CheckIfUserIsAListSubscriber(listIdentifier, user);
        }
        #endregion

        public IEnumerable<ITwitterList> GetUserListsMemberships(IUserIdentifier userIdentifier, IGetUserListMembershipsParameters parameters)
        {
            var queryParameters = new GetUserListMembershipsQueryParameters(userIdentifier);

            if (parameters != null)
            {
                queryParameters.Parameters = parameters;
            }

            return GetUserListsMemberships(queryParameters);
        }

        public IEnumerable<ITwitterList> GetUserListsMemberships(IGetUserListMembershipsQueryParameters parameters)
        {
            var twitterListDtos = _twitterListQueryExecutor.GetUserListMemberships(parameters);
            return _twitterListsFactory.CreateListsFromDTOs(twitterListDtos);
        }
    }
}