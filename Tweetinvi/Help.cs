using System;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi
{
    public class Help
    {
        [ThreadStatic]
        private static IHelpController _helpController;

        /// <summary>
        /// Controller handling any Help request
        /// </summary>
        public static IHelpController HelpController
        {
            get
            {
                if (_helpController == null)
                {
                    Initialize();
                }

                return _helpController;
            }
        }

        static Help()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _helpController = TweetinviContainer.Resolve<IHelpController>();
        }

        /// <summary>
        /// Get the Twitter privacy policy
        /// </summary>
        public static string GetTwitterPrivacyPolicy()
        {
            return HelpController.GetTwitterPrivacyPolicy();
        }

        /// <summary>
        /// Get the Twitter API configuration
        /// </summary>
        public static ITwitterConfiguration GetTwitterConfiguration()
        {
            return HelpController.GetTwitterConfiguration();
        }

        /// <summary>
        /// Get Twitter Terms of Service
        /// </summary>
        public static string GetTermsOfService()
        {
            return HelpController.GetTermsOfService();
        }
    }
}