using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Tweetinvi.Events;
using Tweetinvi.Streaming;

namespace Examplinvi.AccountActivityEvents
{
    public class AccountActivityEventsManager
    {
        private readonly List<long> _trackedStreams = new List<long>();

        public void RegisterAccountActivityStream(IAccountActivityStream accountActivityStream)
        {
            if (_trackedStreams.Contains(accountActivityStream.AccountUserId))
            {
                Console.WriteLine("You are already tracking this user, no need to do that again.");
                return;
            }

            _trackedStreams.Add(accountActivityStream.AccountUserId);

            Console.WriteLine($"Activities for user {accountActivityStream.AccountUserId} are now being tracked.");

            // Tweet events
            accountActivityStream.TweetCreated += TweetCreated;
            accountActivityStream.TweetFavorited += TweetFavorited;
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
            accountActivityStream.EventReceived += AccountActivityEvent;
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
            accountActivityStream.TweetFavorited -= TweetFavorited;
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
            accountActivityStream.EventReceived -= AccountActivityEvent;
            accountActivityStream.UnsupportedEventReceived -= UnmanagedEventReceived;
        }

        // Tweet events
        private void TweetCreated(object sender, TweetCreatedEvent e)
        {
            Console.WriteLine($">>> Tweet has been created:\n{e.Tweet}");
        }

        private void TweetDeleted(object sender, TweetDeletedEvent e)
        {
            Console.WriteLine($">>> Tweet {e.TweetId} has been deleted at {e.EventDate}");
        }

        private void TweetFavorited(object sender, TweetFavoritedEvent e)
        {
            Console.WriteLine($">>> Tweet has been Favorited by {e.FavoritedBy}:\n{e.Tweet}");
        }


        // Message events
        private void MessageSent(object sender, MessageSentEvent e)
        {
            Console.WriteLine($">>> Account user ({e.Message.SenderId}) has sent a message to {e.Message.RecipientId}");
        }

        private void MessageReceived(object sender, MessageReceivedEvent e)
        {
            Console.WriteLine($">>> Account user ({e.Message.SenderId}) has received a message from {e.Message.RecipientId}");
        }

        private void UserIsTypingMessage(object sender, UserIsTypingMessageEvent e)
        {
            Console.WriteLine($">>> User {e.TypingUser} is typing a message to {e.TypingTo}...");
        }

        private void UserReadMessage(object sender, UserReadMessageConversationEvent e)
        {
            Console.WriteLine($">>> User {e.UserWhoReadTheMessageConversation} read the message of {e.UserWhoWroteTheMessage} at {e.EventDate}");
        }

        // User events
        private void FollowedUser(object sender, UserFollowedEvent e)
        {
            if (e.InResultOf == UserFollowedRaisedInResultOf.AccountUserFollowingAnotherUser)
            {
                Console.WriteLine($">>> Account user ({e.FollowedBy.ScreenName}) is now following {e.FollowedUser.ScreenName}");
            }
            else
            {
                Console.WriteLine($">>> Account user ({e.FollowedUser.ScreenName}) is now being followed by {e.FollowedBy.ScreenName}");
            }
        }

        private void UnfollowedUser(object sender, UserUnfollowedEvent e)
        {
            Console.WriteLine($">>> Account user ({e.UnfollowedBy.ScreenName}) is no longer following {e.UnfollowedUser.ScreenName}");
        }

        private void UserBlocked(object sender, UserBlockedEvent e)
        {
            Console.WriteLine($">>> Account user ({e.BlockedBy}) has blocked {e.BlockedUser}");
        }
        private void UserUnblocked(object sender, UserUnblockedEvent e)
        {
            Console.WriteLine($">>> Account user ({e.UnblockedBy}) has unblocked {e.UnblockedUser}");
        }

        private void UserMuted(object sender, UserMutedEvent e)
        {
            Console.WriteLine($">>> Account user ({e.MutedBy}) has muted {e.MutedUser}");
        }

        private void UserUnmuted(object sender, UserUnmutedEvent e)
        {
            Console.WriteLine($">>> Account user ({e.UnmutedBy}) has unmuted {e.UnmutedUser}");
        }

        // Other events
        private void AccountActivityEvent(object sender, AccountActivityEvent args)
        {
            Console.WriteLine($">>> user '{args.AccountUserId}' received:");
            Console.WriteLine($"{args.Json}");
        }

        private void UnmanagedEventReceived(object sender, UnsupportedMessageReceivedEvent e)
        {
            Console.WriteLine(">>> An event that Tweetinvi is not yet capable of analyzing has been received. Please open a github issue with this message: " + e.Message);
        }
    }
}
