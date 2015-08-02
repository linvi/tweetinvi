using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Controllers.Help
{
    public class HelpController : IHelpController
    {
        private readonly IHelpQueryExecutor _helpQueryExecutor;

        public HelpController(IHelpQueryExecutor helpQueryExecutor)
        {
            _helpQueryExecutor = helpQueryExecutor;
        }

        public ITokenRateLimits GetCurrentCredentialsRateLimits()
        {
            return _helpQueryExecutor.GetCurrentCredentialsRateLimits();
        }

        public ITokenRateLimits GetCredentialsRateLimits(ITwitterCredentials credentials)
        {
            return _helpQueryExecutor.GetCredentialsRateLimits(credentials);
        }

        public string GetTwitterPrivacyPolicy()
        {
            return _helpQueryExecutor.GetTwitterPrivacyPolicy();
        }

        public ITwitterConfiguration GetTwitterConfiguration()
        {
            return _helpQueryExecutor.GetTwitterConfiguration();
        }
    }
}