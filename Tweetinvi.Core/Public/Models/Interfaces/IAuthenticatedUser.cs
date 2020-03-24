using System.Collections.Generic;
using System.Threading.Tasks;
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
        IReadOnlyTwitterCredentials Credentials { get; }

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
        /// Get list of recent messages
        /// </summary>
        Task<IMessage[]> GetLatestMessages();

        /// <summary>
        /// Publish a message.
        /// </summary>
        Task<IMessage> PublishMessage(IPublishMessageParameters publishMessageParameters);

        #region Timeline

        /// <inheritdoc cref="ITimelinesClient.GetHomeTimeline()"/>
        Task<ITweet[]> GetHomeTimeline();

        /// <inheritdoc cref="ITimelinesClient.GetMentionsTimeline()"/>
        Task<ITweet[]> GetMentionsTimeline();

        #endregion

        // Relationship

        /// <summary>
        /// Modify the friendship between the authenticated user (source) and another user (target).
        /// </summary>
        Task UpdateRelationship(IUpdateRelationshipParameters parameters);

        // Friends - Followers

        /// <summary>
        /// Get the user ids who requested to follow you.
        /// </summary>
        Task<long[]> GetUserIdsRequestingFriendship();

        /// <summary>
        /// Get the users who requested to follow you.
        /// </summary>
        Task<IUser[]> GetUsersRequestingFriendship();

        /// <summary>
        /// Get the user ids you've requested to follow.
        /// </summary>
        Task<long[]> GetUserIdsYouRequestedToFollow();

        /// <summary>
        /// Get the users you've requested to follow.
        /// </summary>
        Task<IUser[]> GetUsersYouRequestedToFollow();

        /// <summary>
        /// Follow a specific user.
        /// </summary>
        Task FollowUser(IUserIdentifier user);

        /// <summary>
        /// Follow a specific user.
        /// </summary>
        Task FollowUser(long userId);

        /// <summary>
        /// Follow a specific user.
        /// </summary>
        Task FollowUser(string screenName);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        Task UnfollowUser(IUserIdentifier user);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        Task UnfollowUser(long userId);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        Task UnfollowUser(string username);

        // Saved Searches

        /// <summary>
        /// Get the authenticated user saved searches.
        /// </summary>
        Task<ISavedSearch[]> ListSavedSearches();

        // Block

        /// <summary>
        /// Block a specific user.
        /// </summary>
        Task BlockUser(IUserIdentifier user);

        /// <summary>
        /// Block a specific user.
        /// </summary>
        Task BlockUser(long userId);

        /// <summary>
        /// Block a specific user.
        /// </summary>
        Task BlockUser(string username);

        // Unblock

        /// <summary>
        /// Unblock a specific user.
        /// </summary>
        Task UnblockUser(IUserIdentifier user);

        /// <summary>
        /// Unblock a specific user.
        /// </summary>
        Task UnblockUser(long userId);

        /// <summary>
        /// Unblock a specific user.
        /// </summary>
        Task UnblockUser(string username);

        /// <summary>
        /// Get the ids of the user you blocked.
        /// </summary>
        Task<long[]> GetBlockedUserIds();

        /// <summary>
        /// Retrieve the users blocked by the current user.
        /// </summary>
        Task<IUser[]> GetBlockedUsers();

        // Spam

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        Task ReportUserForSpam(IUserIdentifier user);

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        Task ReportUserForSpam(long userId);

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        Task ReportUserForSpam(string userName);

        // Mute

        /// <summary>
        /// Get a list of the users you've muted.
        /// </summary>
        Task<long[]> GetMutedUserIds();

        /// <summary>
        /// Get a list of the users you've muted.
        /// </summary>
        Task<IUser[]> GetMutedUsers();

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task MuteUser(IUserIdentifier user);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task MuteUser(long userId);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task MuteUser(string username);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        Task UnmuteUser(IUserIdentifier user);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        Task UnmuteUser(long userId);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        Task UnmuteUser(string username);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<ITwitterList> SubscribeToList(ITwitterListIdentifier list);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<ITwitterList> SubscribeToList(long listId);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        Task<ITwitterList> UnsubscribeFromList(ITwitterListIdentifier list);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        Task<ITwitterList> UnsubscribeFromList(long listId);

        /// <summary>
        /// Modify the authenticated account settings.
        /// </summary>
        Task<IAccountSettings> UpdateAccountSettings(IUpdateAccountSettingsParameters parameters);
    }
}