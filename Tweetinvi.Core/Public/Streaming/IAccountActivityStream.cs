using System;
using Tweetinvi.Events;
using Tweetinvi.Models.Webhooks;

namespace Tweetinvi.Streaming
{
    public interface IAccountActivityStream
    {
        long UserId { get; set; }

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
        /// Current user is now following another user
        /// </summary>
        EventHandler<AccountActivityUserFollowedEventArgs> UserFollowed { get; set; }

        /// <summary>
        /// Current user has stopped following another user
        /// </summary>
        EventHandler<UserUnFollowedEventArgs> UserUnfollowed { get; set; }

        EventHandler<UserBlockedEventArgs> UserBlocked { get; set; }
        EventHandler<UserMutedEventArgs> UserMuted { get; set; }
        EventHandler<AccountActivityUserRevokedAppPermissionsEventArgs> UserRevokedAppPermissions { get; set; }

        // Messages
        EventHandler<MessageEventArgs> MessageReceived { get; set; }
        EventHandler<MessageEventArgs> MessageSent { get; set; }
        EventHandler<UserIsTypingMessageEventArgs> UserIsTypingMessage { get; set; }
        EventHandler<UserReadMessageConversationEventArgs> UserReadMessage { get; set; }

        // Others
        EventHandler<UnmanagedMessageReceivedEventArgs> UnmanagedEventReceived { get; set; }
        EventHandler<JsonObjectEventArgs> JsonObjectReceived { get; set; }


        void WebhookMessageReceived(IWebhookMessage message);
    }
}
