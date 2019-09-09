using System;
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
        Task<ITwitterResult<IUserDTO[]>> GetUsers(IGetUsersParameters parameters, ITwitterRequest request);


        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIds(IGetFriendIdsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIds(IGetFollowerIdsParameters parameters, ITwitterRequest request);

        Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters, ITwitterRequest request);


        // Favourites
        Task<IEnumerable<ITweetDTO>> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters);

        // Get blocked users
        Task<IEnumerable<IUserDTO>> GetBlockedUsers(int maxUsers = int.MaxValue);

        // Stream Profile Image
        Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal);
        
    }

    public class UserQueryExecutor : IUserQueryExecutor
    {
        private readonly IUserQueryGenerator _userQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IWebHelper _webHelper;

        public UserQueryExecutor(
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

        public Task<ITwitterResult<IUserDTO[]>> GetUsers(IGetUsersParameters parameters, ITwitterRequest request)
        {
            if (parameters?.UserIdentifiers == null)
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("UserIdentifiers");
            }

            var maxSize = request.ExecutionContext.Limits.Users.GetUsersMaxSize;

            if (parameters.UserIdentifiers.Length > maxSize)
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentOutOfRangeException($"UserIdentifiers cannot have more than {maxSize} items", "UserIdentifiers");
            }

            var query = _userQueryGenerator.GetUsersQuery(parameters, request.ExecutionContext.TweetMode);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IUserDTO[]>(request);
        }

        // Friend ids
        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIds(IGetFriendIdsParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetFriendIdsQuery(parameters);

            if (parameters.Cursor != null)
            {
                query = query.AddParameterToQuery("cursor", parameters.Cursor);
            }

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IIdsCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIds(IGetFollowerIdsParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetFollowerIdsQuery(parameters);

            if (parameters.Cursor != null)
            {
                query = query.AddParameterToQuery("cursor", parameters.Cursor);
            }

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IIdsCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetBlockUserQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequest<IUserDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetUnblockUserQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequest<IUserDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetReportUserForSpamQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequest<IUserDTO>(request);
        }

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetBlockedUserIdsQuery(parameters);

            if (parameters.Cursor != null)
            {
                query = query.AddParameterToQuery("cursor", parameters.Cursor);
            }

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IIdsCursorQueryResultDTO>(request);
        }

        // Favourites
        public Task<IEnumerable<ITweetDTO>> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters)
        {
            var query = _userQueryGenerator.GetFavoriteTweetsQuery(parameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        // Get Block List
        public Task<IEnumerable<IUserDTO>> GetBlockedUsers(int maxUsers = int.MaxValue)
        {
            string query = _userQueryGenerator.GetBlockedUsersQuery();
            return _twitterAccessor.ExecuteCursorGETQuery<IUserDTO, IUserCursorQueryResultDTO>(query, maxUsers);
        }

        // Stream Profile Image
        public Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            var url = _userQueryGenerator.DownloadProfileImageURL(userDTO, imageSize);
            return _webHelper.GetResponseStream(url);
        }
    }
}