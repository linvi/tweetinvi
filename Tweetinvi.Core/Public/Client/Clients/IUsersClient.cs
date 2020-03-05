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
        /// <summary>
        /// Validate all the UsersClient parameters
        /// </summary>
        IUsersClientParametersValidator ParametersValidator { get; }

        #region AuthenticatedUser

        /// <inheritdoc cref="GetAuthenticatedUser(IGetAuthenticatedUserParameters)" />
        Task<IAuthenticatedUser> GetAuthenticatedUser();

        /// <summary>
        /// Get the authenticated user based on the client's credentials
        /// <para>Read more : https://dev.twitter.com/rest/reference/get/account/verify_credentials </para>
        /// </summary>
        /// <returns>The client's authenticated user</returns>
        Task<IAuthenticatedUser> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters);

        #endregion

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

        #region Block / Unblock

        /// <inheritdoc cref="BlockUser(IBlockUserParameters)" />
        Task BlockUser(long? userId);

        /// <inheritdoc cref="BlockUser(IBlockUserParameters)" />
        Task BlockUser(string username);

        /// <inheritdoc cref="BlockUser(IBlockUserParameters)" />
        Task BlockUser(IUserIdentifier user);

        /// <summary>
        /// Block a user from the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-blocks-create </para>
        /// </summary>
        Task BlockUser(IBlockUserParameters parameters);

        /// <inheritdoc cref="UnblockUser(IUnblockUserParameters)" />
        Task UnblockUser(long? userId);

        /// <inheritdoc cref="UnblockUser(IUnblockUserParameters)" />
        Task UnblockUser(string username);

        /// <inheritdoc cref="UnblockUser(IUnblockUserParameters)" />
        Task UnblockUser(IUserIdentifier user);

        /// <summary>
        /// Unblock a user from the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-blocks-destroy </para>
        /// </summary>
        Task UnblockUser(IUnblockUserParameters parameters);

        /// <inheritdoc cref="ReportUserForSpam(IReportUserForSpamParameters)" />
        Task ReportUserForSpam(long? userId);

        /// <inheritdoc cref="ReportUserForSpam(IReportUserForSpamParameters)" />
        Task ReportUserForSpam(string username);

        /// <inheritdoc cref="ReportUserForSpam(IReportUserForSpamParameters)" />
        Task ReportUserForSpam(IUserIdentifier user);

        /// <summary>
        /// Report a user for spam
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-users-report_spam </para>
        /// </summary>
        Task ReportUserForSpam(IReportUserForSpamParameters parameters);

        /// <inheritdoc cref="GetBlockedUserIds(IGetBlockedUserIdsParameters)" />
        ITwitterIterator<long> GetBlockedUserIds();

        /// <summary>
        /// Get the user ids blocked by the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids </para>
        /// </summary>
        /// <returns>An iterator to list the blocked users</returns>
        ITwitterIterator<long> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters);

        /// <inheritdoc cref="GetBlockedUsers(IGetBlockedUsersParameters)" />
        ITwitterIterator<IUser> GetBlockedUsers();

        /// <summary>
        /// Get the users blocked by the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids </para>
        /// </summary>
        /// <returns>An iterator to list the blocked users</returns>
        ITwitterIterator<IUser> GetBlockedUsers(IGetBlockedUsersParameters parameters);

        #endregion

        #region Follow / Unfollow

        /// <inheritdoc cref="FollowUser(IFollowUserParameters)" />
        Task FollowUser(long userId);

        /// <inheritdoc cref="FollowUser(IFollowUserParameters)" />
        Task FollowUser(string username);

        /// <inheritdoc cref="FollowUser(IFollowUserParameters)" />
        Task FollowUser(IUserIdentifier user);

        /// <summary>
        /// Follow a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-create </para>
        /// </summary>
        Task FollowUser(IFollowUserParameters parameters);

        /// <inheritdoc cref="UnfollowUser(IUnfollowUserParameters)" />
        Task UnfollowUser(long userId);

        /// <inheritdoc cref="UnfollowUser(IUnfollowUserParameters)" />
        Task UnfollowUser(string username);

        /// <inheritdoc cref="UnfollowUser(IUnfollowUserParameters)" />
        Task UnfollowUser(IUserIdentifier user);

        /// <summary>
        /// Stops following a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-destroy </para>
        /// </summary>
        Task UnfollowUser(IUnfollowUserParameters parameters);

        #endregion

        #region Follower Requests

        /// <inheritdoc cref="GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters)" />
        ITwitterIterator<long> GetUserIdsRequestingFriendship();

        /// <summary>
        /// Get the pending follower ids requests for protected accounts.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets </para>
        /// </summary>
        /// <returns>An iterator to list the user ids who requested to follow the client's account</returns>
        ITwitterIterator<long> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters);

        /// <inheritdoc cref="GetUsersRequestingFriendship(IGetUsersRequestingFriendshipParameters)" />
        IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship();

        /// <summary>
        /// Get the pending follower requests for protected accounts.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets </para>
        /// </summary>
        /// <returns>An iterator to list the users who requested to follow the client's account</returns>
        IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship(IGetUsersRequestingFriendshipParameters parameters);

        /// <inheritdoc cref="GetUserIdsYouRequestedToFollow(IGetUserIdsYouRequestedToFollowParameters)" />
        ITwitterIterator<long> GetUserIdsYouRequestedToFollow();

        /// <summary>
        /// Get the pending follower ids requests that you have requested.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-outgoing </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets</para>
        /// </summary>
        /// <returns>An iterator to list the user ids the client's account requested to follow</returns>
        ITwitterIterator<long> GetUserIdsYouRequestedToFollow(IGetUserIdsYouRequestedToFollowParameters parameters);

        /// <inheritdoc cref="GetUsersYouRequestedToFollow(IGetUsersYouRequestedToFollowParameters)" />
        IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow();

        /// <summary>
        /// Get the pending follower ids requests that you have requested.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-outgoing </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets</para>
        /// </summary>
        /// <returns>An iterator to list the users the client's account requested to follow</returns>
        IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollow(IGetUsersYouRequestedToFollowParameters parameters);

        #endregion

        #region Relationship

        /// <summary>
        /// Modify the relationship between the authenticated user (source) and another user (target).
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-update </para>
        /// </summary>
        /// <returns>Returns whether the update operation was successful.</returns>
        Task UpdateRelationship(IUpdateRelationshipParameters parameters);

        /// <inheritdoc cref="GetRelationshipsWith(IGetRelationshipsWithParameters)" />
        Task<IUserDictionary<IRelationshipState>> GetRelationshipsWith(long[] userIds);
        /// <inheritdoc cref="GetRelationshipsWith(IGetRelationshipsWithParameters)" />
        Task<IUserDictionary<IRelationshipState>> GetRelationshipsWith(long?[] userIds);
        /// <inheritdoc cref="GetRelationshipsWith(IGetRelationshipsWithParameters)" />
        Task<IUserDictionary<IRelationshipState>> GetRelationshipsWith(string[] usernames);
        /// <inheritdoc cref="GetRelationshipsWith(IGetRelationshipsWithParameters)" />
        Task<IUserDictionary<IRelationshipState>> GetRelationshipsWith(IUserIdentifier[] users);
        /// <inheritdoc cref="GetRelationshipsWith(IGetRelationshipsWithParameters)" />
        Task<IUserDictionary<IRelationshipState>> GetRelationshipsWith(IUser[] users);

        /// <summary>
        /// Get the relationships between the account's user and multiple users
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-lookup </para>
        /// </summary>
        /// <returns>Returns a dictionary of user and their relationship with the client's user</returns>
        Task<IUserDictionary<IRelationshipState>> GetRelationshipsWith(IGetRelationshipsWithParameters parameters);

        #endregion

        /// <inheritdoc cref="GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters)" />
        Task<long[]> GetUserIdsWhoseRetweetsAreMuted();

        /// <summary>
        /// Get the user ids for whom the retweets are muted
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-no_retweets-ids </para>
        /// </summary>
        /// <returns>Returns a list of user ids for whom the retweets are muted</returns>
        Task<long[]> GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters parameters);

        /// <inheritdoc cref="GetMutedUserIds(IGetMutedUserIdsParameters)" />
        ITwitterIterator<long> GetMutedUserIds();

        /// <summary>
        /// Get the muted user ids.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-ids </para>
        /// </summary>
        /// <returns>An iterator to list the user ids muted by the client's account</returns>
        ITwitterIterator<long> GetMutedUserIds(IGetMutedUserIdsParameters parameters);

        /// <inheritdoc cref="GetMutedUsers(IGetMutedUsersParameters)" />
        ITwitterIterator<IUser> GetMutedUsers();

        /// <summary>
        /// Get the muted user ids.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-list </para>
        /// </summary>
        /// <returns>An iterator to list the users muted by the client's account</returns>
        ITwitterIterator<IUser> GetMutedUsers(IGetMutedUsersParameters parameters);

        /// <inheritdoc cref="MuteUser(IMuteUserParameters)" />
        Task MuteUser(long? userId);
        /// <inheritdoc cref="MuteUser(IMuteUserParameters)" />
        Task MuteUser(string username);
        /// <inheritdoc cref="MuteUser(IMuteUserParameters)" />
        Task MuteUser(IUserIdentifier user);

        /// <summary>
        /// Mute a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-mutes-users-create </para>
        /// </summary>
        Task MuteUser(IMuteUserParameters parameters);

        /// <inheritdoc cref="UnmuteUser(IUnmuteUserParameters)" />
        Task UnmuteUser(long? userId);
        /// <inheritdoc cref="UnmuteUser(IUnmuteUserParameters)" />
        Task UnmuteUser(string username);
        /// <inheritdoc cref="UnmuteUser(IUnmuteUserParameters)" />
        Task UnmuteUser(IUserIdentifier user);

        /// <summary>
        /// Remove the mute of a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-mutes-users-destroy </para>
        /// </summary>
        Task UnmuteUser(IUnmuteUserParameters parameters);

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

    }
}
