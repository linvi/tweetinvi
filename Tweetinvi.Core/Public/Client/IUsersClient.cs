using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public interface IUsersClient
    {
        #region GetAuthenticatedUser

        /// <summary>
        /// Get the authenticated user based on the TwitterClient's credentials
        /// </summary>
        /// <returns>The client's authenticated user</returns>
        Task<IAuthenticatedUser> GetAuthenticatedUser();

        /// <summary>
        /// Get the authenticated user based on the TwitterClient's credentials
        /// </summary>
        /// <returns>The client's authenticated user</returns>
        Task<IAuthenticatedUser> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters);

        #endregion

        #region Get User

        /// <summary>
        /// Get a user
        /// </summary>
        Task<IUser> GetUser(long? userId);

        /// <summary>
        /// Get a user
        /// </summary>
        Task<IUser> GetUser(long userId);

        /// <summary>
        /// Get a user
        /// </summary>
        Task<IUser> GetUser(string username);

        /// <summary>
        /// Get a user
        /// </summary>
        Task<IUser> GetUser(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get a user
        /// </summary>
        Task<IUser> GetUser(IGetUserParameters parameters);

        #endregion

        #region GetUsers

        /// <summary>
        /// Get multiple users
        /// </summary>
        Task<IUser[]> GetUsers(IEnumerable<long> userIds);

        /// <summary>
        /// Get multiple users
        /// </summary>
        Task<IUser[]> GetUsers(IEnumerable<string> usernames);

        /// <summary>
        /// Get multiple users
        /// </summary>
        Task<IUser[]> GetUsers(IEnumerable<IUserIdentifier> userIdentifiers);

        /// <summary>
        /// Get multiple users
        /// </summary>
        Task<IUser[]> GetUsers(IGetUsersParameters parameters);

        #endregion

        #region GetFriendIds / Friends

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's friends</returns>
        ITwitterIterator<long> GetFriendIds(string username);

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's friends</returns>
        ITwitterIterator<long> GetFriendIds(long userId);

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's friends</returns>
        ITwitterIterator<long> GetFriendIds(IGetFriendIdsParameters parameters);

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's friends</returns>
        ITwitterIterator<long> GetFriendIds(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get friends from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's friends</returns>
        IMultiLevelCursorIterator<long, IUser> GetFriends(IGetFriendsParameters parameters);

        #endregion

        #region GetFollowerIds / Followers

        /// <summary>
        /// Get followers ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's followers</returns>
        ITwitterIterator<long> GetFollowerIds(string username);

        /// <summary>
        /// Get followers ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's followers</returns>
        ITwitterIterator<long> GetFollowerIds(long userId);

        /// <summary>
        /// Get followers ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's followers</returns>
        ITwitterIterator<long> GetFollowerIds(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get followers ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's followers</returns>
        ITwitterIterator<long> GetFollowerIds(IGetFollowerIdsParameters parameters);

        /// <summary>
        /// Get followers from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's followers</returns>
        IMultiLevelCursorIterator<long, IUser> GetFollowers(IGetFollowersParameters parameters);

        #endregion

        #region Block / Unblock

        /// <summary>
        /// Block a user from the clients' user account
        /// </summary>
        Task<bool> BlockUser(long? userId);

        /// <summary>
        /// Block a user from the clients' user account
        /// </summary>
        Task<bool> BlockUser(string username);

        /// <summary>
        /// Block a user from the clients' user account
        /// </summary>
        Task<bool> BlockUser(IUserIdentifier user);

        /// <summary>
        /// Block a user from the clients' user account
        /// </summary>
        Task<bool> BlockUser(IBlockUserParameters parameters); 

        /// <summary>
        /// Unblock a user from the clients' user account
        /// </summary>
        Task<bool> UnblockUser(long? userId);

        /// <summary>
        /// Unblock a user from the clients' user account
        /// </summary>
        Task<bool> UnblockUser(string username);

        /// <summary>
        /// Unblock a user from the clients' user account
        /// </summary>
        Task<bool> UnblockUser(IUserIdentifier user);

        /// <summary>
        /// Unblock a user from the clients' user account
        /// </summary>
        Task<bool> UnblockUser(IUnblockUserParameters parameters); 

        /// <summary>
        /// Report a user
        /// </summary>
        Task<bool> ReportUserForSpam(long? userId);

        /// <summary>
        /// Report a user
        /// </summary>
        Task<bool> ReportUserForSpam(string username);

        /// <summary>
        /// Report a user
        /// </summary>
        Task<bool> ReportUserForSpam(IUserIdentifier user);

        /// <summary>
        /// Report a user
        /// </summary>
        Task<bool> ReportUserForSpam(IReportUserForSpamParameters parameters);

        /// <summary>
        /// Get users blocked from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the blocked users</returns>
        ITwitterIterator<long> GetBlockedUserIds();
        
        /// <summary>
        /// Get users blocked from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the blocked users</returns>
        ITwitterIterator<long> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters);

        /// <summary>
        /// Get users blocked from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the blocked users</returns>
        ITwitterIterator<IUser> GetBlockedUsers(IGetBlockedUsersParameters parameters);

        ITwitterIterator<IUser> GetBlockedUsers();

        #endregion

        #region Follow / Unfollow

        /// <summary>
        /// Follow a user
        /// </summary>
        /// <returns>Whether the operation was successful</returns>
        Task<bool> FollowUser(long userId);

        /// <summary>
        /// Follow a user
        /// </summary>
        /// <returns>Whether the operation was successful</returns>
        Task<bool> FollowUser(string username);

        /// <summary>
        /// Follow a user
        /// </summary>
        /// <returns>Whether the operation was successful</returns>
        Task<bool> FollowUser(IUserIdentifier user);

        /// <summary>
        /// Follow a user
        /// </summary>
        /// <returns>Whether the operation was successful</returns>
        Task<bool> FollowUser(IFollowUserParameters parameters);
        
        /// <summary>
        /// Stop following a user
        /// </summary>
        /// <returns>Whether the operation was successful</returns>
        Task<bool> UnFollowUser(long userId);
        
        /// <summary>
        /// Stop following a user
        /// </summary>
        /// <returns>Whether the operation was successful</returns>
        Task<bool> UnFollowUser(string username);
        
        /// <summary>
        /// Stop following a user
        /// </summary>
        /// <returns>Whether the operation was successful</returns>
        Task<bool> UnFollowUser(IUserIdentifier user);
        
        /// <summary>
        /// Stop following a user
        /// </summary>
        /// <returns>Whether the operation was successful</returns>
        Task<bool> UnFollowUser(IUnFollowUserParameters parameters);

        #endregion

        #region Profile Image

        /// <summary>
        /// Get the profile image of a user
        /// </summary>
        /// <returns>A stream of the image file</returns>
        Task<System.IO.Stream> GetProfileImageStream(string url);

        /// <summary>
        /// Get the profile image of a user
        /// </summary>
        /// <returns>A stream of the image file</returns>
        Task<System.IO.Stream> GetProfileImageStream(IUser user);

        /// <summary>
        /// Get the profile image of a user
        /// </summary>
        /// <returns>A stream of the image file</returns>
        Task<System.IO.Stream> GetProfileImageStream(IUserDTO user);

        /// <summary>
        /// Get the profile image of a user
        /// </summary>
        /// <returns>A stream of the image file</returns>
        Task<System.IO.Stream> GetProfileImageStream(IGetProfileImageParameters parameters);

        #endregion
    }
}
