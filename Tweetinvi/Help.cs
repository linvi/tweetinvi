using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Models.DTO;

namespace Tweetinvi
{
    /// <summary>
    /// Access Twitter about information.
    /// </summary>
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
        public static Task<string> GetTwitterPrivacyPolicy()
        {
            return HelpController.GetTwitterPrivacyPolicy();
        }

        /// <summary>
        /// Get the Twitter API configuration
        /// </summary>
        public static Task<ITwitterConfiguration> GetTwitterConfiguration()
        {
            return HelpController.GetTwitterConfiguration();
        }

        /// <summary>
        /// Get Twitter Terms of Service
        /// </summary>
        public static Task<string> GetTermsOfService()
        {
            return HelpController.GetTermsOfService();
        }
    }
}