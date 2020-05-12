using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
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

        public UserController(IUserQueryExecutor userQueryExecutor)
        {
            _userQueryExecutor = userQueryExecutor;
        }

        public Task<ITwitterResult<IUserDTO>> GetAuthenticatedUserAsync(IGetAuthenticatedUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.GetAuthenticatedUserAsync(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> GetUserAsync(IGetUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.GetUserAsync(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO[]>> GetUsersAsync(IGetUsersParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.GetUsersAsync(parameters, request);
        }

        // Friend Ids
        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIdsIterator(IGetFriendIdsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetFriendIdsParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetFriendIdsAsync(cursoredParameters, new TwitterRequest(request));
                },
                page => page.Model.NextCursorStr,
                page => page.Model.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIdsIterator(IGetFollowerIdsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetFollowerIdsParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetFollowerIdsAsync(cursoredParameters, new TwitterRequest(request));
                },
                page => page.Model.NextCursorStr,
                page => page.Model.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public Task<ITwitterResult<IRelationshipDetailsDTO>> GetRelationshipBetweenAsync(IGetRelationshipBetweenParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.GetRelationshipBetweenAsync(parameters, request);
        }

        // Profile Image
        public Task<Stream> GetProfileImageStreamAsync(IGetProfileImageParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.GetProfileImageStreamAsync(parameters, request);
        }



        // FOLLOW/UNFOLLOW
        public Task<ITwitterResult<IUserDTO>> FollowUserAsync(IFollowUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.FollowUserAsync(parameters, request);
        }

        public Task<ITwitterResult<IRelationshipDetailsDTO>> UpdateRelationshipAsync(IUpdateRelationshipParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.UpdateRelationshipAsync(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UnfollowUserAsync(IUnfollowUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.UnfollowUserAsync(parameters, request);
        }

        // FRIENDSHIP

        public Task<ITwitterResult<IRelationshipStateDTO[]>> GetRelationshipsWithAsync(IGetRelationshipsWithParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.GetRelationshipsWithAsync(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendshipIterator(IGetUserIdsRequestingFriendshipParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetUserIdsRequestingFriendshipParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetUserIdsRequestingFriendshipAsync(cursoredParameters, new TwitterRequest(request));
                },
                page => page.Model.NextCursorStr,
                page => page.Model.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsYouRequestedToFollowIterator(IGetUserIdsYouRequestedToFollowParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetUserIdsYouRequestedToFollowParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetUserIdsYouRequestedToFollowAsync(cursoredParameters, new TwitterRequest(request));
                },
                page => page.Model.NextCursorStr,
                page => page.Model.NextCursorStr == "0");

            return twitterCursorResult;
        }

        // BLOCK
        public Task<ITwitterResult<IUserDTO>> BlockUserAsync(IBlockUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.BlockUserAsync(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUserAsync(IUnblockUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.UnblockUserAsync(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpamAsync(IReportUserForSpamParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.ReportUserForSpamAsync(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIdsIterator(IGetBlockedUserIdsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetBlockedUserIdsParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetBlockedUserIdsAsync(cursoredParameters, new TwitterRequest(request));
                },
                page => page.Model.NextCursorStr,
                page => page.Model.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetBlockedUsersIterator(IGetBlockedUsersParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetBlockedUsersParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetBlockedUsersAsync(cursoredParameters, new TwitterRequest(request));
                },
                page => page.Model.NextCursorStr,
                page => page.Model.NextCursorStr == "0");

            return twitterCursorResult;
        }

        // MUTE
        public Task<ITwitterResult<long[]>> GetUserIdsWhoseRetweetsAreMutedAsync(IGetUserIdsWhoseRetweetsAreMutedParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.GetUserIdsWhoseRetweetsAreMutedAsync(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetMutedUserIdsIterator(IGetMutedUserIdsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetMutedUserIdsParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetMutedUserIdsAsync(cursoredParameters, new TwitterRequest(request));
                },
                page => page.Model.NextCursorStr,
                page => page.Model.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetMutedUsersIterator(IGetMutedUsersParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetMutedUsersParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _userQueryExecutor.GetMutedUsersAsync(cursoredParameters, new TwitterRequest(request));
                },
                page => page.Model.NextCursorStr,
                page => page.Model.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public Task<ITwitterResult<IUserDTO>> MuteUserAsync(IMuteUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.MuteUserAsync(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UnmuteUserAsync(IUnmuteUserParameters parameters, ITwitterRequest request)
        {
            return _userQueryExecutor.UnmuteUserAsync(parameters, request);
        }
    }
}