using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
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
        private readonly IFactory<IAccountSettingsRequestParameters> _accountSettingsRequestParametersFactory;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly ITwitterResultFactory _twitterResultFactory;
        private readonly IUserQueryValidator _userQueryValidator;

        public AccountController(
            IAccountQueryExecutor accountQueryExecutor,
            IUserFactory userFactory,
            IFactory<IAccountSettings> accountSettingsUnityFactory,
            IFactory<IAccountSettingsRequestParameters> accountSettingsRequestParametersFactory,
            IJsonObjectConverter jsonObjectConverter,
            ITwitterResultFactory twitterResultFactory, 
            IUserQueryValidator userQueryValidator)
        {
            _accountQueryExecutor = accountQueryExecutor;
            _userFactory = userFactory;
            _accountSettingsUnityFactory = accountSettingsUnityFactory;
            _accountSettingsRequestParametersFactory = accountSettingsRequestParametersFactory;
            _jsonObjectConverter = jsonObjectConverter;
            _twitterResultFactory = twitterResultFactory;
            _userQueryValidator = userQueryValidator;
        }

        public async Task<ITwitterResult<IUserDTO, IAuthenticatedUser>> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters, ITwitterRequest request)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException($"{nameof(parameters)}");
            }

            var result = await _accountQueryExecutor.GetAuthenticatedUser(parameters, request).ConfigureAwait(false);
            return _twitterResultFactory.Create(result, userDTO => _userFactory.GenerateAuthenticatedUserFromDTO(userDTO));
        }

        // FOLLOW/UNFOLLOW
        public Task<ITwitterResult<IUserDTO>> FollowUser(IFollowUserParameters parameters, ITwitterRequest request)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters?.User, $"${nameof(parameters)}.{nameof(parameters.User)}");
            return _accountQueryExecutor.FollowUser(parameters, request);
        }
        
        public Task<ITwitterResult<IRelationshipDetailsDTO>> UpdateRelationship(IUpdateRelationshipParameters parameters, ITwitterRequest request)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters?.User, $"${nameof(parameters)}.{nameof(parameters.User)}");
            return _accountQueryExecutor.UpdateRelationship(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UnFollowUser(IUnFollowUserParameters parameters, ITwitterRequest request)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters?.User, $"${nameof(parameters)}.{nameof(parameters.User)}");
            return _accountQueryExecutor.UnFollowUser(parameters, request);
        }
        
        // FRIENDSHIP
        
        public Task<ITwitterResult<IRelationshipStateDTO[]>> GetRelationshipsWith(IGetRelationshipsWithParameters parameters, ITwitterRequest request)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            var limits = request.ExecutionContext.Limits;
            var maxUsers = limits.ACCOUNT_GET_RELATIONSHIPS_WITH_MAX_SIZE;
            if (parameters.Users.Length > maxUsers)
            {
                throw new TwitterArgumentLimitException($"${nameof(parameters)}.{nameof(parameters.Users)}", maxUsers, nameof(limits.ACCOUNT_GET_RELATIONSHIPS_WITH_MAX_SIZE), "users");
            }

            return _accountQueryExecutor.GetRelationshipsWith(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters, ITwitterRequest request)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException($"{nameof(parameters)}");
            }
            
            var limits = request.ExecutionContext.Limits;
            var maxPageSize = limits.ACCOUNT_GET_USER_IDS_REQUESTING_FRIENDSHIP_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"${nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(limits.ACCOUNT_GET_USER_IDS_REQUESTING_FRIENDSHIP_MAX_PAGE_SIZE), "page size");
            }

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
            if (parameters == null)
            {
                throw new ArgumentNullException($"{nameof(parameters)}");
            }
            
            var limits = request.ExecutionContext.Limits;
            var maxPageSize = limits.ACCOUNT_GET_REQUESTED_USER_IDS_TO_FOLLOW_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"${nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(limits.ACCOUNT_GET_REQUESTED_USER_IDS_TO_FOLLOW_MAX_PAGE_SIZE), "page size");
            }

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
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User, $"${nameof(parameters)}.{nameof(parameters.User)}");
            return _accountQueryExecutor.BlockUser(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> UnblockUser(IUnblockUserParameters parameters, ITwitterRequest request)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User, $"${nameof(parameters)}.{nameof(parameters.User)}");
            return _accountQueryExecutor.UnblockUser(parameters, request);
        }

        public Task<ITwitterResult<IUserDTO>> ReportUserForSpam(IReportUserForSpamParameters parameters, ITwitterRequest request)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User, $"${nameof(parameters)}.{nameof(parameters.User)}");
            return _accountQueryExecutor.ReportUserForSpam(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters, ITwitterRequest request)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            var limits = request.ExecutionContext.Limits;
            var maxPageSize = limits.ACCOUNT_GET_BLOCKED_USER_IDS_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"${nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(limits.ACCOUNT_GET_BLOCKED_USER_IDS_MAX_PAGE_SIZE), "page size");
            }
            
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
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            var limits = request.ExecutionContext.Limits;
            var maxPageSize = limits.ACCOUNT_GET_BLOCKED_USER_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"${nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(limits.ACCOUNT_GET_BLOCKED_USER_MAX_PAGE_SIZE), "page size");
            }
            
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


        
        
        

        public async Task<IAccountSettings> GetAuthenticatedUserSettings()
        {
            var accountSettingsDTO = await _accountQueryExecutor.GetAuthenticatedUserAccountSettings().ConfigureAwait(false);
            return GenerateAccountSettingsFromDTO(accountSettingsDTO);
        }

        public Task<IAccountSettings> UpdateAuthenticatedUserSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null)
        {
            var settings = _accountSettingsRequestParametersFactory.Create();

            settings.Languages = new List<Language>(languages ?? Enumerable.Empty<Language>());
            settings.TimeZone = timeZone;
            settings.TrendLocationWoeid = trendLocationWoeid;
            settings.SleepTimeEnabled = sleepTimeEnabled;
            settings.StartSleepTime = startSleepTime;
            settings.EndSleepTime = endSleepTime;

            return UpdateAuthenticatedUserSettings(settings);
        }

        public async Task<IAccountSettings> UpdateAuthenticatedUserSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters)
        {
            var accountSettingsDTO = await _accountQueryExecutor.UpdateAuthenticatedUserSettings(accountSettingsRequestParameters).ConfigureAwait(false);
            return GenerateAccountSettingsFromDTO(accountSettingsDTO);
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

        // Profile
        public async Task<IAuthenticatedUser> UpdateAccountProfile(IAccountUpdateProfileParameters parameters)
        {
            var userDTO = await _accountQueryExecutor.UpdateProfileParameters(parameters);
            return _userFactory.GenerateAuthenticatedUserFromDTO(userDTO);
        }

        public Task<bool> RemoveUserProfileBanner()
        {
            return _accountQueryExecutor.RemoveUserProfileBanner();
        }

        public Task<bool> UpdateProfileBackgroundImage(byte[] imageBinary)
        {
            return UpdateProfileBackgroundImage(new AccountUpdateProfileBackgroundImageParameters(imageBinary));
        }

        public Task<bool> UpdateProfileBackgroundImage(long mediaId)
        {
            return UpdateProfileBackgroundImage(new AccountUpdateProfileBackgroundImageParameters(mediaId));
        }

        public Task<bool> UpdateProfileBackgroundImage(IAccountUpdateProfileBackgroundImageParameters parameters)
        {
            return _accountQueryExecutor.UpdateProfileBackgroundImage(parameters);
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