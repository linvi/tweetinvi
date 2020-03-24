using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Iterators;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
// ReSharper disable UnusedMember.Global

namespace Tweetinvi.Models
{
    public interface ITwitterList : ITwitterListIdentifier
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
        /// Client used by the instance to perform any request to Twitter
        /// </summary>
        ITwitterClient Client { get; }

        /// <summary>
        /// Get the tweets from this list.
        /// </summary>
        Task<ITweet[]> GetTweets();

        /// <summary>
        /// Get the members of this list.
        /// </summary>
        Task<IUser[]> GetMembers();

        /// <summary>
        /// Add a member to this list. You must be the owner of the list to do so.
        /// </summary>
        Task AddMember(long userId);

        /// <summary>
        /// Add a member to this list. You must be the owner of the list to do so.
        /// </summary>
        Task AddMember(string username);

        /// <summary>
        /// Add a member to this list. You must be the owner of the list to do so.
        /// </summary>
        Task AddMember(IUserIdentifier user);

        /// <summary>
        /// Add a list of members to this list. You must be the owner of the list to do so.
        /// </summary>
        Task AddMembers(IEnumerable<long> userIds);

        /// <summary>
        /// Add a list of members to this list. You must be the owner of the list to do so.
        /// </summary>
        Task AddMembers(IEnumerable<string> userScreenNames);

        /// <summary>
        /// Add a list of members to this list. You must be the owner of the list to do so.
        /// </summary>
        Task AddMembers(IEnumerable<IUserIdentifier> users);

        /// <summary>
        /// Remove a member from this list. You must be the owner of the list to do so.
        /// </summary>
        Task<bool> RemoveMember(long userId);

        /// <summary>
        /// Remove a member from this list. You must be the owner of the list to do so.
        /// </summary>
        Task<bool> RemoveMember(string username);

        /// <summary>
        /// Remove a member from this list. You must be the owner of the list to do so.
        /// </summary>
        Task<bool> RemoveMember(IUserIdentifier user);

        /// <summary>
        /// Remove a list of members from this list. You must be the owner of the list to do so.
        /// </summary>
        Task RemoveMembers(IEnumerable<long> userIds);

        /// <summary>
        /// Remove a list of members from this list. You must be the owner of the list to do so.
        /// </summary>
        Task RemoveMembers(IEnumerable<string> usernames);

        /// <summary>
        /// Remove a list of members from this list. You must be the owner of the list to do so.
        /// </summary>
        Task RemoveMembers(IEnumerable<IUserIdentifier> users);

        /// <summary>
        /// Test if a user is a member of the list.
        /// </summary>
        Task<bool> CheckUserMembership(long userId);

        /// <summary>
        /// Test if a user is a member of the list.
        /// </summary>
        Task<bool> CheckUserMembership(string userScreenName);

        /// <summary>
        /// Test if a user is a member of the list.
        /// </summary>
        Task<bool> CheckUserMembership(IUserIdentifier user);

        /// <summary>
        /// Get the subscribers of the list.
        /// </summary>
        Task<IUser[]> GetSubscribers();

        /// <summary>
        /// Subscribe the authenticated user to the list.
        /// </summary>
        Task<ITwitterList> Subscribe();

        /// <summary>
        /// Unsubscribe the authenticated user from the list.
        /// </summary>
        Task<ITwitterList> Unsubscribe();

        /// <summary>
        /// Check whether a user has subscribed to the list.
        /// </summary>
        Task<bool> CheckUserSubscription(long userId);

        /// <summary>
        /// Check whether a user has subscribed to the list.
        /// </summary>
        Task<bool> CheckUserSubscription(string username);

        /// <summary>
        /// Check whether a user has subscribed to the list.
        /// </summary>
        Task<bool> CheckUserSubscription(IUserIdentifier user);

        /// <summary>
        /// Update the list.
        /// </summary>
        Task Update(IListMetadataParameters parameters);

        /// <summary>
        /// Destroy the list.
        /// </summary>
        Task Destroy();
    }
}