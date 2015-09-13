using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.Core.Interfaces.RateLimit
{
    public interface IRateLimitCache
    {
        void Clear(ITwitterCredentials credentials);
        void ClearAll();

        void RefreshEntry(ITwitterCredentials credentials, ITokenRateLimits credentialsRateLimits);
        ITokenRateLimits GetTokenRateLimits(ITwitterCredentials credentials);
    }
}