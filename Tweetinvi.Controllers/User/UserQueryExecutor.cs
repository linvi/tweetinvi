using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.User
{
    public interface IUserQueryExecutor
    {
        Task<ITwitterResult<IUserDTO>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> GetUser(IGetUserParameters parameters, ITwitterRequest request);

        // Friend Ids
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIds(IGetFriendIdsParameters parameters, ITwitterRequest request);

        // Followers Ids
        Task<IEnumerable<long>> GetFollowerIds (IUserIdentifier user, int maxFollowersToRetrieve);

        // Favourites
        Task<IEnumerable<ITweetDTO>> GetFavoriteTweets (IGetUserFavoritesQueryParameters parameters);

        // Block User
        Task<bool> BlockUser (IUserIdentifier user);

        // UnBlock User
        Task<bool> UnBlockUser (IUserIdentifier user);

        // Get blocked users
        Task<IEnumerable<long>> GetBlockedUserIds (int maxUserIds = int.MaxValue);
        Task<IEnumerable<IUserDTO>> GetBlockedUsers (int maxUsers = int.MaxValue);

        // Stream Profile Image
        Stream GetProfileImageStream (IUserDTO userDTO, ImageSize imageSize = ImageSize.normal);

        // Spam
        Task<bool> ReportUserForSpam (IUserIdentifier user);
    }

    public class UserQueryExecutor : IUserQueryExecutor
    {
        private readonly IUserQueryGenerator _userQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IWebHelper _webHelper;

        public UserQueryExecutor (
            IUserQueryGenerator userQueryGenerator,
            ITwitterAccessor twitterAccessor,
            IWebHelper webHelper)
        {
            _userQueryGenerator = userQueryGenerator;
            _twitterAccessor = twitterAccessor;
            _webHelper = webHelper;
        }

        public Task<ITwitterResult<IUserDTO>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetAuthenticatedUserQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IUserDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> GetUser(IGetUserParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetUserQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IUserDTO>(request);
        }

        // Friend ids
        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIds(IGetFriendIdsParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetFriendIdsQuery(parameters.UserIdentifier, parameters.MaximumNumberOfResults);

            if (parameters.Cursor != null)
            {
                query = query.AddParameterToQuery("cursor", parameters.Cursor);
            }

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IIdsCursorQueryResultDTO>(request);
        }

        // Followers
        public Task<IEnumerable<long>> GetFollowerIds (IUserIdentifier user, int maxFollowersToRetrieve)
        {
            string query = _userQueryGenerator.GetFollowerIdsQuery (user, maxFollowersToRetrieve);
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO> (query, maxFollowersToRetrieve);
        }

        // Favourites
        public Task<IEnumerable<ITweetDTO>> GetFavoriteTweets (IGetUserFavoritesQueryParameters parameters)
        {
            var query = _userQueryGenerator.GetFavoriteTweetsQuery (parameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>> (query);
        }

        // Block
        public async Task<bool> BlockUser (IUserIdentifier user)
        {
            string query = _userQueryGenerator.GetBlockUserQuery (user);
            var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery (query);

            return asyncOperation.Success;
        }

        // UnBlock User
        public async Task<bool> UnBlockUser (IUserIdentifier user)
        {
            string query = _userQueryGenerator.GetUnBlockUserQuery (user);
            var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery (query);

            return asyncOperation.Success;
        }

        // Get Block List
        public Task<IEnumerable<long>> GetBlockedUserIds (int maxUserIds = int.MaxValue)
        {
            string query = _userQueryGenerator.GetBlockedUserIdsQuery ();
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO> (query, maxUserIds);
        }

        public Task<IEnumerable<IUserDTO>> GetBlockedUsers (int maxUsers = int.MaxValue)
        {
            string query = _userQueryGenerator.GetBlockedUsersQuery ();
            return _twitterAccessor.ExecuteCursorGETQuery<IUserDTO, IUserCursorQueryResultDTO> (query, maxUsers);
        }

        // Stream Profile Image
        public Stream GetProfileImageStream (IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            var url = _userQueryGenerator.DownloadProfileImageURL (userDTO, imageSize);
            return _webHelper.GetResponseStream (url);
        }

        // Report Spam
        public async Task<bool> ReportUserForSpam (IUserIdentifier user)
        {
            string query = _userQueryGenerator.GetReportUserForSpamQuery (user);
            var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery (query);

            return asyncOperation.Success;
        }

    }
}