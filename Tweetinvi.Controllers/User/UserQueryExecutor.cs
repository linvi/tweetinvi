using System;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;
using Tweetinvi.Public.Parameters.UsersClient;

namespace Tweetinvi.Controllers.User
{
    public interface IUserQueryExecutor
    {
        // USERS
        Task<ITwitterResult<IUserDTO>> GetUser(IGetUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO[]>> GetUsers(IGetUsersParameters parameters, ITwitterRequest request);

        // FRIENDS
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIds(IGetFriendIdsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIds(IGetFollowerIdsParameters parameters, ITwitterRequest request);

        Task<ITwitterResult<IRelationshipDetailsDTO>> GetRelationshipBetween(IGetRelationshipBetweenParameters parameters, ITwitterRequest request);

        Task<Stream> GetProfileImageStream(IGetProfileImageParameters parameters, ITwitterRequest request);
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
        
        public Task<ITwitterResult<IUserDTO>> GetUser(IGetUserParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetUserQuery(parameters, request.ExecutionContext.TweetMode);

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
        
        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIds(IGetFollowerIdsParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetFollowerIdsQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IIdsCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIds(IGetFriendIdsParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetFriendIdsQuery(parameters);
            
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IIdsCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IRelationshipDetailsDTO>> GetRelationshipBetween(IGetRelationshipBetweenParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetRelationshipBetweenQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IRelationshipDetailsDTO>(request);
        }

        // Stream Profile Image
        public Task<Stream> GetProfileImageStream(IGetProfileImageParameters parameters, ITwitterRequest request)
        {
            var url = _userQueryGenerator.DownloadProfileImageURL(parameters);

            request.Query.Url = url;
            request.Query.HttpMethod = HttpMethod.GET;

            return _webHelper.GetResponseStreamAsync(request);
        }

    }
}