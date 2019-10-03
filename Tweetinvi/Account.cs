using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
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

        static Account()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _accountController = TweetinviContainer.Resolve<IAccountController>();
        }

        // Profile

        /// <summary>
        /// Update the information of the authenticated user profile.
        /// </summary>
        public static Task<IAuthenticatedUser> UpdateAccountProfile(IAccountUpdateProfileParameters parameters)
        {
            return AccountController.UpdateAccountProfile(parameters);
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