using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;

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