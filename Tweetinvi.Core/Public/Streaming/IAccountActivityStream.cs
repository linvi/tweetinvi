using System;
using Tweetinvi.Events;
using Tweetinvi.Models.Webhooks;

namespace Tweetinvi.Streaming
{
    /// <summary>
    /// An AccountActivity stream from Twitter (https://developer.twitter.com/en/docs/accounts-and-users/subscribe-account-activity/overview)
    /// The stream is linked with a specific user account and raise account related events.
    /// </summary>
    public interface IAccountActivityStream
    {
        /// <summary>
        /// The account user id.
        /// </summary>
        long AccountUserId { get; set; }

        // Tweets

        /// <summary>
        /// A Tweet has been been created.
        /// </summary>
        EventHandler<AccountActivityTweetCreatedEventArgs> TweetCreated { get; set; }

        /// <summary>
        /// A Tweet has been deleted
        /// </summary>
        EventHandler<AccountActivityTweetDeletedEventArgs> TweetDeleted { get; set; }

        /// <summary>
        /// A Tweet has been favourited
        /// </summary>
        EventHandler<AccountActivityTweetFavouritedEventArgs> TweetFavourited { get; set; }

        // User Events

        /// <summary>
        /// Account user is now following another user
        /// </summary>
        EventHandler<AccountActivityUserFollowedEventArgs> UserFollowed { get; set; }

        /// <summary>
        /// Account user has stopped following another user
        /// </summary>
        EventHandler<AccountActivityUserUnfollowedEventArgs> UserUnfollowed { get; set; }

        /// <summary>
        /// Account user has blocked another user
        /// </summary>
        EventHandler<AccountActivityUserBlockedEventArgs> UserBlocked { get; set; }

        /// <summary>
        /// Account user has unblocked another user
        /// </summary>
        EventHandler<AccountActivityUserUnblockedEventArgs> UserUnblocked { get; set; }

        /// <summary>
        /// Account user has muted another user
        /// </summary>
        EventHandler<AccountActivityUserMutedEventArgs> UserMuted { get; set; }

        /// <summary>
        /// Account user has unmuted another user
        /// </summary>
        EventHandler<AccountActivityUserUnmutedEventArgs> UserUnmuted { get; set; }


        // Messages

        /// <summary>
        /// Account user has received a message
        /// </summary>
        EventHandler<AccountActivityMessageReceivedEventArgs> MessageReceived { get; set; }

        /// <summary>
        /// Account user has sent a message
        /// </summary>
        EventHandler<AccountActivityMessageSentEventArgs> MessageSent { get; set; }

        /// <summary>
        /// A user is typing in a conversation with the account user
        /// </summary>
        EventHandler<AccountActivityUserIsTypingMessageEventArgs> UserIsTypingMessage { get; set; }

        /// <summary>
        /// A user has read a message from the account user
        /// </summary>
        EventHandler<AccountActivityUserReadMessageConversationEventArgs> UserReadMessage { get; set; }

        // Permissions

        /// <summary>
        /// Revoke events sent when the account user removes application authorization and subscription is automatically deleted.
        /// </summary>
        EventHandler<AccountActivityUserRevokedAppPermissionsEventArgs> UserRevokedAppPermissions { get; set; }

        // Others

        /// <summary>
        /// An event has been received 
        /// </summary>
        EventHandler<JsonObjectEventArgs> JsonObjectReceived { get; set; }
        EventHandler<UnmanagedMessageReceivedEventArgs> UnmanagedEventReceived { get; set; }

        void WebhookMessageReceived(IWebhookMessage message);
    }
}
