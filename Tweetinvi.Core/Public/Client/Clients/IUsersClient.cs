using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface IUsersClient
    {
        #region Get User

        /// <summary>
        /// Get a user
        /// </summary>
        Task<IUser> GetUser(long? userId);

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
        ITwitterIterator<long> GetFriendIds(IUserIdentifier userIdentifier);

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>A CursorResult to iterate over all the user's friends</returns>
        ITwitterIterator<long> GetFriendIds(IGetFriendIdsParameters parameters);

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
