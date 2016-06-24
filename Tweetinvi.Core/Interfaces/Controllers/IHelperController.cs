using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IHelpController
    {
        ICredentialsRateLimits GetCurrentCredentialsRateLimits();
        ICredentialsRateLimits GetCredentialsRateLimits(ITwitterCredentials credentials);
        string GetTwitterPrivacyPolicy();

        ITwitterConfiguration GetTwitterConfiguration();
        string GetTermsOfService();
    }
}