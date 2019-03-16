using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Tweetinvi.Events;
using Tweetinvi.Streaming;

namespace Examplinvi.WebhooksServer.Controllers
{
    public class AccountActivityEventsManager
    {
        private readonly List<IAccountActivityStream> _trackedStreams = new List<IAccountActivityStream>();

        public void RegisterAccountActivityStream(IAccountActivityStream accountActivityStream)
        {
            if (_trackedStreams.Contains(accountActivityStream))
            {
                return;
            }

            _trackedStreams.Add(accountActivityStream);

            Console.WriteLine($"Activities for user {accountActivityStream.UserId} are now being tracked.");

            accountActivityStream.TweetCreated += TweetCreated;
            accountActivityStream.JsonObjectReceived += JsonObjectReceived;
            accountActivityStream.MessageReceived += MessageReceived;
            accountActivityStream.MessageSent += MessageSent;
            accountActivityStream.UserFollowed += FollowedUser;
            accountActivityStream.UserUnfollowed += UnfollowedUser;
            accountActivityStream.TweetFavourited += TweetFavourited;
            accountActivityStream.TweetDeleted += TweetDeleted;
            accountActivityStream.UnmanagedEventReceived += (sender, args) =>
            {
                Console.WriteLine(args);
            };
        }

        [SuppressMessage("ReSharper", "DelegateSubtraction")]
        public void UnregisterAccountActivityStream(IAccountActivityStream accountActivityStream)
        {
            _trackedStreams.Remove(accountActivityStream);

            accountActivityStream.TweetCreated -= TweetCreated;
            accountActivityStream.JsonObjectReceived -= JsonObjectReceived;
            accountActivityStream.MessageReceived -= MessageReceived;
            accountActivityStream.MessageSent -= MessageSent;
            accountActivityStream.UserFollowed -= FollowedUser;
            accountActivityStream.UserUnfollowed -= UnfollowedUser;
            accountActivityStream.TweetFavourited -= TweetFavourited;
            accountActivityStream.TweetDeleted -= TweetDeleted;
        }

        private void TweetCreated(object sender, AccountActivityTweetCreatedEventArgs e)
        {
            Console.WriteLine("Tweet created", e);
        }

        private void TweetDeleted(object sender, AccountActivityTweetDeletedEventArgs e)
        {
            Console.WriteLine("Tweet deleted", e);
        }

        private void TweetFavourited(object sender, AccountActivityTweetFavouritedEventArgs e)
        {
            Console.WriteLine("Tweet was favourited", e);
        }

        private void FollowedUser(object sender, AccountActivityUserFollowedEventArgs e)
        {
            // Account user followed another user
            if (e.InResultOf == UserFollowedRaisedInResultOf.AccountUserFollowingAnotherUser)
            {
                Console.WriteLine($"Account user ({e.FollowedBy.ScreenName}) is now following {e.UserFollowed.ScreenName}");
            }
            else
            {
                Console.WriteLine($"Account user ({e.UserFollowed.ScreenName}) is now being followed by {e.FollowedBy.ScreenName}");
            }
        }

        private void UnfollowedUser(object sender, UserUnFollowedEventArgs e)
        {
            // Account user unfollowed another user
            Console.WriteLine($"You ({e.Source.ScreenName}) are no longer following {e.Target.ScreenName}");
        }

        private void MessageSent(object sender, MessageEventArgs args)
        {
            Console.WriteLine(args.Message.App);
        }

        private void MessageReceived(object sender, MessageEventArgs args)
        {
            Console.WriteLine(args.Message.App);
        }

        private void JsonObjectReceived(object sender, JsonObjectEventArgs args)
        {
            Console.WriteLine(args.Json);
        }
    }
}
