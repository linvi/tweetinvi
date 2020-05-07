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
        Task<IAuthenticatedUser> GetAuthenticatedUserAsync();

        /// <summary>
        /// Get the authenticated user based on the client's credentials
        /// <para>Read more : https://dev.twitter.com/rest/reference/get/account/verify_credentials </para>
        /// </summary>
        /// <returns>The client's authenticated user</returns>
        Task<IAuthenticatedUser> GetAuthenticatedUserAsync(IGetAuthenticatedUserParameters parameters);

        #endregion

        #region Get User

        /// <inheritdoc cref="GetUser(IGetUserParameters)" />
        Task<IUser> GetUserAsync(long userId);
        /// <inheritdoc cref="GetUser(IGetUserParameters)" />
        Task<IUser> GetUserAsync(string username);
        /// <inheritdoc cref="GetUser(IGetUserParameters)" />
        Task<IUser> GetUserAsync(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-show </para>
        /// </summary>
        /// <returns>Returns a user</returns>
        Task<IUser> GetUserAsync(IGetUserParameters parameters);

        #endregion

        #region GetUsers

        /// <inheritdoc cref="GetUsers(IGetUsersParameters)" />
        Task<IUser[]> GetUsersAsync(IEnumerable<long> userIds);
        /// <inheritdoc cref="GetUsers(IGetUsersParameters)" />
        Task<IUser[]> GetUsersAsync(IEnumerable<string> usernames);
        /// <inheritdoc cref="GetUsers(IGetUsersParameters)" />
        Task<IUser[]> GetUsersAsync(IEnumerable<IUserIdentifier> userIdentifiers);

        /// <summary>
        /// Get multiple users
        /// </summary>
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-lookup </para>
        /// <returns>Returns the list of users requested</returns>
        Task<IUser[]> GetUsersAsync(IGetUsersParameters parameters);

        #endregion

        #region GetFriendIds / Friends

        /// <inheritdoc cref="GetFriendIds(IGetFriendIdsParameters)" />
        Task<long[]> GetFriendIdsAsync(string username);
        /// <inheritdoc cref="GetFriendIds(IGetFriendIdsParameters)" />
        Task<long[]> GetFriendIdsAsync(long userId);
        /// <inheritdoc cref="GetFriendIds(IGetFriendIdsParameters)" />
        Task<long[]> GetFriendIdsAsync(IUserIdentifier user);

        /// <summary>
        /// Get friend ids from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friends-ids </para>
        /// </summary>
        /// <returns>List a user's friend ids</returns>
        Task<long[]> GetFriendIdsAsync(IGetFriendIdsParameters parameters);

        /// <inheritdoc cref="GetFriendIdsIterator(IGetFriendIdsParameters)" />
        ITwitterIterator<long> GetFriendIdsIterator(string username);
        /// <inheritdoc cref="GetFriendIdsIterator(IGetFriendIdsParameters)" />
        ITwitterIterator<long> GetFriendIdsIterator(long userId);
        /// <inheritdoc cref="GetFriendIdsIterator(IGetFriendIdsParameters)" />
        ITwitterIterator<long> GetFriendIdsIterator(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get friend ids from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friends-ids </para>
        /// </summary>
        /// <returns>An iterator to list a user's friend ids</returns>
        ITwitterIterator<long> GetFriendIdsIterator(IGetFriendIdsParameters parameters);

        /// <inheritdoc cref="GetFriends(IGetFriendsParameters)" />
        Task<IUser[]> GetFriendsAsync(long userId);
        /// <inheritdoc cref="GetFriends(IGetFriendsParameters)" />
        Task<IUser[]> GetFriendsAsync(string username);
        /// <inheritdoc cref="GetFriends(IGetFriendsParameters)" />
        Task<IUser[]> GetFriendsAsync(IUserIdentifier user);

        /// <summary>
        /// Get friends from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friends-ids </para>
        /// </summary>
        /// <returns>List of a user's friends</returns>
        Task<IUser[]> GetFriendsAsync(IGetFriendsParameters parameters);

        /// <summary>
        /// Get friends from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friends-ids </para>
        /// </summary>
        /// <returns>An iterator to list a user's friends</returns>
        IMultiLevelCursorIterator<long, IUser> GetFriendsIterator(IGetFriendsParameters parameters);

        #endregion

        #region GetFollowerIds / Followers

        /// <inheritdoc cref="GetFollowerIds(IGetFollowerIdsParameters)" />
        Task<long[]> GetFollowerIdsAsync(long userId);
        /// <inheritdoc cref="GetFollowerIds(IGetFollowerIdsParameters)" />
        Task<long[]> GetFollowerIdsAsync(string username);
        /// <inheritdoc cref="GetFollowerIds(IGetFollowerIdsParameters)" />
        Task<long[]> GetFollowerIdsAsync(IUserIdentifier user);

        /// <summary>
        /// Get the follower ids from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids </para>
        /// </summary>
        /// <returns>List of a user's follower ids</returns>
        Task<long[]> GetFollowerIdsAsync(IGetFollowerIdsParameters parameters);

        /// <inheritdoc cref="GetFollowerIdsIterator(IGetFollowerIdsParameters)" />
        ITwitterIterator<long> GetFollowerIdsIterator(long userId);
        /// <inheritdoc cref="GetFollowerIdsIterator(IGetFollowerIdsParameters)" />
        ITwitterIterator<long> GetFollowerIdsIterator(string username);
        /// <inheritdoc cref="GetFollowerIdsIterator(IGetFollowerIdsParameters)" />
        ITwitterIterator<long> GetFollowerIdsIterator(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get the follower ids from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids </para>
        /// </summary>
        /// <returns>An iterator to list a user's follower ids'</returns>
        ITwitterIterator<long> GetFollowerIdsIterator(IGetFollowerIdsParameters parameters);

        /// <inheritdoc cref="GetFollowers(IGetFollowersParameters)" />
        Task<IUser[]> GetFollowersAsync(long userId);
        /// <inheritdoc cref="GetFollowers(IGetFollowersParameters)" />
        Task<IUser[]> GetFollowersAsync(string username);
        /// <inheritdoc cref="GetFollowers(IGetFollowersParameters)" />
        Task<IUser[]> GetFollowersAsync(IUserIdentifier user);

        /// <summary>
        /// Get the followers from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids </para>
        /// </summary>
        /// <returns>List of a user's followers</returns>
        Task<IUser[]> GetFollowersAsync(IGetFollowersParameters parameters);

        /// <summary>
        /// Get the followers from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids </para>
        /// </summary>
        /// <returns>An iterator to list a user's followers'</returns>
        IMultiLevelCursorIterator<long, IUser> GetFollowersIterator(IGetFollowersParameters parameters);

        #endregion

        #region Relationship between users

        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetweenAsync(long sourceUserId, long targetUserId);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetweenAsync(long sourceUserId, string targetUsername);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetweenAsync(long sourceUserId, IUserIdentifier targetUser);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetweenAsync(string sourceUsername, long targetUserId);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetweenAsync(string sourceUsername, string targetUsername);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetweenAsync(string sourceUsername, IUserIdentifier targetUser);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetweenAsync(IUserIdentifier sourceUser, long targetUserId);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetweenAsync(IUserIdentifier sourceUser, string targetUsername);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetweenAsync(IUserIdentifier sourceUser, IUserIdentifier targetUser);

        /// <summary>
        /// Get the relationship between a source user and the target
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-show </para>
        /// </summary>
        /// <returns>Returns relationship information seen from a source user</returns>
        Task<IRelationshipDetails> GetRelationshipBetweenAsync(IGetRelationshipBetweenParameters parameters);

        #endregion

        #region Block / Unblock

        /// <inheritdoc cref="BlockUser(IBlockUserParameters)" />
        Task<IUser> BlockUserAsync(long userId);

        /// <inheritdoc cref="BlockUser(IBlockUserParameters)" />
        Task<IUser> BlockUserAsync(string username);

        /// <inheritdoc cref="BlockUser(IBlockUserParameters)" />
        Task<IUser> BlockUserAsync(IUserIdentifier user);

        /// <summary>
        /// Block a user from the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-blocks-create </para>
        /// </summary>
        Task<IUser> BlockUserAsync(IBlockUserParameters parameters);

        /// <inheritdoc cref="UnblockUser(IUnblockUserParameters)" />
        Task<IUser> UnblockUserAsync(long userId);

        /// <inheritdoc cref="UnblockUser(IUnblockUserParameters)" />
        Task<IUser> UnblockUserAsync(string username);

        /// <inheritdoc cref="UnblockUser(IUnblockUserParameters)" />
        Task<IUser> UnblockUserAsync(IUserIdentifier user);

        /// <summary>
        /// Unblock a user from the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-blocks-destroy </para>
        /// </summary>
        Task<IUser> UnblockUserAsync(IUnblockUserParameters parameters);

        /// <inheritdoc cref="ReportUserForSpam(IReportUserForSpamParameters)" />
        Task<IUser> ReportUserForSpamAsync(long userId);

        /// <inheritdoc cref="ReportUserForSpam(IReportUserForSpamParameters)" />
        Task<IUser> ReportUserForSpamAsync(string username);

        /// <inheritdoc cref="ReportUserForSpam(IReportUserForSpamParameters)" />
        Task<IUser> ReportUserForSpamAsync(IUserIdentifier user);

        /// <summary>
        /// Report a user for spam
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-users-report_spam </para>
        /// </summary>
        Task<IUser> ReportUserForSpamAsync(IReportUserForSpamParameters parameters);

        /// <inheritdoc cref="GetBlockedUserIds(IGetBlockedUserIdsParameters)" />
        Task<long[]> GetBlockedUserIdsAsync();

        /// <summary>
        /// Get the user ids blocked by the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids </para>
        /// </summary>
        /// <returns>List of the blocked user ids</returns>
        Task<long[]> GetBlockedUserIdsAsync(IGetBlockedUserIdsParameters parameters);

        /// <inheritdoc cref="GetBlockedUserIdsIterator(IGetBlockedUserIdsParameters)" />
        ITwitterIterator<long> GetBlockedUserIdsIterator();

        /// <summary>
        /// Get the user ids blocked by the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids </para>
        /// </summary>
        /// <returns>An iterator to list the blocked users</returns>
        ITwitterIterator<long> GetBlockedUserIdsIterator(IGetBlockedUserIdsParameters parameters);

        /// <inheritdoc cref="GetBlockedUsers(IGetBlockedUsersParameters)" />
        Task<IUser[]> GetBlockedUsersAsync();

        /// <summary>
        /// Get the users blocked by the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids </para>
        /// </summary>
        /// <returns>List of blocked users</returns>
        Task<IUser[]> GetBlockedUsersAsync(IGetBlockedUsersParameters parameters);

        /// <inheritdoc cref="GetBlockedUsersIterator(IGetBlockedUsersParameters)" />
        ITwitterIterator<IUser> GetBlockedUsersIterator();

        /// <summary>
        /// Get the users blocked by the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids </para>
        /// </summary>
        /// <returns>An iterator to list the blocked users</returns>
        ITwitterIterator<IUser> GetBlockedUsersIterator(IGetBlockedUsersParameters parameters);

        #endregion

        #region Follow / Unfollow

        /// <inheritdoc cref="FollowUser(IFollowUserParameters)" />
        Task<IUser> FollowUserAsync(long userId);

        /// <inheritdoc cref="FollowUser(IFollowUserParameters)" />
        Task<IUser> FollowUserAsync(string username);

        /// <inheritdoc cref="FollowUser(IFollowUserParameters)" />
        Task<IUser> FollowUserAsync(IUserIdentifier user);

        /// <summary>
        /// Follow a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-create </para>
        /// </summary>
        Task<IUser> FollowUserAsync(IFollowUserParameters parameters);

        /// <inheritdoc cref="UnfollowUser(IUnfollowUserParameters)" />
        Task<IUser> UnfollowUserAsync(long userId);

        /// <inheritdoc cref="UnfollowUser(IUnfollowUserParameters)" />
        Task<IUser> UnfollowUserAsync(string username);

        /// <inheritdoc cref="UnfollowUser(IUnfollowUserParameters)" />
        Task<IUser> UnfollowUserAsync(IUserIdentifier user);

        /// <summary>
        /// Stops following a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-destroy </para>
        /// </summary>
        Task<IUser> UnfollowUserAsync(IUnfollowUserParameters parameters);

        #endregion

        #region Follower Requests

        /// <inheritdoc cref="GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters)" />
        Task<long[]> GetUserIdsRequestingFriendshipAsync();

        /// <summary>
        /// Get the pending follower ids requests for protected accounts.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets </para>
        /// </summary>
        /// <returns>List the user ids who requested to follow the client's account</returns>
        Task<long[]> GetUserIdsRequestingFriendshipAsync(IGetUserIdsRequestingFriendshipParameters parameters);

        /// <inheritdoc cref="GetUserIdsRequestingFriendshipIterator(IGetUserIdsRequestingFriendshipParameters)" />
        ITwitterIterator<long> GetUserIdsRequestingFriendshipIterator();

        /// <summary>
        /// Get the pending follower ids requests for protected accounts.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets </para>
        /// </summary>
        /// <returns>An iterator to list the user ids who requested to follow the client's account</returns>
        ITwitterIterator<long> GetUserIdsRequestingFriendshipIterator(IGetUserIdsRequestingFriendshipParameters parameters);

        /// <inheritdoc cref="GetUsersRequestingFriendship(IGetUsersRequestingFriendshipParameters)" />
        Task<IUser[]> GetUsersRequestingFriendshipAsync();

        /// <summary>
        /// Get the pending follower requests for protected accounts.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets </para>
        /// </summary>
        /// <returns>List the users who requested to follow the client's account</returns>
        Task<IUser[]> GetUsersRequestingFriendshipAsync(IGetUsersRequestingFriendshipParameters parameters);

        /// <inheritdoc cref="GetUsersRequestingFriendshipIterator(IGetUsersRequestingFriendshipParameters)" />
        IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendshipIterator();

        /// <summary>
        /// Get the pending follower requests for protected accounts.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets </para>
        /// </summary>
        /// <returns>An iterator to list the users who requested to follow the client's account</returns>
        IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendshipIterator(IGetUsersRequestingFriendshipParameters parameters);

        /// <inheritdoc cref="GetUserIdsYouRequestedToFollow(IGetUserIdsYouRequestedToFollowParameters)" />
        Task<long[]> GetUserIdsYouRequestedToFollowAsync();

        /// <summary>
        /// Get the pending follower ids requests that you have requested.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-outgoing </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets</para>
        /// </summary>
        /// <returns>List the user ids the client's account requested to follow</returns>
        Task<long[]> GetUserIdsYouRequestedToFollowAsync(IGetUserIdsYouRequestedToFollowParameters parameters);

        /// <inheritdoc cref="GetUserIdsYouRequestedToFollowIterator(IGetUserIdsYouRequestedToFollowParameters)" />
        ITwitterIterator<long> GetUserIdsYouRequestedToFollowIterator();

        /// <summary>
        /// Get the pending follower ids requests that you have requested.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-outgoing </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets</para>
        /// </summary>
        /// <returns>An iterator to list the user ids the client's account requested to follow</returns>
        ITwitterIterator<long> GetUserIdsYouRequestedToFollowIterator(IGetUserIdsYouRequestedToFollowParameters parameters);

        /// <inheritdoc cref="GetUsersYouRequestedToFollow(IGetUsersYouRequestedToFollowParameters)" />
        Task<IUser[]> GetUsersYouRequestedToFollowAsync();

        /// <summary>
        /// Get the pending follower ids requests that you have requested.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-outgoing </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets</para>
        /// </summary>
        /// <returns>List the users the client's account requested to follow</returns>
        Task<IUser[]> GetUsersYouRequestedToFollowAsync(IGetUsersYouRequestedToFollowParameters parameters);

        /// <inheritdoc cref="GetUsersYouRequestedToFollowIterator(IGetUsersYouRequestedToFollowParameters)" />
        IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollowIterator();

        /// <summary>
        /// Get the pending follower ids requests that you have requested.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-outgoing </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets</para>
        /// </summary>
        /// <returns>An iterator to list the users the client's account requested to follow</returns>
        IMultiLevelCursorIterator<long, IUser> GetUsersYouRequestedToFollowIterator(IGetUsersYouRequestedToFollowParameters parameters);

        #endregion

        #region Relationship

        /// <summary>
        /// Modify the relationship between the authenticated user (source) and another user (target).
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-update </para>
        /// </summary>
        /// <returns>Returns whether the update operation was successful.</returns>
        Task UpdateRelationshipAsync(IUpdateRelationshipParameters parameters);

        /// <inheritdoc cref="GetRelationshipsWith(IGetRelationshipsWithParameters)" />
        Task<IUserDictionary<IRelationshipState>> GetRelationshipsWithAsync(long[] userIds);
        /// <inheritdoc cref="GetRelationshipsWith(IGetRelationshipsWithParameters)" />
        Task<IUserDictionary<IRelationshipState>> GetRelationshipsWithAsync(string[] usernames);
        /// <inheritdoc cref="GetRelationshipsWith(IGetRelationshipsWithParameters)" />
        Task<IUserDictionary<IRelationshipState>> GetRelationshipsWithAsync(IUserIdentifier[] users);
        /// <inheritdoc cref="GetRelationshipsWith(IGetRelationshipsWithParameters)" />
        Task<IUserDictionary<IRelationshipState>> GetRelationshipsWithAsync(IUser[] users);

        /// <summary>
        /// Get the relationships between the account's user and multiple users
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-lookup </para>
        /// </summary>
        /// <returns>Returns a dictionary of user and their relationship with the client's user</returns>
        Task<IUserDictionary<IRelationshipState>> GetRelationshipsWithAsync(IGetRelationshipsWithParameters parameters);

        #endregion

        #region Muted

        /// <inheritdoc cref="GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters)" />
        Task<long[]> GetUserIdsWhoseRetweetsAreMutedAsync();

        /// <summary>
        /// Get the user ids for whom the retweets are muted
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-no_retweets-ids </para>
        /// </summary>
        /// <returns>Returns a list of user ids for whom the retweets are muted</returns>
        Task<long[]> GetUserIdsWhoseRetweetsAreMutedAsync(IGetUserIdsWhoseRetweetsAreMutedParameters parameters);

        /// <inheritdoc cref="GetMutedUserIds(IGetMutedUserIdsParameters)" />
        Task<long[]> GetMutedUserIdsAsync();

        /// <summary>
        /// Get the muted user ids.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-ids </para>
        /// </summary>
        /// <returns>List of the user ids muted by the client's account</returns>
        Task<long[]> GetMutedUserIdsAsync(IGetMutedUserIdsParameters parameters);

        /// <inheritdoc cref="GetMutedUserIdsIterator(IGetMutedUserIdsParameters)" />
        ITwitterIterator<long> GetMutedUserIdsIterator();

        /// <summary>
        /// Get the muted user ids.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-ids </para>
        /// </summary>
        /// <returns>An iterator to list the user ids muted by the client's account</returns>
        ITwitterIterator<long> GetMutedUserIdsIterator(IGetMutedUserIdsParameters parameters);

        /// <inheritdoc cref="GetMutedUsers(IGetMutedUsersParameters)" />
        Task<IUser[]> GetMutedUsersAsync();

        /// <summary>
        /// Get the muted user ids.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-list </para>
        /// </summary>
        /// <returns>List of the users muted by the client's account</returns>
        Task<IUser[]> GetMutedUsersAsync(IGetMutedUsersParameters parameters);

        /// <inheritdoc cref="GetMutedUsersIterator(IGetMutedUsersParameters)" />
        ITwitterIterator<IUser> GetMutedUsersIterator();

        /// <summary>
        /// Get the muted user ids.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-list </para>
        /// </summary>
        /// <returns>An iterator to list the users muted by the client's account</returns>
        ITwitterIterator<IUser> GetMutedUsersIterator(IGetMutedUsersParameters parameters);

        /// <inheritdoc cref="MuteUser(IMuteUserParameters)" />
        Task<IUser> MuteUserAsync(long userId);
        /// <inheritdoc cref="MuteUser(IMuteUserParameters)" />
        Task<IUser> MuteUserAsync(string username);
        /// <inheritdoc cref="MuteUser(IMuteUserParameters)" />
        Task<IUser> MuteUserAsync(IUserIdentifier user);

        /// <summary>
        /// Mute a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-mutes-users-create </para>
        /// </summary>
        Task<IUser> MuteUserAsync(IMuteUserParameters parameters);

        /// <inheritdoc cref="UnmuteUser(IUnmuteUserParameters)" />
        Task<IUser> UnmuteUserAsync(long userId);
        /// <inheritdoc cref="UnmuteUser(IUnmuteUserParameters)" />
        Task<IUser> UnmuteUserAsync(string username);
        /// <inheritdoc cref="UnmuteUser(IUnmuteUserParameters)" />
        Task<IUser> UnmuteUserAsync(IUserIdentifier user);

        /// <summary>
        /// Remove the mute of a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-mutes-users-destroy </para>
        /// </summary>
        Task<IUser> UnmuteUserAsync(IUnmuteUserParameters parameters);

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

    }
}
