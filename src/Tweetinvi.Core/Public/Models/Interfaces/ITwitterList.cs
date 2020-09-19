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
        DateTimeOffset CreatedAt { get; }

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
        Task<ITweet[]> GetTweetsAsync();

        /// <summary>
        /// Get the members of this list.
        /// </summary>
        Task<IUser[]> GetMembersAsync();

        /// <summary>
        /// Add a member to this list. You must be the owner of the list to do so.
        /// </summary>
        Task AddMemberAsync(long userId);

        /// <summary>
        /// Add a member to this list. You must be the owner of the list to do so.
        /// </summary>
        Task AddMemberAsync(string username);

        /// <summary>
        /// Add a member to this list. You must be the owner of the list to do so.
        /// </summary>
        Task AddMemberAsync(IUserIdentifier user);

        /// <summary>
        /// Add a list of members to this list. You must be the owner of the list to do so.
        /// </summary>
        Task AddMembersAsync(IEnumerable<long> userIds);

        /// <summary>
        /// Add a list of members to this list. You must be the owner of the list to do so.
        /// </summary>
        Task AddMembersAsync(IEnumerable<string> userScreenNames);

        /// <summary>
        /// Add a list of members to this list. You must be the owner of the list to do so.
        /// </summary>
        Task AddMembersAsync(IEnumerable<IUserIdentifier> users);

        /// <summary>
        /// Remove a member from this list. You must be the owner of the list to do so.
        /// </summary>
        Task<bool> RemoveMemberAsync(long userId);

        /// <summary>
        /// Remove a member from this list. You must be the owner of the list to do so.
        /// </summary>
        Task<bool> RemoveMemberAsync(string username);

        /// <summary>
        /// Remove a member from this list. You must be the owner of the list to do so.
        /// </summary>
        Task<bool> RemoveMemberAsync(IUserIdentifier user);

        /// <summary>
        /// Remove a list of members from this list. You must be the owner of the list to do so.
        /// </summary>
        Task RemoveMembersAsync(IEnumerable<long> userIds);

        /// <summary>
        /// Remove a list of members from this list. You must be the owner of the list to do so.
        /// </summary>
        Task RemoveMembersAsync(IEnumerable<string> usernames);

        /// <summary>
        /// Remove a list of members from this list. You must be the owner of the list to do so.
        /// </summary>
        Task RemoveMembersAsync(IEnumerable<IUserIdentifier> users);

        /// <summary>
        /// Test if a user is a member of the list.
        /// </summary>
        Task<bool> CheckUserMembershipAsync(long userId);

        /// <summary>
        /// Test if a user is a member of the list.
        /// </summary>
        Task<bool> CheckUserMembershipAsync(string userScreenName);

        /// <summary>
        /// Test if a user is a member of the list.
        /// </summary>
        Task<bool> CheckUserMembershipAsync(IUserIdentifier user);

        /// <summary>
        /// Get the subscribers of the list.
        /// </summary>
        Task<IUser[]> GetSubscribersAsync();

        /// <summary>
        /// Subscribe the authenticated user to the list.
        /// </summary>
        Task<ITwitterList> SubscribeAsync();

        /// <summary>
        /// Unsubscribe the authenticated user from the list.
        /// </summary>
        Task<ITwitterList> UnsubscribeAsync();

        /// <summary>
        /// Check whether a user has subscribed to the list.
        /// </summary>
        Task<bool> CheckUserSubscriptionAsync(long userId);

        /// <summary>
        /// Check whether a user has subscribed to the list.
        /// </summary>
        Task<bool> CheckUserSubscriptionAsync(string username);

        /// <summary>
        /// Check whether a user has subscribed to the list.
        /// </summary>
        Task<bool> CheckUserSubscriptionAsync(IUserIdentifier user);

        /// <summary>
        /// Update the list.
        /// </summary>
        Task UpdateAsync(IListMetadataParameters parameters);

        /// <summary>
        /// Destroy the list.
        /// </summary>
        Task DestroyAsync();
    }
}