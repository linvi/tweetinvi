using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Models
{
    /// <summary>
    /// User associated with a Token, this "privileged" user
    /// has access private information like messages, timeline...
    /// </summary>
    public interface IAuthenticatedUser : IUser
    {
        /// <summary>
        /// Authenticated user email. This value will be null if the application has not been verified and authorized by Twitter.
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Credentials used to authenticate this user.
        /// </summary>
        ITwitterCredentials Credentials { get; set; }

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
        /// Send a Tweet
        /// </summary>
        Task<ITweet> PublishTweet(string text);

        /// <summary>
        /// Send a Tweet
        /// </summary>
        Task<ITweet> PublishTweet(IPublishTweetParameters parameters);

        #endregion

        // Direct Messages

        /// <summary>
        /// List of Messages sent of received
        /// </summary>
        IEnumerable<IMessage> LatestDirectMessages { get; }

        /// <summary>
        /// Get the list of direct messages sent or received by the user
        /// </summary>
        /// <param name="count">Number of messages to request from the Twitter API. Actual amount returned may be less</param>
        /// <returns>Collection of direct messages</returns>
        Task<IEnumerable<IMessage>> GetLatestMessages(int count = TweetinviConsts.MESSAGE_GET_COUNT);

        /// <summary>
        /// Get the list of direct messages sent or received by the user
        /// </summary>
        /// <param name="count">Number of messages to request from the Twitter API. Actual amount returned may be less</param>
        /// <returns>Collection of direct messages</returns>
        Task<AsyncCursorResult<IEnumerable<IMessage>>> GetLatestMessagesWithCursor(int count = TweetinviConsts.MESSAGE_GET_COUNT);

        /// <summary>
        /// Publish a message.
        /// </summary>
        Task<IMessage> PublishMessage(IPublishMessageParameters publishMessageParameters);

        #region Timeline

        /// <summary>
        /// List of tweets as displayed on the Home timeline.
        /// </summary>
        IEnumerable<ITweet> LatestHomeTimeline { get; set; }

        /// <summary>
        /// Get the latest tweets of the authenticated user Home timeline.
        /// </summary>
        Task<IEnumerable<ITweet>> GetHomeTimeline(int maximumNumberOfTweets = 40);

        /// <summary>
        /// Get the latest tweets of the authenticated user Home timeline.
        /// </summary>
        Task<IEnumerable<ITweet>> GetHomeTimeline(IHomeTimelineParameters timelineRequestParameters);

        /// <summary>
        /// List of tweets as displayed on the Mentions timeline.
        /// </summary>
        IEnumerable<IMention> LatestMentionsTimeline { get; set; }

        /// <summary>
        /// Get the latest tweets of the authenticated user Mentions timeline
        /// </summary>
        /// <param name="maximumNumberOfMentions">Number of tweets expected</param>
        /// <returns>Tweets of the Mentions timeline of the connected user</returns>
        Task<IEnumerable<IMention>> GetMentionsTimeline(int maximumNumberOfMentions = 40);

        #endregion

        // Relationship

        /// <summary>
        /// Get the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<IRelationshipDetails> GetRelationshipWith(long userId);

        /// <summary>
        /// Get the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<IRelationshipDetails> GetRelationshipWith(string screenName);

        /// <summary>
        /// Modify the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<bool> UpdateRelationshipAuthorizationsWith(IUserIdentifier user, bool retweetsEnabled, bool deviceNotificationsEnabled);

        /// <summary>
        /// Modify the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<bool> UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotificationsEnabled);

        /// <summary>
        /// Modify the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<bool> UpdateRelationshipAuthorizationsWith(string screenName, bool retweetsEnabled, bool deviceNotificationsEnabled);

        // Friends - Followers

        /// <summary>
        /// Get the users who requested to follow you.
        /// </summary>
        Task<IEnumerable<IUser>> GetUsersRequestingFriendship(int maximumUserIdsToRetrieve = 5000);

        /// <summary>
        /// Get the users you've requested to follow.
        /// </summary>
        Task<IEnumerable<IUser>> GetUsersYouRequestedToFollow(int maximumUserIdsToRetrieve = 5000);

        /// <summary>
        /// Folow a specific user.
        /// </summary>
        Task<bool> FollowUser(IUserIdentifier user);

        /// <summary>
        /// Folow a specific user.
        /// </summary>
        Task<bool> FollowUser(long userId);

        /// <summary>
        /// Folow a specific user.
        /// </summary>
        Task<bool> FollowUser(string screenName);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        Task<bool> UnFollowUser(IUserIdentifier user);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        Task<bool> UnFollowUser(long userId);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        Task<bool> UnFollowUser(string screenName);

        // Saved Searches

        /// <summary>
        /// Get the authenticated user saved searches.
        /// </summary>
        Task<IEnumerable<ISavedSearch>> GetSavedSearches();

        // Block

        /// <summary>
        /// Block a specific user.
        /// </summary>
        Task<bool> BlockUser(IUserIdentifier user);

        /// <summary>
        /// Block a specific user.
        /// </summary>
        Task<bool> BlockUser(long? userId);

        /// <summary>
        /// Block a specific user.
        /// </summary>
        Task<bool> BlockUser(string username);

        // Unblock

        /// <summary>
        /// Unblock a specific user.
        /// </summary>
        Task<bool> UnBlockUser(IUserIdentifier user);

        /// <summary>
        /// Unblock a specific user.
        /// </summary>
        Task<bool> UnBlockUser(long userId);

        /// <summary>
        /// Unblock a specific user.
        /// </summary>
        Task<bool> UnBlockUser(string username);

        /// <summary>
        /// Get the ids of the user you blocked.
        /// </summary>
        Task<ICursorResult<long>> GetBlockedUserIds();

        /// <summary>
        /// Retrieve the users blocked by the current user.
        /// </summary>
        Task<IEnumerable<IUser>> GetBlockedUsers();

        // Spam

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        Task<bool> ReportUserForSpam(IUserIdentifier user);

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        Task<bool> ReportUserForSpam(long userId);

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        Task<bool> ReportUserForSpam(string userName);

        // Mute

        /// <summary>
        /// Get a list of the users you've muted.
        /// </summary>
        Task<IEnumerable<long>> GetMutedUserIds(int maxUserIdsToRetrieve = Int32.MaxValue);

        /// <summary>
        /// Get a list of the users you've muted.
        /// </summary>
        Task<IEnumerable<IUser>> GetMutedUsers(int maxUsersToRetrieve = 250);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task<bool> MuteUser(IUserIdentifier user);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task<bool> MuteUser(long userId);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task<bool> MuteUser(string screenName);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        Task<bool> UnMuteUser(IUserIdentifier user);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        Task<bool> UnMuteUser(long userId);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        Task<bool> UnMuteUser(string screenName);

        #region List

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> SubscribeToList(ITwitterListIdentifier list);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> SubscribeToList(long listId);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> SubscribeToList(string slug, long ownerId);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> SubscribeToList(string slug, string ownerScreenName);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> SubscribeToList(string slug, IUserIdentifier owner);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        Task<bool> UnSubscribeFromList(ITwitterListIdentifier list);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        Task<bool> UnSubscribeFromList(long listId);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        Task<bool> UnSubscribeFromList(string slug, long ownerId);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        Task<bool> UnSubscribeFromList(string slug, string ownerScreenName);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        Task<bool> UnSubscribeFromList(string slug, IUserIdentifier owner);

        #endregion

        /// <summary>
        /// Get the authenticated account settings.
        /// </summary>
        Task<IAccountSettings> GetAccountSettings();

        /// <summary>
        /// Modify the authenticated account settings.
        /// </summary>
        Task<IAccountSettings> UpdateAccountSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null);

        /// <summary>
        /// Modify the authenticated account settings.
        /// </summary>
        Task<IAccountSettings> UpdateAccountSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters);
    }
}