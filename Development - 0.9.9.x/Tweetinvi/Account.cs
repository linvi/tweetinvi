using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi
{
    public static class Account
    {
        [ThreadStatic]
        private static IAccountController _accountController;
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

        [ThreadStatic] 
        private static IFriendshipController _friendshipController;
        private static IFriendshipController FriendshipController
        {
            get
            {
                if (_friendshipController == null)
                {
                    Initialize();
                }

                return _friendshipController;
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
            _friendshipController = TweetinviContainer.Resolve<IFriendshipController>();
        }
        
        // Settings
        public static IAccountSettings GetCurrentAccountSettings()
        {
            return AccountController.GetLoggedUserSettings();
        }

        public static IAccountSettings UpdateAccountSettings(
            IEnumerable<Language> languages = null,
            string timeZone = null,
            long? trendLocationWoeid = null,
            bool? sleepTimeEnabled = null,
            int? startSleepTime = null,
            int? endSleepTime = null)
        {
            return _accountController.UpdateLoggedUserSettings(
                languages,
                timeZone,
                trendLocationWoeid,
                sleepTimeEnabled,
                startSleepTime,
                endSleepTime);
        }

        public static IAccountSettings UpdateAccountSettings(IAccountSettingsRequestParameters settings)
        {
            return _accountController.UpdateLoggedUserSettings(settings);
        }

        public static IAccountSettingsRequestParameters CreateUpdateAccountSettingsRequestParameters()
        {
            return _accountSettingsRequestParametersFactory.Create();
        }

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

            if (accountSettings.TrendLocations != null && accountSettings.TrendLocations.Count() == 1)
            {
                accountSettingsParameter.TrendLocationWoeid = accountSettings.TrendLocations.Single().WoeId;
            }

            return accountSettingsParameter;
        }

        // Mute
        public static IEnumerable<long> GetMutedUserIds(int maxNumberOfUserIdsToRetrieve = Int32.MaxValue)
        {
            return AccountController.GetMutedUserIds(maxNumberOfUserIdsToRetrieve);
        }

        public static IEnumerable<IUser> GetMutedUsers(int maxNumberOfUsersToRetrieve = 250)
        {
            return AccountController.GetMutedUsers(maxNumberOfUsersToRetrieve);
        }

        public static bool MuteUser(IUserIdentifier userIdentifier)
        {
            return AccountController.MuteUser(userIdentifier);
        }

        public static bool MuteUser(long userId)
        {
            return AccountController.MuteUser(userId);
        }

        public static bool MuteUser(string screenName)
        {
            return AccountController.MuteUser(screenName);
        }

        public static bool UnMuteUser(IUserIdentifier userIdentifier)
        {
            return AccountController.UnMuteUser(userIdentifier);
        }

        public static bool UnMuteUser(long userId)
        {
            return AccountController.UnMuteUser(userId);
        }

        public static bool UnMuteUser(string screenName)
        {
            return AccountController.UnMuteUser(screenName);
        }

        #region Friendship

        public static IEnumerable<long> GetUserIdsRequestingFriendship(int maximumUserIdsToRetrieve = 75000)
        {
            return _friendshipController.GetUserIdsRequestingFriendship(maximumUserIdsToRetrieve);
        }

        public static IEnumerable<IUser> GetUsersRequestingFriendship(int maximumUserIdsToRetrieve = 75000)
        {
            return _friendshipController.GetUsersRequestingFriendship(maximumUserIdsToRetrieve);
        }

        public static IEnumerable<long> GetUserIdsYouRequestedToFollow(int maximumUserIdsToRetrieve = 75000)
        {
            return _friendshipController.GetUserIdsYouRequestedToFollow(maximumUserIdsToRetrieve);
        }

        public static IEnumerable<IUser> GetUsersYouRequestedToFollow(int maximumUserIdsToRetrieve = 75000)
        {
            return _friendshipController.GetUsersYouRequestedToFollow(maximumUserIdsToRetrieve);
        }

        // Update Relationship Authorization With
        public static bool UpdateRelationshipAuthorizationsWith(IUserIdentifier userIdentifier, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return _friendshipController.UpdateRelationshipAuthorizationsWith(userIdentifier, retweetsEnabled, deviceNotifictionEnabled);
        }

        public static bool UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return _friendshipController.UpdateRelationshipAuthorizationsWith(userId, retweetsEnabled, deviceNotifictionEnabled);
        }

        public static bool UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return _friendshipController.UpdateRelationshipAuthorizationsWith(userScreenName, retweetsEnabled, deviceNotifictionEnabled);
        }


        // Lookup Relationships
        public static IEnumerable<IRelationshipState> GetMultipleRelationships(IEnumerable<IUserIdentifier> targetUserIdentifiers)
        {
            return _friendshipController.GetMultipleRelationships(targetUserIdentifiers);
        }

        public static IEnumerable<IRelationshipState> GetMultipleRelationships(IEnumerable<long> targetUserIds)
        {
            return _friendshipController.GetMultipleRelationships(targetUserIds);
        }

        public static IEnumerable<IRelationshipState> GetMultipleRelationships(IEnumerable<string> targetUserScreenNames)
        {
            return _friendshipController.GetMultipleRelationships(targetUserScreenNames);
        }


        // Get User Ids Whose Retweets Are Muted
        public static IEnumerable<long> GetUserIdsWhoseRetweetsAreMuted()
        {
            return _friendshipController.GetUserIdsWhoseRetweetsAreMuted();
        }

        public static IEnumerable<IUser> GetUsersWhoseRetweetsAreMuted()
        {
            return _friendshipController.GetUsersWhoseRetweetsAreMuted();
        }

        #endregion

        public static IEnumerable<ICategorySuggestion> GetSuggestedCategories(Language? language = null)
        {
            return _accountController.GetSuggestedCategories(language);
        }

        public static IEnumerable<IUser> GetSuggestedUsers(string filter, Language? language = null)
        {
            return _accountController.GetSuggestedUsers(filter, language);
        }

        public static IEnumerable<IUser> GetSuggestedUsersWithTheirLatestTweet(string filter)
        {
            return _accountController.GetSuggestedUsersWithTheirLatestTweet(filter);
        }
    }
}