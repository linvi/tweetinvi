using System;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Help;

namespace Tweetinvi.Json
{
    public static class HelpJson
    {
        [ThreadStatic]
        private static IHelpJsonController _helpJsonController;
        public static IHelpJsonController HelpJsonController
        {
            get
            {
                if (_helpJsonController == null)
                {
                    Initialize();
                }
                
                return _helpJsonController;
            }
        }        

        static HelpJson()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _helpJsonController = TweetinviContainer.Resolve<IHelpJsonController>();
        }

        public static Task<string> GetCredentialsRateLimits()
        {
            return HelpJsonController.GetCredentialsRateLimits();
        }

        public static Task<string> GetTwitterPrivacyPolicy()
        {
            return HelpJsonController.GetTwitterPrivacyPolicy();
        }
    }
}