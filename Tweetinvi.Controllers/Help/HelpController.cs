using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters.HelpClient;

namespace Tweetinvi.Controllers.Help
{
    public class HelpController : IHelpController
    {
        private readonly IHelpQueryExecutor _helpQueryExecutor;

        public HelpController(IHelpQueryExecutor helpQueryExecutor)
        {
            _helpQueryExecutor = helpQueryExecutor;
        }

        public Task<ITwitterResult<ICredentialsRateLimits>> GetRateLimits(IGetRateLimitsParameters parameters, ITwitterRequest request)
        {
            return _helpQueryExecutor.GetRateLimits(parameters, request);
        }

        public Task<ICredentialsRateLimits> GetCurrentCredentialsRateLimits()
        {
            return _helpQueryExecutor.GetCurrentCredentialsRateLimits();
        }

        public Task<ICredentialsRateLimits> GetCredentialsRateLimits(ITwitterCredentials credentials)
        {
            return _helpQueryExecutor.GetCredentialsRateLimits(credentials);
        }

        public Task<string> GetTwitterPrivacyPolicy()
        {
            return _helpQueryExecutor.GetTwitterPrivacyPolicy();
        }

        public Task<ITwitterConfiguration> GetTwitterConfiguration()
        {
            return _helpQueryExecutor.GetTwitterConfiguration();
        }

        public Task<string> GetTermsOfService()
        {
            return _helpQueryExecutor.GetTermsOfService();
        }
    }
}