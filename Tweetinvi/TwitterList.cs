using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    /// <summary>
    /// Access and manage user lists.
    /// </summary>
    public static class TwitterList
    {
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


        static TwitterList()
        {
            Initialize();

            _twitterListQueryParameterGenerator = TweetinviContainer.Resolve<ITwitterListQueryParameterGenerator>();
        }

        private static void Initialize()
        {
            _twitterListController = TweetinviContainer.Resolve<ITwitterListController>();
        }

        // Get Tweets from List

        /// <summary>
        /// Get tweets displayed in a specific list
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetTweetsFromList(long listId)
        {
            return TwitterListController.GetTweetsFromList(listId);
        }

        /// <summary>
        /// Get tweets displayed in a specific list
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, IUserIdentifier owner)
        {
            return TwitterListController.GetTweetsFromList(slug, owner);
        }

        /// <summary>
        /// Get tweets displayed in a specific list
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, string ownerScreenName)
        {
            return TwitterListController.GetTweetsFromList(slug, ownerScreenName);
        }

        /// <summary>
        /// Get tweets displayed in a specific list
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetTweetsFromList(string slug, long ownerId)
        {
            return TwitterListController.GetTweetsFromList(slug, ownerId);
        }

        /// <summary>
        /// Get tweets displayed in a specific list
        /// </summary>
        public static Task<IEnumerable<ITweet>> GetTweetsFromList(ITwitterListIdentifier list, IGetTweetsFromListParameters parameters = null)
        {
            return TwitterListController.GetTweetsFromList(list, parameters);
        }

        // GET User Subscription List

        /// <summary>
        /// Get the lists the authenticated user has subsribed to
        /// </summary>
        public static Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(long userId, int maxNumberOfListsToRetrieve = TweetinviConsts.LIST_GET_USER_SUBSCRIPTIONS_COUNT)
        {
            return TwitterListController.GetUserSubscribedLists(userId, maxNumberOfListsToRetrieve);
        }

        /// <summary>
        /// Get the lists the authenticated user has subsribed to
        /// </summary>
        public static Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(string userScreenName, int maxNumberOfListsToRetrieve = TweetinviConsts.LIST_GET_USER_SUBSCRIPTIONS_COUNT)
        {
            return TwitterListController.GetUserSubscribedLists(userScreenName, maxNumberOfListsToRetrieve);
        }

        /// <summary>
        /// Get the lists the authenticated user has subsribed to
        /// </summary>
        public static Task<IEnumerable<ITwitterList>> GetUserSubscribedLists(IUserIdentifier user, int maxNumberOfListsToRetrieve = TweetinviConsts.LIST_GET_USER_SUBSCRIPTIONS_COUNT)
        {
            return TwitterListController.GetUserSubscribedLists(user, maxNumberOfListsToRetrieve);
        }

        // Get List Subscribers

        /// <summary>
        /// Get the users who subscribed to a specific list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetListSubscribers(long listId, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(listId, maximumNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Get the users who subscribed to a specific list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetListSubscribers(string slug, IUserIdentifier owner, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(slug, owner, maximumNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Get the users who subscribed to a specific list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetListSubscribers(string slug, string ownerScreenName, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(slug, ownerScreenName, maximumNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Get the users who subscribed to a specific list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetListSubscribers(string slug, long ownerId, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(slug, ownerId, maximumNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Get the users who subscribed to a specific list
        /// </summary>
        public static Task<IEnumerable<IUser>> GetListSubscribers(ITwitterListIdentifier list, int maximumNumberOfUsersToRetrieve = 100)
        {
            return TwitterListController.GetListSubscribers(list, maximumNumberOfUsersToRetrieve);
        }

        // CREATE Subscription

        /// <summary>
        /// Subscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> SubscribeAuthenticatedUserToList(long listId, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(listId);
            }

            return TwitterListController.SubscribeAuthenticatedUserToList(listId);
        }

        /// <summary>
        /// Subscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> SubscribeAuthenticatedUserToList(string slug, IUserIdentifier owner, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(slug, owner);
            }

            return TwitterListController.SubscribeAuthenticatedUserToList(slug, owner);
        }

        /// <summary>
        /// Subscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> SubscribeAuthenticatedUserToList(string slug, string ownerScreenName, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(slug, ownerScreenName);
            }

            return TwitterListController.SubscribeAuthenticatedUserToList(slug, ownerScreenName);
        }

        /// <summary>
        /// Subscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> SubscribeAuthenticatedUserToList(string slug, long ownerId, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(slug, ownerId);
            }

            return TwitterListController.SubscribeAuthenticatedUserToList(slug, ownerId);
        }

        /// <summary>
        /// Subscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> SubscribeAuthenticatedUserToList(ITwitterListIdentifier listIdentifier, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(listIdentifier);
            }

            return TwitterListController.SubscribeAuthenticatedUserToList(listIdentifier);
        }

        // Remove Subscription

        /// <summary>
        /// Unubscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> UnSubscribeAuthenticatedUserToList(long listId, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(listId);
            }

            return TwitterListController.UnSubscribeAuthenticatedUserFromList(listId);
        }

        /// <summary>
        /// Unubscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> UnSubscribeAuthenticatedUserFromList(string slug, IUserIdentifier owner, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(slug, owner);
            }

            return TwitterListController.UnSubscribeAuthenticatedUserFromList(slug, owner);
        }

        /// <summary>
        /// Unubscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> UnSubscribeAuthenticatedUserFromList(string slug, string ownerScreenName, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(slug, ownerScreenName);
            }

            return TwitterListController.UnSubscribeAuthenticatedUserFromList(slug, ownerScreenName);
        }

        /// <summary>
        /// Unubscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> UnSubscribeAuthenticatedUserFromList(string slug, long ownerId, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(slug, ownerId);
            }

            return TwitterListController.UnSubscribeAuthenticatedUserFromList(slug, ownerId);
        }

        /// <summary>
        /// Unubscribe the authenticated user to a specific list
        /// </summary>
        public static Task<bool> UnSubscribeAuthenticatedUserFromList(ITwitterListIdentifier listIdentifier, IAuthenticatedUser authenticatedUser = null)
        {
            if (authenticatedUser != null)
            {
                return authenticatedUser.UnSubscribeFromList(listIdentifier);
            }

            return TwitterListController.UnSubscribeAuthenticatedUserFromList(listIdentifier);
        }

        // Check Subscription

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(long listId, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(listId, newUserId);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(long listId, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(listId, newUserName);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(long listId, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(listId, newUser);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, long ownerId, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerId, newUserId);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, long ownerId, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerId, newUserName);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, long ownerId, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerId, newUser);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerScreenName, newUserId);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerScreenName, newUserName);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, string ownerScreenName, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, ownerScreenName, newUser);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, owner, newUserId);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, string newUserName)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, owner, newUserName);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(string slug, IUserIdentifier owner, IUserIdentifier newUser)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(slug, owner, newUser);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier list, long newUserId)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(list, newUserId);
        }

        /// <summary>
        /// Check if a user is a subscriber of a specific list
        /// </summary>
        public static Task<bool> CheckIfUserIsAListSubscriber(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            return TwitterListController.CheckIfUserIsAListSubscriber(listIdentifier, user);
        }

        // Parameters - Tweets From List

        /// <summary>
        /// Create a parameter to get tweets from a List
        /// </summary>
        public static IGetTweetsFromListParameters CreateTweetsFromListParameters()
        {
            return TwitterListQueryParameterGenerator.CreateTweetsFromListParameters();
        }

        /// <summary>
        /// Create a parameter to update a list
        /// </summary>
        public static ITwitterListUpdateParameters CreateUpdateParameters()
        {
            return TwitterListQueryParameterGenerator.CreateUpdateListParameters();
        }
    }
}