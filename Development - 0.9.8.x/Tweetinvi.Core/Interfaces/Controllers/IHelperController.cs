using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IHelpController
    {
        ITokenRateLimits GetCurrentCredentialsRateLimits();
        ITokenRateLimits GetCredentialsRateLimits(IOAuthCredentials credentials);
        string GetTwitterPrivacyPolicy();

        ITwitterConfiguration GetTwitterConfiguration();
    }
}