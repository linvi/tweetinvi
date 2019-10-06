using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.User
{
    public class UserController : IUserController
    {
        private readonly IUserQueryExecutor _userQueryExecutor;
        private readonly IUserFactory _userFactory;
        private readonly ITwitterResultFactory _twitterResultFactory;

        public UserController(
            IUserQueryExecutor userQueryExecutor,
            IUserFactory userFactory,
            ITwitterResultFactory twitterResultFactory)
        {
            _userQueryExecutor = userQueryExecutor;
            _userFactory = userFactory;
            _twitterResultFactory = twitterResultFactory;
        }

        public async Task<ITwitterResult<IUserDTO, IUser>> GetUser(IGetUserParameters parameters, ITwitterRequest request)
        {
            var result = await _userQueryExecutor.GetUser(parameters, request);
            return _twitterResultFactory.Create(result, userDTO => _userFactory.GenerateUserFromDTO(userDTO, null));
        }

        public async Task<ITwitterResult<IUserDTO[], IUser[]>> GetUsers(IGetUsersParameters parameters, ITwitterRequest request)
        {
            var result = await _userQueryExecutor.GetUsers(parameters, request);
            return _twitterResultFactory.Create(result, userDTO => _userFactory.GenerateUsersFromDTO(userDTO, null));
        }

        // Friend Ids
        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIds(IGetFriendIdsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                // ReSharper disable once PossibleNullReferenceException
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetFriendIdsParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetFriendIds(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIds(IGetFollowerIdsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                // ReSharper disable once PossibleNullReferenceException
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetFollowerIdsParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetFollowerIds(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public Task<ITwitterResult<IRelationshipDetailsDTO>> GetRelationshipBetween(IGetRelationshipBetweenParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.GetRelationshipBetween(parameters, request);
        }

        // Profile Image
        public Task<Stream> GetProfileImageStream(IGetProfileImageParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.GetProfileImageStream(parameters, request);
        }
    }
}