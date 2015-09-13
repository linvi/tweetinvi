using System;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi
{
    public class Help
    {
        [ThreadStatic]
        private static IHelpController _helpController;
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

        public static string GetTwitterPrivacyPolicy()
        {
            return HelpController.GetTwitterPrivacyPolicy();
        }

        public static ITwitterConfiguration GetTwitterConfiguration()
        {
            return HelpController.GetTwitterConfiguration();
        }
    }
}