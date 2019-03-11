using System;
using Tweetinvi.Events;
using Tweetinvi.Models.Webhooks;

namespace Tweetinvi.Core.Public.Streaming
{
    public interface IAccountActivityStream
    {
        long UserId { get; set; }

        // Tweets

        EventHandler<TweetReceivedEventArgs> TweetCreated { get; set; }

        EventHandler<TweetDeletedEventArgs> TweetDeleted { get; set; }

        EventHandler<TweetFavouritedEventArgs> TweetFavourited { get; set; }

        // User Events

        /// <summary>
        /// Current user is now following another user
        /// </summary>
        EventHandler<UserFollowedEventArgs> FollowedUser { get; set; }

        /// <summary>
        /// Current user has stopped following another user
        /// </summary>
        EventHandler<UserUnFollowedEventArgs> UnfollowedUser { get; set; }

        /// <summary>
        /// Current user has been followed by another user.
        /// </summary>
        EventHandler<UserFollowedEventArgs> FollowedByUser { get; set; }

        EventHandler<UserBlockedEventArgs> UserBlocked { get; set; }
        EventHandler<UserMutedEventArgs> UserMuted { get; set; }
        EventHandler<UserRevokedAppPermissionsEventArgs> UserRevokedAppPermissions { get; set; }

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
