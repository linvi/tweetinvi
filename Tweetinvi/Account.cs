using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

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

        /// <summary>
        /// Get the current account settings
        /// </summary>
        public static IAccountSettings GetCurrentAccountSettings()
        {
            return AccountController.GetAuthenticatedUserSettings();
        }

        /// <summary>
        /// Update the current account settings
        /// </summary>
        public static IAccountSettings UpdateAccountSettings(
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
        public static IAccountSettings UpdateAccountSettings(IAccountSettingsRequestParameters settings)
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

            if (accountSettings.TrendLocations != null && accountSettings.TrendLocations.Count() == 1)
            {
                accountSettingsParameter.TrendLocationWoeid = accountSettings.TrendLocations.Single().WoeId;
            }

            return accountSettingsParameter;
        }

        // Profile

        /// <summary>
        /// Update the information of the authenticated user profile.
        /// </summary>
        public static IAuthenticatedUser UpdateAccountProfile(IAccountUpdateProfileParameters parameters)
        {
            return AccountController.UpdateAccountProfile(parameters);
        }

        /// <summary>
        /// Updates the authenticated user’s profile image. 
        /// </summary>
        /// <param name="imageBinary">Must be a valid GIF, JPG, or PNG image of less than 800 kilobytes in size. Images with width larger than 2048 pixels will be forcibly scaled down.</param>
        public static bool UpdateProfileImage(byte[] imageBinary)
        {
            return AccountController.UpdateProfileImage(imageBinary);
        }

        /// <summary>
        /// Updates the authenticated user’s profile image. 
        /// </summary>
        /// <param name="parameters">The parameters must contain a valid GIF, JPG, or PNG image of less than 800 kilobytes in size. Images with width larger than 2048 pixels will be forcibly scaled down.</param>
        public static bool UpdateProfileImage(IAccountUpdateProfileImageParameters parameters)
        {
            return AccountController.UpdateProfileImage(parameters);
        }

        /// <summary>
        /// Uploads a profile banner on behalf of the authenticated user.
        /// </summary>
        public static bool UpdateProfileBanner(byte[] imageBinary)
        {
            return AccountController.UpdateProfileBanner(imageBinary);
        }

        /// <summary>
        /// Uploads a profile banner on behalf of the authenticated user.
        /// </summary>
        public static bool UpdateProfileBanner(IAccountUpdateProfileBannerParameters parameters)
        {
            return AccountController.UpdateProfileBanner(parameters);
        }

        /// <summary>
        /// Removes the uploaded profile banner for the authenticated user.
        /// </summary>
        public static bool RemoveUserProfileBanner()
        {
            return AccountController.RemoveUserProfileBanner();
        }

        /// <summary>
        /// Updates the authenticated user’s profile background image. 
        /// </summary>
        public static bool UpdateProfileBackgroundImage(byte[] imageBinary)
        {
            return AccountController.UpdateProfileBackgroundImage(imageBinary);
        }

        /// <summary>
        /// Updates the authenticated user’s profile background image. 
        /// </summary>
        public static bool UpdateProfileBackgroundImage(long mediaId)
        {
            return AccountController.UpdateProfileBackgroundImage(mediaId);
        }

        /// <summary>
        /// Updates the authenticated user’s profile background image. 
        /// </summary>
        public static bool UpdateProfileBackgroundImage(IAccountUpdateProfileBackgroundImageParameters parameters)
        {
            return AccountController.UpdateProfileBackgroundImage(parameters);
        }

        // Mute

        /// <summary>
        /// Get the muted user's ids of the current account.
        /// </summary>
        public static IEnumerable<long> GetMutedUserIds(int maxNumberOfUserIdsToRetrieve = Int32.MaxValue)
        {
            return AccountController.GetMutedUserIds(maxNumberOfUserIdsToRetrieve);
        }

        /// <summary>
        /// Get the muted users of the current account.
        /// </summary>
        public static IEnumerable<IUser> GetMutedUsers(int maxNumberOfUsersToRetrieve = 250)
        {
            return AccountController.GetMutedUsers(maxNumberOfUsersToRetrieve);
        }

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        public static bool MuteUser(IUserIdentifier userIdentifier)
        {
            return AccountController.MuteUser(userIdentifier);
        }

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        public static bool MuteUser(long userId)
        {
            return AccountController.MuteUser(userId);
        }

        /// <summary>
        /// Mute a specific user.
        /// </summary>
        public static bool MuteUser(string screenName)
        {
            return AccountController.MuteUser(screenName);
        }

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        public static bool UnMuteUser(IUserIdentifier userIdentifier)
        {
            return AccountController.UnMuteUser(userIdentifier);
        }

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        public static bool UnMuteUser(long userId)
        {
            return AccountController.UnMuteUser(userId);
        }

        /// <summary>
        /// Unmute a specific user.
        /// </summary>
        public static bool UnMuteUser(string screenName)
        {
            return AccountController.UnMuteUser(screenName);
        }

        #region Friendship

        /// <summary>
        /// Get the ids of the users who want to follow you.
        /// </summary>
        public static IEnumerable<long> GetUserIdsRequestingFriendship(int maximumUserIdsToRetrieve = 75000)
        {
            return FriendshipController.GetUserIdsRequestingFriendship(maximumUserIdsToRetrieve);
        }

        /// <summary>
        /// Get the users who want to follow you.
        /// </summary>
        public static IEnumerable<IUser> GetUsersRequestingFriendship(int maximumUserIdsToRetrieve = 75000)
        {
            return FriendshipController.GetUsersRequestingFriendship(maximumUserIdsToRetrieve);
        }

        /// <summary>
        /// Get the user ids of the people you requested to follow.
        /// </summary>
        public static IEnumerable<long> GetUserIdsYouRequestedToFollow(int maximumUserIdsToRetrieve = 75000)
        {
            return FriendshipController.GetUserIdsYouRequestedToFollow(maximumUserIdsToRetrieve);
        }

        /// <summary>
        /// Get the user ids of the people you requested to follow.
        /// </summary>
        public static IEnumerable<IUser> GetUsersYouRequestedToFollow(int maximumUserIdsToRetrieve = 75000)
        {
            return FriendshipController.GetUsersYouRequestedToFollow(maximumUserIdsToRetrieve);
        }

        // Update Relationship Authorization With

        /// <summary>
        /// Changes the authorizations you give to a specific user.
        /// </summary>
        public static bool UpdateRelationshipAuthorizationsWith(IUserIdentifier userIdentifier, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return FriendshipController.UpdateRelationshipAuthorizationsWith(userIdentifier, retweetsEnabled, deviceNotifictionEnabled);
        }

        /// <summary>
        /// Changes the authorizations you give to a specific user.
        /// </summary>
        public static bool UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return FriendshipController.UpdateRelationshipAuthorizationsWith(userId, retweetsEnabled, deviceNotifictionEnabled);
        }

        /// <summary>
        /// Changes the authorizations you give to a specific user.
        /// </summary>
        public static bool UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            return FriendshipController.UpdateRelationshipAuthorizationsWith(userScreenName, retweetsEnabled, deviceNotifictionEnabled);
        }


        // Lookup Relationships

        /// <summary>
        /// Get the states of relationships you have with a collection of users.
        /// </summary>
        public static IEnumerable<IRelationshipState> GetMultipleRelationships(IEnumerable<IUserIdentifier> targetUserIdentifiers)
        {
            return FriendshipController.GetMultipleRelationships(targetUserIdentifiers);
        }

        /// <summary>
        /// Get the states of relationships you have with a collection of users.
        /// </summary>
        public static IEnumerable<IRelationshipState> GetMultipleRelationships(IEnumerable<long> targetUserIds)
        {
            return FriendshipController.GetMultipleRelationships(targetUserIds);
        }

        /// <summary>
        /// Get the states of relationships you have with a collection of users.
        /// </summary>
        public static IEnumerable<IRelationshipState> GetMultipleRelationships(IEnumerable<string> targetUserScreenNames)
        {
            return FriendshipController.GetMultipleRelationships(targetUserScreenNames);
        }


        // Get User Ids Whose Retweets Are Muted

        /// <summary>
        /// Get ids of users from whom you won't receive retweets.
        /// </summary>
        public static IEnumerable<long> GetUserIdsWhoseRetweetsAreMuted()
        {
            return FriendshipController.GetUserIdsWhoseRetweetsAreMuted();
        }

        /// <summary>
        /// Get ids of users from whom you won't receive retweets.
        /// </summary>
        public static IEnumerable<IUser> GetUsersWhoseRetweetsAreMuted()
        {
            return FriendshipController.GetUsersWhoseRetweetsAreMuted();
        }

        #endregion

        /// <summary>
        /// Get a list of categories that can interest the user of the current account.
        /// </summary>
        public static IEnumerable<ICategorySuggestion> GetSuggestedCategories(Language? language = null)
        {
            return AccountController.GetSuggestedCategories(language);
        }

        /// <summary>
        /// Get a list of categories that can interest the user of the current account.
        /// </summary>
        public static IEnumerable<IUser> GetSuggestedUsers(string filter, Language? language = null)
        {
            return AccountController.GetSuggestedUsers(filter, language);
        }

        /// <summary>
        /// Get a list of categories that can interest the user of the current account.
        /// </summary>
        public static IEnumerable<IUser> GetSuggestedUsersWithTheirLatestTweet(string filter)
        {
            return AccountController.GetSuggestedUsersWithTheirLatestTweet(filter);
        }
    }
}