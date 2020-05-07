using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Core.Helpers;
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
        Task<ITwitterResult<IUserDTO>> GetAuthenticatedUserAsync(IGetAuthenticatedUserParameters parameters, ITwitterRequest request);

        // USERS
        Task<ITwitterResult<IUserDTO>> GetUserAsync(IGetUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO[]>> GetUsersAsync(IGetUsersParameters parameters, ITwitterRequest request);

        // FRIENDS
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIdsAsync(IGetFriendIdsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIdsAsync(IGetFollowerIdsParameters parameters, ITwitterRequest request);

        // BLOCK
        Task<ITwitterResult<IUserDTO>> BlockUserAsync(IBlockUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> UnblockUserAsync(IUnblockUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> ReportUserForSpamAsync(IReportUserForSpamParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIdsAsync(IGetBlockedUserIdsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserCursorQueryResultDTO>> GetBlockedUsersAsync(IGetBlockedUsersParameters parameters, ITwitterRequest request);

        // FOLLOWERS
        Task<ITwitterResult<IUserDTO>> FollowUserAsync(IFollowUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> UnfollowUserAsync(IUnfollowUserParameters parameters, ITwitterRequest request);

        // ONGOING REQUESTS
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendshipAsync(IGetUserIdsRequestingFriendshipParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsYouRequestedToFollowAsync(IGetUserIdsYouRequestedToFollowParameters parameters, ITwitterRequest request);

        // FRIENDSHIPS
        Task<ITwitterResult<IRelationshipDetailsDTO>> GetRelationshipBetweenAsync(IGetRelationshipBetweenParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IRelationshipStateDTO[]>> GetRelationshipsWithAsync(IGetRelationshipsWithParameters parameters, ITwitterRequest request);

        // MUTE
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetMutedUserIdsAsync(IGetMutedUserIdsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserCursorQueryResultDTO>> GetMutedUsersAsync(IGetMutedUsersParameters cursoredParameters, ITwitterRequest request);

        Task<ITwitterResult<IUserDTO>> MuteUserAsync(IMuteUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> UnmuteUserAsync(IUnmuteUserParameters parameters, ITwitterRequest request);

        Task<ITwitterResult<IRelationshipDetailsDTO>> UpdateRelationshipAsync(IUpdateRelationshipParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<long[]>> GetUserIdsWhoseRetweetsAreMutedAsync(IGetUserIdsWhoseRetweetsAreMutedParameters parameters, ITwitterRequest request);

        Task<Stream> GetProfileImageStreamAsync(IGetProfileImageParameters parameters, ITwitterRequest request);
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

        public Task<ITwitterResult<IUserDTO>> GetAuthenticatedUserAsync(IGetAuthenticatedUserParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetAuthenticatedUserQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequestAsync<IUserDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> GetUserAsync(IGetUserParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetUserQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequestAsync<IUserDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO[]>> GetUsersAsync(IGetUsersParameters parameters, ITwitterRequest request)
        {
            if (parameters.Users.Length == 0)
            {
                ITwitterResult<IUserDTO[]> result = new TwitterResult<IUserDTO[]>(null)
                {
                    Request = null,
                    Response = null,
                    DataTransferObject = new IUserDTO[0]
                };

                return Task.FromResult(result);
            }

            var query = _userQueryGenerator.GetUsersQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequestAsync<IUserDTO[]>(request);
        }

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFollowerIdsAsync(IGetFollowerIdsParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetFollowerIdsQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetFriendIdsAsync(IGetFriendIdsParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetFriendIdsQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IRelationshipDetailsDTO>> GetRelationshipBetweenAsync(IGetRelationshipBetweenParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetRelationshipBetweenQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequestAsync<IRelationshipDetailsDTO>(request);
        }

        // Stream Profile Image
        public Task<Stream> GetProfileImageStreamAsync(IGetProfileImageParameters parameters, ITwitterRequest request)
        {
            var url = _userQueryGenerator.DownloadProfileImageURL(parameters);

            request.Query.Url = url;
            request.Query.HttpMethod = HttpMethod.GET;

            return _webHelper.GetResponseStreamAsync(request);
        }

        // BLOCK
        public Task<ITwitterResult<IUserDTO>> BlockUserAsync(IBlockUserParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetBlockUserQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequestAsync<IUserDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUserAsync(IUnblockUserParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetUnblockUserQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequestAsync<IUserDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpamAsync(IReportUserForSpamParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetReportUserForSpamQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequestAsync<IUserDTO>(request);
        }

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIdsAsync(IGetBlockedUserIdsParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetBlockedUserIdsQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IUserCursorQueryResultDTO>> GetBlockedUsersAsync(IGetBlockedUsersParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetBlockedUsersQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<IUserCursorQueryResultDTO>(request);
        }

        // FOLLOWERS
        public Task<ITwitterResult<IUserDTO>> FollowUserAsync(IFollowUserParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetFollowUserQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<IUserDTO>(request);
        }

        public Task<ITwitterResult<IRelationshipDetailsDTO>> UpdateRelationshipAsync(IUpdateRelationshipParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetUpdateRelationshipQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<IRelationshipDetailsDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> UnfollowUserAsync(IUnfollowUserParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetUnfollowUserQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<IUserDTO>(request);
        }

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendshipAsync(IGetUserIdsRequestingFriendshipParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetUserIdsRequestingFriendshipQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsYouRequestedToFollowAsync(IGetUserIdsYouRequestedToFollowParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetUserIdsYouRequestedToFollowQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request);
        }

        // FRIENDSHIPS
        public Task<ITwitterResult<IRelationshipStateDTO[]>> GetRelationshipsWithAsync(IGetRelationshipsWithParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetRelationshipsWithQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<IRelationshipStateDTO[]>(request);
        }

        // MUTE
        public Task<ITwitterResult<long[]>> GetUserIdsWhoseRetweetsAreMutedAsync(IGetUserIdsWhoseRetweetsAreMutedParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetUserIdsWhoseRetweetsAreMutedQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<long[]>(request);
        }

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetMutedUserIdsAsync(IGetMutedUserIdsParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetMutedUserIdsQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<IIdsCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IUserCursorQueryResultDTO>> GetMutedUsersAsync(IGetMutedUsersParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetMutedUsersQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequestAsync<IUserCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> MuteUserAsync(IMuteUserParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetMuteUserQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<IUserDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> UnmuteUserAsync(IUnmuteUserParameters parameters, ITwitterRequest request)
        {
            var query = _userQueryGenerator.GetUnmuteUserQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;
            return _twitterAccessor.ExecuteRequestAsync<IUserDTO>(request);
        }
    }
}