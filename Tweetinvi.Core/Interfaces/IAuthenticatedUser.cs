using System;
using System.Collections.Generic;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Async;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces
{
    /// <summary>
    /// User associated with a Token, this "privileged" user
    /// has access private information like messages, timeline...
    /// </summary>
    public interface IAuthenticatedUser : IAuthenticatedUserAsync, IUser
    {
        /// <summary>
        /// Authenticated user email. This value will be null if the application has not been verified and authorized by Twitter.
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Credentials used to authenticate this user.
        /// </summary>
        ITwitterCredentials Credentials { get; }

        /// <summary>
        /// Set the credentials of the user.
        /// </summary>
        /// <param name="credentials"></param>
        void SetCredentials(ITwitterCredentials credentials);

        /// <summary>
        /// Execute an operation with the context of this authenticated user.
        /// </summary>
        T ExecuteAuthenticatedUserOperation<T>(Func<T> operation);

        /// <summary>
        /// Execute an operation with the context of this authenticated user.
        /// </summary>
        void ExecuteAuthenticatedUserOperation(Action operation);

        #region Tweets

        /// <summary>
        /// Send a very simple Tweet with a simple message
        /// </summary>
        ITweet PublishTweet(string text, IPublishTweetOptionalParameters parameters = null);

        #endregion

        // Direct Messages

        /// <summary>
        /// List of Messages received
        /// </summary>
        IEnumerable<IMessage> LatestDirectMessagesReceived { get; set; }

        /// <summary>
        /// List of messages sent
        /// </summary>
        IEnumerable<IMessage> LatestDirectMessagesSent { get; set; }

        /// <summary>
        /// Get the list of direct messages received by the user.
        /// </summary>
        /// <param name="maximumNumberOfMessagesToRetrieve">Maximum number of messages retrieved</param>
        /// <returns>Collection of direct messages received by the user</returns>
        IEnumerable<IMessage> GetLatestMessagesReceived(int maximumNumberOfMessagesToRetrieve = 40);

        /// <summary>
        /// Get the list of direct messages sent by the user.
        /// </summary>
        /// <param name="maximumNumberOfMessagesToRetrieve">Maximum number of messages retrieved</param>
        /// <returns>Collection of direct messages received by the user</returns>
        IEnumerable<IMessage> GetLatestMessagesSent(int maximumNumberOfMessagesToRetrieve = 40);

        /// <summary>
        /// Publish a message.
        /// </summary>
        IMessage PublishMessage(IPublishMessageParameters publishMessageParameters);

        #region Timeline

        /// <summary>
        /// List of tweets as displayed on the Home timeline.
        /// </summary>
        IEnumerable<ITweet> LatestHomeTimeline { get; set; }

        /// <summary>
        /// Get the latest tweets of the authenticated user Home timeline.
        /// </summary>
        IEnumerable<ITweet> GetHomeTimeline(int maximumNumberOfTweets = 40);

        /// <summary>
        /// Get the latest tweets of the authenticated user Home timeline.
        /// </summary>
        IEnumerable<ITweet> GetHomeTimeline(IHomeTimelineParameters timelineRequestParameters);

        /// <summary>
        /// List of tweets as displayed on the Mentions timeline.
        /// </summary>
        IEnumerable<IMention> LatestMentionsTimeline { get; set; }

        /// <summary>
        /// Get the latest tweets of the authenticated user Mentions timeline
        /// </summary>
        /// <param name="maximumNumberOfMentions">Number of tweets expected</param>
        /// <returns>Tweets of the Mentions timeline of the connected user</returns>
        IEnumerable<IMention> GetMentionsTimeline(int maximumNumberOfMentions = 40);

        #endregion

        // Relationship

        /// <summary>
        /// Get the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        IRelationshipDetails GetRelationshipWith(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        IRelationshipDetails GetRelationshipWith(long userId);

        /// <summary>
        /// Get the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        IRelationshipDetails GetRelationshipWith(string screenName);

        /// <summary>
        /// Modify the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        bool UpdateRelationshipAuthorizationsWith(IUserIdentifier userIdentifier, bool retweetsEnabled, bool deviceNotificationsEnabled);

        /// <summary>
        /// Modify the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        bool UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotificationsEnabled);

        /// <summary>
        /// Modify the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        bool UpdateRelationshipAuthorizationsWith(string screenName, bool retweetsEnabled, bool deviceNotificationsEnabled);

        // Friends - Followers

        /// <summary>
        /// Get the users who requested to follow you.
        /// </summary>
        IEnumerable<IUser> GetUsersRequestingFriendship(int maximumUserIdsToRetrieve = 5000);

        /// <summary>
        /// Get the users you've requested to follow.
        /// </summary>
        IEnumerable<IUser> GetUsersYouRequestedToFollow(int maximumUserIdsToRetrieve = 5000);

        /// <summary>
        /// Folow a specific user.
        /// </summary>
        bool FollowUser(IUser user);

        /// <summary>
        /// Folow a specific user.
        /// </summary>
        bool FollowUser(long userId);

        /// <summary>
        /// Folow a specific user.
        /// </summary>
        bool FollowUser(string screenName);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        bool UnFollowUser(IUser user);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        bool UnFollowUser(long userId);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        bool UnFollowUser(string screenName);

        // Saved Searches

        /// <summary>
        /// Get the authenticated user saved searches.
        /// </summary>
        IEnumerable<ISavedSearch> GetSavedSearches();

        // Block

        /// <summary>
        /// Block a specific user.
        /// </summary>
        bool BlockUser(IUserIdentifier userIdentifier);

        /// <summary>
        /// Block a specific user.
        /// </summary>
        bool BlockUser(long userId);

        /// <summary>
        /// Block a specific user.
        /// </summary>
        bool BlockUser(string userName);

        // Unblock

        /// <summary>
        /// Unblock a specific user.
        /// </summary>
        bool UnBlockUser(IUserIdentifier userIdentifier);

        /// <summary>
        /// Unblock a specific user.
        /// </summary>
        bool UnBlockUser(long userId);

        /// <summary>
        /// Unblock a specific user.
        /// </summary>
        bool UnBlockUser(string userName);

        /// <summary>
        /// Get the ids of the user you blocked.
        /// </summary>
        IEnumerable<long> GetBlockedUserIds();

        /// <summary>
        /// Retrieve the users blocked by the current user.
        /// </summary>
        IEnumerable<IUser> GetBlockedUsers();

        // Spam

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        bool ReportUserForSpam(IUser user);

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        bool ReportUserForSpam(IUserIdentifier userIdentifier);

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        bool ReportUserForSpam(long userId);

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        bool ReportUserForSpam(string userName);

        // Mute

        /// <summary>
        /// Get a list of the users you've muted.
        /// </summary>
        IEnumerable<long> GetMutedUserIds(int maxUserIdsToRetrieve = Int32.MaxValue);

        /// <summary>
        /// Get a list of the users you've muted.
        /// </summary>
        IEnumerable<IUser> GetMutedUsers(int maxUsersToRetrieve = 250);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        bool MuteUser(IUserIdentifier userIdentifier);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        bool MuteUser(long userId);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        bool MuteUser(string screenName);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        bool UnMuteUser(IUserIdentifier userIdentifier);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        bool UnMuteUser(long userId);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        bool UnMuteUser(string screenName);

        #region List

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        bool SubsribeToList(ITwitterListIdentifier list);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        bool SubsribeToList(long listId);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        bool SubsribeToList(string slug, long ownerId);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        bool SubsribeToList(string slug, string ownerScreenName);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        bool SubsribeToList(string slug, IUserIdentifier owner);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        bool UnSubscribeFromList(ITwitterListIdentifier list);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        bool UnSubscribeFromList(long listId);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        bool UnSubscribeFromList(string slug, long ownerId);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        bool UnSubscribeFromList(string slug, string ownerScreenName);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        bool UnSubscribeFromList(string slug, IUserIdentifier owner);

        #endregion

        /// <summary>
        /// Property available to store the account settings.
        /// </summary>
        IAccountSettings AccountSettings { get; set; }

        /// <summary>
        /// Get the authenticated account settings.
        /// </summary>
        IAccountSettings GetAccountSettings();

        /// <summary>
        /// Modify the authenticated account settings.
        /// </summary>
        IAccountSettings UpdateAccountSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null);

        /// <summary>
        /// Modify the authenticated account settings.
        /// </summary>
        IAccountSettings UpdateAccountSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters);
    }
}