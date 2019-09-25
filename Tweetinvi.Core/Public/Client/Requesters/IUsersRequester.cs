using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    /// <summary>
    /// A client providing all the methods related with users.
    /// The results from this client contain additional metadata.
    /// </summary>
    public interface IUsersRequester
    {
        /// <summary>
        /// Get the authenticated user based on the TwitterClient's credentials
        /// </summary>
        /// <returns>TwitterResult containing the client's authenticated user</returns>
        Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters);

        /// <summary>
        /// Get a user
        /// </summary>
        /// <returns>TwitterResult containing a user</returns>
        Task<ITwitterResult<IUserDTO, IUser>> GetUser(IGetUserParameters parameters);

        /// <summary>
        /// Get multiple users
        /// </summary>
        /// <returns>TwitterResult containing a collection of users</returns>
        Task<ITwitterResult<IUserDTO[], IUser[]>> GetUsers(IGetUsersParameters parameters);

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>TwitterCursorResult to iterate over all the user's friends</returns>
        ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIds(IGetFriendIdsParameters parameters);

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>TwitterCursorResult to iterate over all the user's friends</returns>
        ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIds(IGetFollowerIdsParameters parameters);

        /// <summary>
        /// Block a user
        /// </summary>
        /// <returns>TwitterResult containing the blocked user</returns>
        Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters);

        /// <summary>
        /// Unblock a user
        /// </summary>
        /// <returns>TwitterResult containing the unblocked user</returns>
        Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters);

        /// <summary>
        /// Unblock a user
        /// </summary>
        /// <returns>TwitterResult containing the reported user</returns>
        Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters);

        /// <summary>
        /// Get blocked user ids
        /// </summary>
        /// <returns>TwitterCursorResult to iterate over all the blocked users</returns>
        ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters);

        /// <summary>
        /// Get blocked user ids
        /// </summary>
        /// <returns>TwitterCursorResult to iterate over all the blocked users</returns>
        ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetBlockedUsers(IGetBlockedUsersParameters parameters);

        /// <summary>
        /// Follow a user
        /// </summary>
        /// <returns>Twitter result containing the followed user</returns>
        Task<ITwitterResult<IUserDTO>> FollowUser(IFollowUserParameters parameters);

        /// <summary>
        /// Stop following a user
        /// </summary>
        /// <returns>Twitter result containing the user who is no longer followed</returns>
        Task<ITwitterResult<IUserDTO>> UnFollowUser(IUnFollowUserParameters parameters);

        /// <summary>
        /// Get the profile image of a user
        /// </summary>
        /// <returns>A stream of the image file</returns>
        Task<System.IO.Stream> GetProfileImageStream(IGetProfileImageParameters parameters);
    }
}