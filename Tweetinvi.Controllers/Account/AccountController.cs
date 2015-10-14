using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Account
{
    public class AccountController : IAccountController
    {
        private readonly IAccountQueryExecutor _accountQueryExecutor;
        private readonly IUserFactory _userFactory;
        private readonly IFactory<IAccountSettings> _accountSettingsUnityFactory;
        private readonly IFactory<IAccountSettingsRequestParameters> _accountSettingsRequestParametersFactory;

        public AccountController(
            IAccountQueryExecutor accountQueryExecutor,
            IUserFactory userFactory,
            IFactory<IAccountSettings> accountSettingsUnityFactory,
            IFactory<IAccountSettingsRequestParameters> accountSettingsRequestParametersFactory)
        {
            _accountQueryExecutor = accountQueryExecutor;
            _userFactory = userFactory;
            _accountSettingsUnityFactory = accountSettingsUnityFactory;
            _accountSettingsRequestParametersFactory = accountSettingsRequestParametersFactory;
        }

        public IAccountSettings GetLoggedUserSettings()
        {
            var accountSettingsDTO = _accountQueryExecutor.GetLoggedUserAccountSettings();
            return GenerateAccountSettingsFromDTO(accountSettingsDTO);
        }

        public IAccountSettings UpdateLoggedUserSettings(
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

            return UpdateLoggedUserSettings(settings);
        }

        public IAccountSettings UpdateLoggedUserSettings(IAccountSettingsRequestParameters accountSettingsRequestParameters)
        {
            var accountSettingsDTO = _accountQueryExecutor.UpdateLoggedUserSettings(accountSettingsRequestParameters);
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
        public IEnumerable<long> GetMutedUserIds(int maxUserIds = Int32.MaxValue)
        {
            return _accountQueryExecutor.GetMutedUserIds(maxUserIds);
        }

        public IEnumerable<IUser> GetMutedUsers(int maxUsersToRetrieve = 250)
        {
            var usersIds = GetMutedUserIds(maxUsersToRetrieve);
            return _userFactory.GetUsersFromIds(usersIds);
        }

        public bool MuteUser(IUserIdentifier userIdentifier)
        {
            return _accountQueryExecutor.MuteUser(userIdentifier);
        }

        public bool MuteUser(long userId)
        {
            return _accountQueryExecutor.MuteUser(userId);
        }

        public bool MuteUser(string screenName)
        {
            return _accountQueryExecutor.MuteUser(screenName);
        }

        public bool UnMuteUser(IUserIdentifier userIdentifier)
        {
            return _accountQueryExecutor.UnMuteUser(userIdentifier);
        }

        public bool UnMuteUser(long userId)
        {
            return _accountQueryExecutor.UnMuteUser(userId);
        }

        public bool UnMuteUser(string screenName)
        {
            return _accountQueryExecutor.UnMuteUser(screenName);
        }

        // Suggestions
        public IEnumerable<ICategorySuggestion> GetSuggestedCategories(Language? language)
        {
            return _accountQueryExecutor.GetSuggestedCategories(language);
        }

        public IEnumerable<IUser> GetSuggestedUsers(string slug, Language? language)
        {
            var userDTOs = _accountQueryExecutor.GetSuggestedUsers(slug, language);
            return _userFactory.GenerateUsersFromDTO(userDTOs);
        }

        public IEnumerable<IUser> GetSuggestedUsersWithTheirLatestTweet(string slug)
        {
            var userDTOs = _accountQueryExecutor.GetSuggestedUsersWithTheirLatestTweet(slug);
            return _userFactory.GenerateUsersFromDTO(userDTOs);
        }
    }
}