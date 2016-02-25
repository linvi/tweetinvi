using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    public interface IUserStream : ITwitterStream
    {
        // Parameters
        RepliesFilterType RepliesFilterType { get; set; }
        WithFilterType WithFilterType { get; set; }

        /// <summary>
        /// The user stream may take some time to get initialized.
        /// During this duration twitter does not send any live information
        /// StreamIsReady informs the developper that all the events are now captured.
        /// </summary>
        event EventHandler StreamIsReady;

        /// <summary>
        /// Event informing that a Tweet has been created by the AuthenticatedUser
        /// </summary>
        event EventHandler<TweetReceivedEventArgs> TweetCreatedByMe;
        event EventHandler<TweetReceivedEventArgs> TweetCreatedByFriend;
        event EventHandler<TweetReceivedEventArgs> TweetCreatedByAnyone;
        event EventHandler<TweetReceivedEventArgs> TweetCreatedByAnyoneButMe;

        // Message
        event EventHandler<MessageEventArgs> MessageSent;
        event EventHandler<MessageEventArgs> MessageReceived;

        // Follow
        event EventHandler<UserFollowedEventArgs> FollowedUser;
        event EventHandler<UserFollowedEventArgs> FollowedByUser;

        // Un Follow
        event EventHandler<UserFollowedEventArgs> UnFollowedUser;
        
        // Friends
        event EventHandler<GenericEventArgs<IEnumerable<long>>> FriendIdsReceived;

        // Tweet Favourited
        event EventHandler<TweetFavouritedEventArgs> TweetFavouritedByMe;
        event EventHandler<TweetFavouritedEventArgs> TweetFavouritedByAnyone;
        event EventHandler<TweetFavouritedEventArgs> TweetFavouritedByAnyoneButMe;

        // Tweet UnFavourited
        event EventHandler<TweetFavouritedEventArgs> TweetUnFavouritedByMe;
        event EventHandler<TweetFavouritedEventArgs> TweetUnFavouritedByAnyone;
        event EventHandler<TweetFavouritedEventArgs> TweetUnFavouritedByAnyoneButMe;

        // List Events
        event EventHandler<ListEventArgs> ListCreated;
        event EventHandler<ListEventArgs> ListUpdated;
        event EventHandler<ListEventArgs> ListDestroyed;

        event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserAddedMemberToList;
        event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserAddedToListBy;

        event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserRemovedMemberFromList;
        event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserRemovedFromListBy;

        event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserSubscribedToListCreatedBy;
        event EventHandler<ListUserUpdatedEventArgs> UserSubscribedToListCreatedByMe;

        event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserUnsubscribedToListCreatedBy;
        event EventHandler<ListUserUpdatedEventArgs> UserUnsubscribedToListCreatedByMe;

        // Blocked - Unblocked
        event EventHandler<UserBlockedEventArgs> BlockedUser;
        event EventHandler<UserBlockedEventArgs> UnBlockedUser;

        // User Updated
        event EventHandler<AuthenticatedUserUpdatedEventArgs> AuthenticatedUserProfileUpdated;

        // Warning
        event EventHandler<WarningTooManyFollowersEventArgs> WarningTooManyFollowersDetected;

        event EventHandler<AccessRevokedEventArgs> AccessRevoked;

        void StartStream();
        Task StartStreamAsync();
    }
}