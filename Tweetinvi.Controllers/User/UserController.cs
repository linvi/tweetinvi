using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Logic.QueryParameters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.User
{
    /// <summary>
    /// Reason for change : Twitter changes the operation exposed on its REST API
    /// </summary>
    public class UserController : IUserController
    {
        private readonly IUserQueryExecutor _userQueryExecutor;
        private readonly ITweetFactory _tweetFactory;
        private readonly IUserFactory _userFactory;
        private readonly ITwitterResultFactory _twitterResultFactory;

        public UserController(
            IUserQueryExecutor userQueryExecutor,
            ITweetFactory tweetFactory,
            IUserFactory userFactory,
            ITwitterResultFactory twitterResultFactory)
        {
            _userQueryExecutor = userQueryExecutor;
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;
            _twitterResultFactory = twitterResultFactory;
        }

        public async Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters, ITwitterRequest request)
        {
            var result = await _userQueryExecutor.GetAuthenticatedUser(parameters, request);
            return _twitterResultFactory.Create(result, userDTO => _userFactory.GenerateAuthenticatedUserFromDTO(userDTO));
        }

        public async Task<ITwitterResult<IUserDTO, IUser>> GetUser(IGetUserParameters parameters, ITwitterRequest request)
        {
            var result = await _userQueryExecutor.GetUser(parameters, request);
            return _twitterResultFactory.Create(result, userDTO => _userFactory.GenerateUserFromDTO(userDTO));
        }

        public async Task<ITwitterResult<IUserDTO[], IUser[]>> GetUsers(IGetUsersParameters parameters, ITwitterRequest request)
        {
            var result = await _userQueryExecutor.GetUsers(parameters, request);
            return _twitterResultFactory.Create(result, userDTO => _userFactory.GenerateUsersFromDTO(userDTO));
        }

        // Friend Ids
        public TwitterCursorResult<long, IIdsCursorQueryResultDTO> GetFriendIds(IGetFriendIdsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterCursorResult<long, IIdsCursorQueryResultDTO>(async cursor => 
            {
                var cursoredParameters = new GetFriendIdsParameters(parameters)
                {
                    Cursor = cursor
                };

                return await _userQueryExecutor.GetFriendIds(cursoredParameters, new TwitterRequest(request));
            });

            twitterCursorResult.NextCursor = parameters.Cursor;
            
            return twitterCursorResult;
        }

        public TwitterCursorResult<long, IIdsCursorQueryResultDTO> GetFollowerIds(IGetFollowerIdsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterCursorResult<long, IIdsCursorQueryResultDTO>(async cursor =>
            {
                var cursoredParameters = new GetFollowerIdsParameters(parameters)
                {
                    Cursor = cursor
                };

                return await _userQueryExecutor.GetFollowerIds(cursoredParameters, new TwitterRequest(request));
            });

            twitterCursorResult.NextCursor = parameters.Cursor;

            return twitterCursorResult;
        }

        // Block
        public Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.BlockUser(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.UnblockUser(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.ReportUserForSpam(parameters, request);
        }

        public TwitterCursorResult<long, IIdsCursorQueryResultDTO> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterCursorResult<long, IIdsCursorQueryResultDTO>(async cursor =>
            {
                var cursoredParameters = new GetBlockedUserIdsParameters(parameters)
                {
                    Cursor = cursor
                };

                return await _userQueryExecutor.GetBlockedUserIds(cursoredParameters, new TwitterRequest(request));
            });

            twitterCursorResult.NextCursor = parameters.Cursor;

            return twitterCursorResult;
        }

        // Favourites
        public Task<IEnumerable<ITweet>> GetFavoriteTweets(IUserIdentifier user, IGetUserFavoritesParameters parameters)
        {
            var favoriteParameters = new GetUserFavoritesQueryParameters(user, parameters);
            return GetFavoriteTweets(favoriteParameters);
        }

        public async Task<IEnumerable<ITweet>> GetFavoriteTweets(IGetUserFavoritesQueryParameters parameters)
        {
            var tweetDTOs = await _userQueryExecutor.GetFavoriteTweets(parameters);
            return _tweetFactory.GenerateTweetsFromDTO(tweetDTOs, null, null);
        }

        public async Task<IEnumerable<IUser>> GetBlockedUsers(int maxUsers = Int32.MaxValue)
        {
            var userDTOs = await _userQueryExecutor.GetBlockedUsers(maxUsers);
            return _userFactory.GenerateUsersFromDTO(userDTOs);
        }

        // Stream Profile Image
        public Stream GetProfileImageStream(IUser user, ImageSize imageSize = ImageSize.normal)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return GetProfileImageStream(user.UserDTO, imageSize);
        }

        public Stream GetProfileImageStream(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            return _userQueryExecutor.GetProfileImageStream(userDTO, imageSize);
        }
    }
}