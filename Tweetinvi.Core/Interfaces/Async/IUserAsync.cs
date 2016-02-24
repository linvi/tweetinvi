using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces.Async
{
    public interface IUserAsync
    {
        // Friends

        /// <summary>
        /// Get a list of the user's friends.
        /// </summary>
        Task<IEnumerable<long>> GetFriendIdsAsync(int maxFriendsToRetrieve = 5000);

        /// <summary>
        /// Get a list of the user's friends.
        /// </summary>
        Task<IEnumerable<IUser>> GetFriendsAsync(int maxFriendsToRetrieve = 250);

        // Followers

        /// <summary>
        /// Get a list of the user's follower ids.
        /// </summary>
        Task<IEnumerable<long>> GetFollowerIdsAsync(int maxFriendsToRetrieve = 5000);

        /// <summary>
        /// Get a list of the user's followers.
        /// </summary>
        Task<IEnumerable<IUser>> GetFollowersAsync(int maxFriendsToRetrieve = 250);

        // Friendship

        /// <summary>
        /// Get the relationship details between the user and another one.
        /// </summary>
        Task<IRelationshipDetails> GetRelationshipWithAsync(IUser user);

        // Timeline

        /// <summary>
        /// Get the tweets published by the user.
        /// </summary>
        Task<IEnumerable<ITweet>> GetUserTimelineAsync(int maximumTweet = 40);

        /// <summary>
        /// Get the tweets published by the user.
        /// </summary>
        Task<IEnumerable<ITweet>> GetUserTimelineAsync(IUserTimelineParameters timelineParameters);

        // Get Favorites

        /// <summary>
        /// Get the tweets favourited by the user.
        /// </summary>
        Task<IEnumerable<ITweet>> GetFavoritesAsync(int maximumTweets = 40);

        // Block  

        /// <summary>
        /// Make the authenticated user block the user.
        /// </summary>
        Task<bool> BlockAsync();

        // Unblock

        /// <summary>
        /// Make the authenticated user unblock the user.
        /// </summary>
        Task<bool> UnBlockAsync();

        // Stream Profile Image
        Task<Stream> GetProfileImageStreamAsync(ImageSize imageSize = ImageSize.normal);

        /// <summary>
        /// Get the list of contributors to the account of the current user
        /// Update the matching attribute of the current user if the parameter is true
        /// Return the list of contributors
        /// </summary>
        /// <param name="createContributorList">False by default. Indicates if the _contributors attribute needs to be updated with the result</param>
        /// <returns>The list of contributors to the account of the current user</returns>
        Task<IEnumerable<IUser>> GetContributorsAsync(bool createContributorList = false);

        /// <summary>
        /// Get the list of accounts the current user is allowed to update
        /// Update the matching attribute of the current user if the parameter is true
        /// Return the list of contributees
        /// </summary>
        /// <param name="createContributeeList">False by default. Indicates if the _contributees attribute needs to be updated with the result</param>
        /// <returns>The list of accounts the current user is allowed to update</returns>
        Task<IEnumerable<IUser>> GetContributeesAsync(bool createContributeeList = false);
    }
}
