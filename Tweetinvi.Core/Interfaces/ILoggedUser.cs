using System;
using System.Collections.Generic;
using Tweetinvi.Core.Credentials;
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
    public interface ILoggedUser : ILoggedUserAsync, IUser
    {
        ITwitterCredentials Credentials { get; }

        void SetCredentials(ITwitterCredentials credentials);

        T ExecuteLoggedUserOperation<T>(Func<T> operation);
        void ExecuteLoggedUserOperation(Action operation);

        #region Tweets

        /// <summary>
        /// Send a very simple Tweet with a simple message
        /// </summary>
        /// <param name="text">Text of the tweet</param>
        /// <returns>If the tweet has been sent returns the Tweet</returns>
        ITweet PublishTweet(string text, IPublishTweetOptionalParameters parameters = null);

        #endregion

        // Direct Messages
        /// <summary>
        /// List of Messages received
        /// </summary>
        IEnumerable<IMessage> LatestDirectMessagesReceived { get; }

        /// <summary>
        /// List of messages sent
        /// </summary>
        IEnumerable<IMessage> LatestDirectMessagesSent { get; }

        /// <summary>
        /// Get the list of direct messages received by the user
        /// Update the matching attribute of the current user if the parameter is true
        /// Return the list of direct messages received by the user
        /// </summary>
        /// <param name="count">Maximum number of messages retrieved</param>
        /// <returns>Collection of direct messages received by the user</returns>
        IEnumerable<IMessage> GetLatestMessagesReceived(int count = 40);

        IEnumerable<IMessage> GetLatestMessagesSent(int maximumMessages = 40);

        IMessage PublishMessage(IMessage message);

        #region Timeline

        /// <summary>
        /// List of tweets as displayed on the Home timeline
        /// Storing the information so that it is not required 
        /// to fetch the data again
        /// </summary>
        IEnumerable<ITweet> LatestHomeTimeline { get; set; }

        /// <summary>
        /// Get the latest tweets of the authenticated user Home timeline
        /// </summary>
        IEnumerable<ITweet> GetHomeTimeline(int maximumNumberOfTweets = 40);

        /// <summary>
        /// Get the latest tweets of the authenticated user Home timeline
        /// </summary>
        IEnumerable<ITweet> GetHomeTimeline(IHomeTimelineParameters timelineRequestParameters);

        /// <summary>
        /// List of tweets as displayed on the Mentions timeline
        /// Storing the information so that it is not required 
        /// to fetch the data again
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
        IRelationshipDetails GetRelationshipWith(IUserIdentifier userIdentifier);
        IRelationshipDetails GetRelationshipWith(long userId);
        IRelationshipDetails GetRelationshipWith(string screenName);

        bool UpdateRelationshipAuthorizationsWith(IUserIdentifier userIdentifier, bool retweetsEnabled, bool deviceNotificationsEnabled);
        bool UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotificationsEnabled);
        bool UpdateRelationshipAuthorizationsWith(string screenName, bool retweetsEnabled, bool deviceNotificationsEnabled);

        // Friends - Followers
        IEnumerable<IUser> GetUsersRequestingFriendship(int maximumUserIdsToRetrieve = 75000);
        IEnumerable<IUser> GetUsersYouRequestedToFollow(int maximumUserIdsToRetrieve = 75000);

        bool FollowUser(IUser user);
        bool FollowUser(long userId);
        bool FollowUser(string screenName);

        bool UnFollowUser(IUser user);
        bool UnFollowUser(long userId);
        bool UnFollowUser(string screenName);

        // Saved Searches
        IEnumerable<ISavedSearch> GetSavedSearches();

        // Block
        bool BlockUser(IUserIdentifier userIdentifier);
        bool BlockUser(long userId);
        bool BlockUser(string userName);

        // Unblock
        bool UnBlockUser(IUserIdentifier userIdentifier);
        bool UnBlockUser(long userId);
        bool UnBlockUser(string userName);

        IEnumerable<long> GetBlockedUserIds();

        /// <summary>
        /// Retrieve the users blocked by the current user.
        /// Populate the corresponding attributes according to the value of the boolean parameters.
        /// Return the list of users blocked by the current user.
        /// </summary>
        IEnumerable<IUser> GetBlockedUsers();

        // Spam
        bool ReportUserForSpam(IUser user);
        bool ReportUserForSpam(IUserIdentifier userIdentifier);
        bool ReportUserForSpam(long userId);
        bool ReportUserForSpam(string userName);

        // Mute
        IEnumerable<long> GetMutedUserIds(int maxUserIdsToRetrieve = Int32.MaxValue);
        IEnumerable<IUser> GetMutedUsers(int maxUsersToRetrieve = 250);

        bool MuteUser(IUserIdentifier userIdentifier);
        bool MuteUser(long userId);
        bool MuteUser(string screenName);

        bool UnMuteUser(IUserIdentifier userIdentifier);
        bool UnMuteUser(long userId);
        bool UnMuteUser(string screenName);

        #region List

        bool SubsribeToList(ITwitterListIdentifier list);
        bool SubsribeToList(long listId);
        bool SubsribeToList(string slug, long ownerId);
        bool SubsribeToList(string slug, string ownerScreenName);
        bool SubsribeToList(string slug, IUserIdentifier owner);

        bool UnSubscribeFromList(ITwitterListIdentifier list);
        bool UnSubscribeFromList(long listId);
        bool UnSubscribeFromList(string slug, long ownerId);
        bool UnSubscribeFromList(string slug, string ownerScreenName);
        bool UnSubscribeFromList(string slug, IUserIdentifier owner);

       #endregion

        IAccountSettings AccountSettings { get; set; }
        IAccountSettings GetAccountSettings();

        IAccountSettings UpdateAccountSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null);

        IAccountSettings UpdateAccountSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters);
    }
}