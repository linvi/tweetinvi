using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Controllers.Help
{
    public class HelpController : IHelpController
    {
        private readonly IHelpQueryExecutor _helpQueryExecutor;

        public HelpController(IHelpQueryExecutor helpQueryExecutor)
        {
            _helpQueryExecutor = helpQueryExecutor;
        }

        public ICredentialsRateLimits GetCurrentCredentialsRateLimits()
        {
            return _helpQueryExecutor.GetCurrentCredentialsRateLimits();
        }

        public ICredentialsRateLimits GetCredentialsRateLimits(ITwitterCredentials credentials)
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

        public string GetTermsOfService()
        {
            return _helpQueryExecutor.GetTermsOfService();
        }
    }
}