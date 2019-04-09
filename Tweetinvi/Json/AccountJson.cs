using System;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Account;

namespace Tweetinvi.Json
{
    public static class AccountJson
    {
        [ThreadStatic]
        private static IAccountJsonController _accountJsonController;
        public static IAccountJsonController AccountJsonController
        {
            get
            {
                if (_accountJsonController == null)
                {
                    Initialize();
                }

                return _accountJsonController;
            }
        }
        
        static AccountJson()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _accountJsonController = TweetinviContainer.Resolve<IAccountJsonController>();
        }

        public static Task<string> GetAuthenticatedUserSettingsJson()
        {
            return AccountJsonController.GetAuthenticatedUserSettingsJson();
        }
    }
}