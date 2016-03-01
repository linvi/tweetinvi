using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    /// <summary>
    /// Stream notifying the client about everything that can happen to a user.
    /// </summary>
    public interface IUserStream : ITwitterStream
    {
        // Parameters

        /// <summary>
        /// Filter the type of replies received in the stream.
        /// </summary>
        RepliesFilterType RepliesFilterType { get; set; }

        /// <summary>
        /// Filter the tweets based on whether it has been received by the 
        /// authenticated user or one of the user he follows.
        /// </summary>
        WithFilterType WithFilterType { get; set; }

        /// <summary>
        /// The user stream may take some time to get initialized.
        /// During this duration twitter does not send any live information
        /// StreamIsReady informs the developper that all the events are now captured.
        /// </summary>
        event EventHandler StreamIsReady;

        /// <summary>
        /// Event informing that a Tweet has been created by the autheenticated user.
        /// </summary>
        event EventHandler<TweetReceivedEventArgs> TweetCreatedByMe;

        /// <summary>
        /// Event informing that a Tweet has been created by a friend.
        /// </summary>
        event EventHandler<TweetReceivedEventArgs> TweetCreatedByFriend;

        /// <summary>
        /// Event informing that a Tweet has been created by anyone related to the authenticated user.
        /// </summary>
        event EventHandler<TweetReceivedEventArgs> TweetCreatedByAnyone;

        /// <summary>
        /// Event informing that a Tweet has been created by anyone but the authenticated user.
        /// </summary>
        event EventHandler<TweetReceivedEventArgs> TweetCreatedByAnyoneButMe;

        // Message

        /// <summary>
        /// Event informing that a message has been sent.
        /// </summary>
        event EventHandler<MessageEventArgs> MessageSent;

        /// <summary>
        /// Event informing that a message has been received.
        /// </summary>
        event EventHandler<MessageEventArgs> MessageReceived;

        // Follow

        /// <summary>
        /// Event informing that authenticated user has just followed a user.
        /// </summary>
        event EventHandler<UserFollowedEventArgs> FollowedUser;

        /// <summary>
        /// Event informing that authenticated user has just been followed by another user.
        /// </summary>
        event EventHandler<UserFollowedEventArgs> FollowedByUser;

        // Un Follow

        /// <summary>
        /// Event informing that the authenticated user is no longer following another user.
        /// </summary>
        event EventHandler<UserFollowedEventArgs> UnFollowedUser;

        // Friends

        /// <summary>
        /// Ids of the friend from whom you will receive events.
        /// If you have more than 5000 friends, you will not receive any event from 
        /// the user who are not part of this list.
        /// </summary>
        event EventHandler<GenericEventArgs<IEnumerable<long>>> FriendIdsReceived;

        // Tweet Favourited

        /// <summary>
        /// Event informing that the authenticated user has favourited a tweet.
        /// </summary>
        event EventHandler<TweetFavouritedEventArgs> TweetFavouritedByMe;

        /// <summary>
        /// Event informing that a tweet has been favourited.
        /// </summary>
        event EventHandler<TweetFavouritedEventArgs> TweetFavouritedByAnyone;

        /// <summary>
        /// Event informing that a tweet has been favourited by someone else.
        /// </summary>
        event EventHandler<TweetFavouritedEventArgs> TweetFavouritedByAnyoneButMe;

        // Tweet UnFavourited

        /// <summary>
        /// Event informing that the authenticated user has unfavourited a tweet.
        /// </summary>
        event EventHandler<TweetFavouritedEventArgs> TweetUnFavouritedByMe;

        /// <summary>
        /// Event informing that a tweet has been favourited.
        /// </summary>
        event EventHandler<TweetFavouritedEventArgs> TweetUnFavouritedByAnyone;

        /// <summary>
        /// Event informing that a tweet has been unfavourited by someone else.
        /// </summary>
        event EventHandler<TweetFavouritedEventArgs> TweetUnFavouritedByAnyoneButMe;

        // List Events

        /// <summary>
        /// Event informing that a list has been created.
        /// </summary>
        event EventHandler<ListEventArgs> ListCreated;

        /// <summary>
        /// Event informing that a list has been updated.
        /// </summary>
        event EventHandler<ListEventArgs> ListUpdated;

        /// <summary>
        /// Event informing that a list has been destroyed.
        /// </summary>
        event EventHandler<ListEventArgs> ListDestroyed;

        /// <summary>
        /// Event informing that the authenticated user has added a member to one of his list.
        /// </summary>
        event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserAddedMemberToList;

        /// <summary>
        /// Event informing that the authenticated user has been added to a list.
        /// </summary>
        event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserAddedToListBy;

        /// <summary>
        /// Event informing that the authenticated user has removed a member from one of his list.
        /// </summary>
        event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserRemovedMemberFromList;

        /// <summary>
        /// Event informing that the authenticated user has been removed from a list.
        /// </summary>
        event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserRemovedFromListBy;

        /// <summary>
        /// Event informing that the authenticated user has subscribed to a list.
        /// </summary>
        event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserSubscribedToListCreatedBy;

        /// <summary>
        /// Event informing that a user has subscribed to one of the authenticated user's list.
        /// </summary>
        event EventHandler<ListUserUpdatedEventArgs> UserSubscribedToListCreatedByMe;

        /// <summary>
        /// Event informing that the authenticated user has unsubscribed from a list.
        /// </summary>
        event EventHandler<ListUserUpdatedEventArgs> AuthenticatedUserUnsubscribedToListCreatedBy;

        /// <summary>
        /// Event informing that a user has unsubscribed from one of the authenticated user's list.
        /// </summary>
        event EventHandler<ListUserUpdatedEventArgs> UserUnsubscribedToListCreatedByMe;

        // Blocked - Unblocked

        /// <summary>
        /// Event informing that a user has been blocked.
        /// </summary>
        event EventHandler<UserBlockedEventArgs> BlockedUser;

        /// <summary>
        /// Event informing that user has been unblocked.
        /// </summary>
        event EventHandler<UserBlockedEventArgs> UnBlockedUser;

        // User Updated

        /// <summary>
        /// Event informing that the profile of the authenticated user has been changed.
        /// </summary>
        event EventHandler<AuthenticatedUserUpdatedEventArgs> AuthenticatedUserProfileUpdated;

        // Warning

        /// <summary>
        /// Event informing that there are too many followers to subscribe to and 
        /// that Twitter API won't send notification for all of them.
        /// </summary>
        event EventHandler<WarningTooManyFollowersEventArgs> WarningTooManyFollowersDetected;

        /// <summary>
        /// Event informing that your credentials have been revoked.
        /// </summary>
        event EventHandler<AccessRevokedEventArgs> AccessRevoked;

        /// <summary>
        /// Start a stream SYNCHRONOUSLY. The thread will continue after the stream has stopped.
        /// </summary>
        void StartStream();

        /// <summary>
        /// Start a stream ASYNCHRONOUSLY. The task will complete when the stream stops.
        /// </summary>
        Task StartStreamAsync();
    }
}