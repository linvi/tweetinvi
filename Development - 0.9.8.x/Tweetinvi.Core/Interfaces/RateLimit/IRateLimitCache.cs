using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Core.Interfaces.RateLimit
{
    public interface IRateLimitCache
    {
        void Clear(IOAuthCredentials credentials);
        void ClearAll();

        void RefreshEntry(IOAuthCredentials credentials, ITokenRateLimits credentialsRateLimits);
        ITokenRateLimits GetTokenRateLimits(IOAuthCredentials credentials);
    }
}