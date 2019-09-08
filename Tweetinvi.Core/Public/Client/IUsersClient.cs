using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public interface IUsersClient
    {
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

        // GET_USERS

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

        // FRIENDS

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's friends</returns>
        Task<ICursorResult<long>> GetFriendIds(string username);

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's friends</returns>
        Task<ICursorResult<long>> GetFriendIds(long userId);

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's friends</returns>
        Task<ICursorResult<long>> GetFriendIds(IGetFriendIdsParameters parameters);

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's friends</returns>
        Task<ICursorResult<long>> GetFriendIds(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get friends from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's friends</returns>
        Task<ICursorResult<IUser>> GetFriends(IGetFriendsParameters parameters);

        // FOLLOWERS

        /// <summary>
        /// Get followers ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's followers</returns>
        Task<ICursorResult<long>> GetFollowerIds(string username);

        /// <summary>
        /// Get followers ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's followers</returns>
        Task<ICursorResult<long>> GetFollowerIds(long userId);

        /// <summary>
        /// Get followers ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's followers</returns>
        Task<ICursorResult<long>> GetFollowerIds(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get followers ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's followers</returns>
        Task<ICursorResult<long>> GetFollowerIds(IGetFollowerIdsParameters parameters);

        /// <summary>
        /// Get followers from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's followers</returns>
        Task<ICursorResult<IUser>> GetFollowers(IGetFollowersParameters parameters);

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
    }
}
