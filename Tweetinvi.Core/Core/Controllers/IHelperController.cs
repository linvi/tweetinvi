using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters.HelpClient;

namespace Tweetinvi.Core.Controllers
{
    public interface IHelpController
    {
        Task<ICredentialsRateLimits> GetCurrentCredentialsRateLimits();
        Task<ICredentialsRateLimits> GetCredentialsRateLimits(ITwitterCredentials credentials);
        Task<string> GetTwitterPrivacyPolicy();

        Task<ITwitterConfiguration> GetTwitterConfiguration();
        Task<string> GetTermsOfService();
        Task<ITwitterResult<ICredentialsRateLimits>> GetRateLimits(IGetRateLimitsParameters parameters, ITwitterRequest request);
    }
}