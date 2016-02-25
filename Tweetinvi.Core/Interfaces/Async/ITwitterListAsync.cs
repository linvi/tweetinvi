using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces.Async
{
    public interface ITwitterListAsync
    {
        /// <summary>
        /// Update the list.
        /// </summary>
        Task<bool> UpdateAsync(ITwitterListUpdateParameters parameters);

        /// <summary>
        /// Destroy the list.
        /// </summary>
        Task<bool> DestroyAsync();

        /// <summary>
        /// Get the tweets from this list.
        /// </summary>
        Task<IEnumerable<ITweet>> GetTweetsAsync(IGetTweetsFromListParameters getTweetsFromListParameters = null);

        // Membership

        /// <summary>
        /// Get the members of this list.
        /// </summary>
        Task<IEnumerable<IUser>> GetMembersAsync(int maxNumberOfUsersToRetrieve = 100);

        /// <summary>
        /// Add a member to this list. You must be the owner of the list to do so.
        /// </summary>
        Task<bool> AddMemberAsync(IUserIdentifier user);

        /// <summary>
        /// Add a list of members to this list. You must be the owner of the list to do so.
        /// </summary>
        Task<MultiRequestsResult> AddMultipleMembersAsync(IEnumerable<IUserIdentifier> users);

        /// <summary>
        /// Remove a member from this list. You must be the owner of the list to do so.
        /// </summary>
        Task<bool> RemoveMemberAsync(IUserIdentifier user);

        /// <summary>
        /// Remove a list of members from this list. You must be the owner of the list to do so.
        /// </summary>
        Task<MultiRequestsResult> RemoveMultipleMembersAsync(IEnumerable<IUserIdentifier> users);

        /// <summary>
        /// Test if a user is a member of the list.
        /// </summary>
        Task<bool> CheckUserMembershipAsync(IUserIdentifier user);

        // Subscription

        /// <summary>
        /// Get the subscribers of the list.
        /// </summary>
        Task<IEnumerable<IUser>> GetSubscribersAsync(int maximumNumberOfUsersToRetrieve = 100);

        /// <summary>
        /// Subscribe the authenticated user to the list.
        /// </summary>
        Task<bool> SubscribeAuthenticatedUserToListAsync(IAuthenticatedUser authenticatedUser = null);

        /// <summary>
        /// Unsubscribe the authenticated user from the list.
        /// </summary>
        Task<bool> UnSubscribeAuthenticatedUserFromListAsync(IAuthenticatedUser authenticatedUser = null);

        /// <summary>
        /// Check whether a user has subscribed to the list.
        /// </summary>
        Task<bool> CheckUserSubscriptionAsync(IUserIdentifier user);
    }
}