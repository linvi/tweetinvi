using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Tweetinvi.Events;
using Tweetinvi.Streaming;

namespace Examplinvi.WebhooksEvents
{
    public class AccountActivityEventsManager
    {
        private readonly List<long> _trackedStreams = new List<long>();

        public void RegisterAccountActivityStream(IAccountActivityStream accountActivityStream)
        {
            if (_trackedStreams.Contains(accountActivityStream.AccountUserId))
            {
                return;
            }

            _trackedStreams.Add(accountActivityStream.AccountUserId);

            Console.WriteLine($"Activities for user {accountActivityStream.AccountUserId} are now being tracked.");

            // Tweet events
            accountActivityStream.TweetCreated += TweetCreated;
            accountActivityStream.TweetFavourited += TweetFavourited;
            accountActivityStream.TweetDeleted += TweetDeleted;

             // Message events
            accountActivityStream.MessageReceived += MessageReceived;
            accountActivityStream.MessageSent += MessageSent;

            accountActivityStream.UserIsTypingMessage += UserIsTypingMessage;
            accountActivityStream.UserReadMessage += UserReadMessage;

            // User events
            accountActivityStream.UserFollowed += FollowedUser;
            accountActivityStream.UserUnfollowed += UnfollowedUser;

            accountActivityStream.UserBlocked += UserBlocked;
            accountActivityStream.UserUnblocked += UserUnblocked;

            accountActivityStream.UserMuted += UserMuted;
            accountActivityStream.UserUnmuted += UserUnmuted;

            // Other events
            accountActivityStream.JsonObjectReceived += JsonObjectReceived;
            accountActivityStream.UnsupportedEventReceived += UnmanagedEventReceived;
        }

        [SuppressMessage("ReSharper", "DelegateSubtraction")]
        public void UnregisterAccountActivityStream(IAccountActivityStream accountActivityStream)
        {
            if (!_trackedStreams.Contains(accountActivityStream.AccountUserId))
            {
                return;
            }

            _trackedStreams.Remove(accountActivityStream.AccountUserId);

            // Tweet events
            accountActivityStream.TweetCreated -= TweetCreated;
            accountActivityStream.TweetFavourited -= TweetFavourited;
            accountActivityStream.TweetDeleted -= TweetDeleted;

            // Message events
            accountActivityStream.MessageReceived -= MessageReceived;
            accountActivityStream.MessageSent -= MessageSent;

            accountActivityStream.UserIsTypingMessage -= UserIsTypingMessage;
            accountActivityStream.UserReadMessage -= UserReadMessage;

            // User events
            accountActivityStream.UserFollowed -= FollowedUser;
            accountActivityStream.UserUnfollowed -= UnfollowedUser;

            accountActivityStream.UserBlocked -= UserBlocked;
            accountActivityStream.UserUnblocked -= UserUnblocked;

            accountActivityStream.UserMuted += UserMuted;
            accountActivityStream.UserUnmuted += UserUnmuted;

            // Other events
            accountActivityStream.JsonObjectReceived -= JsonObjectReceived;
            accountActivityStream.UnsupportedEventReceived -= UnmanagedEventReceived;
        }

        // Tweet events
        private void TweetCreated(object sender, AccountActivityTweetCreatedEventArgs e)
        {
            Console.WriteLine($"Tweet has been created:\n${e.Tweet}");
        }

        private void TweetDeleted(object sender, AccountActivityTweetDeletedEventArgs e)
        {
            Console.WriteLine($"Tweet ${e.TweetId} has been deleted at ${e.EventDate}");
        }

        private void TweetFavourited(object sender, AccountActivityTweetFavouritedEventArgs e)
        {
            Console.WriteLine($"Tweet has been favourited by ${e.FavouritedBy}:\n${e.Tweet}");
        }

      
        // Message events
        private void MessageSent(object sender, AccountActivityMessageSentEventArgs args)
        {
            Console.WriteLine($"Account user ({args.Message.SenderId}) has sent a message to {args.Message.RecipientId}");
        }

        private void MessageReceived(object sender, AccountActivityMessageReceivedEventArgs args)
        {
            Console.WriteLine($"Account user ({args.Message.SenderId}) has received a message from {args.Message.RecipientId}");
        }

        private void UserIsTypingMessage(object sender, AccountActivityUserIsTypingMessageEventArgs e)
        {
            Console.WriteLine($"User {e.TypingUser} is typing a message to {e.TypingTo}...");
        }

        private void UserReadMessage(object sender, AccountActivityUserReadMessageConversationEventArgs e)
        {
            Console.WriteLine($"User {e.UserWhoReadTheMessageConversation} read the message of {e.UserWhoWroteTheMessage} at {e.EventDate}");
        }

        // User events
        private void FollowedUser(object sender, AccountActivityUserFollowedEventArgs e)
        {
            if (e.InResultOf == UserFollowedRaisedInResultOf.AccountUserFollowingAnotherUser)
            {
                Console.WriteLine($"Account user ({e.FollowedBy.ScreenName}) is now following {e.FollowedUser.ScreenName}");
            }
            else
            {
                Console.WriteLine($"Account user ({e.FollowedUser.ScreenName}) is now being followed by {e.FollowedBy.ScreenName}");
            }
        }

        private void UnfollowedUser(object sender, AccountActivityUserUnfollowedEventArgs e)
        {
            Console.WriteLine($"Account user ({e.UnfollowedBy.ScreenName}) is no longer following {e.UnfollowedUser.ScreenName}");
        }

        private void UserBlocked(object sender, AccountActivityUserBlockedEventArgs e)
        {
            Console.WriteLine($"Account user ({e.BlockedBy}) has blocked {e.BlockedUser}");
        }
        private void UserUnblocked(object sender, AccountActivityUserUnblockedEventArgs e)
        {
            Console.WriteLine($"Account user ({e.UnblockedBy}) has unblocked {e.UnblockedUser}");
        }
        
        private void UserMuted(object sender, AccountActivityUserMutedEventArgs e)
        {
            Console.WriteLine($"Account user ({e.MutedBy}) has unmuted {e.MutedUser}");
        }

        private void UserUnmuted(object sender, AccountActivityUserUnmutedEventArgs e)
        {
            Console.WriteLine($"Account user ({e.UnmutedBy}) has unmuted {e.UnmutedUser}");
        }


        // Other events
        private void JsonObjectReceived(object sender, JsonObjectEventArgs args)
        {
            Console.WriteLine(args.Json);
        }

        private void UnmanagedEventReceived(object sender, UnsupportedEventReceivedEventArgs e)
        {
            Console.WriteLine("An event that Tweetinvi is not yet capable of analyzing has been received. Please open a github issue with this message: " + e.JsonMessageReceived);
        }
    }
}
