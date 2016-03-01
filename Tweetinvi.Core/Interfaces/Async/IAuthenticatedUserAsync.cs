using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces.Async
{
    public interface IAuthenticatedUserAsync
    {
        /// <summary>
        /// Get the list of direct messages received by the user.
        /// </summary>
        /// <returns>Collection of direct messages received by the user</returns>
        Task<IEnumerable<IMessage>> GetLatestMessagesReceivedAsync(int count = 40);

        /// <summary>
        /// Get the list of direct messages sent by the user.
        /// </summary>
        /// <returns>Collection of direct messages received by the user</returns>
        Task<IEnumerable<IMessage>> GetLatestMessagesSentAsync(int maximumMessages = 40);

        /// <summary>
        /// Publish a message.
        /// </summary>
        Task<IMessage> PublishMessageAsync(IPublishMessageParameters publishMessageParameters);

        /// <summary>
        /// Get the latest tweets of the authenticated user Home timeline.
        /// </summary>
        Task<IEnumerable<ITweet>> GetHomeTimelineAsync(int count = 40);

        /// <summary>
        /// Get the latest tweets of the authenticated user Home timeline.
        /// </summary>
        Task<IEnumerable<ITweet>> GetHomeTimelineAsync(IHomeTimelineParameters timelineRequestParameters);

        /// <summary>
        /// List of tweets as displayed on the Mentions timeline.
        /// </summary>
        Task<IEnumerable<IMention>> GetMentionsTimelineAsync(int count = 40);

        /// <summary>
        /// Get the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<IRelationshipDetails> GetRelationshipWithAsync(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<IRelationshipDetails> GetRelationshipWithAsync(long userId);

        /// <summary>
        /// Get the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<IRelationshipDetails> GetRelationshipWithAsync(string screenName);

        /// <summary>
        /// Modify the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<bool> UpdateRelationshipAuthorizationsWithAsync(IUserIdentifier user, bool retweetsEnabled, bool deviceNotificationsEnabled);

        /// <summary>
        /// Modify the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<bool> UpdateRelationshipAuthorizationsWithAsync(long userId, bool retweetsEnabled, bool deviceNotificationsEnabled);

        /// <summary>
        /// Modify the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<bool> UpdateRelationshipAuthorizationsWithAsync(string screenName, bool retweetsEnabled, bool deviceNotificationsEnabled);

        /// <summary>
        /// Get the users who requested to follow you.
        /// </summary>
        Task<IEnumerable<IUser>> GetUsersRequestingFriendshipAsync(int maximumUserIdsToRetrieve = 75000);

        /// <summary>
        /// Get the users you've requested to follow.
        /// </summary>
        Task<IEnumerable<IUser>> GetUsersYouRequestedToFollowAsync(int maximumUsersToRetrieve = 75000);

        /// <summary>
        /// Folow a specific user.
        /// </summary>
        Task<bool> FollowUserAsync(IUser user);

        /// <summary>
        /// Folow a specific user.
        /// </summary>
        Task<bool> FollowUserAsync(long userId);

        /// <summary>
        /// Folow a specific user.
        /// </summary>
        Task<bool> FollowUserAsync(string screenName);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        Task<bool> UnFollowUserAsync(IUser user);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        Task<bool> UnFollowUserAsync(long userId);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        Task<bool> UnFollowUserAsync(string screenName);

        /// <summary>
        /// Get the authenticated user saved searches.
        /// </summary>
        Task<IEnumerable<ISavedSearch>> GetSavedSearchesAsync();

        /// <summary>
        /// Retrieve the users blocked by the current user.
        /// </summary>
        Task<IEnumerable<long>> GetBlockedUsersIdsAsync();

        /// <summary>
        /// Retrieve the users blocked by the current user.
        /// </summary>
        Task<IEnumerable<IUser>> GetBlockedUsersAsync();

        /// <summary>
        /// Get the authenticated account settings.
        /// </summary>
        Task<IAccountSettings> GetAccountSettingsAsync();

        /// <summary>
        /// Modify the authenticated account settings.
        /// </summary>
        Task<IAccountSettings> UpdateAccountSettingsAsync(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null);

        /// <summary>
        /// Modify the authenticated account settings.
        /// </summary>
        Task<IAccountSettings> UpdateAccountSettingsAsync(IAccountSettingsRequestParameters accountSettingsRequestParameters);

        // Mute

        /// <summary>
        /// Get a list of the users you've muted.
        /// </summary>
        Task<IEnumerable<long>> GetMutedUserIdsAsync(int maxUserIds = Int32.MaxValue);

        /// <summary>
        /// Get a list of the users you've muted.
        /// </summary>
        Task<IEnumerable<IUser>> GetMutedUsersAsync(int maxUserIds = 250);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task<bool> MuteUserAsync(IUserIdentifier userIdentifier);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task<bool> MuteUserAsync(long userId);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task<bool> MuteUserAsync(string screenName);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        Task<bool> UnMuteUserAsync(IUserIdentifier userIdentifier);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        Task<bool> UnMuteUserAsync(long userId);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        Task<bool> UnMuteUserAsync(string screenName);

        // Subscription

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> SubscribeToListAsync(ITwitterListIdentifier list);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> SubscribeToListAsync(long listId);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> SubscribeToListAsync(string slug, long ownerId);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> SubscribeToListAsync(string slug, string ownerScreenName);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> SubscribeToListAsync(string slug, IUserIdentifier owner);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> UnSubscribeFromListAsync(ITwitterListIdentifier list);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> UnSubscribeFromListAsync(long listId);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> UnSubscribeFromListAsync(string slug, long ownerId);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> UnSubscribeFromListAsync(string slug, string ownerScreenName);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<bool> UnSubscribeFromListAsync(string slug, IUserIdentifier owner);
    }
}