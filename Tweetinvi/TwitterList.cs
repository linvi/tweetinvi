using System;
using System.Collections.Generic;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi
{
    public static class TwitterList
    {
        [ThreadStatic]
        private static ITwitterListFactory _twitterListFactory;
        public static ITwitterListFactory TwitterListFactory
        {
            get
            {
                if (_twitterListFactory == null)
                {
                    Initialize();
                }

                return _twitterListFactory;
            }
        }

        [ThreadStatic]
        private static ITwitterListController _twitterListController;
        public static ITwitterListController TwitterListController
        {
            get
            {
                if (_twitterListController == null)
                {
                    Initialize();
                }

                return _twitterListController;
            }
        }

        private static readonly ITwitterListQueryParameterGenerator _twitterListQueryParameterGenerator;
        public static ITwitterListQueryParameterGenerator TwitterListQueryParameterGenerator
        {
            get { return _twitterListQueryParameterGenerator; }
        }

        private static readonly ITwitterListIdentifierFactory _twitterListIdentifierFactory;

        static TwitterList()
        {
            Initialize();

            _twitterListIdentifierFactory = TweetinviContainer.Resolve<ITwitterListIdentifierFactory>();
            _twitterListQueryParameterGenerator = TweetinviContainer.Resolve<ITwitterListQueryParameterGenerator>();
        }

        private static void Initialize()
        {
            _twitterListFactory = TweetinviContainer.Resolve<ITwitterListFactory>();
            _twitterListController = TweetinviContainer.Resolve<ITwitterListController>();
        }

       // Get Existing List
        public static ITwitterList GetExistingList(ITwitterListIdentifier twitterListIdentifier)
        {
            return TwitterListFactory.GetExistingList(twitterListIdentifier);
        }

        public static ITwitterList GetExistingList(long listId)
        {
            return TwitterListFactory.GetExistingList(listId);
        }

        public static ITwitterList GetExistingList(string slug, IUser user)
        {
            return TwitterListFactory.GetExistingList(slug, user);
        }

        public static ITwitterList GetExistingList(string slug, IUserIdentifier userDTO)
        {
            return TwitterListFactory.GetExistingList(slug, userDTO);
        }

        public static ITwitterList GetExistingList(string slug, long userId)
        {
            return TwitterListFactory.GetExistingList(slug, userId);
        }

        public static ITwitterList GetExistingList(string slug, string userScreenName)
        {
            return TwitterListFactory.GetExistingList(slug, userScreenName);
        }

        // Owner Lists
        public static IEnumerable<ITwitterList> GetUserOwnedLists(long userId, int maximumNumberOfListsToRetrieve = TweetinviConsts.TWITTER_LIST_OWNED_COUNT)
        {
            return TwitterListController.GetUserOwnedLists(userId, maximumNumberOfListsToRetrieve);
        }

        public static IEnumerable<ITwitterList> GetUserOwnedLists(string userScreenName, int maximumNumberOfListsToRetrieve = TweetinviConsts.TWITTER_LIST_OWNED_COUNT)
        {
            return TwitterListController.GetUserOwnedLists(userScreenName, maximumNumberOfListsToRetrieve);
        }

        public static IEnumerable<ITwitterList> GetUserOwnedLists(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve = TweetinviConsts.TWITTER_LIST_OWNED_COUNT)
        {
            return TwitterListController.GetUserOwnedLists(userIdentifier, maximumNumberOfListsToRetrieve);
        }

        // Create List
        public static ITwitterList CreateList(string name, PrivacyMode privacyMode, string description = null)
        {
            return TwitterListFactory.CreateList(name, privacyMode, description);
        }

        // Update List
        public static ITwitterList UpdateList(ITwitterListIdentifier twitterListIdentifier, ITwitterListUpdateParameters parameters)
        {
            return TwitterListController.UpdateList(twitterListIdentifier, parameters);
        }

        public static ITwitterList UpdateList(long listId, ITwitterListUpdateParameters parameters)
        {
            return TwitterListController.UpdateList(listId, parameters);
        }

        public static ITwitterList UpdateList(string slug, IUser owner, ITwitterListUpdateParameters parameters)
        {
            return TwitterListController.UpdateList(slug, owner, parameters);
        }

        public static ITwitterList UpdateList(string slug, IUserIdentifier ownerDTO, ITwitterListUpdateParameters parameters)
        {
            return TwitterListController.UpdateList(slug, ownerDTO, parameters);
        }

        public static ITwitterList UpdateList(string slug, long ownerId, ITwitterListUpdateParameters parameters)
        {
            return TwitterListController.UpdateList(slug, ownerId, parameters);
        }

        public static ITwitterList UpdateList(string slug, string ownerScreenName, ITwitterListUpdateParameters parameters)
        {
            return TwitterListController.UpdateList(slug, ownerScreenName, parameters);
        }

        // Destroy List
        public static bool DestroyList(ITwitterListIdentifier list)
        {
            return TwitterListController.DestroyList(list);
        }

        public static bool DestroyList(long listId)
        {
            return TwitterListController.DestroyList(listId);
        }

        public static bool DestroyList(string slug, IUser owner)
        {
            return TwitterListController.DestroyList(slug, owner);
        }

        public static bool DestroyList(string slug, IUserDTO ownerDTO)
        {
            return TwitterListController.DestroyList(slug, ownerDTO);
        }

        public static bool DestroyList(string slug, long ownerId)
        {
            return TwitterListController.DestroyList(slug, ownerId);
        }

        public static bool DestroyList(string slug, string ownerScreenName)
        {
            return TwitterListController.DestroyList(slug, ownerScreenName);
        }

        // Get Tweets from List
        public static IEnumerable<ITweet> GetTweetsFromList(long listId)
        {
            return TwitterListController.GetTweetsFromList(listId);
        }

        public static IEnumerable<ITweet> GetTweetsFromList(string slug, IUser owner)
        {
            return TwitterListController.GetTweetsFromList(slug, owner);
        }

        public static IEnumerable<ITweet> GetTweetsFromList(string slug, IUserIdentifier ownerDTO)
        {
            return TwitterListController.GetTweetsFromList(slug, ownerDTO);
        }

        public static IEnumerable<ITweet> GetTweetsFromList(string slug, string ownerScreenName)
        {
            return TwitterListController.GetTweetsFromList(slug, ownerScreenName);
        }

        public static IEnumerable<ITweet> GetTweetsFromList(string slug, long ownerId)
        {
            return TwitterListController.GetTweetsFromList(slug, ownerId);
        }

        public static IEnumerable<ITweet> GetTweetsFromList(ITwitterListIdentifier list, IGetTweetsFromListParameters parameters = null)
        {
            return TwitterListController.GetTweetsFromList(list, parameters);
        }

        // Get Members of List
        public static IEnumerable<IUser> GetMembersOfList(ITwitterListIdentifier list, int maxNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListMembers(list, maxNumberOfUsersToRetrieve);
        }

        public static IEnumerable<IUser> GetMembersOfList(long listId, int maxNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListMembers(listId, maxNumberOfUsersToRetrieve);
        }

        public static IEnumerable<IUser> GetMembersOfList(string slug, IUser owner, int maxNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListMembers(slug, owner, maxNumberOfUsersToRetrieve);
        }

        public static IEnumerable<IUser> GetMembersOfList(string slug, IUserIdentifier ownerDTO, int maxNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListMembers(slug, ownerDTO, maxNumberOfUsersToRetrieve);
        }

        public static IEnumerable<IUser> GetMembersOfList(string slug, string ownerScreenName, int maxNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListMembers(slug, ownerScreenName, maxNumberOfUsersToRetrieve);
        }

        public static IEnumerable<IUser> GetMembersOfList(string slug, long ownerId, int maxNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListMembers(slug, ownerId, maxNumberOfUsersToRetrieve);
        }

        // Create Member
        public static bool AddMemberToList(long listId, long newUserId)
        {
            return TwitterListController.AddMemberToList(listId, newUserId);
        }

        public static bool AddMemberToList(long listId, string newUserName)
        {
            return TwitterListController.AddMemberToList(listId, newUserName);
        }

        public static bool AddMemberToList(long listId, IUserIdentifier newUser)
        {
            return TwitterListController.AddMemberToList(listId, newUser);
        }

        public static bool AddMemberToList(string slug, long ownerId, long newUserId)
        {
            return TwitterListController.AddMemberToList(slug, ownerId, newUserId);
        }

        public static bool AddMemberToList(string slug, long ownerId, string newUserName)
        {
            return TwitterListController.AddMemberToList(slug, ownerId, newUserName);
        }

        public static bool AddMemberToList(string slug, long ownerId, IUserIdentifier newUser)
        {
            return TwitterListController.AddMemberToList(slug, ownerId, newUser);
        }

        public static bool AddMemberToList(string slug, string ownerScreenName, long newUserId)
        {
            return TwitterListController.AddMemberToList(slug, ownerScreenName, newUserId);
        }

        public static bool AddMemberToList(string slug, string ownerScreenName, string newUserName)
        {
            return TwitterListController.AddMemberToList(slug, ownerScreenName, newUserName);
        }

        public static bool AddMemberToList(string slug, string ownerScreenName, IUserIdentifier newUser)
        {
            return TwitterListController.AddMemberToList(slug, ownerScreenName, newUser);
        }

        public static bool AddMemberToList(string slug, IUserIdentifier owner, long newUserId)
        {
            return TwitterListController.AddMemberToList(slug, owner, newUserId);
        }

        public static bool AddMemberToList(string slug, IUserIdentifier owner, string newUserName)
        {
            return TwitterListController.AddMemberToList(slug, owner, newUserName);
        }

        public static bool AddMemberToList(string slug, IUserIdentifier owner, IUserIdentifier newUser)
        {
            return TwitterListController.AddMemberToList(slug, owner, newUser);
        }

        public static bool AddMemberToList(ITwitterListIdentifier list, long newUserId)
        {
            return TwitterListController.AddMemberToList(list, newUserId);
        }

        public static bool AddMemberToList(ITwitterListIdentifier list, string newUserName)
        {
            return TwitterListController.AddMemberToList(list, newUserName);
        }

        public static bool AddMemberToList(ITwitterListIdentifier list, IUserIdentifier newUser)
        {
            return TwitterListController.AddMemberToList(list, newUser);
        } 

        // Create Multiple Members
        public static MultiRequestsResult AddMultipleMembersToList(long listId, IEnumerable<long> newUserIds)
        {
            return TwitterListController.AddMultipleMembersToList(listId, newUserIds);
        }

        public static MultiRequestsResult AddMultipleMembersToList(long listId, IEnumerable<string> newUserScreenNames)
        {
            return TwitterListController.AddMultipleMembersToList(listId, newUserScreenNames);
        }

        public static MultiRequestsResult AddMultipleMembersToList(long listId, IEnumerable<IUserIdentifier> newUsers)
        {
            return TwitterListController.AddMultipleMembersToList(listId, newUsers);
        }

        public static MultiRequestsResult AddMultipleMembersToList(string slug, long ownerId, IEnumerable<long> newUserIds)
        {
            return TwitterListController.AddMultipleMembersToList(slug, ownerId, newUserIds);
        }

        public static MultiRequestsResult AddMultipleMembersToList(string slug, long ownerId, IEnumerable<string> newUserScreenNames)
        {
            return TwitterListController.AddMultipleMembersToList(slug, ownerId, newUserScreenNames);
        }

        public static MultiRequestsResult AddMultipleMembersToList(string slug, long ownerId, IEnumerable<IUserIdentifier> newUsers)
        {
            return TwitterListController.AddMultipleMembersToList(slug, ownerId, newUsers);
        }

        public static MultiRequestsResult AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<long> newUserIds)
        {
            return TwitterListController.AddMultipleMembersToList(slug, ownerScreenName, newUserIds);
        }

        public static MultiRequestsResult AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<string> newUserScreenNames)
        {
            return TwitterListController.AddMultipleMembersToList(slug, ownerScreenName, newUserScreenNames);
        }

        public static MultiRequestsResult AddMultipleMembersToList(string slug, string ownerScreenName, IEnumerable<IUserIdentifier> newUsers)
        {
            return TwitterListController.AddMultipleMembersToList(slug, ownerScreenName, newUsers);
        }

        public static MultiRequestsResult AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<long> newUserIds)
        {
            return TwitterListController.AddMultipleMembersToList(slug, owner, newUserIds);
        }

        public static MultiRequestsResult AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<string> newUserScreenNames)
        {
            return TwitterListController.AddMultipleMembersToList(slug, owner, newUserScreenNames);
        }

        public static MultiRequestsResult AddMultipleMembersToList(string slug, IUserIdentifier owner, IEnumerable<IUserIdentifier> newUsers)
        {
            return TwitterListController.AddMultipleMembersToList(slug, owner, newUsers);
        }

        public static MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<long> newUserIds)
        {
            return TwitterListController.AddMultipleMembersToList(list, newUserIds);
        }

        public static MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<string> newUserScreenNames)
        {
            return TwitterListController.AddMultipleMembersToList(list, newUserScreenNames);
        }

        public static MultiRequestsResult AddMultipleMembersToList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            return TwitterListController.AddMultipleMembersToList(list, userIdentifiers);
        }


        // Remove Member From List
        public static bool RemoveMemberFromList(long listId, long userId)
        {
            return TwitterListController.RemoveMemberFromList(listId, userId);
        }

        public static bool RemoveMemberFromList(long listId, string userName)
        {
            return TwitterListController.RemoveMemberFromList(listId, userName);
        }

        public static bool RemoveMemberFromList(long listId, IUserIdentifier user)
        {
            return TwitterListController.RemoveMemberFromList(listId, user);
        }

        public static bool RemoveMemberFromList(string slug, long ownerId, long userId)
        {
            return TwitterListController.RemoveMemberFromList(slug, ownerId, userId);
        }

        public static bool RemoveMemberFromList(string slug, long ownerId, string userName)
        {
            return TwitterListController.RemoveMemberFromList(slug, ownerId, userName);
        }

        public static bool RemoveMemberFromList(string slug, long ownerId, IUserIdentifier user)
        {
            return TwitterListController.RemoveMemberFromList(slug, ownerId, user);
        }

        public static bool RemoveMemberFromList(string slug, string ownerScreenName, long userId)
        {
            return TwitterListController.RemoveMemberFromList(slug, ownerScreenName, userId);
        }

        public static bool RemoveMemberFromList(string slug, string ownerScreenName, string userName)
        {
            return TwitterListController.RemoveMemberFromList(slug, ownerScreenName, userName);
        }

        public static bool RemoveMemberFromList(string slug, string ownerScreenName, IUserIdentifier user)
        {
            return TwitterListController.RemoveMemberFromList(slug, ownerScreenName, user);
        }

        public static bool RemoveMemberFromList(string slug, IUserIdentifier owner, long userId)
        {
            return TwitterListController.RemoveMemberFromList(slug, owner, userId);
        }

        public static bool RemoveMemberFromList(string slug, IUserIdentifier owner, string userName)
        {
            return TwitterListController.RemoveMemberFromList(slug, owner, userName);
        }

        public static bool RemoveMemberFromList(string slug, IUserIdentifier owner, IUserIdentifier user)
        {
            return TwitterListController.RemoveMemberFromList(slug, owner, user);
        }

        public static bool RemoveMemberFromList(ITwitterListIdentifier list, long userId)
        {
            return TwitterListController.RemoveMemberFromList(list, userId);
        }

        public static bool RemoveMemberFromList(ITwitterListIdentifier list, string userName)
        {
            return TwitterListController.RemoveMemberFromList(list, userName);
        }

        public static bool RemoveMemberFromList(ITwitterListIdentifier list, IUserIdentifier user)
        {
            return TwitterListController.RemoveMemberFromList(list, user);
        }

        // Remove Multiple
        public static MultiRequestsResult RemoveMultipleMembersFromList(long listId, IEnumerable<long> userIdsToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(listId, userIdsToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(long listId, IEnumerable<string> userScreenNamesToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(listId, userScreenNamesToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(long listId, IEnumerable<IUserIdentifier> userIdentifiersToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(listId, userIdentifiersToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<long> userIdsToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, ownerId, userIdsToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<string> userScreenNamesToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, ownerId, userScreenNamesToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(string slug, long ownerId, IEnumerable<IUserIdentifier> userIdentifiersToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, ownerId, userIdentifiersToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<long> userIdsToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, ownerScreenName, userIdsToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<string> userScreenNamesToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, ownerScreenName, userScreenNamesToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(string slug, string ownerScreenName, IEnumerable<IUserIdentifier> userIdentifiersToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, ownerScreenName, userIdentifiersToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<long> userIdsToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, owner, userIdsToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<string> userScreenNamesToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, owner, userScreenNamesToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(string slug, IUserIdentifier owner, IEnumerable<IUserIdentifier> userIdentifiersToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(slug, owner, userIdentifiersToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<long> userIdsToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(list, userIdsToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<string> userScreenNamesToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(list, userScreenNamesToRemove);
        }

        public static MultiRequestsResult RemoveMultipleMembersFromList(ITwitterListIdentifier list, IEnumerable<IUserIdentifier> userIdentifiersToRemove)
        {
            return TwitterListController.RemoveMultipleMembersFromList(list, userIdentifiersToRemove);
        }

        // Check Membership

        public static bool CheckIfUserIsAListMember(long listId, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListMember(listId, newUserId);
        }

        public static bool CheckIfUserIsAListMember(long listId, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListMember(listId, newUserName);
        }

        public static bool CheckIfUserIsAListMember(long listId, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListMember(listId, newUser);
        }

        public static bool CheckIfUserIsAListMember(string slug, long ownerId, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, ownerId, newUserId);
        }

        public static bool CheckIfUserIsAListMember(string slug, long ownerId, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, ownerId, newUserName);
        }

        public static bool CheckIfUserIsAListMember(string slug, long ownerId, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, ownerId, newUser);
        }

        public static bool CheckIfUserIsAListMember(string slug, string ownerScreenName, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, ownerScreenName, newUserId);
        }

        public static bool CheckIfUserIsAListMember(string slug, string ownerScreenName, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, ownerScreenName, newUserName);
        }

        public static bool CheckIfUserIsAListMember(string slug, string ownerScreenName, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, ownerScreenName, newUser);
        }

        public static bool CheckIfUserIsAListMember(string slug, IUserIdentifier owner, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, owner, newUserId);
        }

        public static bool CheckIfUserIsAListMember(string slug, IUserIdentifier owner, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, owner, newUserName);
        }

        public static bool CheckIfUserIsAListMember(string slug, IUserIdentifier owner, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListMember(slug, owner, newUser);
        }

        public static bool CheckIfUserIsAListMember(ITwitterListIdentifier list, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListMember(list, newUserId);
        }

        public static bool CheckIfUserIsAListMember(ITwitterListIdentifier list, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListMember(list, newUserName);
        }

        public static bool CheckIfUserIsAListMember(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            return TwitterListController.CheckIfUserIsAListMember(listIdentifier, userIdentifier);
        }

        // GET User Subscription List
        public static IEnumerable<ITwitterList> GetUserSubscribedLists(long userId, int maxNumberOfListsToRetrieve = TweetinviConsts.TWITTER_LIST_GET_USER_SUBSCRIPTIONS_COUNT)
        {
            return TwitterListController.GetUserSubscribedLists(userId, maxNumberOfListsToRetrieve);
        }

        public static IEnumerable<ITwitterList> GetUserSubscribedLists(string userScreenName, int maxNumberOfListsToRetrieve = TweetinviConsts.TWITTER_LIST_GET_USER_SUBSCRIPTIONS_COUNT)
        {
            return TwitterListController.GetUserSubscribedLists(userScreenName, maxNumberOfListsToRetrieve);
        }

        public static IEnumerable<ITwitterList> GetUserSubscribedLists(IUserIdentifier userIdentifier, int maxNumberOfListsToRetrieve = TweetinviConsts.TWITTER_LIST_GET_USER_SUBSCRIPTIONS_COUNT)
        {
            return TwitterListController.GetUserSubscribedLists(userIdentifier, maxNumberOfListsToRetrieve);
        }

        // Get List Subscribers
        public static IEnumerable<IUser> GetListSubscribers(long listId, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(listId, maximumNumberOfUsersToRetrieve);
        }

        public static IEnumerable<IUser> GetListSubscribers(string slug, IUserIdentifier owner, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(slug, owner, maximumNumberOfUsersToRetrieve);
        }

        public static IEnumerable<IUser> GetListSubscribers(string slug, string ownerScreenName, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(slug, ownerScreenName, maximumNumberOfUsersToRetrieve);
        }

        public static IEnumerable<IUser> GetListSubscribers(string slug, long ownerId, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(slug, ownerId, maximumNumberOfUsersToRetrieve);
        }

        public static IEnumerable<IUser> GetListSubscribers(ITwitterListIdentifier list, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(list, maximumNumberOfUsersToRetrieve);
        } 

        // CREATE Subscription
        public static bool SubscribeLoggedUserToList(long listId, ILoggedUser loggedUser = null)
        {
            if (loggedUser != null)
            {
                return loggedUser.UnSubscribeFromList(listId);
            }

            return TwitterListController.SubscribeLoggedUserToList(listId);
        }

        public static bool SubscribeLoggedUserToList(string slug, IUserIdentifier owner, ILoggedUser loggedUser = null)
        {
            if (loggedUser != null)
            {
                return loggedUser.UnSubscribeFromList(slug, owner);
            }

            return TwitterListController.SubscribeLoggedUserToList(slug, owner);
        }

        public static bool SubscribeLoggedUserToList(string slug, string ownerScreenName, ILoggedUser loggedUser = null)
        {
            if (loggedUser != null)
            {
                return loggedUser.UnSubscribeFromList(slug, ownerScreenName);
            }

            return TwitterListController.SubscribeLoggedUserToList(slug, ownerScreenName);
        }

        public static bool SubscribeLoggedUserToList(string slug, long ownerId, ILoggedUser loggedUser = null)
        {
            if (loggedUser != null)
            {
                return loggedUser.UnSubscribeFromList(slug, ownerId);
            }

            return TwitterListController.SubscribeLoggedUserToList(slug, ownerId);
        }

        public static bool SubscribeLoggedUserToList(ITwitterListIdentifier listIdentifier, ILoggedUser loggedUser = null)
        {
            if (loggedUser != null)
            {
                return loggedUser.UnSubscribeFromList(listIdentifier);
            }

            return TwitterListController.SubscribeLoggedUserToList(listIdentifier);
        }

        // Remove Subscription
        public static bool UnSubscribeLoggedUserToList(long listId, ILoggedUser loggedUser = null)
        {
            if (loggedUser != null)
            {
                return loggedUser.UnSubscribeFromList(listId);
            }

            return TwitterListController.UnSubscribeLoggedUserFromList(listId);
        }

        public static bool UnSubscribeLoggedUserFromList(string slug, IUserIdentifier owner, ILoggedUser loggedUser = null)
        {
            if (loggedUser != null)
            {
                return loggedUser.UnSubscribeFromList(slug, owner);
            }

            return TwitterListController.UnSubscribeLoggedUserFromList(slug, owner);
        }

        public static bool UnSubscribeLoggedUserFromList(string slug, string ownerScreenName, ILoggedUser loggedUser = null)
        {
            if (loggedUser != null)
            {
                return loggedUser.UnSubscribeFromList(slug, ownerScreenName);
            }

            return TwitterListController.UnSubscribeLoggedUserFromList(slug, ownerScreenName);
        }

        public static bool UnSubscribeLoggedUserFromList(string slug, long ownerId, ILoggedUser loggedUser = null)
        {
            if (loggedUser != null)
            {
                return loggedUser.UnSubscribeFromList(slug, ownerId);
            }

            return TwitterListController.UnSubscribeLoggedUserFromList(slug, ownerId);
        }

        public static bool UnSubscribeLoggedUserFromList(ITwitterListIdentifier listIdentifier, ILoggedUser loggedUser = null)
        {
            if (loggedUser != null)
            {
                return loggedUser.UnSubscribeFromList(listIdentifier);
            }

            return TwitterListController.UnSubscribeLoggedUserFromList(listIdentifier);
        }

        // Check Subscription
        public static bool CheckIfUserIsAListSubscriber(long listId, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(listId, newUserId);
        }

        public static bool CheckIfUserIsAListSubscriber(long listId, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(listId, newUserName);
        }

        public static bool CheckIfUserIsAListSubscriber(long listId, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(listId, newUser);
        }

        public static bool CheckIfUserIsAListSubscriber(string slug, long ownerId, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerId, newUserId);
        }

        public static bool CheckIfUserIsAListSubscriber(string slug, long ownerId, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerId, newUserName);
        }

        public static bool CheckIfUserIsAListSubscriber(string slug, long ownerId, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerId, newUser);
        }

        public static bool CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerScreenName, newUserId);
        }

        public static bool CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerScreenName, newUserName);
        }

        public static bool CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerScreenName, newUser);
        }

        public static bool CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, owner, newUserId);
        }

        public static bool CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, owner, newUserName);
        }

        public static bool CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, owner, newUser);
        }

        public static bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier list, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(list, newUserId);
        }

        public static bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier list, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListMember(list, newUserName);
        }

        public static bool CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(listIdentifier, userIdentifier);
        }

        // Tweet List Identifier
        public static ITwitterListIdentifier CreateListIdentifier(long listId)
        {
            return _twitterListIdentifierFactory.Create(listId);
        }

        public static ITwitterListIdentifier CreateListIdentifier(string slug, IUserIdentifier userIdentifier)
        {
            return _twitterListIdentifierFactory.Create(slug, userIdentifier);
        }

        public static ITwitterListIdentifier CreateListIdentifier(string slug, string ownerScreenName)
        {
            return _twitterListIdentifierFactory.Create(slug, ownerScreenName);
        }

        public static ITwitterListIdentifier CreateListIdentifier(string slug, long ownerId)
        {
            return _twitterListIdentifierFactory.Create(slug, ownerId);
        }

        // Parameters - Tweets From List
        public static IGetTweetsFromListParameters CreateTweetsFromListParameters()
        {
            return TwitterListQueryParameterGenerator.CreateTweetsFromListParameters();
        }
        
        public static ITwitterListUpdateParameters CreateUpdateParameters()
        {
            return TwitterListQueryParameterGenerator.CreateUpdateListParameters();
        }
    }
}