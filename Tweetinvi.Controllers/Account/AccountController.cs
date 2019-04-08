using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
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

        public AccountController(
            IAccountQueryExecutor accountQueryExecutor,
            IUserFactory userFactory,
            IFactory<IAccountSettings> accountSettingsUnityFactory,
            IFactory<IAccountSettingsRequestParameters> accountSettingsRequestParametersFactory,
            IJsonObjectConverter jsonObjectConverter)
        {
            _accountQueryExecutor = accountQueryExecutor;
            _userFactory = userFactory;
            _accountSettingsUnityFactory = accountSettingsUnityFactory;
            _accountSettingsRequestParametersFactory = accountSettingsRequestParametersFactory;
            _jsonObjectConverter = jsonObjectConverter;
        }

        public async Task<IAccountSettings> GetAuthenticatedUserSettings()
        {
            var accountSettingsDTO = await _accountQueryExecutor.GetAuthenticatedUserAccountSettings();
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
            var accountSettingsDTO = await _accountQueryExecutor.UpdateAuthenticatedUserSettings(accountSettingsRequestParameters);
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

        public Task<bool> UpdateProfileImage(byte[] imageBinary)
        {
            return UpdateProfileImage(new AccountUpdateProfileImageParameters(imageBinary));
        }

        public Task<bool> UpdateProfileImage(IAccountUpdateProfileImageParameters parameters)
        {
            return _accountQueryExecutor.UpdateProfileImage(parameters);
        }

        public Task<bool> UpdateProfileBanner(byte[] imageBinary)
        {
            return UpdateProfileBanner(new AccountUpdateProfileBannerParameters(imageBinary));
        }

        public Task<bool> UpdateProfileBanner(IAccountUpdateProfileBannerParameters parameters)
        {
            return _accountQueryExecutor.UpdateProfileBanner(parameters);
        }

        public Task<bool> RemoveUserProfileBanner()
        {
            return _accountQueryExecutor.RemoveUserProfileBanner();
        }

        public bool UpdateProfileBackgroundImage(byte[] imageBinary)
        {
            return UpdateProfileBackgroundImage(new AccountUpdateProfileBackgroundImageParameters(imageBinary));
        }

        public bool UpdateProfileBackgroundImage(long mediaId)
        {
            return UpdateProfileBackgroundImage(new AccountUpdateProfileBackgroundImageParameters(mediaId));
        }

        public bool UpdateProfileBackgroundImage(IAccountUpdateProfileBackgroundImageParameters parameters)
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

        public bool UnMuteUser(IUserIdentifier user)
        {
            return _accountQueryExecutor.UnMuteUser(user);
        }

        public bool UnMuteUser(long userId)
        {
            return _accountQueryExecutor.UnMuteUser(new UserIdentifier(userId));
        }

        public bool UnMuteUser(string screenName)
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
            return _userFactory.GenerateUsersFromDTO(userDTOs);
        }

        public async Task<IEnumerable<IUser>> GetSuggestedUsersWithTheirLatestTweet(string slug)
        {
            var userDTOs = await _accountQueryExecutor.GetSuggestedUsersWithTheirLatestTweet(slug);
            return _userFactory.GenerateUsersFromDTO(userDTOs);
        }
    }
}