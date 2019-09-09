using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.QueryJsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IInternalUsersRequester : IUsersRequester, IBaseRequester
    {
    }

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
        ITwitterCursorResult<long, IIdsCursorQueryResultDTO> GetFriendIds(IGetFriendIdsParameters parameters);

        /// <summary>
        /// Get friend ids from a specific user
        /// </summary>
        /// <returns>TwitterCursorResult to iterate over all the user's friends</returns>
        ITwitterCursorResult<long, IIdsCursorQueryResultDTO> GetFollowerIds(IGetFollowerIdsParameters parameters);

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
        ITwitterCursorResult<long, IIdsCursorQueryResultDTO> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters);
    }

    public class UsersRequester : BaseRequester, IInternalUsersRequester
    {
        private readonly IUserController _userController;

        public UsersRequester(IUserController userController)
        {
            _userController = userController;
        }

        public async Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            var result = await ExecuteRequest(() => _userController.GetAuthenticatedUser(parameters, request), request).ConfigureAwait(false);

            var user = result.Result;

            if (user != null) { user.Client = _twitterClient; }

            return result;
        }

        public async Task<ITwitterResult<IUserDTO, IUser>> GetUser(IGetUserParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            var result = await ExecuteRequest(() => _userController.GetUser(parameters, request), request).ConfigureAwait(false);
            var user = result.Result;

            if (user != null) { user.Client = _twitterClient; }

            return result;
        }

        public async Task<ITwitterResult<IUserDTO[], IUser[]>> GetUsers(IGetUsersParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            var result = await ExecuteRequest(() => _userController.GetUsers(parameters, request), request).ConfigureAwait(false);

            var users = result.Result;

            users?.ForEach(x => x.Client = _twitterClient);

            return result;
        }

        public ITwitterCursorResult<long, IIdsCursorQueryResultDTO> GetFriendIds(IGetFriendIdsParameters parameters)
        {
            var request = _twitterClient.CreateRequest();

            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;

            return _userController.GetFriendIds(parameters, request);
        }

        public ITwitterCursorResult<long, IIdsCursorQueryResultDTO> GetFollowerIds(IGetFollowerIdsParameters parameters)
        {
            var request = _twitterClient.CreateRequest();

            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;

            return _userController.GetFollowerIds(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _userController.BlockUser(parameters, request), request);
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _userController.UnblockUser(parameters, request), request);
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _userController.ReportUserForSpam(parameters, request), request);
        }

        public ITwitterCursorResult<long, IIdsCursorQueryResultDTO> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters)
        {
            var request = _twitterClient.CreateRequest();

            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;

            return _userController.GetBlockedUserIds(parameters, request);
        }
    }
}
