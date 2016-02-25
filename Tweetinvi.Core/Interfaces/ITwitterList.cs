using System;
using System.Collections.Generic;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Async;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces
{
    public interface ITwitterList : ITwitterListAsync, ITwitterListIdentifier
    {
        ITwitterListDTO TwitterListDTO { get; set; }

        /// <summary>
        /// List Id as a string provided by Twitter.
        /// </summary>
        string IdStr { get; }

        /// <summary>
        /// List Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// List Fullname
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// User who owns the list.
        /// </summary>
        IUser Owner { get; }

        /// <summary>
        /// Date when the list was created.
        /// </summary>
        DateTime CreatedAt { get; }

        string Uri { get; }

        /// <summary>
        /// Description of the list.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Is the authenticated user following this list.
        /// </summary>
        bool Following { get; }

        /// <summary>
        /// Whether this list is private or public.
        /// </summary>
        PrivacyMode PrivacyMode { get; }

        /// <summary>
        /// Number of members in this list.
        /// </summary>
        int MemberCount { get; }

        /// <summary>
        /// Number of users who subscribed to this list.
        /// </summary>
        int SubscriberCount { get; }

        /// <summary>
        /// Get the tweets from this list.
        /// </summary>
        IEnumerable<ITweet> GetTweets(IGetTweetsFromListParameters getTweetsFromListParameters = null);

        /// <summary>
        /// Get the members of this list.
        /// </summary>
        IEnumerable<IUser> GetMembers(int maximumNumberOfUsersToRetrieve = 100);

        /// <summary>
        /// Add a member to this list. You must be the owner of the list to do so.
        /// </summary>
        bool AddMember(long userId);

        /// <summary>
        /// Add a member to this list. You must be the owner of the list to do so.
        /// </summary>
        bool AddMember(string userScreenName);

        /// <summary>
        /// Add a member to this list. You must be the owner of the list to do so.
        /// </summary>
        bool AddMember(IUserIdentifier user);

        /// <summary>
        /// Add a list of members to this list. You must be the owner of the list to do so.
        /// </summary>
        MultiRequestsResult AddMultipleMembers(IEnumerable<long> userIds);

        /// <summary>
        /// Add a list of members to this list. You must be the owner of the list to do so.
        /// </summary>
        MultiRequestsResult AddMultipleMembers(IEnumerable<string> userScreenNames);

        /// <summary>
        /// Add a list of members to this list. You must be the owner of the list to do so.
        /// </summary>
        MultiRequestsResult AddMultipleMembers(IEnumerable<IUserIdentifier> users);

        /// <summary>
        /// Remove a member from this list. You must be the owner of the list to do so.
        /// </summary>
        bool RemoveMember(long userId);

        /// <summary>
        /// Remove a member from this list. You must be the owner of the list to do so.
        /// </summary>
        bool RemoveMember(string userScreenName);

        /// <summary>
        /// Remove a member from this list. You must be the owner of the list to do so.
        /// </summary>
        bool RemoveMember(IUserIdentifier user);

        /// <summary>
        /// Remove a list of members from this list. You must be the owner of the list to do so.
        /// </summary>
        MultiRequestsResult RemoveMultipleMembers(IEnumerable<long> userIds);

        /// <summary>
        /// Remove a list of members from this list. You must be the owner of the list to do so.
        /// </summary>
        MultiRequestsResult RemoveMultipleMembers(IEnumerable<string> userScreenNames);

        /// <summary>
        /// Remove a list of members from this list. You must be the owner of the list to do so.
        /// </summary>
        MultiRequestsResult RemoveMultipleMembers(IEnumerable<IUserIdentifier> users);

        /// <summary>
        /// Test if a user is a member of the list.
        /// </summary>
        bool CheckUserMembership(long userId);

        /// <summary>
        /// Test if a user is a member of the list.
        /// </summary>
        bool CheckUserMembership(string userScreenName);
        
        /// <summary>
        /// Test if a user is a member of the list.
        /// </summary>
        bool CheckUserMembership(IUserIdentifier user);

        /// <summary>
        /// Get the subscribers of the list.
        /// </summary>
        IEnumerable<IUser> GetSubscribers(int maximumNumberOfUsersToRetrieve = 100);

        /// <summary>
        /// Subscribe the authenticated user to the list.
        /// </summary>
        bool SubscribeAuthenticatedUserToList(IAuthenticatedUser authenticatedUser = null);

        /// <summary>
        /// Unsubscribe the authenticated user from the list.
        /// </summary>
        bool UnSubscribeAuthenticatedUserFromList(IAuthenticatedUser authenticatedUser = null);

        /// <summary>
        /// Check whether a user has subscribed to the list.
        /// </summary>
        bool CheckUserSubscription(long userId);

        /// <summary>
        /// Check whether a user has subscribed to the list.
        /// </summary>
        bool CheckUserSubscription(string userScreenName);

        /// <summary>
        /// Check whether a user has subscribed to the list.
        /// </summary>
        bool CheckUserSubscription(IUserIdentifier user);

        /// <summary>
        /// Update the list.
        /// </summary>
        bool Update(ITwitterListUpdateParameters parameters);

        /// <summary>
        /// Destroy the list.
        /// </summary>
        bool Destroy();
    }
}