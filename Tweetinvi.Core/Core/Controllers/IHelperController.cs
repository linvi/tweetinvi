using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Controllers
{
    public interface IHelpController
    {
        Task<ICredentialsRateLimits> GetCurrentCredentialsRateLimits();
        Task<ICredentialsRateLimits> GetCredentialsRateLimits(ITwitterCredentials credentials);
        Task<string> GetTwitterPrivacyPolicy();

        Task<ITwitterConfiguration> GetTwitterConfiguration();
        Task<string> GetTermsOfService();
    }
}