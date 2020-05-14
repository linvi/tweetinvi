using System.Threading.Tasks;
using Tweetinvi.Parameters;
using Tweetinvi.Client;
using Tweetinvi.Core.Models.Authentication;

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
        Task<ITweet> PublishTweetAsync(string text);

        /// <summary>
        /// Send a Tweet
        /// </summary>
        Task<ITweet> PublishTweetAsync(IPublishTweetParameters parameters);

        #endregion

        /// <summary>
        /// Get list of recent messages
        /// </summary>
        Task<IMessage[]> GetLatestMessagesAsync();

        /// <summary>
        /// Publish a message.
        /// </summary>
        Task<IMessage> PublishMessageAsync(IPublishMessageParameters publishMessageParameters);

        #region Timeline

        /// <inheritdoc cref="ITimelinesClient.GetHomeTimelineAsync()"/>
        Task<ITweet[]> GetHomeTimelineAsync();

        /// <inheritdoc cref="ITimelinesClient.GetHomeTimelineAsync()"/>
        Task<ITweet[]> GetMentionsTimelineAsync();

        #endregion

        // Relationship

        /// <summary>
        /// Modify the friendship between the authenticated user (source) and another user (target).
        /// </summary>
        Task UpdateRelationshipAsync(IUpdateRelationshipParameters parameters);

        // Friends - Followers

        /// <summary>
        /// Get the user ids who requested to follow you.
        /// </summary>
        Task<long[]> GetUserIdsRequestingFriendshipAsync();

        /// <summary>
        /// Get the users who requested to follow you.
        /// </summary>
        Task<IUser[]> GetUsersRequestingFriendshipAsync();

        /// <summary>
        /// Get the user ids you've requested to follow.
        /// </summary>
        Task<long[]> GetUserIdsYouRequestedToFollowAsync();

        /// <summary>
        /// Get the users you've requested to follow.
        /// </summary>
        Task<IUser[]> GetUsersYouRequestedToFollowAsync();

        /// <summary>
        /// Follow a specific user.
        /// </summary>
        Task FollowUserAsync(IUserIdentifier user);

        /// <summary>
        /// Follow a specific user.
        /// </summary>
        Task FollowUserAsync(long userId);

        /// <summary>
        /// Follow a specific user.
        /// </summary>
        Task FollowUserAsync(string screenName);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        Task UnfollowUserAsync(IUserIdentifier user);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        Task UnfollowUserAsync(long userId);

        /// <summary>
        /// Unfollow a specific user.
        /// </summary>
        Task UnfollowUserAsync(string username);

        // Saved Searches

        /// <summary>
        /// Get the authenticated user saved searches.
        /// </summary>
        Task<ISavedSearch[]> ListSavedSearchesAsync();

        // Block

        /// <summary>
        /// Block a specific user.
        /// </summary>
        Task BlockUserAsync(IUserIdentifier user);

        /// <summary>
        /// Block a specific user.
        /// </summary>
        Task BlockUserAsync(long userId);

        /// <summary>
        /// Block a specific user.
        /// </summary>
        Task BlockUserAsync(string username);

        // Unblock

        /// <summary>
        /// Unblock a specific user.
        /// </summary>
        Task UnblockUserAsync(IUserIdentifier user);

        /// <summary>
        /// Unblock a specific user.
        /// </summary>
        Task UnblockUserAsync(long userId);

        /// <summary>
        /// Unblock a specific user.
        /// </summary>
        Task UnblockUserAsync(string username);

        /// <summary>
        /// Get the ids of the user you blocked.
        /// </summary>
        Task<long[]> GetBlockedUserIdsAsync();

        /// <summary>
        /// Retrieve the users blocked by the current user.
        /// </summary>
        Task<IUser[]> GetBlockedUsersAsync();

        // Spam

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        Task ReportUserForSpamAsync(IUserIdentifier user);

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        Task ReportUserForSpamAsync(long userId);

        /// <summary>
        /// Report a specific user for being a spammer.
        /// </summary>
        Task ReportUserForSpamAsync(string userName);

        // Mute

        /// <summary>
        /// Get a list of the users you've muted.
        /// </summary>
        Task<long[]> GetMutedUserIdsAsync();

        /// <summary>
        /// Get a list of the users you've muted.
        /// </summary>
        Task<IUser[]> GetMutedUsersAsync();

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task MuteUserAsync(IUserIdentifier user);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task MuteUserAsync(long userId);

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        Task MuteUserAsync(string username);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        Task UnmuteUserAsync(IUserIdentifier user);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        Task UnmuteUserAsync(long userId);

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        Task UnmuteUserAsync(string username);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<ITwitterList> SubscribeToListAsync(ITwitterListIdentifier list);

        /// <summary>
        /// Subscribe the authenticated user to a list.
        /// </summary>
        Task<ITwitterList> SubscribeToListAsync(long listId);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        Task<ITwitterList> UnsubscribeFromListAsync(ITwitterListIdentifier list);

        /// <summary>
        /// Unsubscribe the authenticated user to a list.
        /// </summary>
        Task<ITwitterList> UnsubscribeFromListAsync(long listId);

        /// <summary>
        /// Modify the authenticated account settings.
        /// </summary>
        Task<IAccountSettings> UpdateAccountSettingsAsync(IUpdateAccountSettingsParameters parameters);
    }
}