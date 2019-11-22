using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;
using Tweetinvi.Iterators;
using Tweetinvi.Parameters;
using Tweetinvi.Client;

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

        /// <inheritdoc cref="ITimelineClient.GetHomeTimelineIterator()"/>
        ITwitterIterator<ITweet, long?> GetHomeTimelineIterator();

        /// <inheritdoc cref="ITimelineClient.GetHomeTimelineIterator(IGetHomeTimelineParameters)"/>
        ITwitterIterator<ITweet, long?> GetHomeTimelineIterator(IGetHomeTimelineParameters parameters);

        /// <inheritdoc cref="ITimelineClient.GetMentionsTimelineIterator()"/>
        ITwitterIterator<ITweet, long?> GetMentionsTimelineIterator();

        /// <inheritdoc cref="ITimelineClient.GetMentionsTimelineIterator(IGetMentionsTimelineParameters)"/>
        ITwitterIterator<ITweet, long?> GetMentionsTimelineIterator(IGetMentionsTimelineParameters parameters);

        #endregion

        // Relationship

        /// <summary>
        /// Modify the friendship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<bool> UpdateRelationship(IUpdateRelationshipParameters parameters);

        // Friends - Followers

        /// <summary>
        /// Get the user ids who requested to follow you.
        /// </summary>
        ITwitterIterator<long> GetUserIdsRequestingFriendship();

        /// <summary>
        /// Get the users who requested to follow you.
        /// </summary>
        IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship();

        /// <summary>
        /// Get the user ids you've requested to follow.
        /// </summary>
        ITwitterIterator<long> GetUserIdsYouRequestedToFollow();

        /// <summary>
        /// Get the users you've requested to follow.
        /// </summary>
        IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow();

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
        Task<bool> UnFollowUser(string username);

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
        ITwitterIterator<long> GetBlockedUserIds();

        /// <summary>
        /// Retrieve the users blocked by the current user.
        /// </summary>
        ITwitterIterator<IUser> GetBlockedUsers();

        // Spam

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        Task<bool> ReportUserForSpam(IUserIdentifier user);

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        Task<bool> ReportUserForSpam(long? userId);

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        Task<bool> ReportUserForSpam(string userName);

        // Mute

        /// <summary>
        /// Get a list of the users you've muted.
        /// </summary>
        ITwitterIterator<long> GetMutedUserIds();

        /// <summary>
        /// Get a list of the users you've muted.
        /// </summary>
        ITwitterIterator<IUser> GetMutedUsers();

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task<bool> MuteUser(IUserIdentifier user);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task<bool> MuteUser(long? userId);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task<bool> MuteUser(string username);

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
        Task<bool> UnMuteUser(string username);

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
        /// Modify the authenticated account settings.
        /// </summary>
        Task<IAccountSettings> UpdateAccountSettings(IUpdateAccountSettingsParameters parameters);
    }
}