using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public static class Account
    {
        [ThreadStatic]
        private static IAccountController _accountController;

        /// <summary>
        /// Controller handling any Account request
        /// </summary>
        public static IAccountController AccountController
        {
            get
            {
                if (_accountController == null)
                {
                    Initialize();
                }

                return _accountController;
            }
        }

        private static readonly IFactory<IAccountSettingsRequestParameters> _accountSettingsRequestParametersFactory;

        static Account()
        {
            Initialize();

            _accountSettingsRequestParametersFactory = TweetinviContainer.Resolve<IFactory<IAccountSettingsRequestParameters>>();
        }

        private static void Initialize()
        {
            _accountController = TweetinviContainer.Resolve<IAccountController>();
        }

        // Settings

        /// <summary>
        /// Get the current account settings
        /// </summary>
        public static Task<IAccountSettings> GetCurrentAccountSettings()
        {
            return AccountController.GetAuthenticatedUserSettings();
        }

        /// <summary>
        /// Update the current account settings
        /// </summary>
        public static Task<IAccountSettings> UpdateAccountSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null)
        {
            return AccountController.UpdateAuthenticatedUserSettings(
                languages,
                timeZone,
                trendLocationWoeid,
                sleepTimeEnabled,
                startSleepTime,
                endSleepTime);
        }

        /// <summary>
        /// Update the current account settings
        /// </summary>
        public static Task<IAccountSettings> UpdateAccountSettings(IAccountSettingsRequestParameters settings)
        {
            return AccountController.UpdateAuthenticatedUserSettings(settings);
        }

        /// <summary>
        /// Create an object that contains all the Twitter configuration settings
        /// </summary>
        public static IAccountSettingsRequestParameters CreateUpdateAccountSettingsRequestParameters()
        {
            return _accountSettingsRequestParametersFactory.Create();
        }

        /// <summary>
        /// Create an object that contains all the Twitter configuration settings.
        /// This object is initialized based on the parameter.
        /// </summary>
        public static IAccountSettingsRequestParameters CreateUpdateAccountSettingsRequestParameters(IAccountSettings accountSettings)
        {
            var accountSettingsParameter = _accountSettingsRequestParametersFactory.Create();

            accountSettingsParameter.Languages.Add(accountSettings.Language);

            accountSettingsParameter.SleepTimeEnabled = accountSettings.SleepTimeEnabled;
            accountSettingsParameter.StartSleepTime = accountSettings.SleepTimeStartHour;
            accountSettingsParameter.EndSleepTime = accountSettings.SleepTimeEndHour;

            if (accountSettings.TimeZone != null)
            {
                accountSettingsParameter.TimeZone = accountSettings.TimeZone.TzinfoName;
            }

            return accountSettingsParameter;
        }

        // Profile

        /// <summary>
        /// Update the information of the authenticated user profile.
        /// </summary>
        public static Task<IAuthenticatedUser> UpdateAccountProfile(IAccountUpdateProfileParameters parameters)
        {
            return AccountController.UpdateAccountProfile(parameters);
        }

        /// <summary>
        /// Removes the uploaded profile banner for the authenticated user.
        /// </summary>
        public static Task<bool> RemoveUserProfileBanner()
        {
            return AccountController.RemoveUserProfileBanner();
        }

        /// <summary>
        /// Updates the authenticated user’s profile background image. 
        /// </summary>
        public static Task<bool> UpdateProfileBackgroundImage(byte[] imageBinary)
        {
            return AccountController.UpdateProfileBackgroundImage(imageBinary);
        }

        /// <summary>
        /// Updates the authenticated user’s profile background image. 
        /// </summary>
        public static Task<bool> UpdateProfileBackgroundImage(long mediaId)
        {
            return AccountController.UpdateProfileBackgroundImage(mediaId);
        }

        /// <summary>
        /// Updates the authenticated user’s profile background image. 
        /// </summary>
        public static Task<bool> UpdateProfileBackgroundImage(IAccountUpdateProfileBackgroundImageParameters parameters)
        {
            return AccountController.UpdateProfileBackgroundImage(parameters);
        }

        // Mute

        /// <summary>
        /// Get the muted user's ids of the current account.
        /// </summary>
        public static Task<IEnumerable<long>> GetMutedUserIds(int maxNumberOfUserIdsToRetrieve = Int32.MaxValue)
        {
            return AccountController.GetMutedUserIds(maxNumberOfUserIdsToRetrieve);
        }

        /// <summary>
        /// Get the muted users of the current account.
        /// </summary>
        public static Task<IEnumerable<IUser>> GetMutedUsers(int maxNumberOfUsersToRetrieve = 250)
        {
            return AccountController.GetMutedUsers(maxNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        public static Task<bool> MuteUser(IUserIdentifier user)
        {
            return AccountController.MuteUser(user);
        }

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        public static Task<bool> MuteUser(long userId)
        {
            return AccountController.MuteUser(userId);
        }

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        public static Task<bool> MuteUser(string screenName)
        {
            return AccountController.MuteUser(screenName);
        }

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        public static Task<bool> UnMuteUser(IUserIdentifier user)
        {
            return AccountController.UnMuteUser(user);
        }

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        public static Task<bool> UnMuteUser(long userId)
        {
            return AccountController.UnMuteUser(userId);
        }

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        public static Task<bool> UnMuteUser(string screenName)
        {
            return AccountController.UnMuteUser(screenName);
        }

        /// <summary>
        /// Get a list of categories that can interest the user of the current account.
        /// </summary>
        public static Task<IEnumerable<ICategorySuggestion>> GetSuggestedCategories(Language? language = null)
        {
            return AccountController.GetSuggestedCategories(language);
        }

        /// <summary>
        /// Get a list of categories that can interest the user of the current account.
        /// </summary>
        public static Task<IEnumerable<IUser>> GetSuggestedUsers(string filter, Language? language = null)
        {
            return AccountController.GetSuggestedUsers(filter, language);
        }

        /// <summary>
        /// Get a list of categories that can interest the user of the current account.
        /// </summary>
        public static Task<IEnumerable<IUser>> GetSuggestedUsersWithTheirLatestTweet(string filter)
        {
            return AccountController.GetSuggestedUsersWithTheirLatestTweet(filter);
        }
    }
}