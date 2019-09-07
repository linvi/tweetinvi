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

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's friends</returns>
        Task<ICursorResult<long>> GetFriendIds(IGetFriendIdsParameters parameters);

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
        Task<ICursorResult<long>> GetFriendIds(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's friends</returns>
        Task<ICursorResult<IUser>> GetFriends(IGetFriendsParameters parameters);
    }
}
