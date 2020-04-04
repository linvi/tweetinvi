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
        Task<IUser> GetUser(long userId);
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
        Task<long[]> GetFriendIds(string username);
        /// <inheritdoc cref="GetFriendIds(IGetFriendIdsParameters)" />
        Task<long[]> GetFriendIds(long userId);
        /// <inheritdoc cref="GetFriendIds(IGetFriendIdsParameters)" />
        Task<long[]> GetFriendIds(IUserIdentifier user);

        /// <summary>
        /// Get friend ids from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friends-ids </para>
        /// </summary>
        /// <returns>List a user's friend ids</returns>
        Task<long[]> GetFriendIds(IGetFriendIdsParameters parameters);

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
        Task<IUser[]> GetFriends(long userId);
        /// <inheritdoc cref="GetFriends(IGetFriendsParameters)" />
        Task<IUser[]> GetFriends(string username);
        /// <inheritdoc cref="GetFriends(IGetFriendsParameters)" />
        Task<IUser[]> GetFriends(IUserIdentifier user);

        /// <summary>
        /// Get friends from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friends-ids </para>
        /// </summary>
        /// <returns>List of a user's friends</returns>
        Task<IUser[]> GetFriends(IGetFriendsParameters parameters);

        /// <summary>
        /// Get friends from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friends-ids </para>
        /// </summary>
        /// <returns>An iterator to list a user's friends</returns>
        IMultiLevelCursorIterator<long, IUser> GetFriendsIterator(IGetFriendsParameters parameters);

        #endregion

        #region GetFollowerIds / Followers

        /// <inheritdoc cref="GetFollowerIds(IGetFollowerIdsParameters)" />
        Task<long[]> GetFollowerIds(long userId);
        /// <inheritdoc cref="GetFollowerIds(IGetFollowerIdsParameters)" />
        Task<long[]> GetFollowerIds(string username);
        /// <inheritdoc cref="GetFollowerIds(IGetFollowerIdsParameters)" />
        Task<long[]> GetFollowerIds(IUserIdentifier user);

        /// <summary>
        /// Get the follower ids from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids </para>
        /// </summary>
        /// <returns>List of a user's follower ids</returns>
        Task<long[]> GetFollowerIds(IGetFollowerIdsParameters parameters);

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
        Task<IUser[]> GetFollowers(long userId);
        /// <inheritdoc cref="GetFollowers(IGetFollowersParameters)" />
        Task<IUser[]> GetFollowers(string username);
        /// <inheritdoc cref="GetFollowers(IGetFollowersParameters)" />
        Task<IUser[]> GetFollowers(IUserIdentifier user);

        /// <summary>
        /// Get the followers from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids </para>
        /// </summary>
        /// <returns>List of a user's followers</returns>
        Task<IUser[]> GetFollowers(IGetFollowersParameters parameters);

        /// <summary>
        /// Get the followers from a specific user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids </para>
        /// </summary>
        /// <returns>An iterator to list a user's followers'</returns>
        IMultiLevelCursorIterator<long, IUser> GetFollowersIterator(IGetFollowersParameters parameters);

        #endregion

        #region Relationship between users

        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(long sourceUserId, long targetUserId);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(long sourceUserId, string targetUsername);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(long sourceUserId, IUserIdentifier targetUser);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(string sourceUsername, long targetUserId);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(string sourceUsername, string targetUsername);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(string sourceUsername, IUserIdentifier targetUser);
        /// <inheritdoc cref="GetRelationshipBetween(IGetRelationshipBetweenParameters)" />
        Task<IRelationshipDetails> GetRelationshipBetween(IUserIdentifier sourceUser, long targetUserId);
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
        Task<IUser> BlockUser(long userId);

        /// <inheritdoc cref="BlockUser(IBlockUserParameters)" />
        Task<IUser> BlockUser(string username);

        /// <inheritdoc cref="BlockUser(IBlockUserParameters)" />
        Task<IUser> BlockUser(IUserIdentifier user);

        /// <summary>
        /// Block a user from the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-blocks-create </para>
        /// </summary>
        Task<IUser> BlockUser(IBlockUserParameters parameters);

        /// <inheritdoc cref="UnblockUser(IUnblockUserParameters)" />
        Task<IUser> UnblockUser(long userId);

        /// <inheritdoc cref="UnblockUser(IUnblockUserParameters)" />
        Task<IUser> UnblockUser(string username);

        /// <inheritdoc cref="UnblockUser(IUnblockUserParameters)" />
        Task<IUser> UnblockUser(IUserIdentifier user);

        /// <summary>
        /// Unblock a user from the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-blocks-destroy </para>
        /// </summary>
        Task<IUser> UnblockUser(IUnblockUserParameters parameters);

        /// <inheritdoc cref="ReportUserForSpam(IReportUserForSpamParameters)" />
        Task<IUser> ReportUserForSpam(long userId);

        /// <inheritdoc cref="ReportUserForSpam(IReportUserForSpamParameters)" />
        Task<IUser> ReportUserForSpam(string username);

        /// <inheritdoc cref="ReportUserForSpam(IReportUserForSpamParameters)" />
        Task<IUser> ReportUserForSpam(IUserIdentifier user);

        /// <summary>
        /// Report a user for spam
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-users-report_spam </para>
        /// </summary>
        Task<IUser> ReportUserForSpam(IReportUserForSpamParameters parameters);

        /// <inheritdoc cref="GetBlockedUserIds(IGetBlockedUserIdsParameters)" />
        Task<long[]> GetBlockedUserIds();

        /// <summary>
        /// Get the user ids blocked by the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids </para>
        /// </summary>
        /// <returns>List of the blocked user ids</returns>
        Task<long[]> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters);

        /// <inheritdoc cref="GetBlockedUserIdsIterator(IGetBlockedUserIdsParameters)" />
        ITwitterIterator<long> GetBlockedUserIdsIterator();

        /// <summary>
        /// Get the user ids blocked by the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids </para>
        /// </summary>
        /// <returns>An iterator to list the blocked users</returns>
        ITwitterIterator<long> GetBlockedUserIdsIterator(IGetBlockedUserIdsParameters parameters);

        /// <inheritdoc cref="GetBlockedUsers(IGetBlockedUsersParameters)" />
        Task<IUser[]> GetBlockedUsers();

        /// <summary>
        /// Get the users blocked by the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids </para>
        /// </summary>
        /// <returns>List of blocked users</returns>
        Task<IUser[]> GetBlockedUsers(IGetBlockedUsersParameters parameters);

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
        Task<IUser> FollowUser(long userId);

        /// <inheritdoc cref="FollowUser(IFollowUserParameters)" />
        Task<IUser> FollowUser(string username);

        /// <inheritdoc cref="FollowUser(IFollowUserParameters)" />
        Task<IUser> FollowUser(IUserIdentifier user);

        /// <summary>
        /// Follow a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-create </para>
        /// </summary>
        Task<IUser> FollowUser(IFollowUserParameters parameters);

        /// <inheritdoc cref="UnfollowUser(IUnfollowUserParameters)" />
        Task<IUser> UnfollowUser(long userId);

        /// <inheritdoc cref="UnfollowUser(IUnfollowUserParameters)" />
        Task<IUser> UnfollowUser(string username);

        /// <inheritdoc cref="UnfollowUser(IUnfollowUserParameters)" />
        Task<IUser> UnfollowUser(IUserIdentifier user);

        /// <summary>
        /// Stops following a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-destroy </para>
        /// </summary>
        Task<IUser> UnfollowUser(IUnfollowUserParameters parameters);

        #endregion

        #region Follower Requests

        /// <inheritdoc cref="GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters)" />
        Task<long[]> GetUserIdsRequestingFriendship();

        /// <summary>
        /// Get the pending follower ids requests for protected accounts.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets </para>
        /// </summary>
        /// <returns>List the user ids who requested to follow the client's account</returns>
        Task<long[]> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters);

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
        Task<IUser[]> GetUsersRequestingFriendship();

        /// <summary>
        /// Get the pending follower requests for protected accounts.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets </para>
        /// </summary>
        /// <returns>List the users who requested to follow the client's account</returns>
        Task<IUser[]> GetUsersRequestingFriendship(IGetUsersRequestingFriendshipParameters parameters);

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
        Task<long[]> GetUserIdsYouRequestedToFollow();

        /// <summary>
        /// Get the pending follower ids requests that you have requested.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-outgoing </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets</para>
        /// </summary>
        /// <returns>List the user ids the client's account requested to follow</returns>
        Task<long[]> GetUserIdsYouRequestedToFollow(IGetUserIdsYouRequestedToFollowParameters parameters);

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
        Task<IUser[]> GetUsersYouRequestedToFollow();

        /// <summary>
        /// Get the pending follower ids requests that you have requested.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-outgoing </para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets</para>
        /// </summary>
        /// <returns>List the users the client's account requested to follow</returns>
        Task<IUser[]> GetUsersYouRequestedToFollow(IGetUsersYouRequestedToFollowParameters parameters);

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
        Task UpdateRelationship(IUpdateRelationshipParameters parameters);

        /// <inheritdoc cref="GetRelationshipsWith(IGetRelationshipsWithParameters)" />
        Task<IUserDictionary<IRelationshipState>> GetRelationshipsWith(long[] userIds);
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

        #region Muted

        /// <inheritdoc cref="GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters)" />
        Task<long[]> GetUserIdsWhoseRetweetsAreMuted();

        /// <summary>
        /// Get the user ids for whom the retweets are muted
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-no_retweets-ids </para>
        /// </summary>
        /// <returns>Returns a list of user ids for whom the retweets are muted</returns>
        Task<long[]> GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters parameters);

        /// <inheritdoc cref="GetMutedUserIds(IGetMutedUserIdsParameters)" />
        Task<long[]> GetMutedUserIds();

        /// <summary>
        /// Get the muted user ids.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-ids </para>
        /// </summary>
        /// <returns>List of the user ids muted by the client's account</returns>
        Task<long[]> GetMutedUserIds(IGetMutedUserIdsParameters parameters);

        /// <inheritdoc cref="GetMutedUserIdsIterator(IGetMutedUserIdsParameters)" />
        ITwitterIterator<long> GetMutedUserIdsIterator();

        /// <summary>
        /// Get the muted user ids.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-ids </para>
        /// </summary>
        /// <returns>An iterator to list the user ids muted by the client's account</returns>
        ITwitterIterator<long> GetMutedUserIdsIterator(IGetMutedUserIdsParameters parameters);

        /// <inheritdoc cref="GetMutedUsers(IGetMutedUsersParameters)" />
        Task<IUser[]> GetMutedUsers();

        /// <summary>
        /// Get the muted user ids.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-list </para>
        /// </summary>
        /// <returns>List of the users muted by the client's account</returns>
        Task<IUser[]> GetMutedUsers(IGetMutedUsersParameters parameters);

        /// <inheritdoc cref="GetMutedUsersIterator(IGetMutedUsersParameters)" />
        ITwitterIterator<IUser> GetMutedUsersIterator();

        /// <summary>
        /// Get the muted user ids.
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-list </para>
        /// </summary>
        /// <returns>An iterator to list the users muted by the client's account</returns>
        ITwitterIterator<IUser> GetMutedUsersIterator(IGetMutedUsersParameters parameters);

        /// <inheritdoc cref="MuteUser(IMuteUserParameters)" />
        Task<IUser> MuteUser(long userId);
        /// <inheritdoc cref="MuteUser(IMuteUserParameters)" />
        Task<IUser> MuteUser(string username);
        /// <inheritdoc cref="MuteUser(IMuteUserParameters)" />
        Task<IUser> MuteUser(IUserIdentifier user);

        /// <summary>
        /// Mute a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-mutes-users-create </para>
        /// </summary>
        Task<IUser> MuteUser(IMuteUserParameters parameters);

        /// <inheritdoc cref="UnmuteUser(IUnmuteUserParameters)" />
        Task<IUser> UnmuteUser(long userId);
        /// <inheritdoc cref="UnmuteUser(IUnmuteUserParameters)" />
        Task<IUser> UnmuteUser(string username);
        /// <inheritdoc cref="UnmuteUser(IUnmuteUserParameters)" />
        Task<IUser> UnmuteUser(IUserIdentifier user);

        /// <summary>
        /// Remove the mute of a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-mutes-users-destroy </para>
        /// </summary>
        Task<IUser> UnmuteUser(IUnmuteUserParameters parameters);

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
