using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Models;

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

        // Mute

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