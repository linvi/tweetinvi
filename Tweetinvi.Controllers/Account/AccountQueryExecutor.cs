using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Account
{
    public interface IAccountQueryExecutor
    {
        Task<ITwitterResult<IUserDTO>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters, ITwitterRequest request);

        // BLOCK
        Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserCursorQueryResultDTO>> GetBlockedUsers(IGetBlockedUsersParameters parameters, ITwitterRequest request);

        // FOLLOWERS
        Task<ITwitterResult<IUserDTO>> FollowUser(IFollowUserParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IUserDTO>> UnFollowUser(IUnFollowUserParameters parameters, ITwitterRequest request);
        
        // ONGOING REQUESTS
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsYouRequestedToFollow(IGetUserIdsYouRequestedToFollowParameters parameters, ITwitterRequest request);
        
        // FRIENDSHIPS
        Task<ITwitterResult<IRelationshipStateDTO[]>> GetRelationshipsWith(IGetRelationshipsWithParameters parameters, ITwitterRequest request);
        
        // SETTINGS
        








        // Profile
        Task<IUserDTO> UpdateProfileParameters(IAccountUpdateProfileParameters parameters);
        // Mute
        Task<IEnumerable<long>> GetMutedUserIds(int maxUserIds = int.MaxValue);

        Task<bool> MuteUser(IUserIdentifier user);
        Task<bool> UnMuteUser(IUserIdentifier user);

        // Suggestions
        Task<IEnumerable<IUserDTO>> GetSuggestedUsers(string slug, Language? language);
        Task<IEnumerable<ICategorySuggestion>> GetSuggestedCategories(Language? language);
        Task<IEnumerable<IUserDTO>> GetSuggestedUsersWithTheirLatestTweet(string slug);
        Task<ITwitterResult<IRelationshipDetailsDTO>> UpdateRelationship(IUpdateRelationshipParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<long[]>> GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters parameters, ITwitterRequest request);
    }

    public class AccountQueryExecutor : IAccountQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IAccountQueryGenerator _accountQueryGenerator;

        public AccountQueryExecutor(
            ITwitterAccessor twitterAccessor,
            IAccountQueryGenerator accountQueryGenerator)
        {
            _twitterAccessor = twitterAccessor;
            _accountQueryGenerator = accountQueryGenerator;
        }

        public Task<ITwitterResult<IUserDTO>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters, ITwitterRequest request)
        {
            var query = _accountQueryGenerator.GetAuthenticatedUserQuery(parameters, request.ExecutionContext.TweetMode);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IUserDTO>(request);
        }

        // BLOCK
        public Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters, ITwitterRequest request)
        {
            var query = _accountQueryGenerator.GetBlockUserQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequest<IUserDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters, ITwitterRequest request)
        {
            var query = _accountQueryGenerator.GetUnblockUserQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequest<IUserDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters, ITwitterRequest request)
        {
            var query = _accountQueryGenerator.GetReportUserForSpamQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequest<IUserDTO>(request);
        }

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters, ITwitterRequest request)
        {
            var query = _accountQueryGenerator.GetBlockedUserIdsQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IIdsCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IUserCursorQueryResultDTO>> GetBlockedUsers(IGetBlockedUsersParameters parameters, ITwitterRequest request)
        {
            var query = _accountQueryGenerator.GetBlockedUsersQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IUserCursorQueryResultDTO>(request);
        }

        // FOLLOWERS
        public Task<ITwitterResult<IUserDTO>> FollowUser(IFollowUserParameters parameters, ITwitterRequest request)
        {
            var query = _accountQueryGenerator.GetFollowUserQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequest<IUserDTO>(request);
        }
        
        public Task<ITwitterResult<IRelationshipDetailsDTO>> UpdateRelationship(IUpdateRelationshipParameters parameters, ITwitterRequest request)
        {
            var query = _accountQueryGenerator.GetUpdateRelationshipQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequest<IRelationshipDetailsDTO>(request);
        }

        public Task<ITwitterResult<IUserDTO>> UnFollowUser(IUnFollowUserParameters parameters, ITwitterRequest request)
        {
            var query = _accountQueryGenerator.GetUnFollowUserQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.POST;

            return _twitterAccessor.ExecuteRequest<IUserDTO>(request);
        }

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters, ITwitterRequest request)
        {
            var query = _accountQueryGenerator.GetUserIdsRequestingFriendshipQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IIdsCursorQueryResultDTO>(request);
        }

        public Task<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsYouRequestedToFollow(IGetUserIdsYouRequestedToFollowParameters parameters, ITwitterRequest request)
        {
            var query = _accountQueryGenerator.GetUserIdsYouRequestedToFollowQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IIdsCursorQueryResultDTO>(request);
        }

        // FRIENDSHIPS
        public Task<ITwitterResult<IRelationshipStateDTO[]>> GetRelationshipsWith(IGetRelationshipsWithParameters parameters, ITwitterRequest request)
        {
            var query = _accountQueryGenerator.GetRelationshipsWithQuery(parameters);

            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;

            return _twitterAccessor.ExecuteRequest<IRelationshipStateDTO[]>(request);
        }

        public Task<ITwitterResult<long[]>> GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters parameters, ITwitterRequest request)
        {
            var query = _accountQueryGenerator.GetUserIdsWhoseRetweetsAreMutedQuery(parameters);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<long[]>(request);
        }

       








        

        public Task<IUserDTO> UpdateProfileParameters(IAccountUpdateProfileParameters parameters)
        {
            var query = _accountQueryGenerator.GetUpdateProfileParametersQuery(parameters);
            return _twitterAccessor.ExecutePOSTQuery<IUserDTO>(query);
        }

        // Mute
        public Task<IEnumerable<long>> GetMutedUserIds(int maxUserIds = Int32.MaxValue)
        {
            string query = _accountQueryGenerator.GetMutedUserIdsQuery();
            return _twitterAccessor.ExecuteCursorGETQuery<long, IIdsCursorQueryResultDTO>(query, maxUserIds);
        }

        public async Task<bool> MuteUser(IUserIdentifier user)
        {
            var query = _accountQueryGenerator.GetMuteQuery(user);
            var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery(query);

            return asyncOperation.Success;
        }

        public async Task<bool> UnMuteUser(IUserIdentifier user)
        {
            var query = _accountQueryGenerator.GetUnMuteQuery(user);
            var asyncOperation = await _twitterAccessor.TryExecutePOSTQuery(query);

            return asyncOperation.Success;
        }

        // Suggestions
        public Task<IEnumerable<ICategorySuggestion>> GetSuggestedCategories(Language? language)
        {
            var query = _accountQueryGenerator.GetSuggestedCategories(language);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ICategorySuggestion>>(query);
        }

        public Task<IEnumerable<IUserDTO>> GetSuggestedUsers(string slug, Language? language)
        {
            var query = _accountQueryGenerator.GetUserSuggestionsQuery(slug, language);
            return _twitterAccessor.ExecuteGETQueryWithPath<IEnumerable<IUserDTO>>(query, "users");
        }

        public Task<IEnumerable<IUserDTO>> GetSuggestedUsersWithTheirLatestTweet(string slug)
        {
            var query = _accountQueryGenerator.GetSuggestedUsersWithTheirLatestTweetQuery(slug);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<IUserDTO>>(query);
        }

    }
}