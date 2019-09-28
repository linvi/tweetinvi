using System;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
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
        private readonly IUserQueryValidator _userQueryValidator;
        private readonly ITwitterResultFactory _twitterResultFactory;

        public UserController(
            IUserQueryExecutor userQueryExecutor,
            IUserFactory userFactory,
            ITwitterResultFactory twitterResultFactory,
            IUserQueryValidator userQueryValidator)
        {
            _userQueryExecutor = userQueryExecutor;
            _userFactory = userFactory;
            _twitterResultFactory = twitterResultFactory;
            _userQueryValidator = userQueryValidator;
        }

        public async Task<ITwitterResult<IUserDTO, IUser>> GetUser(IGetUserParameters parameters, ITwitterRequest request)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters?.User, $"${nameof(parameters)}.{nameof(parameters.User)}");

            var result = await _userQueryExecutor.GetUser(parameters, request);
            return _twitterResultFactory.Create(result, userDTO => _userFactory.GenerateUserFromDTO(userDTO, null));
        }

        public async Task<ITwitterResult<IUserDTO[], IUser[]>> GetUsers(IGetUsersParameters parameters, ITwitterRequest request)
        {
            if (parameters?.Users == null)
            {
                throw new ArgumentNullException($"${nameof(parameters)}.{nameof(parameters.Users)}");
            }

            var limits = request.ExecutionContext.Limits;
            var maxSize = limits.USERS_GET_USERS_MAX_SIZE;
            
            if (parameters.Users.Length > maxSize)
            {
                throw new TwitterArgumentLimitException($"${nameof(parameters)}.{nameof(parameters.Users)}", maxSize, nameof(limits.USERS_GET_USERS_MAX_SIZE), "users");
            }

            var result = await _userQueryExecutor.GetUsers(parameters, request);
            return _twitterResultFactory.Create(result, userDTO => _userFactory.GenerateUsersFromDTO(userDTO, null));
        }

        // Friend Ids
        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIds(IGetFriendIdsParameters parameters, ITwitterRequest request)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User, $"${nameof(parameters)}.{nameof(parameters.User)}");

            var limits = request.ExecutionContext.Limits;
            var maxPageSize = limits.USERS_GET_FRIEND_IDS_PAGE_MAX_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"${nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(limits.USERS_GET_FRIEND_IDS_PAGE_MAX_SIZE), "page size");
            }

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
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User, $"${nameof(parameters)}.{nameof(parameters.User)}");

            var limits = request.ExecutionContext.Limits;
            var maxPageSize = limits.USERS_GET_FOLLOWER_IDS_PAGE_MAX_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"${nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(limits.USERS_GET_FOLLOWER_IDS_PAGE_MAX_SIZE), "page size");
            }

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
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.SourceUser, $"${nameof(parameters)}.{nameof(parameters.SourceUser)}");
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.TargetUser, $"${nameof(parameters)}.{nameof(parameters.TargetUser)}");

            return _userQueryExecutor.GetRelationshipBetween(parameters, request);
        }

        // Profile Image
        public Task<Stream> GetProfileImageStream(IGetProfileImageParameters parameters, ITwitterRequest request)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            return _userQueryExecutor.GetProfileImageStream(parameters, request);
        }
    }
}