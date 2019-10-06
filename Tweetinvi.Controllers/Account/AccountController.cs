using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Account
{
    public class AccountController : IAccountController
    {
        private readonly IAccountQueryExecutor _accountQueryExecutor;
        private readonly IUserFactory _userFactory;
        private readonly IFactory<IAccountSettings> _accountSettingsUnityFactory;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly ITwitterResultFactory _twitterResultFactory;

        public AccountController(
            IAccountQueryExecutor accountQueryExecutor,
            IUserFactory userFactory,
            IFactory<IAccountSettings> accountSettingsUnityFactory,
            IJsonObjectConverter jsonObjectConverter,
            ITwitterResultFactory twitterResultFactory)
        {
            _accountQueryExecutor = accountQueryExecutor;
            _userFactory = userFactory;
            _accountSettingsUnityFactory = accountSettingsUnityFactory;
            _jsonObjectConverter = jsonObjectConverter;
            _twitterResultFactory = twitterResultFactory;
        }

        public async Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters, ITwitterRequest request)
        {
            var result = await _accountQueryExecutor.GetAuthenticatedUser(parameters, request).ConfigureAwait(false);
            return _twitterResultFactory.Create(result, userDTO => _userFactory.GenerateAuthenticatedUserFromDTO(userDTO));
        }

        // FOLLOW/UNFOLLOW
        public Task<ITwitterResult<IUserDTO>> FollowUser(IFollowUserParameters parameters, ITwitterRequest request)
        {
            return _accountQueryExecutor.FollowUser(parameters, request);
        }
        
        public Task<ITwitterResult<IRelationshipDetailsDTO>> UpdateRelationship(IUpdateRelationshipParameters parameters, ITwitterRequest request)
        {
            return _accountQueryExecutor.UpdateRelationship(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UnFollowUser(IUnFollowUserParameters parameters, ITwitterRequest request)
        {
            return _accountQueryExecutor.UnFollowUser(parameters, request);
        }
        
        // FRIENDSHIP
        
        public Task<ITwitterResult<IRelationshipStateDTO[]>> GetRelationshipsWith(IGetRelationshipsWithParameters parameters, ITwitterRequest request)
        {
            return _accountQueryExecutor.GetRelationshipsWith(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetUserIdsRequestingFriendshipParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _accountQueryExecutor.GetUserIdsRequestingFriendship(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }
        
        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsYouRequestedToFollow(IGetUserIdsYouRequestedToFollowParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetUserIdsYouRequestedToFollowParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _accountQueryExecutor.GetUserIdsYouRequestedToFollow(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        // BLOCK
        public Task<ITwitterResult<IUserDTO>> BlockUser(IBlockUserParameters parameters, ITwitterRequest request)
        {
            return _accountQueryExecutor.BlockUser(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters, ITwitterRequest request)
        {
            return _accountQueryExecutor.UnblockUser(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters, ITwitterRequest request)
        {
            return _accountQueryExecutor.ReportUserForSpam(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetBlockedUserIdsParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _accountQueryExecutor.GetBlockedUserIds(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public ITwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>> GetBlockedUsers(IGetBlockedUsersParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<IUserCursorQueryResultDTO>>(
                parameters.Cursor,
                cursor =>
                {
                    var cursoredParameters = new GetBlockedUsersParameters(parameters)
                    {
                        Cursor = cursor
                    };

                    return _accountQueryExecutor.GetBlockedUsers(cursoredParameters, new TwitterRequest(request));
                },
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }

        public Task<ITwitterResult<long[]>> GetUserIdsWhoseRetweetsAreMuted(IGetUserIdsWhoseRetweetsAreMutedParameters parameters, ITwitterRequest request)
        {
            return _accountQueryExecutor.GetUserIdsWhoseRetweetsAreMuted(parameters, request);
        }


        
        
        

       


        public IAccountSettings GenerateAccountSettingsFromJson(string json)
        {
            var accountSettingsDTO = _jsonObjectConverter.DeserializeObject<IAccountSettingsDTO>(json);

            if (accountSettingsDTO == null)
            {
                return null;
            }

            return GenerateAccountSettingsFromDTO(accountSettingsDTO);
        }

        private IAccountSettings GenerateAccountSettingsFromDTO(IAccountSettingsDTO accountSettingsDTO)
        {
            if (accountSettingsDTO == null)
            {
                return null;
            }

            var parameterOverride = _accountSettingsUnityFactory.GenerateParameterOverrideWrapper("accountSettingsDTO", accountSettingsDTO);
            return _accountSettingsUnityFactory.Create(parameterOverride);
        }

        // Mute
        public Task<IEnumerable<long>> GetMutedUserIds(int maxUserIds = Int32.MaxValue)
        {
            return _accountQueryExecutor.GetMutedUserIds(maxUserIds);
        }

        public async Task<IEnumerable<IUser>> GetMutedUsers(int maxUsersToRetrieve = 250)
        {
            var usersIds = await GetMutedUserIds(maxUsersToRetrieve);
            return await _userFactory.GetUsersFromIds(usersIds);
        }

        public Task<bool> MuteUser(IUserIdentifier user)
        {
            return _accountQueryExecutor.MuteUser(user);
        }

        public Task<bool> MuteUser(long userId)
        {
            return _accountQueryExecutor.MuteUser(new UserIdentifier(userId));
        }

        public Task<bool> MuteUser(string screenName)
        {
            return _accountQueryExecutor.MuteUser(new UserIdentifier(screenName));
        }

        public Task<bool> UnMuteUser(IUserIdentifier user)
        {
            return _accountQueryExecutor.UnMuteUser(user);
        }

        public Task<bool> UnMuteUser(long userId)
        {
            return _accountQueryExecutor.UnMuteUser(new UserIdentifier(userId));
        }

        public Task<bool> UnMuteUser(string screenName)
        {
            return _accountQueryExecutor.UnMuteUser(new UserIdentifier(screenName));
        }

        // Suggestions
        public Task<IEnumerable<ICategorySuggestion>> GetSuggestedCategories(Language? language)
        {
            return _accountQueryExecutor.GetSuggestedCategories(language);
        }

        public async Task<IEnumerable<IUser>> GetSuggestedUsers(string slug, Language? language)
        {
            var userDTOs = await _accountQueryExecutor.GetSuggestedUsers(slug, language);
            return _userFactory.GenerateUsersFromDTO(userDTOs, null);
        }

        public async Task<IEnumerable<IUser>> GetSuggestedUsersWithTheirLatestTweet(string slug)
        {
            var userDTOs = await _accountQueryExecutor.GetSuggestedUsersWithTheirLatestTweet(slug);
            return _userFactory.GenerateUsersFromDTO(userDTOs, null);
        }
    }
}