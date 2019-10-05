using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface IUsersClient
    {
        IUsersClientParametersValidator ParametersValidator { get; }
        
        #region Get User

        /// <inheritdoc cref="GetUser(IGetUserParameters)" />
        Task<IUser> GetUser(long? userId);
        /// <inheritdoc cref="GetUser(IGetUserParameters)" />
        Task<IUser> GetUser(string username);
        /// <inheritdoc cref="GetUser(IGetUserParameters)" />
        Task<IUser> GetUser(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-show </para>
        /// </summary>
        /// <returns>Returns a user</returns>
        Task<IUser> GetUser(IGetUserParameters parameters);

        #endregion

        #region GetUsers

        /// <inheritdoc cref="GetUsers(IGetUsersParameters)" />
        Task<IUser[]> GetUsers(IEnumerable<long> userIds);
        /// <inheritdoc cref="GetUsers(IGetUsersParameters)" />
        Task<IUser[]> GetUsers(IEnumerable<string> usernames);
        /// <inheritdoc cref="GetUsers(IGetUsersParameters)" />
        Task<IUser[]> GetUsers(IEnumerable<IUserIdentifier> userIdentifiers);

        /// <summary>
        /// Get multiple users
        /// </summary>
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-lookup </para>
        /// <returns>Returns the list of users requested</returns>
        Task<IUser[]> GetUsers(IGetUsersParameters parameters);

        #endregion

        #region GetFriendIds / Friends

        /// <inheritdoc cref="GetFriendIds(IGetFriendIdsParameters)" />
        ITwitterIterator<long> GetFriendIds(string username);
        /// <inheritdoc cref="GetFriendIds(IGetFriendIdsParameters)" />
        ITwitterIterator<long> GetFriendIds(long userId);
        /// <inheritdoc cref="GetFriendIds(IGetFriendIdsParameters)" />
        ITwitterIterator<long> GetFriendIds(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get friend ids from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friends-ids </para>
        /// </summary>
        /// <returns>An iterator to list a user's friend ids</returns>
        ITwitterIterator<long> GetFriendIds(IGetFriendIdsParameters parameters);

        /// <summary>
        /// Get friends from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friends-ids </para>
        /// </summary>
        /// <returns>An iterator to list a user's friends</returns>
        IMultiLevelCursorIterator<long, IUser> GetFriends(IGetFriendsParameters parameters);

        #endregion

        #region GetFollowerIds / Followers

        /// <inheritdoc cref="GetFollowerIds(IGetFollowerIdsParameters)" />
        ITwitterIterator<long> GetFollowerIds(long? userId);
        /// <inheritdoc cref="GetFollowerIds(IGetFollowerIdsParameters)" />
        ITwitterIterator<long> GetFollowerIds(string username);
        /// <inheritdoc cref="GetFollowerIds(IGetFollowerIdsParameters)" />
        ITwitterIterator<long> GetFollowerIds(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get the follower ids from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids </para>
        /// </summary>
        /// <returns>An iterator to list a user's follower ids'</returns>
        ITwitterIterator<long> GetFollowerIds(IGetFollowerIdsParameters parameters);

        /// <summary>
        /// Get the followers from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids </para>
        /// </summary>
        /// <returns>An iterator to list a user's followers'</returns>
        IMultiLevelCursorIterator<long, IUser> GetFollowers(IGetFollowersParameters parameters);

        #endregion

        #region Profile Image

        /// <inheritdoc cref="GetProfileImageStream(IGetProfileImageParameters)" />
        Task<System.IO.Stream> GetProfileImageStream(string url);

        /// <inheritdoc cref="GetProfileImageStream(IGetProfileImageParameters)" />
        Task<System.IO.Stream> GetProfileImageStream(IUser user);

        /// <inheritdoc cref="GetProfileImageStream(IGetProfileImageParameters)" />
        Task<System.IO.Stream> GetProfileImageStream(IUserDTO user);

        /// <summary>
        /// Get the profile image of a user
        /// </summary>
        /// <returns>A stream of the image file</returns>
        Task<System.IO.Stream> GetProfileImageStream(IGetProfileImageParameters parameters);

        #endregion

        #region Relationship between users

        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(long? sourceUserId, long? targetUserId);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(long? sourceUserId, string targetUsername);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(long? sourceUserId, IUserIdentifier targetUser);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(string sourceUsername, long? targetUserId);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(string sourceUsername, string targetUsername);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(string sourceUsername, IUserIdentifier targetUser);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(IUserIdentifier sourceUser, long? targetUserId);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(IUserIdentifier sourceUser, string targetUsername);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(IUserIdentifier sourceUser, IUserIdentifier targetUser);

        /// <summary>
        /// Get the relationship between a source user and the target
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-show </para>
        /// </summary>
        /// <returns>Returns relationship information seen from a source user</returns>
        Task<IRelationshipDetails> GetRelationshipBetween(IGetRelationshipBetweenParameters parameters);

        #endregion
    }
}
